using System;
using System.Collections.Generic;
using System.Linq;


namespace Anxilaris.Utils
{
    public static class IoCAnxilaris
    {
        private static Dictionary<string, object> typesDictionary { get; set; }

        //public static void LoadFromConfig(string configSection)
        //{

        //}

        //public static void GetTypeForName(string typeForName)
        //{

        //}

        public static T GetDependence<T, U>()
        {
            if (typesDictionary == null && typeof(T).IsInterface)
                throw new Exception("Dictionary has not been initilized");

            var internalDict = typesDictionary.Where(type => type.Key == typeof(U).ToString()).ToList();

            for (int i = 0; i < internalDict.Count; i++)
            {
                if (typeof(T).IsAssignableFrom(internalDict[i].Value.GetType()))
                {

                    Type typeImplementing = internalDict[i].Value.GetType();

                    T result = (T)Activator.CreateInstance(typeImplementing);

                    return result;

                }
            }
            throw new Exception("Implementation not found");
        }

        public static void Register<T>(T typetoRegister, Type forClass)
        {
            typesDictionary = typesDictionary ?? new Dictionary<string, object>();

            typesDictionary.Add(forClass.ToString(), typetoRegister);
        }

        public static void Register<T>(string typetoRegister, Type forClass)
        {
            typesDictionary = typesDictionary ?? new Dictionary<string, object>();

            typesDictionary.Add(typetoRegister, typetoRegister);
        }
    }
}
