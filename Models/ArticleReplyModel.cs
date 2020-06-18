using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Models
{
    public class ArticleReplyModel : _Model
    {
        public string ARTI_REPLY_NUM { get; set; }
        public string ARTI_NUM { get; set; }
        public string MEM_ID { get; set; }
        public string ARTI_REPLY_CONT { get; set; }
        public string POSITIVE_WORDS { get; set; }
        public string NEGATIVE_WORDS { get; set; }
        public string SWEAR_WORDS { get; set; }
        public string ANALYZE_SCORE { get; set; }
        public string SCORE2 { get; set; }

    }
}