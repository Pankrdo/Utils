namespace Anxilaris.Utils
{
    using System;

    public class AppSettings
    {
        public static T GetKey<T>(string keyName)
        {
            try
            {
                String value = System.Configuration.ConfigurationManager.AppSettings[keyName];

                T resutValue = value == null ? default(T) : (T)Convert.ChangeType(value, typeof(T));

                return resutValue;
            }
            catch
            {
                return default(T);
            }
        }
    }
}