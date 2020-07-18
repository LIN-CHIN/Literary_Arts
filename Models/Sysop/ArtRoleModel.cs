using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Models.Sysop
{
    public class ArtRoleModel
    {
        /// <summary>
        /// 角色代號
        /// </summary>
        public string ROLE_ID { get; set; }
        
        /// <summary>
        /// 角色中文名稱
        /// </summary>
        public string ROLE_NAME { get; set; }

        /// <summary>
        /// 是否停用
        /// </summary>
        public string IS_STOP { get; set; }
        public DateTime? CRT_DATE { get; set; }
        public string CRT_MEMID { get; set; }
        public DateTime? MDF_DATE { get; set; }
        public string MDF_MEMID { get; set; }

    }
}