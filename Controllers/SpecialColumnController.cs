using Literary_Arts.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Literary_Arts.Controllers
{
    public class SpecialColumnController : _Controller
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
        [ArtAuthorizeModel("CreateSpecialColumn")]
        public ActionResult Post()
        {
            return View();
        }

        /// <summary>
        /// 編輯專欄 頁面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Update()
        {
            return View();
        }
    }
}