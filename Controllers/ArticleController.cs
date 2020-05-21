using Literary_Arts.Dao;
using Literary_Arts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Literary_Arts.Controllers
{
    public class ArticleController : _Controller
    {
        /// <summary>
        /// 文章討論區頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            using (ArticleDao dao = new ArticleDao())
            {
                IList <ArticleModel> model = dao.GetArticleList();
                ViewBag.ArticleList = model; 
                ViewBag.TagData = dao.TagRouter(model, "01");
                return View();
            }  
        }

        /// <summary>
        /// 文章內容頁面(點擊討論區其中一篇文章)
        /// </summary>
        /// <param name="arti_num">文章編號</param>
        /// <returns></returns>
        public ActionResult Content(string arti_num)
        {
            using (ArticleDao dao = new ArticleDao())
            {
                ArticleModel model = dao.ByArtiNumGetArticle(HttpUtility.HtmlEncode(arti_num));
                ViewBag.TagData = dao.ByNumGetTag<ArticleModel>("01", HttpUtility.HtmlEncode(arti_num));
                return View("Content", model);
            }
        }

        /// <summary>
        /// 文章內容分類的頁面
        /// </summary>
        /// <param name="arti_class">
        ///     01 = '電影'
        ///     02 = '音樂'
        ///     03 = '書籍'
        ///     04 = '展覽'
        /// </param>
        /// <returns></returns>
        public ActionResult ContentClass(string arti_class)
        {
            using (ArticleDao dao = new ArticleDao())
            {
                IList<ArticleModel> model = dao.ByClassTypeGetList(HttpUtility.HtmlEncode(arti_class));
                ViewBag.ArticleList = model;
                ViewBag.TagData = dao.TagRouter(model, "01");
                return View("Index");
            }
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