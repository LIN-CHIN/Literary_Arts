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
                IList<ArticleModel> collList = dao.ByIdGetCollList(GetLoginUser().MEM_ID);
                ViewBag.ArticleList = model;
                ViewBag.LikeList = likeList;
                ViewBag.CollList = collList;
                ViewBag.TagData = dao.TagRouter(model, "01");

                foreach(var m in model) {
                    m.ARTI_CONT = HttpUtility.HtmlDecode(m.ARTI_CONT);
                }
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
                IList<ArticleModel> likeList = dao.ByIdGetLikeList(GetLoginUser().MEM_ID);
                IList<ArticleModel> collList = dao.ByIdGetCollList(GetLoginUser().MEM_ID);
                ViewBag.TagData = dao.ByNumGetTag<ArticleModel>("01", HttpUtility.HtmlEncode(arti_num));
                ViewBag.ReplyData = dao.GetReplyData(HttpUtility.HtmlEncode(arti_num));
                ViewBag.LikeList = likeList;
                ViewBag.CollList = collList;

                model.ARTI_CONT = HttpUtility.HtmlDecode(model.ARTI_CONT);
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
            System.Web.HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoServerCaching();
            System.Web.HttpContext.Current.Response.Cache.SetNoStore();
            //中英轉換
            arti_class = ReverseParamLanguage("LITERARY_CLASS_ENG", "LITERARY_CLASS_CHI", arti_class);

            using (ArticleDao dao = new ArticleDao(GetLoginUser()))
            {
                IList<ArticleModel> model = dao.ByClassTypeGetList(HttpUtility.HtmlEncode(arti_class));
                IList<ArticleModel> likeList = dao.ByIdGetLikeList(GetLoginUser().MEM_ID);
                IList<ArticleModel> collList = dao.ByIdGetCollList(GetLoginUser().MEM_ID);
                ViewBag.ArticleList = model;
                ViewBag.TagData = dao.TagRouter(model, "01");
                ViewBag.LikeList = likeList;
                ViewBag.CollList = collList;
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

                    //內容解碼
                    model.ARTI_CONT = HttpUtility.HtmlDecode(model.ARTI_CONT);

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
        [ValidateInput(false)]
        public JsonResult UpdateArticle(ArticleModel model)
        {
            //將英文代號轉為數字代號
            if (!string.IsNullOrEmpty(model.ARTI_CLASS))
            {
                model.ARTI_CLASS = SysSet.GetParamItemType("LITERARY_CLASS_ENG", model.ARTI_CLASS);
            }

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

        /// <summary>
        /// 點愛心的動作
        /// </summary>
        /// <param name="num"></param>
        /// <param name="isReply"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ClickLike(string num,bool isReply) 
        {
            //沒登入
            if (string.IsNullOrEmpty(GetLoginUser().MEM_ID))  
            {
                return Json(new RtnResultModel(false, ""));
            }

            using (ArticleDao dao = new ArticleDao(GetLoginUser())) {
                //判斷有沒有點過愛心
                bool isClick = dao.IsClickLike(num, GetLoginUser().MEM_ID, isReply);
                bool result = false;
                if (!isClick)
                {
                    result = dao.AddLike(num, GetLoginUser().MEM_ID, isReply);
                }
                else {
                    result = dao.DelLike(num, GetLoginUser().MEM_ID, isReply);
                }
                int likeCount = dao.GetLikeCount(num, isReply);
                object obj = new
                {
                    isClick = isClick,
                    likeCount = likeCount
                };

                if (!result)
                {
                    return Json(new RtnResultModel(false, "新增/刪除愛心失敗"));
                }
                else {
                    return Json(obj);
                }
            } 
        }

        /// <summary>
        /// 取得愛心數量
        /// </summary>
        /// <param name="num"></param>
        /// <param name="isReply"></param>
        /// <returns></returns>
        public int GetLikeCount(string num, bool isReply) 
        {
            using (ArticleDao dao = new ArticleDao(GetLoginUser()))
            {
                return dao.GetLikeCount(num, isReply);
            }
        }

        /// <summary>
        /// 取得所有文章的個別愛心數量
        /// </summary>
        /// <param name="isReply"></param>
        /// <returns></returns>
        public JsonResult GetLikeCount(bool isReply)
        {
            using (ArticleDao dao = new ArticleDao(GetLoginUser()))
            {
                return Json(dao.GetLikeCount());
            }
        }


        /// <summary>
        /// 點收藏的動作
        /// </summary>
        /// <param name="num"></param>
        /// <param name="isReply"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ClickCollection(string num)
        {
            //沒登入
            if (string.IsNullOrEmpty(GetLoginUser().MEM_ID))
            {
                return Json(new RtnResultModel(false, ""));
            }

            using (ArticleDao dao = new ArticleDao(GetLoginUser()))
            {
                //判斷有沒有點過收藏
                bool isClick = dao.IsClickCollection(num, GetLoginUser().MEM_ID);
                bool result = false;
                if (!isClick)
                {
                    result = dao.AddCollection(num, GetLoginUser().MEM_ID);
                }
                else
                {
                    result = dao.DelCollection(num, GetLoginUser().MEM_ID);
                }

                if (!result)
                {
                    return Json(new RtnResultModel(false, "新增/刪除收藏失敗"));
                }
                else
                {
                    return Json(isClick);
                }
            }
        }

        /// <summary>
        /// 按瀏覽器的下/上下一頁時 愛心、收藏 按鈕需Reload
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBackForwardData()
        {
            using (ArticleDao dao = new ArticleDao(GetLoginUser()))
            {
                IList<ArticleModel> userLikeList = dao.ByIdGetLikeList(GetLoginUser().MEM_ID);
                IList<ArticleModel> userCollList = dao.ByIdGetCollList(GetLoginUser().MEM_ID);
                IList<ArticleModel> likeCountList = dao.GetLikeCount();

                var result = new
                {
                   userLikeList = userLikeList,
                   userCollList = userCollList,
                   likeCountList = likeCountList
                };

                return Json(result);
            }
        }

        [Authorize]
        [HttpPost]
        public JsonResult PostReply(string inputContent, string arti_num) 
        {
            RtnResultModel result = new RtnResultModel(false,"");

            if (string.IsNullOrWhiteSpace(HttpUtility.HtmlEncode(inputContent)))
            {
                result.message = "請先輸入留言，再進行提交！";
            }
            else 
            {
                using (ArticleDao dao = new ArticleDao(GetLoginUser())) {
                    result = dao.PostReply(HttpUtility.HtmlEncode(inputContent), HttpUtility.HtmlEncode(arti_num), GetLoginUser().MEM_ID);
                }
            }
            return Json(result);
        }


    }
}