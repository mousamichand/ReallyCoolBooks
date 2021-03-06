﻿using System;
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

        public static bool IsLoggedIn(HttpSessionStateBase session)
        {
            return (session["UserInfo"] != null);
        }

        public static bool IsAdmin(HttpSessionStateBase session)
        {
            if (session["UserInfo"] == null)
                return false;
            else
                return (bool)session["IsAdmin"];
        }

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

        public ActionResult Password()
        {
            return View();
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
            bool isAdmin = false;
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
                isAdmin = userInfo.AspNetRoles.Contains(db.AspNetRoles.Find("1"));
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
                Session["IsAdmin"] = isAdmin;
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