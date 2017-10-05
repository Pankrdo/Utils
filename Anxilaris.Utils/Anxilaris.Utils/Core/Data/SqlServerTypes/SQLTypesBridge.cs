namespace Anxilaris.Utils.Core.DAO
{
    using System.Collections.Generic;
    using Microsoft.SqlServer.Types;
    using System.Data.SqlTypes;
    using System;

    public abstract class SQLTypesBridge:DALBase
    {
        protected SqlGeography GetGeographicPoint(KeyValuePair<double, double> point)
        {
            SqlGeography gPoint = null;

            //if (point != null)
            {
                string points = string.Format("{0} {1}", point.Value, point.Key).Replace(",", ".");                
                gPoint = SqlGeography.STPointFromText(new SqlChars("POINT(" + points + ")"),4326);
                gPoint.MakeValid();
            }

            return gPoint;
        }

        protected SqlGeography GetGeographicPoint(string point)
        {
            if (point == null)
            {
                return new SqlGeography();
            }

            SqlGeography gPoint = new SqlGeography();                                
            gPoint = SqlGeography.STPointFromText(new SqlChars(point.ToString()),4326);
            gPoint.MakeValid();              
            
            return gPoint;

        }

        protected SqlGeography GetGeographicArea(List<KeyValuePair<double,double>> points)
        {
            if (points == null || points.Count < 3)
                return null;

            int length = points.Count;

            string pointToAdd = string.Empty ;

            for (int i = 0; i < length; i++)
            {
                var point = points[i];

                
                {
                    pointToAdd += string.Format("{0} {1}", point.Value.ToString().Replace(",", "."), point.Key.ToString().Replace(",", "."));
                    if(i< length-1)
                    pointToAdd += ",";
                }
            }

            SqlGeography area = SqlGeography.STPolyFromText(new SqlChars("POLYGON ((" + pointToAdd + "))"), 4326);
            area=area.MakeValid();
          
            return area;
        }

        protected KeyValuePair<double,double> GetLocationFromGeographic(SqlGeography point)
        {
            return new KeyValuePair<double, double>((double)point.Lat,(double) point.Long);
        }

        protected IEnumerable<SqlGeography> ToListFromGeographic(SqlGeography area)
        {
            if(area==null)
                return new List<SqlGeography>();

            var results = new List<SqlGeography>();

            for (int i = 0; i < area.STNumPoints(); i++)
            {
                var point = area.STPointN(i);
               
                results.Add(point);
            }
            return results;
        }

        protected SqlGeography GetFromText(string WKTgeographicString)
        {
            return SqlGeography.STPointFromText(new SqlChars(WKTgeographicString), 4326);
        }
    }   
}
