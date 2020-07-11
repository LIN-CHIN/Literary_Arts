using Literary_Arts.Dao;
using Literary_Arts.Models;
using Literary_Arts.Models.System;
using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                return Redirect("/");
            }
            return View();
        }

        /// <summary>
        /// 觸發登入
        /// </summary>
        /// <param name="MEM_ID"></param>
        /// <param name="MEM_PASS"></param>
        /// <returns></returns>
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

                //驗證成功
                return Json(new RtnResultModel(true, ""));
            }
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