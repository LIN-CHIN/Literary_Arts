using Literary_Arts.Dao;
using Literary_Arts.Models;
using Literary_Arts.Models.System;
using NLog.LayoutRenderers.Wrappers;
using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Literary_Arts.Controllers
{
    public class MemberController : _Controller
    {
        /// <summary>
        /// 登入頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated) {
                return Redirect(FormsAuthentication.DefaultUrl);
            }
            return View();
        }

        /// <summary>
        /// 觸發登入
        /// </summary>
        /// <param name="MEM_ID"></param>
        /// <param name="MEM_PASS"></param>
        /// <returns></returns>
        /// TODO: 1. 可以將 帳號檢查 做成function  
        ///       2. 可以將 設定session 也做成function
        ///  目的 :  讓controller 乾淨好讀
        [HttpPost,ValidateAntiForgeryToken]
        public JsonResult Login(string MEM_ID, string MEM_PASS) 
        {
            using (MemberDao dao = new MemberDao(GetLoginUser())) 
            {
                //檢查是否為會員
                MemberUserModel model = dao.CheckLoginUserID(HttpUtility.HtmlEncode(MEM_ID));
                
                //如果沒找到帳號資料
                if (string.IsNullOrEmpty(model.MEM_ID))
                {
                    return Json(new RtnResultModel(false, SysSet.GetParamItemValue("SYS_MESSAGE", "login_id_error")));
                }

                //TODO: 目前想法是: 不想將密碼 SELECT 出來 所以犧牲在一次 執行SQL的時間 來判斷密碼是否正確
                //與 直接SELECT 密碼出來 和變數比較 兩種方法可以再思考 選擇哪種較好
                model = dao.CheckLoginUserPass(HttpUtility.HtmlEncode(MEM_ID), HttpUtility.HtmlEncode(MEM_PASS));

                //如果密碼錯誤
                if (string.IsNullOrEmpty(model.MEM_ID))
                {
                    return Json(new RtnResultModel(false, SysSet.GetParamItemValue("SYS_MESSAGE", "login_pass_error")));
                }

                model.USER_IP = Request.UserHostAddress;
                Session["loginUser"] = model;
                FormsAuthentication.SetAuthCookie(GetLoginUser().MEM_ID, false);
                Response.AppendCookie(new HttpCookie(FormsAuthentication.FormsCookieName)
                {
                    Value = FormsAuthentication.Encrypt(new FormsAuthenticationTicket(1, GetLoginUser().MEM_ID, DateTime.Now, DateTime.Now.AddMinutes(30), false, GetLoginUser().MEM_ID, FormsAuthentication.FormsCookiePath))
                });



                //驗證成功
                return Json(new RtnResultModel(true, ""));
            }
        }

        public ActionResult Logout() {
            Session.Clear();
            
            //將Cookies設為到期
            foreach(string cookieName in Response.Cookies) {
                Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(-1);
            }

            //清除表單驗證票證
            FormsAuthentication.SignOut();
            return View("Login");
        }

        /// <summary>
        /// 註冊頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 會員通知
        /// </summary>
        /// <returns></returns>
        public ActionResult Notify()
        {
            return View();
        }
    }
}