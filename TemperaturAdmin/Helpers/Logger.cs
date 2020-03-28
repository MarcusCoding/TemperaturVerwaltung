using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemperaturAdmin.Models;

namespace TemperaturAdmin.Helpers
{
    public class Logger
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void writeLog(LogType type, string message, Exception ex = null)
        {
            switch (type)
            {
                case LogType.INFO:
                    logger.Info(message);
                    break;
                case LogType.DEBUG:
                    logger.Debug(message);
                    break;
                case LogType.WARNING:
                    logger.Warn(message);
                    break;
                case LogType.ERROR:
                    if(ex == null)
                    {
                        logger.Error(message);
                    }
                    else
                    {
                        logger.Error(ex, message);
                    }
                    break;
            }        
        }
    }
}
