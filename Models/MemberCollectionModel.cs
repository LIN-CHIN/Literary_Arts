using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Models
{
    public class MemberCollectionModel
    {
        public string COLL_NUM { get; set; }
        public string MEM_ID { get; set; }
        public string RECOM_NUM { get; set; }
        public string ARTI_NUM { get; set; }
        public DateTime? COLL_DATETIME { get; set; }

    }
}