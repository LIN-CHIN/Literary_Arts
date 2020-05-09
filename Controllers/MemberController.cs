using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Literary_Arts.Controllers
{
    public class MemberController : Controller
    {
        /// <summary>
        /// 登入頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
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