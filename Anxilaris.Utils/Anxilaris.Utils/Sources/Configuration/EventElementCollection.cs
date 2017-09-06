// <copyright file="EventElementCollection.cs" company="BestDay">
//     Copyright (c) Sprocket Enterprises. All rights reserved.
// </copyright>
// <author>Jose Martín Trejo Pancardo</author>
//----------------------------------------------------------------------

namespace Anxilaris.Utils.Configuration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;

    [ConfigurationCollection(typeof(EventElement))]
    public class EventElementCollection : ConfigurationElementCollection, IEnumerable<EventElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new EventElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EventElement)element).Name;
        }

        IEnumerator<EventElement> IEnumerable<EventElement>.GetEnumerator()
        {
            int count = base.Count;
            for (int i = 0; i < count; i++)
            {
                yield return base.BaseGet(i) as EventElement;
            }
        }

        new public EventElement this[string name]
        {
            get { return (EventElement)base.BaseGet(name); }
        }

        public EventElement this[int index]
        {
            get { return (EventElement)base.BaseGet(index); }
        }
    }
}
