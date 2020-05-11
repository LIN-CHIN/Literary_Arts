﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Models
{
    public class RecommendModel
    {
        public string RECOM_NUM { get; set; }
        public DateTime? RECOM_DATETIME { get; set; }
        public string RECOM_HEAD { get; set; }
        public string RECOM_CONT { get; set; }
        public string RECOM_CLASS { get; set; }
        public string POSITIVE_WORDS { get; set; }
        public string NEGATIVE_WORDS { get; set; }
        public string ANALYZE_SCORE { get; set; }
        public string SCORE2 { get; set; }
    }
}