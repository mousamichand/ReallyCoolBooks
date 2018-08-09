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

        public int IsRegistered()
        {
            if (Session["UserInfo"] == null)
            {
                return 0;
            }

            else return 1;
        }

        //public static bool IsAdmin(HttpSessionStateBase session)
        //{
        //    // if (((AspNetUsers)(Session["UserInfo"]).Id))

        //    var sessionId = ((AspNetUsers)session["UserInfo"]).AspNetRoles.Contains(db.AspNetRoles.Find("1"));

        //    if (session["UserInfo"] != null)
        //    {
        //        if( ((AspNetUsers)session["UserInfo"]).AspNetRoles.Contains(db.AspNetRoles.Find("1")))
        //        {
        //            return true;
        //        }

        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    return false;
        //}


        public ActionResult Index()
        {
            var books = db.Books.Include(b => b.AspNetUsers).Include(b => b.Authors).Include(b => b.Genres);

            List<int> ids = (from id in db.Books
                         select id.Id).ToList<int>();
            if (ids.Count > 0)
            {
                Random random = new Random();
                int i = random.Next(0, ids.Count);
                ViewData["Books"] = db.Books.Find(ids[i]);
            }
            else
            {
                ViewData["Books"] = null;
                return View();
            }
            return View(books.ToList());
        }

        // GET: AspNetUsers/LogIn
        public ActionResult LogIn()
        {
            return View();
        }

        public ActionResult Password()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn([Bind(Include = "UserName")] AspNetUsers aspNetUsers)
        {
            bool hasErrors = false;
            string userName = Request.Form["UserName"]; // TODO: Seee above and in cshtml
            string password = Request.Form["Password"];
            string passwordHash = AspNetUsersController.GetPasswordHash(password);
            AspNetUsers userInfo = null;

            int count = (from i in db.AspNetUsers
                         where (i.UserName == userName)
                         select i).Count();
            if (0 == count)
            {
                ViewBag.ErrMessage = "User name does not exist.";
                hasErrors = true;
            } else
            {
                userInfo = (from user in db.AspNetUsers
                                        where user.UserName == userName
                                        select user).First<AspNetUsers>();
                if (userInfo.PasswordHash != passwordHash)
                {
                    ViewBag.ErrMessage = "Password is incorrect.";
                    hasErrors = true;
                }
            }
            if (password.Trim() == "")
            {
                ViewBag.ErrMessage = "Password must be filled in.";
                hasErrors = true;
            }
            if (userName.Trim() == "")
            {
                ViewBag.ErrMessage = "User name must be filled in.";
                hasErrors = true;
            }
            if (hasErrors)
            {
                return View();
            } else {
                Session["UserInfo"] = userInfo;
                Session["UserName"] = userInfo.UserName;
                Session["UserId"] = userInfo.Id;
                return RedirectToAction("../Home");
            }
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("../Home");
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