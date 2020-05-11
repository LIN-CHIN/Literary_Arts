using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Models
{
    /// <summary>
    /// 會員Momdel
    /// </summary>
    public class ArtMemberModel
    {
        public string MEM_ID { get; set; }
        public string LINE_ID { get; set; }
        public string MEM_PASS { get; set; }
        public DateTime? MEM_BIRTH { get; set; }
        public string MEM_NAME { get; set; }
        public string MEM_MAIL { get; set; }
        public string MEM_GENDER { get; set; }
        public string MEM_NOTI { get; set; }
        public string MEM_ADDR { get; set; }
        public string IS_STOP { get; set; }

    }
}