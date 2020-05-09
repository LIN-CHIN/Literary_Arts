using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Literary_Arts.Controllers
{
    public class SpecialColumnController : Controller
    {
        /// <summary>
        /// 專欄列表 頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 專欄內容 頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult Content()
        {
            return View();
        }

        /// <summary>
        /// 專欄發文 頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult Post()
        {
            return View();
        }

        /// <summary>
        /// 編輯專欄 頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult Update()
        {
            return View();
        }
    }
}