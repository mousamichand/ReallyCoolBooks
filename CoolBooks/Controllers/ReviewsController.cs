using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoolBooks.Models;

namespace CoolBooks.Controllers
{
    public class ReviewsController : Controller
    {

        private CoolBooksEntities db = new CoolBooksEntities();
        // GET: Reviews
        public ActionResult Index()
        {
            return View();
        }

        // GET: Reviews/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Reviews/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reviews/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {

                if (Session["UserInfo"] != null) // If registered
                {
                    Reviews rev = new Reviews();
                    rev.UserId = "mchand";
                    rev.Text = collection["resume"];
                    db.Reviews.Add(rev);
                }

                else
                {
                    return RedirectToAction("Home", "Noaccess"); // **** ;
                    //return RedirectToAction("Noaccess", "Home"); // **** ;
                }

                return RedirectToAction("Books","Index"); // **** ;
            }
            catch
            {
                return View();
            }
        }

        // GET: Reviews/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reviews/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reviews/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
