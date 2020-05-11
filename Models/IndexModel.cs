using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Models
{
    public class IndexModel
    {
        #region 文章 model (ArticleModel)
        public string ARTI_NUM { get; set; }
        public string MEM_ID { get; set; }
        public DateTime? ARTI_DATETIME { get; set; }
        public string ARTI_HEAD { get; set; }
        public string ARTI_CONT { get; set; }
        public string ARTI_CLASS { get; set; }
        #endregion

        #region 推薦model (RecommendModel)
        public string RECOM_NUM { get; set; }
        public DateTime? RECOM_DATETIME { get; set; }
        public string RECOM_HEAD { get; set; }
        public string RECOM_CONT { get; set; }
        public string RECOM_CLASS { get; set; }

        #endregion

        #region 重複
        public string POSITIVE_WORDS { get; set; }
        public string NEGATIVE_WORDS { get; set; }
        public string ANALYZE_SCORE { get; set; }
        public string SCORE2 { get; set; }

        public DateTime? CRT_DATE { get; set; }
        #endregion

    }
}