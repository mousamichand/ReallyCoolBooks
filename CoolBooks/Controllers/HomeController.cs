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

namespace CoolBooks.Controllers
{
    public class HomeController: Controller
    {
        private CoolBooksEntities db = new CoolBooksEntities();

        public ActionResult Index()
        {
            var books = db.Books.Include(b => b.AspNetUsers).Include(b => b.Authors).Include(b => b.Genres);
            return View(books.ToList());
        }

        // GET: AspNetUsers/LogIn
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn([Bind(Include = "UserName")] AspNetUsers aspNetUsers)
        {
            bool hasErrors = false;
            string userName = Request.Form["UserName"];
            string password = Request.Form["Password"];
            string passwordHash = AspNetUsersController.GetPasswordHash(password);

            if (password.Trim() == "")
            {
                ViewBag.ErrMessage = "Password must be filled in";
                hasErrors = true;
            }
            if (userName.Trim() == "")
            {
                ViewBag.ErrMessage = "Username must be filled in";
                hasErrors = true;
            }
            int count = (from i in db.AspNetUsers
                         where (i.UserName == userName)
                         select i).Count();
            if (0 == count)
            {
                ViewBag.ErrMessage = "User name does not exist";
                hasErrors = true;
            }
            AspNetUsers userInfo = (from user in db.AspNetUsers
                                    where user.UserName == userName
                                    select user).First<AspNetUsers>();
            if (userInfo.PasswordHash != passwordHash)
            {
                ViewBag.ErrMessage = "Password is incorrect";
                hasErrors = true;
            }
            if (hasErrors)
            {
                return View();
            } else {
                Session["UserInfo"] = userInfo; 
                return RedirectToAction("../Home");
            }
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