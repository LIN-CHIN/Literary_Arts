using Literary_Arts.Dao.Sysop;
using Literary_Arts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Literary_Arts.Web_Common
{
    /// <summary>
    /// 自訂驗證 : 根據角色權限來判斷當前的Action是否能夠讓使用者進入
    /// 使用方法 : 在要Controller前 +[CustomAuthorize("CreateRecommend")]  
    ///            "CreateRecommend" = DB set_function的 function_id
    /// </summary>
    public class CustomAuthorize : AuthorizeAttribute
    {
        private IList<SetFunctionModel> SetFunctionModel { get; set; }
        private string loginUser { get; set; }

        private string fn_name { get; set; }

        public CustomAuthorize(string fn_name)
        {
            this.fn_name = HttpUtility.HtmlEncode(fn_name);
        }

        /// <summary>
        /// 自訂授權檢查點
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //取得登入者ID
            loginUser = httpContext.User.Identity.Name;

            if (httpContext == null || string.IsNullOrWhiteSpace(loginUser))
            {
                return false;
            }

            //判斷是否有權利使用function
            using (SetFunctionDao dao = new SetFunctionDao())
            {
                SetFunctionModel = dao.GetFunctionDataByID(HttpUtility.HtmlEncode(loginUser));
                if (SetFunctionModel.Where(d => d.FUNCTION_ID == this.fn_name).Count() == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}