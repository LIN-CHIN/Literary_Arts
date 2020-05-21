using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Literary_Arts.Controllers
{
    public class RecommendController : _Controller
    {
        /// <summary>
        /// 推薦列表頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 官方推薦內容頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult Content()
        {
            return View();
        }

        /// <summary>
        /// 官方推薦發文頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult Post()
        {
            return View();
        }

        /// <summary>
        /// 編輯官方推薦頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult Update()
        {
            return View();
        }

        public ActionResult UpdateReply()
        {
            return View();
        }
    }
}