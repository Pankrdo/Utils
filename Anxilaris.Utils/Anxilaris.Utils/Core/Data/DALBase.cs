namespace Anxilaris.Utils.Core.DAO
{
    
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Transactions;
    

    public abstract class DALBase : IDisposable
    {        
         
        private bool disposed = false;

        private string stringconnection;

        private SqlConnection connection = null;

        private SqlDataReader reader = null;

        protected SqlCommand storeProcedure = null;

        protected TransactionScope transactionScope = null;
        
        public DALBase()
        {
            this.stringconnection = ConfigurationManager.ConnectionStrings["DMCLink"].ConnectionString;
           
        }       

        internal SqlConnection Connection
        {
            get { return this.connection; }
            set { this.connection = value; }
        }

        protected SqlDataReader Reader
        {
            get { return this.reader; }
        }        
        protected bool KeepConnection { get; set; }
        protected void PrepareStoreProcedure(string storedProcedureName)
        {

            this.storeProcedure = new SqlCommand(storedProcedureName);
            this.storeProcedure.CommandType = CommandType.StoredProcedure;
        }

        protected bool OpenConnection()
        {
            if (this.connection == null || this.connection.State == ConnectionState.Closed)
            {
                this.connection = new SqlConnection(this.stringconnection);
            }

            if (this.connection.State != ConnectionState.Open)
            {
                this.connection.Open();
            }

            return this.connection.State == ConnectionState.Open;
        }

        protected void CompleteTransaction()
        {
            if (this.transactionScope != null)
                this.transactionScope.Complete();
        }

        protected void ExecuteNonQuery()
        {
            try
            {
                this.PrepareExecution();
                int affectedRows = this.storeProcedure.ExecuteNonQuery();
                this.CloseConnection();
            }
            catch
            {
                this.KeepConnection = false;
                this.CloseConnection();
                throw;
            }

        }

        protected void ExecuteReader()
        {
            try
            {
                this.PrepareExecution();
                this.reader = this.storeProcedure.ExecuteReader();

            }
            catch
            {
                this.KeepConnection = false;
                this.CloseConnection();
                throw;
            }
        }

        protected T ExecuteScalar<T>()
        {
            try
            {
                this.PrepareExecution();

                object obj = this.storeProcedure.ExecuteScalar();

                T value = obj == null ? default(T) : (T)obj;

                this.CloseConnection();

                return value;
            }
            catch
            {
                this.KeepConnection = false;
                this.CloseConnection();
                throw;
            }
        }

        protected void CloseConnection()
        {
            if (!KeepConnection)
            {
                if (this.reader != null && !this.reader.IsClosed)
                    this.reader.Close();

                if (this.connection != null)
                {
                    if (this.connection.State == ConnectionState.Open)
                    {
                        this.connection.Close();
                    }

                    this.connection.Dispose();
                }
            }
        }

        protected void AddParameter(string paramName, object value)
        {
            this.storeProcedure.Parameters.AddWithValue(paramName, value);
        }

        protected void AddParameter(string name, SqlDbType type, ParameterDirection direction)
        {
            var parameter = new SqlParameter(name, type);
            parameter.Direction = direction;
            this.storeProcedure.Parameters.Add(parameter);
        }

        protected void AddParameter(string name, SqlDbType type, ParameterDirection direction, object value)
        {
            var parameter = new SqlParameter(name, type);
            parameter.Direction = direction;
            parameter.Value = value;
            this.storeProcedure.Parameters.Add(parameter);
        }

        protected T GetData<T>(string fieldName, bool throwException = false)
        {
            if (HasColumn(fieldName))
            {
                var field = this.reader[fieldName];

                if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return field == DBNull.Value ? default(T) : (T)field;
                }

                if (field is IConvertible)
                    return field == DBNull.Value ? default(T) : (T)Convert.ChangeType(field, typeof(T));
                else
                    return (T)field;
            }

            if (throwException)
            {
                throw new Exception("Column [" + fieldName + "] was not found");
            }
            else
            {
                return default(T);
            }
        }

        protected List<T> GetFromJsonArray<T>(string fieldName)
        {
            if (HasColumn(fieldName))
            {
                var field = this.reader[fieldName] ?? string.Empty;

                return JsonConvert.DeserializeObject<List<T>>(field.ToString());
            }

            return new List<T>();
        }

        protected T GetFromJsonObject<T>(string fieldName)
        {
            List<T> result = this.GetFromJsonArray<T>(fieldName) ?? new List<T>();

            return result.Count > 0 ? result[0] : Activator.CreateInstance<T>();
        }

        private bool HasColumn(string ColumnName)
        {
            int columncount = reader.FieldCount;
            for (int i = 0; i < columncount; i++)
            {
                if (reader.GetName(i) == ColumnName)
                    return true;
            }

            return false;
        }


        private void PrepareExecution()
        {
            this.OpenConnection();
            if (this.storeProcedure.Connection == null)
                this.storeProcedure.Connection = this.connection;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {

                    if (this.storeProcedure != null)
                        this.storeProcedure.Dispose();

                    if (this.connection != null)
                        this.CloseConnection();
                }

                this.stringconnection = null;

                this.reader = null;
                this.disposed = true;
            }
        }

        ~DALBase()
        {
            this.Dispose(false);
        }
    }
}
