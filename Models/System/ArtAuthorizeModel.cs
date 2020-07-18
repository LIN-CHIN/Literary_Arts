using Literary_Arts.Controllers;
using Literary_Arts.Controllers.Sysop;
using Literary_Arts.Dao.Sysop;
using NLog;
using NLog.LayoutRenderers.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
namespace Literary_Arts.Models.System
{
    /// <summary>
    /// 自訂驗證 : 根據
    /// 使用方法 : 在要Controller前 +[ArtAuthorizeModel("CreateRecommend")]
    /// </summary>
    public class ArtAuthorizeModel : AuthorizeAttribute
    {
        public IList<SetFunctionModel> SetFunctionModel { get; set; }
        public string loginUser { get; set; }
       
        public string fn_name { get; set; }

        public ArtAuthorizeModel(string fn_name ) {
            this.fn_name = fn_name;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //取得登入者ID
            loginUser = httpContext.User.Identity.Name; 

            if (httpContext == null) {
                return false;
            }

            //判斷是否有權利使用function
            using (SetFunctionDao dao = new SetFunctionDao()) 
            {
                SetFunctionModel = dao.GetFunctionDataByID(HttpUtility.HtmlEncode(loginUser));
                if(SetFunctionModel.Where(d => d.FUNCTION_ID == this.fn_name).Count() == 0){
                    return false;
                }
            }

            return true; 
        }

    }
}