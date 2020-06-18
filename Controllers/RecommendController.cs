using Literary_Arts.Dao;
using Literary_Arts.Models;
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
            using (RecommendDao dao = new RecommendDao())
            {
                IList<RecommendModel> model = dao.GetRecommendList();
                ViewBag.RecommendList = model;
                return View();
            }
        }

        /// <summary>
        /// 文章內容頁面(點擊討論區其中一篇文章)
        /// </summary>
        /// <param name="recom_num">文章編號</param>
        /// <returns></returns>
        public ActionResult Content(string recom_num)
        {
            using (RecommendDao dao = new RecommendDao())
            {
                RecommendModel model = dao.ByRecomNumGetRecommend(HttpUtility.HtmlEncode(recom_num));
                ViewBag.TagData = dao.ByNumGetTag<RecommendModel>("02", HttpUtility.HtmlEncode(recom_num));
                return View("Content", model);
            }
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