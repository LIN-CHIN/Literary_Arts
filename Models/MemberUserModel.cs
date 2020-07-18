using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Models
{
    public class MemberUserModel : ICloneable
    {
        public Object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// 會員帳號
        /// </summary>
        public string MEM_ID { get; set; }

        /// <summary>
        /// 會員密碼
        /// </summary>
        public string MEM_PASS { get; set; }

        /// <summary>
        /// LINE ID
        /// </summary>
        public string LINE_ID { get; set; }

        /// <summary>
        /// 會員暱稱
        /// </summary>
        public string MEM_NAME { get; set; }

        /// <summary>
        /// 會員生日
        /// </summary>
        public string MEM_BIRTH { get; set; }

        /// <summary>
        /// 會員 E-Mail
        /// </summary>
        public string MEM_MAIL { get; set; }

        /// <summary>
        /// 會員性別
        /// </summary>
        public string MEM_GENDER { get; set; }

        /// <summary>
        /// 會員通知
        /// </summary>
        public string MEM_NOTI { get; set; }

        /// <summary>
        /// 會員地址
        /// </summary>
        public string MEM_ADDR { get; set; }

        /// <summary>
        /// 是否停用
        /// </summary>
        public string IS_STOP { get; set; }

        /// <summary>
        /// 使用者IP
        /// </summary>
        public string USER_IP { get; set; }

        /// <summary>
        /// 創建日期
        /// </summary>
        public DateTime CRT_DATE { get; set; }

        /// <summary>
        /// 創建帳號
        /// </summary>
        public string CRT_MEMID { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime? MDF_DATE { get; set; }

        /// <summary>
        /// 修改帳號
        /// </summary>
        public string MDF_MEMID { get; set; }
 
    }
}