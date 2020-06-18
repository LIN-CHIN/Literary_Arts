using Literary_Arts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Dao
{
    public class RecommendDao : _Dao
    {
        public IList<RecommendModel> GetRecommendList() 
        {
            try
            {
                strSql = @"SELECT    RECOM_NUM	            --推薦編號		
		                            ,RECOM_HEAD             --推薦標題
		                            ,RECOM_CONT             --推薦內容
		                            ,RECOM_CLASS            --推薦分類
		                            ,REPLY_COUNT            --留言數量
		                            ,LIKE_COUNT             --愛心數量
		                            ,CRT_DATE               --發文時間
                        FROM VW_RECOMMEND_LIST
                        ORDER BY CRT_DATE DESC ";

                return ExecuteQuery<RecommendModel>(strSql);
            }
            catch (Exception ex)
            {
                throw ex ;
            }
        }

        /// <summary>
        /// 根據推薦編號 取得推薦資訊
        /// </summary>
        /// <param name="artiNum"></param>
        /// <returns></returns>
        public RecommendModel ByRecomNumGetRecommend(string recom_Num)
        {
            try
            {
                strSql = @"SELECT    RECOM_NUM	            --推薦編號		
		                            ,RECOM_HEAD             --推薦標題
		                            ,RECOM_CONT             --推薦內容
		                            ,RECOM_CLASS            --推薦分類
		                            ,REPLY_COUNT            --留言數量
		                            ,LIKE_COUNT             --愛心數量
		                            ,CRT_DATE               --發文時間
                          FROM VW_RECOMMEND_LIST
                          WHERE RECOM_NUM = @recom_Num
                          ORDER BY CRT_DATE DESC ";

                objParam = new
                {
                    recom_Num = recom_Num
                };

                return ExecuteQuery<RecommendModel>(strSql, objParam).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}