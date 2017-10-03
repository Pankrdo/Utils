namespace Anxilaris.Utils.Core.Entities
{
    using Microsoft.Win32.SafeHandles;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.InteropServices;
    using System.Web.Script.Serialization;

    public class ValidatorBase: IDisposable
    {
        private bool disposed = false;
        private List<string> stringErrors;
        private SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        [JsonIgnore]
        [ScriptIgnore]
        public IList<ValidationResult> Errors { get; private set; }

        [JsonIgnore]
        [ScriptIgnore]
        public bool hasError
        {
            get { return (Errors != null) ? Errors.Count > 0 : false; }
        }

        public bool IsValid()
        {
            Errors = new List<ValidationResult>();
            var vc = new ValidationContext(this, null, null);
            var isValid = Validator.TryValidateObject(this, vc, Errors, true);
            return isValid;
        }

        public List<string> ErrorsAsString()
        {
            this.stringErrors = new List<string>();

            foreach (var item in Errors)
            {
                stringErrors.Add(item.ErrorMessage);
            }

            return stringErrors;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
            }

            disposed = true;
        }
    }
}
