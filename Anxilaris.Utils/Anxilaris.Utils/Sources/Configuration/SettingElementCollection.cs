// <copyright file="MailElementCollection.cs" company="BestDay">
//     Copyright (c) Sprocket Enterprises. All rights reserved.
// </copyright>
// <author>Jose Martín Trejo Pancardo</author>
//----------------------------------------------------------------------

namespace Anxilaris.Utils.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;

    public class SettingElementCollection : ConfigurationElementCollection, IEnumerable<SettingElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SettingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SettingElement)element).Name;
        }

        IEnumerator<SettingElement> IEnumerable<SettingElement>.GetEnumerator()
        {
            int count = base.Count;
            for (int i = 0; i < count; i++)
            {
                yield return base.BaseGet(i) as SettingElement;
            }
        }

        new public SettingElement this[string name]
        {
            get { return (SettingElement)base.BaseGet(name); }
        }

        public SettingElement this[int index]
        {
            get { return (SettingElement)base.BaseGet(index); }
        }
    }
}
