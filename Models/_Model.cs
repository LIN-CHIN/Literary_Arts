using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Models
{         
    /// <summary>
    /// 共用的model屬性放這邊
    /// </summary>
    public class _Model
    {
        /// <summary>
        /// TAG中文
        /// </summary>
        public string TAG_NAME { get; set; }

        public DateTime? CRT_DATE { get; set; }

        public string CRT_MEMID { get; set; }

        public string CRT_MEMNAME { get; set; }
        public DateTime? MDF_DATE { get; set; }

        public string MDF_MEMID { get; set; }

        public string MDF_MEMNAME { get; set; }
    }
}