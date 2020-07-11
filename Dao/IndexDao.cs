using Literary_Arts.Models;
using Literary_Arts.Web_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Dao
{
    public class IndexDao : _Dao
    {
        public IndexDao(MemberUserModel loginUser) : base(loginUser) { }

        /// <summary>
        /// 取得首頁四大推薦的資料
        /// 待修改 : 還沒加上 取四篇的邏輯
        /// </summary>
        /// <returns></returns>
        public IList<IndexModel> GetRecomData() {
            try
            {
                strSql = @"SELECT RECOM_NUM             --推薦編號
	                             ,RECOM_HEAD            --推薦標題
	                             ,RECOM_CONT            --推薦內容
	                             ,RECOM_CLASS           --推薦分類
	                             ,POSITIVE_WORDS        --正向詞數
	                             ,NEGATIVE_WORDS        --負向詞數
	                             ,ANALYZE_SCORE         --情感分析值
	                             ,SCORE2                --情感分析值2
	                             ,CRT_DATE              --新建的日期
	                             ,CRT_MEMID             --新建的會員帳號
	                             ,CRT_MEMNAME           --新建的會員暱稱
	                             ,MDF_DATE              --修改的日期
	                             ,MDF_MEMID             --修改的會員帳號
	                             ,MDF_MEMNAME           --修改的會員暱稱
                           FROM Recommend
                           ";

                return ExecuteQuery<IndexModel>(strSql);
            }
            catch (Exception ex)
            {
                LogSet.LogError(ex.ToString());
                return new List<IndexModel>();
            }
        }

        /// <summary>
        /// 取得熱門文章
        /// </summary>
        /// <returns></returns>
        public IList<IndexModel> GetHotArticle() {
            try
            {
                strSql = @"SELECT  ARTI_NUM					--文章編號
	                          ,ARTI_HEAD				--文章標題
	                          ,ARTI_CONT				--文章內容
	                          ,ARTI_CLASS				--文章分類
	                          ,CRT_DATE					--發文時間
	                          ,MEM_ID					--會員帳號
	                          ,MEM_NAME					--會員暱稱
                        FROM VW_HOT_ARTICLE
                        ORDER BY LIKE_COUNT DESC ";

                return ExecuteQuery<IndexModel>(strSql);
            }
            catch (Exception ex) 
            {
                LogSet.LogError(ex.ToString());
                return new List<IndexModel>();
            }    
        }
    }
}