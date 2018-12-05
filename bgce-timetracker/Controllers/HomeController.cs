using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using bgce_timetracker.Models;

namespace bgce_timetracker.Controllers
{
    public class HomeController : Controller
   {
        private trackerEntities db = new trackerEntities();
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("IndexInternal", "Home");
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "A time tracking application.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult IndexInternal()
        {
            
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Message = "Home.";
            var nOTIFICATIONs = db.NOTIFICATIONs.Where(n => n.type == "homepage" && n.active == true);
            return View(nOTIFICATIONs.ToList());
        }
    }
}