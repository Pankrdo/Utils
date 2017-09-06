// <copyright file="EventElement.cs" company="BestDay">
//     Copyright (c) Sprocket Enterprises. All rights reserved.
// </copyright>
// <author>Jose Martín Trejo Pancardo</author>
//----------------------------------------------------------------------

namespace Anxilaris.Utils.Configuration
{
    using System.Configuration;

    public class EventElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
        }

        [ConfigurationProperty("mails", IsRequired = true, IsDefaultCollection = false)]
        public MailElementCollection Mails
        {
            get
            {
                return (MailElementCollection)this["mails"];
            }
        }
    }
}
