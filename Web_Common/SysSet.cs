using Literary_Arts.Controllers;
using Literary_Arts.Dao.Sysop;
using Literary_Arts.Models.Sysop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Literary_Arts
{
    public class SysSet 
    {
        /// <summary>
        /// 取得參數檔(set_param_item) 的 value 
        /// </summary>
        /// <param name="set_item"> 項目(大項)代號 </param>
        /// <param name="set_type"> 項目(細項)代號 </param>
        /// <returns></returns>
        public static string GetParamItemValue(string set_item, string set_type)
        {
            using (SetParamDao dao = new SetParamDao()) {
                return dao.GetParamItemValue(HttpUtility.HtmlEncode(set_item), HttpUtility.HtmlEncode(set_type));
            }
        }

        /// <summary>
        /// 取得 參數檔(細項) 的 type (代號)
        /// </summary>
        /// <param name="set_item"> 項目(大)代號 </param>
        /// <param name="set_value"> 項目(細項) 值 </param>
        /// <returns></returns>
        public static string GetParamItemType(string set_item, string set_value)
        {
            using (SetParamDao dao = new SetParamDao())
            {
                return dao.GetParamItemType(HttpUtility.HtmlEncode(set_item), HttpUtility.HtmlEncode(set_value));
            }
        }

        /// <summary>
        /// 取得參數檔項目
        /// </summary>
        /// <param name="set_item"> 項目代號 </param>
        /// <returns></returns>
        public static IList<SetParamItemModel> GetParamItem(string set_item)
        {
            using (SetParamDao dao = new SetParamDao())
            {
                return dao.GetParamItem(HttpUtility.HtmlEncode(set_item));
            }
        }
    }
}