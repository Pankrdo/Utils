//-----------------------------------------------------------------------
// <copyright file="CustomConfig.cs" company="BestDay">
//     Copyright (c) Sprocket Enterprises. All rights reserved.
// </copyright>
// <author>Jose Martín Trejo Pancardo</author>
//----------------------------------------------------------------------

namespace Anxilaris.Utils.Configuration
{
    using System;
    using System.Configuration;

    public class CustomConfig : ConfigurationSection
    {
        public static CustomConfig Load()
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) as Configuration;

                return config.GetSection("customConfig") as CustomConfig;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ConfigurationProperty("notifyEvents", IsDefaultCollection = false)]
        public EventElementCollection NotifyEvents
        {
            get
            {
                return (EventElementCollection)this["notifyEvents"];
            }
        }

        [ConfigurationProperty("settings", IsDefaultCollection = false)]
        public SettingElementCollection Settings
        {
            get
            {
                return (SettingElementCollection)this["settings"];
            }
        }

        [ConfigurationProperty("appLogin")]
        public MailElement AppLogin
        {
            get
            {
                return (MailElement)this["appLogin"];
            }
        }

        [ConfigurationProperty("smtp")]
        public SmtpElement Smtp
        {
            get
            {
                return (SmtpElement)this["smtp"];
            }
        }

        [ConfigurationProperty("sendGrid")]
        public SendGridElement SendGrid
        {
            get
            {
                return (SendGridElement)this["sendGrid"];
            }
        }
    }
}
