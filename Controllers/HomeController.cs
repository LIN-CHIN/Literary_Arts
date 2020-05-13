using Literary_Arts.Dao;
using Literary_Arts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Literary_Arts.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (IndexDao dao = new IndexDao()) {
                IList<IndexModel> hotArticle = dao.GetHotArticle();
                ViewBag.RecomData = dao.GetRecomData();
                ViewBag.HotArtiData = hotArticle;
                ViewBag.TagData = dao.TagRouter(hotArticle, "01");
                return View();
            }
            
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}