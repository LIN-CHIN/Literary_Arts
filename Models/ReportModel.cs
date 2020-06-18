using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Models
{
    public class ReportModel : _Model
    {
        public string REPORT_NUM { get; set; }
        public string MEM_ID { get; set; }
        public string ARTI_NUM { get; set; }
        public string ARTI_MESS_NUM { get; set; }
        public string RECOM_MESS_NUM { get; set; }
        public string REPORT_REAS { get; set; }

    }
}