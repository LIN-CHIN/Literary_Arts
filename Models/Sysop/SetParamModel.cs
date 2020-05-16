using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Models.Sysop
{
    public class SetParamModel
    {
        public string FUNCTION_ID { get; set; }
        public string FUNCTION_NAME { get; set; }
        public string FUNCTION_URL { get; set; }
        public string PARENT_ID { get; set; }
        public string SORT_ID { get; set; }
        public string CLICK_FN { get; set; }
        public string IS_STOP { get; set; }
        public DateTime? CRT_DATE { get; set; }
        public string CRT_MEMID { get; set; }
        public string CRT_MEMNAME { get; set; }
        public DateTime? MDF_DATE { get; set; }
        public string MDF_MEMID { get; set; }
        public string MDF_MEMNAME { get; set; }
    }
}