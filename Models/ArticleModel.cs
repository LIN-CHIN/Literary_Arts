using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Models
{
    public class ArticleModel : _Model
    {
        /// <summary>
        /// 文章編號
        /// </summary>
        public string ARTI_NUM { get; set; }

        /// <summary>
        /// 會員帳號
        /// </summary>
        public string MEM_ID { get; set; }

        /// <summary>
        /// 會員暱稱
        /// </summary>
        public string MEM_NAME { get; set; }

        /// <summary>
        /// 會員顯示方式 = MEM_ID-MEM_NAME
        /// </summary>
        public string MEM_DISPLAY { get; set; }

        /// <summary>
        /// 文章標題
        /// </summary>
        public string ARTI_HEAD { get; set; }

        /// <summary>
        /// 文章內容
        /// </summary>
        public string ARTI_CONT { get; set; }

        /// <summary>
        /// 文章分類
        /// </summary>
        public string ARTI_CLASS { get; set; }

        /// <summary>
        /// 愛心數量
        /// </summary>
        public int? LIKE_COUNT { get; set; }

        /// <summary>
        /// 留言數量
        /// </summary>
        public int? REPLY_COUNT { get; set; }

        /// <summary>
        /// 留言內容
        /// </summary>
        public string ARTI_REPLY_CONT { get; set; }

        /// <summary>
        /// 留言編號
        /// </summary>
        public string ARTI_REPLY_NUM { get; set; }

        /// <summary>
        /// 留言愛心數量
        /// </summary>
        public int? REPLY_LIKE_COUNT { get; set; }



        public string POSITIVE_WORDS { get; set; }
        public string NEGATIVE_WORDS { get; set; }
        public string SWEAR_WORDS { get; set; }
        public string ANALYZE_SCORE { get; set; }
        public string SCORE2 { get; set; }
    }
}