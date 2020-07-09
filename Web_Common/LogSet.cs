using NLog;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Web_Common
{
    public class LogSet
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        public static void LogDebug(string message) {
            logger.Debug(HttpUtility.HtmlEncode(message));
        }

        public static void LogError(string message) {
            logger.Error(HttpUtility.HtmlEncode(message));
        }

        public static void LogInfo(string message) {
            logger.Info(HttpUtility.HtmlEncode(message));
        }
    }
}