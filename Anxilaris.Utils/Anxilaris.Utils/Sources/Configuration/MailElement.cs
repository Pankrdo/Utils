//-----------------------------------------------------------------------
// <copyright file="MailElement.cs" company="BestDay">
//     Copyright (c) Sprocket Enterprises. All rights reserved.
// </copyright>
// <author>Jose Martín Trejo Pancardo</author>
//----------------------------------------------------------------------

namespace Anxilaris.Utils.Configuration
{
    using System.Configuration;

    public class MailElement : ConfigurationElement
    {
        [ConfigurationProperty("mail", IsRequired = true, IsKey = true)]
        public string Mail
        {
            get
            {
                return (string)this["mail"];
            }
        }
    }
}
