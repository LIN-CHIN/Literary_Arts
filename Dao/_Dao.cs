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
using Literary_Arts.Models;
using System.Collections;

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
        /// 在取得所有類別文章的Tag之前 必須經過此Router
        /// 來整理取得tag 所需要的 ARTI_NUM or  RECOM_NUM ... 等資訊後
        /// 再傳送到 GetTag<T> 方法中
        /// </summary>
        /// <param name="model">用來取得 文章的編號 或是 推薦的編號，或是其他種類的編號</param>
        /// <param name="type"> 01 = 文章 , 02 = 推薦 </param>
        public IList<T> TagRouter<T>(IList<T> model, string type) where T : new() 
        {
            //用來存放 需要查找tag的所有num 且設為參數 例如我有 編號41,42,43 要查找 
            //則會以 @ARTI_NUM0 , @ARTI_NUM1 ,@ARTI_NUM2 的方式存放
            List<string> num = new List<string>();

            //存放所有num參數的值
            List<string> value = new List<string>();

            //文章 ARTICLE
            if (type == "01") {
                for (var i = 0; i < model.ToArray().Length; i++)
                {
                    //取得泛型model 的屬性值
                    string val = model.ToArray()[i].GetType().GetProperty("ARTI_NUM").GetValue(model.ToArray()[i], null).ToString();
                    //以參數的形式存放
                    num.Add("@ARTI_NUM" + i.ToString());

                    //存放參數值
                    value.Add(val);

                }
            }
            //推薦  RECOMMEND
            else if (type == "02")
            {
                //num.Add("@" + item.RECOM_NUM);
            }

            IList<T> list = GetTag<T>(num.ToArray(), type, value.ToArray());
            return list;
        }

        /// <summary>
        /// 取得tag標籤
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="num">
        ///     參數陣列 
        ///     ex: arti_num  or recom_num 
        /// </param>
        /// <param name="type"> 
        ///     01: 文章 article
        ///     02: 推薦 recommend
        /// </param>
        /// <returns>回傳List型態的所有tag</returns>
        public IList<T> GetTag<T>(string[] num, string type, string[] value) { 
            try
            {
                IDictionary objParam = new Dictionary<string, object>();
                
                //文章 ARTICLE
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
                                WHERE 1=1 AND ( ";
                    //將參數num 串接字串內
                    for (var i = 0; i < value.Length; i++)
                    {
                        strSql += @" V.ARTI_NUM = " + num[i] + " OR ";
                        objParam.Add(num[i], value[i]);
                    }

                    //將最後的 OR 刪掉
                    strSql = strSql.Substring(0, strSql.Length - 4);
                    strSql += @" ) ORDER BY TGL.ARTI_NUM  ";
                }
                //推薦 RECOMMEND
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

                return ExecuteQuery<T>(strSql, objParam);

            } catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得物件屬性名稱(Properties Name)
        /// </summary>
        /// <param name="pObject">任意物件</param>
        /// <returns></returns>
        public static List<string> GetPropertiesNameOfClass(object pObject)
        {
            List<string> propertyList = new List<string>();
            if (pObject != null)
            {
                foreach (var prop in pObject.GetType().GetProperties())
                {
                    propertyList.Add(prop.Name);
                }
            }
            return propertyList;
        }

        /// <summary>
        /// 取得物件屬性值(Property Value)
        /// </summary>
        /// <param name="src">已宣告的物件</param>
        /// <param name="propName">屬性名稱</param>
        /// <returns></returns>
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }



    }
}