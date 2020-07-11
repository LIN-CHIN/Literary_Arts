using Literary_Arts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Literary_Arts.Controllers
{
    public class _Controller : Controller
    {
        protected MemberUserModel GetLoginUser()
        {
            return Session == null || Session["loginUser"] == null ? new MemberUserModel() : (MemberUserModel)((MemberUserModel)Session["loginUser"]).Clone();
        }



        /// <summary>
        /// 參數檔 set_type互相對照的中英文 轉換
        /// </summary>
        /// <param name="set_item"> 目前的set_item (大項)代號 </param>
        /// <param name="target_item"> 要轉換的 set_item (大項) 代號</param>
        /// <param name="set_value"> 目前的值 </param>
        /// <returns> set_value 轉換後的值 </returns>
        public string ReverseParamLanguage(string set_item, string target_item, string set_value) {
            //因參數檔的 LITERARY_CLASS 分類 有中文和英文 且 set_type 是互相對應的 
            //因DB內 文章 或 推薦 的View  class欄位為中文 而 從前端傳回的class為英文
            //為了方便將 'movie' 轉為 '電影' , 'music' 轉為 '音樂' 
            //先找到set_type (細項)代號 
            string class_type = SysSet.GetParamItemType(set_item, set_value);

            //轉換為中文
            set_value = SysSet.GetParamItemValue(target_item, class_type);

            return set_value;
        }    


    }
}