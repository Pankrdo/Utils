//-----------------------------------------------------------------------
// <copyright file="StringEnum.cs" company="BestDay">
//     Copyright (c) Sprocket Enterprises. All rights reserved.
// </copyright>
// <author>José Martín Trejo Pancardo</author>
//----------------------------------------------------------------------

namespace Anxilaris.Utils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;

    public class StringEnum
    {
        private static Hashtable _displayNames = new Hashtable();
        private Type _enumType;

        public Type EnumType
        {
            get
            {
                return this._enumType;
            }
        }

        public StringEnum(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException(string.Format("Supplied type must be an Enum.  Type was {0}", (object)enumType.ToString()));
            this._enumType = enumType;
        }

        public static StringEnum Asign(Type enumType)
        {
            return new StringEnum(enumType);
        }

        public string GetDisplayName(string displayName)
        {
            string str = (string)null;
            try
            {
                str = StringEnum.GetDisplayName((Enum)Enum.Parse(this._enumType, displayName));
            }
            catch
            {
                str = null;
            }
            return str;
        }

        public  SortedList<int, string> ToArray()
        {
            ArrayList values = this.GetPairValues();

            SortedList<int, string> results = new SortedList<int, string>();

            for (int i = 0; i < values.Count; i++)
            {
                var item = (DictionaryEntry)values[i];
                results.Add(Convert.ToInt32(item.Key), Convert.ToString(item.Value));
            }

            return results;
        }

        public ArrayList GetPairValues()
        {
            ArrayList arrayList = new ArrayList();
            foreach (FieldInfo field in this._enumType.GetFields())
            {
                DisplayAttribute[] DisplayNameArray = field.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
                if (DisplayNameArray.Length > 0)
                    arrayList.Add((object)new DictionaryEntry(Enum.Parse(this._enumType, field.Name), (object)DisplayNameArray[0].Name));
            }
            return arrayList;
        }

        public string[] GetArrayNames()
        {
            return (from ext in this.GetPairValues().ToArray()
                    select (string)((DictionaryEntry)ext).Value).ToArray();
        }

        public bool IsStringDefined(string displayName)
        {
            return StringEnum.Parse(this._enumType, displayName) != null;
        }

        public bool IsStringDefined(string displayName, bool ignoreCase)
        {
            return StringEnum.Parse(this._enumType, displayName, ignoreCase) != null;
        }

        public static string GetDisplayName(Enum value)
        {
            string str = (string)null;
            Type type = value.GetType();
            if (StringEnum._displayNames.ContainsKey((object)value))
            {
                str = (StringEnum._displayNames[(object)value] as DisplayAttribute).Name;
            }
            else
            {
                DisplayAttribute[] DisplayNameArray = type.GetField(value.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
                if (DisplayNameArray.Length > 0)
                {
                    StringEnum._displayNames.Add((object)value, (object)DisplayNameArray[0]);
                    str = DisplayNameArray[0].Name;
                }
            }
            return str;
        }

        public static object Parse(Type type, string displayName)
        {
            return StringEnum.Parse(type, displayName, false);
        }

        public static object Parse(Type type, string displayName, bool ignoreCase)
        {
            object obj = (object)null;
            string strA = (string)null;
            if (!type.IsEnum)
                throw new ArgumentException(string.Format("Supplied type must be an Enum.  Type was {0}", (object)type.ToString()));
            foreach (FieldInfo field in type.GetFields())
            {
                DisplayAttribute[] DisplayNameArray = field.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
                if (DisplayNameArray.Length > 0)
                    strA = DisplayNameArray[0].Name;
                if (string.Compare(strA, displayName, ignoreCase) == 0)
                {
                    obj = Enum.Parse(type, field.Name);
                    break;
                }
            }
            return obj;
        }

        public static bool IsStringDefined(Type enumType, string displayName)
        {
            return StringEnum.Parse(enumType, displayName) != null;
        }

        public static bool IsStringDefined(Type enumType, string displayName, bool ignoreCase)
        {
            return StringEnum.Parse(enumType, displayName, ignoreCase) != null;
        }


    }
}
