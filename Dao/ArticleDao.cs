using Literary_Arts.Models;
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
    }
}