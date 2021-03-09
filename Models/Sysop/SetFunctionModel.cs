using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Models
{
    public class SetFunctionModel
    {
        /// <summary>
        /// 功能的代號
        /// </summary>
        public string FUNCTION_ID { get; set; }

        /// <summary>
        /// 功能中文
        /// </summary>
        public string FUNCTION_NAME { get; set; }

        /// <summary>
        /// 功能 URL
        /// </summary>
        public string FUNCTION_URL { get; set; }

        public string CONTROLLER { get; set; }
        public string ACTION { get; set; }

        /// <summary>
        /// 父功能 代號
        /// </summary>
        public string PARENT_ID { get; set; }

        /// <summary>
        /// 排序 (大項目) 例如: 討論區 、 會員下拉清單 
        /// </summary>
        public string SORT_ID { get; set; }

        /// <summary>
        /// 排序 下拉清單 (小項目)  例如: 會員下拉清單 > 發文、登出 等功能
        /// </summary>
        public string SORT_ITEM_ID { get; set; }

        /// <summary>
        /// 點擊 Click() Function
        /// </summary>
        public string CLICK_FN { get; set; }

        /// <summary>
        /// 是否為特殊的function 例如 小幫手、通知 有自己的css要另外處理
        /// </summary>
        public string IS_SPECIAL_FN { get; set; }

        /// <summary>
        /// 是否停用  0 = 不停用  1 = 停用
        /// </summary>
        public string IS_STOP { get; set; }

        /// <summary>
        /// 會員帳號
        /// </summary>
        public string MEM_ID {get; set; }
    }
}