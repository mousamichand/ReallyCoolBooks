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
                // TODO: Add insert logic here

                return RedirectToAction("Index");
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
        [HttpGet]

        public ActionResult Delete(int id)
        {
           // string Rid = id;
            Reviews reviews = db.Reviews.Find(id);
            db.Reviews.Remove(reviews);
            db.SaveChanges();
            //  return RedirectToAction("Index");

            return RedirectToAction("../Books");

        }

        // POST: Reviews/Delete/5
        [HttpPost]

        public ActionResult Delete( FormCollection collection)
        {
            //try
            {
                // TODO: Add delete logic 
               int Rid = Int32.Parse(collection["n1"]);
             
                Reviews reviews = db.Reviews.Find(Rid);
                db.Reviews.Remove(reviews);
                db.SaveChanges();
              //  return RedirectToAction("Index");

                return RedirectToAction("Details", "Books", new { id = collection["TxtBookId"] });
            //}
            //catch
            //{
              //  return RedirectToAction("Details", "Books", new { id = collection["TxtBookId"] });
            }
        }
    }
}
