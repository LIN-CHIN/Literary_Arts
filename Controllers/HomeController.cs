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
                ViewBag.RecomData = dao.GetRecomData();
                ViewBag.HotArtiData = dao.GetHotArticle();
                ViewBag.test = dao.GetTag<IndexModel>("41","01");
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