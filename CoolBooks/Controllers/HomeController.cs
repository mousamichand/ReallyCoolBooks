using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CoolBooks.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using CoolBooks.ViewModels;
//using CoolBooks.ViewModels;


namespace CoolBooks.Controllers
{
    public class HomeController: Controller
    {
        private CoolBooksEntities db = new CoolBooksEntities();

        public ActionResult Index()
        {
            var books = db.Books.Include(b => b.AspNetUsers).Include(b => b.Authors).Include(b => b.Genres);
            



            ViewData["Books"] = db.Books.Find(73);


            return View(books.ToList());
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