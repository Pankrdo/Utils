//-----------------------------------------------------------------------
// <copyright file="SmtpServerElement.cs" company="BestDay">
//     Copyright (c) Sprocket Enterprises. All rights reserved.
// </copyright>
// <author>Jose Martín Trejo Pancardo</author>
//----------------------------------------------------------------------

namespace Anxilaris.Utils.Configuration
{
    using System.Configuration;

    public class SmtpCredentialElement : ConfigurationElement
    {        
        [ConfigurationProperty("userName")]
        public string UserName
        {
            get
            {
                return (string)this["userName"];
            }
        }

        [ConfigurationProperty("displayName")]
        public string DisplayName
        {
            get
            {
                return (string)this["displayName"];
            }
        }

        [ConfigurationProperty("password")]
        public string Password
        {
            get
            {
                return (string)this["password"];
            }
        }
    }
}
