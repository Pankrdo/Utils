//-----------------------------------------------------------------------
// <copyright file="SmtpElement.cs" company="BestDay">
//     Copyright (c) Sprocket Enterprises. All rights reserved.
// </copyright>
// <author>Jose Martín Trejo Pancardo</author>
//----------------------------------------------------------------------

namespace Anxilaris.Utils.Configuration
{
    using System.Configuration;

    public class SmtpElement : ConfigurationElement
    {    
        [ConfigurationProperty("host", IsRequired = true)]
        public string Host
        {
            get
            {
                return (string)this["host"];
            }
        }

        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get
            {
                return (int)this["port"];
            }
        }

        [ConfigurationProperty("ssl", IsRequired = true)]
        public bool Ssl
        {
            get
            {
                return (bool)this["ssl"];
            }
        }

        [ConfigurationProperty("credential")]
        public SmtpCredentialElement Credential
        {
            get
            {
                return (SmtpCredentialElement)this["credential"];
            }
        }
    }
}
