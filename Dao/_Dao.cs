using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MiniProfiler.Integrations;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Literary_Arts.Dao
{
    public class _Dao : IDisposable
    {
        //存放連線字串
        protected string strConnMain;

        //存放SQL 指令
        protected string strSql;

        //存放SQL 參數
        protected object objParam;    

        private bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public _Dao(){
            strConnMain = ConfigurationManager.ConnectionStrings["MainDBConnection"].ConnectionString;
            strSql = "";
            objParam = null;

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~_Dao()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
            }

            disposed = true;
        }



        protected IList<IDictionary<string, object>> ExecuteQuery(string sql, object param = null)
        {
            using (var conn = DbConnectionFactoryHelper.New(new SqlServerDbConnectionFactory(strConnMain), CustomDbProfiler.Current))
            {
                return conn.Query<IDictionary<string, object>>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 執行 SQL Query 使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected IList<T> ExecuteQuery<T>(string sql, object param = null)
        {
            //using (SqlConnection conn = new SqlConnection(strConnMain)) {
            //    return conn.Query<T>(sql).ToList();
            //}
            using (var conn = DbConnectionFactoryHelper.New(new SqlServerDbConnectionFactory(strConnMain), CustomDbProfiler.Current))
            {
                return conn.Query<T>(sql, param).ToList();
            }
        }

        /// <summary>
        /// 執行SQL insert update 等指令
        /// </summary>
        /// <param name="sql"> sql指令 </param>
        /// <param name="param"> sql的各種參數 </param>
        /// <returns></returns>
        protected bool ExecuteCommand(string sql, object param = null)
        {
            using (var conn = DbConnectionFactoryHelper.New(new SqlServerDbConnectionFactory(strConnMain), CustomDbProfiler.Current))
            {
                return conn.Execute(sql, param) > 0;
            }

        }

        /// <summary>
        /// 取得tag標籤
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="num">
        ///     任何編號 
        ///     ex: arti_num  or recom_num 
        /// </param>
        /// <param name="type"> 
        ///     01: 文章 article
        ///     02: 推薦 recommend
        /// </param>
        /// <returns>回傳List型態的所有tag</returns>
        public IList<T> GetTag<T>(string num, string type) {
            try
            {
  
                if (type == "01")
                {
                    strSql = @"SELECT  V.ARTI_NUM                      --文章編號
	                                  ,TG.TAG_NUM                      --TAG編號
	                                  ,TG.TAG_NAME                     --TAG中文名稱
                               FROM VW_ARTICLE_LIST AS V		               
	                               INNER JOIN TAG_LINK AS TGL			
		                               ON V.ARTI_NUM = TGL.ARTI_NUM		    
	                               INNER JOIN TAG AS TG				
		                               ON TGL.TAG_NUM = TG.TAG_NUM
                               WHERE V.ARTI_NUM = @num  
                               ORDER BY TGL.ARTI_NUM  ";
                }
                else if (type == "02")
                {
                    strSql = @"SELECT  V.RECOM_NUM                     --推薦編號
	                                  ,TG.TAG_NUM                      --TAG編號
	                                  ,TG.TAG_NAME                     --TAG中文名稱
                               FROM VW_RECOMMEND_LIST AS V		               
	                               INNER JOIN TAG_LINK AS TGL			
		                               ON V.RECOM_NUM = TGL.RECOM_NUM		    
	                               INNER JOIN TAG AS TG				
		                               ON TGL.TAG_NUM = TG.TAG_NUM
                               WHERE V.RECOM_NUM = @num  
                               ORDER BY TGL.RECOM_NUM  ";
                }


                objParam = new
                {
                    num = num
                };

                return ExecuteQuery<T>(strSql, objParam);

            } catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}