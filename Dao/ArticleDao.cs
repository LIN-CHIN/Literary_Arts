using Literary_Arts.Models;
using Literary_Arts.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Dao
{
    public class ArticleDao : _Dao
    {

        /// <summary>
        /// 取得所有文章清單 
        /// </summary>
        /// <returns></returns>
        public IList<ArticleModel> GetArticleList()
        {
            try
            {
                strSql = @"SELECT    ARTI_NUM	            --文章編號		
		                            ,ARTI_HEAD              --文章標題
		                            ,ARTI_CONT              --文章內容
		                            ,ARTI_CLASS             --文章分類
		                            ,REPLY_COUNT            --留言數量
		                            ,LIKE_COUNT             --愛心數量
		                            ,MEM_ID                 --會員帳號
		                            ,MEM_NAME               --會員暱稱
                                    ,MEM_DISPLAY            --帳號+暱稱
		                            ,CRT_DATE               --發文時間
                        FROM VW_ARTICLE_LIST
                        ORDER BY CRT_DATE DESC ";

                return ExecuteQuery<ArticleModel>(strSql);
            }
            catch (Exception ex)
            {
                throw ex ;
            }
        }

        /// <summary>
        /// 根據文章編號 取得文章資訊
        /// </summary>
        /// <param name="artiNum"></param>
        /// <returns></returns>
        public ArticleModel ByArtiNumGetArticle(string arti_Num) {
            try
            {
                strSql = @"SELECT    ARTI_NUM	            --文章編號		
		                            ,ARTI_HEAD              --文章標題
		                            ,ARTI_CONT              --文章內容
		                            ,ARTI_CLASS             --文章分類
		                            ,REPLY_COUNT            --留言數量
		                            ,LIKE_COUNT             --愛心數量
		                            ,MEM_ID                 --會員帳號
		                            ,MEM_NAME               --會員暱稱
                                    ,MEM_DISPLAY            --帳號+暱稱
		                            ,CRT_DATE               --發文時間
                        FROM VW_ARTICLE_LIST
                        WHERE ARTI_NUM = @arti_num
                        ORDER BY CRT_DATE DESC ";

                objParam = new
                {
                    arti_Num = arti_Num
                };

                return ExecuteQuery<ArticleModel>(strSql, objParam).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根據類別種類取得文章清單
        /// </summary>
        /// <param name="arti_class"> 文章分類 
        ///      01 = '電影'
        ///      02 = '音樂'
        ///      03 = '書籍'
        ///      04 = '展覽'
        /// </param>
        /// <returns></returns>
        public IList<ArticleModel> ByClassTypeGetList(string arti_class) {
            try
            {
                strSql = @" SELECT   ARTI_NUM	            --文章編號		
		                            ,ARTI_HEAD              --文章標題
		                            ,ARTI_CONT              --文章內容
		                            ,ARTI_CLASS             --文章分類
		                            ,REPLY_COUNT            --留言數量
		                            ,LIKE_COUNT             --愛心數量
		                            ,MEM_ID                 --會員帳號
		                            ,MEM_NAME               --會員暱稱
                                    ,MEM_DISPLAY            --帳號+暱稱
		                            ,CRT_DATE               --發文時間
                            FROM VW_ARTICLE_LIST
                            WHERE ARTI_CLASS = @arti_class
                            ORDER BY CRT_DATE DESC";

                objParam = new
                {
                    arti_class = arti_class
                };

                return ExecuteQuery<ArticleModel>(strSql, objParam);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得留言資訊
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public IList<ArticleModel> GetReplyData(string num) {
            try
            {
                strSql = @" SELECT 
                                    vw.ARTI_NUM,                                            --文章編號
		                            ar.ARTI_REPLY_NUM										--文章留言編號
	                               ,ar.MEM_ID + '-' + am.MEM_NAME AS MEM_DISPLAY			--留言者帳號
	                               ,CAST(ARTI_REPLY_CONT AS varchar) AS ARTI_REPLY_CONT		--文章留言內容
	                               ,count(ar.ARTI_REPLY_NUM) AS REPLY_LIKE_COUNT			--文章留言愛心數量
	                               ,ar.CRT_DATE												--留言時間
                            FROM VW_ARTICLE_LIST AS vw                                      --文章view表
                            INNER JOIN ARTICLE_REPLY AS ar                                  --文章留言表
	                            ON vw.ARTI_NUM = ar.ARTI_NUM
                            INNER JOIN ARTICLE_REPLY_LIKE AS r_like                         --文章留言愛心表
	                            ON ar.ARTI_REPLY_NUM = r_like.ARTI_REPLY_NUM
                            INNER JOIN ART_MEMBER AS am                                     --會員資料表
	                            ON ar.MEM_ID = am.MEM_ID
                            WHERE VW.ARTI_NUM = @num
                            GROUP BY 
                                    vw.ARTI_NUM
		                           ,ar.ARTI_REPLY_NUM
	                               ,ar.MEM_ID
	                               ,am.MEM_NAME
	                               ,CAST(ARTI_REPLY_CONT AS varchar)
	                               ,ar.CRT_DATE
                            ORDER BY count(ar.ARTI_REPLY_NUM) DESC";

                objParam = new
                {
                    num = num
                };

                return ExecuteQuery<ArticleModel>(strSql, objParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 編輯文章功能
        /// </summary>
        /// <param name="arti_num"></param>
        /// <returns></returns>
        public RtnResultModel UpdateArticle(ArticleModel model) {
            try
            {
                strSql = @"UPDATE ARTICLE 
                           SET ARTI_HEAD = @arti_head
                               , ARTI_CONT = @arti_cont 
                               , ARTI_CLASS = @arti_class
                               , MDF_DATE = GETDATE() 
                               , MDF_MEMID = @mdf_memid
                               , MDF_MEMNAME = @mdf_memname 
                           WHERE ARTI_NUM = @arti_num";
                objParam = new
                {
                    arti_head = model.ARTI_HEAD,
                    arti_cont = model.ARTI_CONT,
                    arti_class = model.ARTI_CLASS,
                    mdf_memid = model.MEM_ID,
                    mdf_memname = model.MEM_NAME,
                    arti_num = model.ARTI_NUM
                };
                return new RtnResultModel(true, SysSet.GetParamItemValue("SYS_MESSAGE", "update_true"));
            }
            catch(Exception ex)
            {
                return new RtnResultModel(true, SysSet.GetParamItemValue("SYS_MESSAGE", "update_false"));
            }
        }

        #region 刪除
        /// <summary>
        /// 刪除文章
        /// </summary>
        /// <param name="arti_num"></param>
        /// <returns></returns>
        public RtnResultModel DeleteArticle(string arti_num) {
            try
            {                               
                strSql = @"DELETE FROM ARTICLE WHERE arti_num = @arti_num";
                objParam = new
                {
                    arti_num = arti_num
                };

                ExecuteCommand(strSql, objParam);

                return new RtnResultModel(true, SysSet.GetParamItemValue("SYS_MESSAGE", "del_true"));
            }
            catch(Exception ex)
            {
                return new RtnResultModel(false, SysSet.GetParamItemValue("SYS_MESSAGE", "del_false"));
            }
        }

        /// <summary>
        /// 刪除留言
        /// </summary>
        /// <param name="arti_num"></param>
        /// <returns></returns>
        public RtnResultModel DeleteReply(string arti_reply_num)
        {
            try
            {
                strSql = @"DELETE FROM ARTICLE_REPLY WHERE arti_reply_num = @arti_reply_num";
                objParam = new
                {
                    arti_reply_num = arti_reply_num
                };

                ExecuteCommand(strSql, objParam);

                return new RtnResultModel(true, SysSet.GetParamItemValue("SYS_MESSAGE", "del_true"));
            }
            catch (Exception ex)
            {
                return new RtnResultModel(false, SysSet.GetParamItemValue("SYS_MESSAGE", "del_false"));
            }
        }
        #endregion
    }
}