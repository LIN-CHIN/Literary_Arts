using Literary_Arts.Dao;
using Literary_Arts.Models;
using Literary_Arts.Models.Sysop;
using Literary_Arts.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace Literary_Arts.Controllers
{
    public class ArticleController : _Controller
    {
        #region 頁面
        /// <summary>
        /// 文章討論區頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            using (ArticleDao dao = new ArticleDao(GetLoginUser()))
            {
                IList<ArticleModel> model = dao.GetArticleList();
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
            using (ArticleDao dao = new ArticleDao(GetLoginUser()))
            {
                ArticleModel model = dao.ByArtiNumGetArticle(HttpUtility.HtmlEncode(arti_num));
                ViewBag.TagData = dao.ByNumGetTag<ArticleModel>("01", HttpUtility.HtmlEncode(arti_num));
                ViewBag.ReplyData = dao.GetReplyData(HttpUtility.HtmlEncode(arti_num));
                return View("Content", model);
            }
        }

        /// <summary>
        /// 文章內容分類的頁面
        /// </summary>
        /// <param name="arti_class"> 文章分類(英文) ex : 'movie'</param>
        /// <returns></returns>
        public ActionResult ContentClass(string arti_class)
        {
            //中英轉換
            arti_class = ReverseParamLanguage("LITERARY_CLASS_ENG", "LITERARY_CLASS_CHI", arti_class);

            using (ArticleDao dao = new ArticleDao(GetLoginUser()))
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
        [ArtAuthorizeModel("ArticlePost")]
        public ActionResult Post()
        {
            return View();
        }

        #endregion

        #region 編輯
        /// <summary>
        /// 編輯文章 頁面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Update(string arti_num)
        {
            using (ArticleDao dao = new ArticleDao(GetLoginUser()))
            {
                ArticleModel model = dao.ByArtiNumGetArticle(HttpUtility.HtmlEncode(arti_num));
                ViewBag.TagData = dao.ByNumGetTag<ArticleModel>("01", HttpUtility.HtmlEncode(arti_num));
                return View(model);
            }
        }

        /// <summary>
        /// 編輯文章 功能
        /// </summary>
        /// <param name="arti_num"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public JsonResult UpdateArticle(ArticleModel model)
        {
            using (ArticleDao dao = new ArticleDao(GetLoginUser()))
            {
                //ArticleModel model = dao.ByArtiNumGetArticle(HttpUtility.HtmlEncode(arti_num));
                //ViewBag.TagData = dao.ByNumGetTag<ArticleModel>("01", HttpUtility.HtmlEncode(arti_num));
                return Json("");
            }
        } 

        /// <summary>
        /// 編輯文章 留言
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult UpdateReply()
        {
            return View();
        }
        #endregion

        #region 刪除
        /// <summary>
        /// 刪除文章
        /// </summary>
        /// <param name="arti_num"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public JsonResult DeleteArticle(string arti_num)
        {
            using (ArticleDao dao = new ArticleDao(GetLoginUser()))
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    RtnResultModel del_article = dao.DeleteArticle(HttpUtility.HtmlEncode(arti_num));
                    RtnResultModel del_reply = dao.DeleteReply(HttpUtility.HtmlEncode(arti_num));
                    scope.Complete();
                    if (del_article.success && del_reply.success)
                    {
                        return Json(del_article.message);
                    }
                    else {
                        return Json(SysSet.GetParamItemValue("SYS_MESSAGE", "sys_error"));
                    }
                }
            }
        }

        /// <summary>
        /// 刪除留言
        /// </summary>
        /// <param name="arti_num"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public JsonResult DeleteReply(string arti_reply_num)
        {
            using (ArticleDao dao = new ArticleDao(GetLoginUser()))
            {
                RtnResultModel del_reply = dao.DeleteReply(HttpUtility.HtmlEncode(arti_reply_num));
                if (del_reply.success)
                {
                    return Json(del_reply.message);
                }
                else
                {
                    return Json(SysSet.GetParamItemValue("SYS_MESSAGE", "sys_error"));
                }
            }
        }
        #endregion
    }
}