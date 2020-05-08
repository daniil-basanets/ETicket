using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using log4net;
using log4net.Config;

namespace ETicket.ApplicationServices.Logger
{
    public static class LoggerService
    {
        public static void Initialize()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
    }
}
