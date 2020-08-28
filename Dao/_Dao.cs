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
using Literary_Arts.Web_Common;
using System.Text;
using Microsoft.Ajax.Utilities;
using Literary_Arts.Dao.Sysop;
using Microsoft.SqlServer.Server;

namespace Literary_Arts.Dao
{
    public class _Dao : IDisposable
    {
        
        protected string strConnMain;           //存放連線字串 
        protected string strSql;                //存放SQL 指令  
        protected object objParam;              //存放SQL 參數
        protected MemberUserModel loggedUser;   
        private bool disposed = false;

        /// <summary>
        /// 建構
        /// </summary>
        public _Dao(MemberUserModel loginUser){
            strConnMain = ConfigurationManager.ConnectionStrings["MainDBConnection"].ConnectionString;
            strSql = "";
            objParam = null;
            loggedUser = loginUser ?? new MemberUserModel(); // 若loginUser == null 則new 一個 MemberUserModel 

        }

        #region Dispose
        public void Dispose()
        {   
            //釋放資源 
            Dispose(true);

            // 這是告訴CLR，在進行垃圾回收的時候，不用再繼續調用析構方法了 因已經手動釋放資源了
            GC.SuppressFinalize(this);
        }

        // ~ 為 析構函數  目的 : 用於釋放被占用的系統資源。
        // 垃圾回收器決定了析構函數的調用，無法控制何時調用析構函數。
        ~_Dao()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed) {
                if (disposing) 
                {
                    SetProfilerLog();                
                }
                disposed = true;
            }
        }
        #endregion

        private void SetProfilerLog() {
            lock (CustomDbProfiler.Current.ProfilerContext.ExecutedCommands)
            {
                object objParam = new ArrayList();
                foreach (DbCommandInfo command in CustomDbProfiler.Current.ProfilerContext.ExecutedCommands.ToArray())
                {
                    ((ArrayList)objParam).Add(new
                    {
                        user_id =  "",
                        user_ip =  "",
                        commandtext = command.CommandText,
                        parameters = command.Parameters.Aggregate(new StringBuilder(),
                                 (sb, p) => sb.AppendLine(string.Format("@{0}='{1}'", p.Key, p.Value)),
                                 (sb) => sb.ToString()),
                        request_url = HttpContext.Current.Request.Url.AbsoluteUri
                    });
                }
                LogSet.LogSqlTrace(((ArrayList)objParam).ToArray());
                CustomDbProfiler.Current.ProfilerContext.Reset();
            }
        }


        #region 執行SQL function
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

        #endregion

        #region Tag function
        /// <summary>
        /// 主要使用時機 : 取得List的文章、推薦或其他type的tag
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
        /// 取得tag標籤 (List型態)
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
                LogSet.LogError(ex.ToString());
                return new List<T>();
            }
        }

        /// <summary>
        /// 根據編號 取得tag 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"> 01 = 文章 , 02 = 推薦 </param>
        /// <param name="num"> 根據不同type傳來的編號</param>
        /// <returns></returns>
        public IList<T> ByNumGetTag<T>(string type, string num) where T :new() {
            try
            {
                strSql = @"SELECT TAG_NAME
                           FROM TAG_LINK AS tl
	                           INNER JOIN TAG AS t
		                           ON tl.TAG_NUM = t.TAG_NUM
                            WHERE 1=1 ";
                //根據文章編號取得tag
                if (type == "01")
                {
                    strSql += @" AND tl.ARTI_NUM = @num ";
                }
                //根據推薦編號取得tag
                else if (type == "02")
                {
                    strSql += @" AND tl.RECOM_NUM = @num ";
                }

                objParam = new
                {
                    num = num
                };

                return ExecuteQuery<T>(strSql, objParam);
            }
            catch (Exception ex) {
                LogSet.LogError(ex.ToString());
                return new List<T>();
            }
            
        }
        #endregion

        /// <summary>
        /// 是否有權限操作編輯/刪除的function
        /// (自己發的文才可以操作)
        /// </summary>
        /// <param name="type">
        ///     01 = 文章 
        ///     02 = 推薦
        ///     03 = 活動
        ///     04 = 專欄
        /// </param>
        /// <param name="is_message">
        ///     true = 留言功能
        ///     false = 文章功能
        /// </param>
        /// <returns></returns>
        public bool IsHaveAuthorityOperateFn(string type, bool is_message,string num, MemberUserModel model)
        {
            string table_name = "";
            string num_name = "";
            bool is_sysop = false;
            using (ArtRoleDao dao = new ArtRoleDao(model)) {
                //如果帳號有找到關於系統管理員的角色  is_sysop = true
                is_sysop = dao.GetRoleByMemID(model.MEM_ID).Where(d => d.ROLE_ID == "SystemManagerRole").Count() > 0 ;
            }

            #region 取得table name 和 num名稱
            //依照參數 找對應的table name 和 num名稱
            //文章
            if (type == "01")  
            {
                //文章留言
                if (is_message) 
                {
                    table_name = "ARTICLE_REPLY";
                    num_name = "ARTI_REPLY_NUM";
                }
                //文章
                else
                {
                    table_name = "VW_ARTICLE_LIST";
                    num_name = "ARTI_NUM";
                }
            }
            //推薦、活動、專欄
            else if (type == "02" || type == "03" || type == "04")
            {
                //如果是 推薦、活動、專欄 留言
                if (is_message)
                {
                    //推薦留言
                    if (type == "02")
                    {
                        table_name = "RECOMMEND_REPLY";
                        num_name = "RECOM_REPLY_NUM";
                    }
                    //活動
                    else if (type == "03")
                    {
                        //TODO 還沒有活動留言的表
                    }
                    else if (type == "04")
                    {
                        //TODO 還沒有專欄留言的表
                    }

                }
                //推薦、活動、專欄 只有管理員可以使用
                else
                {
                    //如果為系統管理員
                    if (is_sysop) 
                    {
                        return true;
                    }
                    else 
                    {
                        return false;
                    }
                }
            }
            #endregion
            try
            {
                strSql = @"SELECT COUNT(1) AS total
                           FROM {0}
                           WHERE MEM_ID = @mem_id AND {1} = @num ";
                //替換table and num
                strSql = string.Format(strSql, table_name, num_name);

                objParam = new
                {
                    mem_id = model.MEM_ID,
                    num = num
                };

                return ExecuteQuery<int>(strSql, objParam).FirstOrDefault() > 0 ? true :false ;
            }
            catch(Exception ex) 
            {
                LogSet.LogError(ex.ToString());
                return false;
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