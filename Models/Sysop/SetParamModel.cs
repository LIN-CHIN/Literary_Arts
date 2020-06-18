using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts.Models.Sysop
{
    /// <summary>
    /// 參數檔(細項)
    /// </summary>
    public class SetParamItemModel
    {
        /// <summary>
        /// SET_PARAM(大項) 的代號
        /// </summary>
        public string SET_ITEM { get; set; }

        /// <summary>
        /// 項目種類、編號
        /// </summary>
        public string SET_TYPE { get; }

        /// <summary>
        /// 項目的值
        /// </summary>
        public string SET_VALUE { get; }

        /// <summary>
        /// 說明註解
        /// </summary>
        public string MEMO { get; }

        /// <summary>
        /// 排序
        /// </summary>
        public string SORT { get; }

        /// <summary>
        /// 是否停用 0 = 不停用 
        ///          1 = 停用
        /// </summary>
        public string IS_STOP { get; }
        public string CRT_DATE { get; }
        public string CRT_MEMID { get; }
        public string CRT_MEMNAME { get; }
        public string MDF_DATE { get; }
        public string MDF_MEMID { get; }
        public string MDF_MEMNAME { get; }
    }
}