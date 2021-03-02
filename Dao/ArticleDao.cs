using Literary_Arts.Models;
using Literary_Arts.Models.System;
using Literary_Arts.Web_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Dao
{
    public class ArticleDao : _Dao
    {
        public ArticleDao(MemberUserModel loginUser) : base(loginUser) { }

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
                LogSet.LogError(ex.ToString());
                return new List<ArticleModel>();
            }
        }

        /// <summary>
        /// 取得使用者有按過讚的文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<ArticleModel> ByIdGetLikeList(string id)
        {
            try
            {
                strSql = @" SELECT ARTI_NUM
                            FROM ARTICLE_LIKE
                            WHERE MEM_ID = @mem_id ";

                objParam = new
                {
                    mem_id = HttpUtility.HtmlEncode(id)
                };

                return ExecuteQuery<ArticleModel>(strSql, objParam);
            }
            catch (Exception ex)
            {
                LogSet.LogError(ex.ToString());
                return new List<ArticleModel>();
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
                    arti_Num = HttpUtility.HtmlEncode(arti_Num)
                };

                return ExecuteQuery<ArticleModel>(strSql, objParam).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogSet.LogError(ex.ToString());
                return new ArticleModel();
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
                    arti_class = HttpUtility.HtmlEncode(arti_class)
                };

                return ExecuteQuery<ArticleModel>(strSql, objParam);
            }
            catch (Exception ex)
            {
                LogSet.LogError(ex.ToString());
                return new List<ArticleModel>();
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
                    num = HttpUtility.HtmlEncode(num)
                };

                return ExecuteQuery<ArticleModel>(strSql, objParam);
            }
            catch (Exception ex)
            {
                LogSet.LogError(ex.ToString());
                return new List<ArticleModel>();
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
                    arti_cont = HttpUtility.HtmlDecode(model.ARTI_CONT),
                    arti_class = model.ARTI_CLASS,
                    mdf_memid = model.MEM_ID,
                    mdf_memname = model.MEM_NAME,
                    arti_num = model.ARTI_NUM
                };
                
                return ExecuteCommand(strSql, objParam) ? new RtnResultModel(true, SysSet.GetParamItemValue("SYS_MESSAGE", "update_true")): new RtnResultModel(true, SysSet.GetParamItemValue("SYS_MESSAGE", "update_true"));
            }
            catch (Exception ex)
            {
                LogSet.LogError(ex.ToString());
                return new RtnResultModel(true, SysSet.GetParamItemValue("SYS_MESSAGE", "update_false"));
            }
        }

        /// <summary>
        /// 編輯文章留言功能
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public RtnResultModel UpdateArticleReply(ArticleModel model)
        {
            try
            {
                strSql = @"UPDATE ARTICLE_REPLY 
                                SET ARTI_REPLY_CONT= ''         --留言內容
                                  , MDF_DATE= GETDATE()         --修改日期
                           WHERE ARTI_REPLY_NUM = @arti_reply_num";
                objParam = new
                {
                    arti_reply_num = model.ARTI_REPLY_NUM
                };
                return new RtnResultModel(true, SysSet.GetParamItemValue("SYS_MESSAGE", "update_true"));
            }
            catch (Exception ex)
            {
                LogSet.LogError(ex.ToString());
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
                    arti_num = HttpUtility.HtmlEncode(arti_num)
                };

                ExecuteCommand(strSql, objParam);

                return new RtnResultModel(true, SysSet.GetParamItemValue("SYS_MESSAGE", "del_true"));
            }
            catch (Exception ex)
            {
                LogSet.LogError(ex.ToString());
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
                    arti_reply_num = HttpUtility.HtmlEncode(arti_reply_num)
                };

                ExecuteCommand(strSql, objParam);

                return new RtnResultModel(true, SysSet.GetParamItemValue("SYS_MESSAGE", "del_true"));
            }
            catch (Exception ex)
            {
                LogSet.LogError(ex.ToString());
                return new RtnResultModel(false, SysSet.GetParamItemValue("SYS_MESSAGE", "del_false"));
            }
        }
        #endregion

        /// <summary>
        /// 判斷有沒有按過愛心
        /// </summary>
        /// <param name="num">編號</param>
        /// <param name="id">會員帳號</param>
        /// <param name="IsReply">用來判斷是不是留言愛心</param>
        /// <returns></returns>
        public bool IsClickLike(string num,string id, bool IsReply) {
            try
            {
                strSql = @" SELECT COUNT(1)
                                FROM {0}
                                WHERE {1} = @num AND MEM_ID = @mem_id";
                //文章
                if (!IsReply)
                {
                    strSql = string.Format(strSql, "ARTICLE_LIKE", "ARTI_NUM");
                }
                else {
                    strSql = string.Format(strSql, "ARTICLE_REPLY_LIKE", "ARTI_REPLY_NUM");
                }

                objParam = new
                {
                    num = HttpUtility.HtmlEncode(num),
                    mem_id = HttpUtility.HtmlEncode(id)
                };

                return ExecuteQuery<int>(strSql, objParam).FirstOrDefault() >= 1  ? true : false;
            }
            catch (Exception ex) 
            {
                LogSet.LogError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 新增愛心
        /// </summary>
        /// <param name="num"></param>
        /// <param name="id"></param>
        /// <param name="IsReply">判斷是不是留言</param>
        /// <returns></returns>
        public bool addLike(string num, string id, bool IsReply) {
            try
            {
               
                strSql = @"INSERT INTO {0}
                               (MEM_ID, 
                                {1},
                                CRT_DATE)
                            VALUES
                               ( @mem_id,
                                 @num,
                                 GETDATE() )";

                //文章
                if (!IsReply)
                {
                    strSql = string.Format(strSql, "ARTICLE_LIKE", "ARTI_NUM");
                }
                else
                {
                    strSql = string.Format(strSql, "ARTICLE_REPLY_LIKE", "ARTI_REPLY_NUM");
                }

                objParam = new
                {
                    num = HttpUtility.HtmlEncode(num),
                    mem_id = HttpUtility.HtmlEncode(id)
                };


                return ExecuteCommand(strSql, objParam);
            }
            catch (Exception ex)
            {
                LogSet.LogError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 刪除愛心
        /// </summary>
        /// <param name="num"></param>
        /// <param name="id"></param>
        /// <param name="IsReply"></param>
        /// <returns></returns>
        public bool delLike(string num, string id, bool IsReply)
        {
            try
            {

                strSql = @"DELETE FROM {0} WHERE MEM_ID = @mem_id AND {1} = @num";

                //文章
                if (!IsReply)
                {
                    strSql = string.Format(strSql, "ARTICLE_LIKE", "ARTI_NUM");
                }
                else
                {
                    strSql = string.Format(strSql, "ARTICLE_REPLY_LIKE", "ARTI_REPLY_NUM");
                }

                objParam = new
                {
                    num = HttpUtility.HtmlEncode(num),
                    mem_id = HttpUtility.HtmlEncode(id)
                };


                return ExecuteCommand(strSql, objParam);
            }
            catch (Exception ex)
            {
                LogSet.LogError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 取得愛心數量
        /// </summary>
        /// <param name="num"></param>
        /// <param name="IsReply"></param>
        /// <returns></returns>
        public int GetLikeCount(string num, bool IsReply)
        {
            try
            {
                strSql = @" SELECT COUNT({1}) AS likeCount
                            FROM {0}         
                            WHERE ARTI_NUM = @num 
                            ";
                //文章
                if (!IsReply)
                {
                    strSql = string.Format(strSql, "ARTICLE_LIKE", "ARTI_NUM");
                }
                else
                {
                    strSql = string.Format(strSql, "ARTICLE_REPLY_LIKE", "ARTI_REPLY_NUM");
                }

                objParam = new
                {
                    num = HttpUtility.HtmlEncode(num)
                };

                return ExecuteQuery<int>(strSql, objParam).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogSet.LogError(ex.ToString());
                return 0;
            }

        }

        /// <summary>
        /// 取得所有文章的個別愛心數量
        /// </summary>
        /// <param name="num"></param>
        /// <param name="IsReply"></param>
        /// <returns></returns>
        public IList<ArticleModel> GetLikeCount()
        {
            try
            {
                strSql = @" SELECT ARTI_NUM, LIKE_COUNT
                            FROM VW_ARTICLE_LIST
                            ORDER BY ARTI_NUM DESC ";


                return ExecuteQuery<ArticleModel>(strSql, objParam);
            }
            catch (Exception ex)
            {
                LogSet.LogError(ex.ToString());
                return new List<ArticleModel>();
            }

        }

        /// <summary>
        /// 判斷有沒有按過收藏
        /// </summary>
        /// <param name="num">編號</param>
        /// <param name="id">會員帳號</param>
        /// <param name="IsReply">用來判斷是不是留言愛心</param>
        /// <returns></returns>
        public bool IsClickCollection(string num, string id)
        {
            try
            {
                strSql = @" SELECT COUNT(1)
                                FROM MEMBER_COLLECTION
                                WHERE ARTI_NUM = @num AND MEM_ID = @mem_id";

                objParam = new
                {
                    num = HttpUtility.HtmlEncode(num),
                    mem_id = HttpUtility.HtmlEncode(id)
                };

                return ExecuteQuery<int>(strSql, objParam).FirstOrDefault() >= 1 ? true : false;
            }
            catch (Exception ex)
            {
                LogSet.LogError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 新增收藏
        /// </summary>
        /// <param name="num"></param>
        /// <param name="id"></param>
        /// <param name="IsReply">判斷是不是留言</param>
        /// <returns></returns>
        public bool addCollection(string num, string id)
        {
            try
            {

                strSql = @"INSERT INTO MEMBER_COLLECTION
                               (MEM_ID, 
                                ARTI_NUM,
                                CRT_DATE)
                            VALUES
                               ( @mem_id,
                                 @num,
                                 GETDATE() )";

                objParam = new
                {
                    num = HttpUtility.HtmlEncode(num),
                    mem_id = HttpUtility.HtmlEncode(id)
                };


                return ExecuteCommand(strSql, objParam);
            }
            catch (Exception ex)
            {
                LogSet.LogError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 刪除收藏
        /// </summary>
        /// <param name="num"></param>
        /// <param name="id"></param>
        /// <param name="IsReply"></param>
        /// <returns></returns>
        public bool delCollection(string num, string id)
        {
            try
            {

                strSql = @"DELETE FROM MEMBER_COLLECTION WHERE MEM_ID = @mem_id AND ARTI_NUM = @num";

                objParam = new
                {
                    num = HttpUtility.HtmlEncode(num),
                    mem_id = HttpUtility.HtmlEncode(id)
                };


                return ExecuteCommand(strSql, objParam);
            }
            catch (Exception ex)
            {
                LogSet.LogError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 取得使用者有按過的收藏
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<ArticleModel> ByIdGetCollList(string id)
        {
            try
            {
                strSql = @" SELECT ARTI_NUM
                            FROM MEMBER_COLLECTION
                            WHERE MEM_ID = @mem_id ";

                objParam = new
                {
                    mem_id = HttpUtility.HtmlEncode(id)
                };

                return ExecuteQuery<ArticleModel>(strSql, objParam);
            }
            catch (Exception ex)
            {
                LogSet.LogError(ex.ToString());
                return new List<ArticleModel>();
            }
        }



    }
}