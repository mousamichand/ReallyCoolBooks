using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoolBooks.Controllers
{
    public class HomeController: Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Login.";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "CoolBooks description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Donate()
        {
            ViewBag.Message = "Donate to our project.";

            return View();
        }
    }
}