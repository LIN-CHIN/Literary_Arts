using Dapper;
using MiniProfiler.Integrations;
using NLog;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Configuration;
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

        public static void LogSqlTrace(object objParam) {
            SqlServerDbConnectionFactory mainConn = new SqlServerDbConnectionFactory(ConfigurationManager.ConnectionStrings["MainDBConnection"].ConnectionString);
            using (var conn = DbConnectionFactoryHelper.New(mainConn, CustomDbProfiler.Current)) 
            {
                string strSql = @"insert into dbo.SQL_TRACE(USER_ID,USER_IP,COMMAND_TEXT,PARAMETERS,REQUEST_URL) 
                                        values(@user_id,@user_ip,@commandtext,@parameters,@request_url)";
                conn.Execute(strSql, objParam);
            }


        }

    }
}