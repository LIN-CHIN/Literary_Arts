using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Models.System
{
    /// <summary>
    /// 回傳結果用的model
    /// </summary>
    public class RtnResultModel
    {   
        public bool success { get; set; }

        public string _message { get; set; }
        public string message
        {
            get
            {
                return HttpUtility.HtmlEncode(_message);
            }
            set
            {
                _message = value;
            }
        }

        public RtnResultModel(bool success, string message) {
            this.success = success;
            this.message = message;
        } 
    }
}