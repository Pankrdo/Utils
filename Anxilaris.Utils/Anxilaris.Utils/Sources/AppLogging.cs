//-----------------------------------------------------------------------
// <copyright file="AppLogging.cs" company="BestDay">
//     Copyright (c) Sprocket Enterprises. All rights reserved.
// </copyright>
// <author>Adrian Castan Melchor/author>
//----------------------------------------------------------------------

using System;
using System.IO;

namespace Anxilaris.Utils
{    
    public class AppLogging
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static log4net.ILog Get(bool configureFromConfig=false,string logName=null)
        {
            if (configureFromConfig)
            {
                log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            }           

            return logger;
        }   
        
        private void SetLogName()
        {
            var appenders = logger.Logger.Repository.GetAppenders();

            if (appenders != null && appenders.Length > 0)
            {
                appenders[0].Name = "Anxilaris.Services.Viator.Log";
            }            
        }
    }
}