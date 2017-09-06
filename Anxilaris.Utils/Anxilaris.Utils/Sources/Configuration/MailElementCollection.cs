//-----------------------------------------------------------------------
// <copyright file="MailElementCollection.cs" company="BestDay">
//     Copyright (c) Sprocket Enterprises. All rights reserved.
// </copyright>
// <author>Jose Martín Trejo Pancardo</author>
//----------------------------------------------------------------------

namespace Anxilaris.Utils.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;

    [ConfigurationCollection(typeof(MailElement))]
    public class MailElementCollection : ConfigurationElementCollection, IEnumerable<MailElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MailElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MailElement)element).Mail;
        }

        IEnumerator<MailElement> IEnumerable<MailElement>.GetEnumerator()
        {
            int count = base.Count;
            for (int i = 0; i < count; i++)
            {
                yield return base.BaseGet(i) as MailElement;
            }
        }

        new public MailElement this[string name]
        {
            get { return (MailElement)base.BaseGet(name); }
        }

        public MailElement this[int index]
        {
            get { return (MailElement)base.BaseGet(index); }
        }
    }
}
