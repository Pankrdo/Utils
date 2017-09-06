//-----------------------------------------------------------------------
// <copyright file="SendGridElement.cs" company="BestDay">
//     Copyright (c) Sprocket Enterprises. All rights reserved.
// </copyright>
// <author>Jose Martín Trejo Pancardo</author>
//----------------------------------------------------------------------

namespace Anxilaris.Utils.Configuration
{
    using System.Configuration;

    public class SendGridElement : ConfigurationElement
    {
        [ConfigurationProperty("apiKey", IsRequired = true, IsKey = true)]
        public string ApiKey
        {
            get
            {
                return (string)this["apiKey"];
            }
        }
        
        public SmtpCredentialElement Credential
        {
            get
            {
                return (SmtpCredentialElement)this["credential"];
            }
        }
    }
}
