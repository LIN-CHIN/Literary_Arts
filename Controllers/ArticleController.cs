using Literary_Arts.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Literary_Arts.Controllers
{
    public class ArticleController : Controller
    {
        /// <summary>
        /// 文章討論區頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            using (ArticleDao dao = new ArticleDao())
            {
                ViewBag.test = dao.Query();
            }
                return View();
        }

        /// <summary>
        /// 文章內容頁面(點擊討論區其中一篇文章)
        /// </summary>
        /// <returns></returns>
        public ActionResult Content()
        {
            return View();
        }

        /// <summary>
        /// 文章發文 頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult Post()
        {
            return View();
        }

        /// <summary>
        /// 編輯文章 頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult Update()
        {
            return View();
        }

        /// <summary>
        /// 編輯文章 留言
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateReply()
        {
            return View();
        }
    }
}