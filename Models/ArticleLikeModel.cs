using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Models
{
    public class ArticleLikeModel
    {
        public string ARTI_LIKE_NUM { get; set; }
        public string MEM_ID { get; set; }
        public string ARTI_NUM { get; set; }
        public DateTime? ARTI_LIKE_DATETIME { get; set; }
    }
}