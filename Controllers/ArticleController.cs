using Literary_Arts.Dao;
using Literary_Arts.Models;
using Literary_Arts.Models.Sysop;
using Literary_Arts.Models.System;
using Literary_Arts.Web_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Literary_Arts.Controllers
{
    public class ArticleController : _Controller
    {
        //是否有權限對各種文章操作 update 或 insert
        bool IsHaveAuth = false;

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
                IList<ArticleModel> likeList = dao.ByIdGetLikeList(GetLoginUser().MEM_ID);
                ViewBag.ArticleList = model;
                ViewBag.LikeList = likeList;
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
        [CustomAuthorize("ArticlePost")]
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
                //是否有權利操作此編輯功能
                IsHaveAuth = dao.IsHaveAuthorityOperateFn("01", false, arti_num, GetLoginUser());
                if (IsHaveAuth)
                {
                    ArticleModel model = dao.ByArtiNumGetArticle(HttpUtility.HtmlEncode(arti_num));
                    ViewBag.TagData = dao.ByNumGetTag<ArticleModel>("01", HttpUtility.HtmlEncode(arti_num));
                    return View(model);
                }
                else
                {
                    return Redirect("/Home/Index");
                }
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
                //是否有權利操作此功能
                IsHaveAuth = dao.IsHaveAuthorityOperateFn("01", false, model.ARTI_NUM, GetLoginUser());
                if (IsHaveAuth)
                {
                    upd_result = dao.UpdateArticle(model);
                }

                return Json(upd_result); 
            }
        } 

        /// <summary>
        /// 編輯文章留言功能
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public JsonResult UpdateReply(ArticleModel model)
        {
            using (ArticleDao dao = new ArticleDao(GetLoginUser())) 
            {
                //是否有權利操作此功能
                IsHaveAuth = dao.IsHaveAuthorityOperateFn("01", true, model.ARTI_NUM, GetLoginUser());
                if (IsHaveAuth)
                {
                    upd_reply_result = dao.UpdateArticleReply(model);
                }

                return Json(upd_reply_result);
            }
               
           
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
                //判斷是否有權限刪除
                IsHaveAuth = dao.IsHaveAuthorityOperateFn("01", false, HttpUtility.HtmlEncode(arti_num), GetLoginUser());
                if (IsHaveAuth) 
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        //刪除文章 和 文章留言
                        RtnResultModel del_article = dao.DeleteArticle(HttpUtility.HtmlEncode(arti_num));
                        RtnResultModel del_reply = dao.DeleteReply(HttpUtility.HtmlEncode(arti_num));
                        scope.Complete();
                        if (del_article.success && del_reply.success)
                        {
                            //如果刪除成功，將結果給del_result
                            del_result = del_article;
                        }
                    }
                }
                return Json(del_result);
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
                //是否有權限刪除留言
                IsHaveAuth = dao.IsHaveAuthorityOperateFn("01", false, arti_reply_num, GetLoginUser());

                if (IsHaveAuth) 
                {
                    //取得刪除結果
                    del_reply_result = dao.DeleteReply(HttpUtility.HtmlEncode(arti_reply_num));
                }

                return Json(del_reply_result);
            }
        }
        #endregion
    }
}