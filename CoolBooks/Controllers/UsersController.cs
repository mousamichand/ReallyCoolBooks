using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.HtmlHelpers;
using CoolBooks.Models;

namespace CoolBooks.Controllers
{
    public class UsersController : Controller
    {
        private CoolBooksEntities db = new CoolBooksEntities();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.AspNetUsers);
            return View();
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Users/Profile
        public ActionResult Profile()
        {
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Users/Profile
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Profile([Bind(Include = "UserId,FirstName,LastName,Gender,Birthdate,Picture,Phone,Address,ZipCode,City,Country,Email,Info")] Users users)
        {
            bool hasErrors = false;

            if (!ModelState.IsValid)
            {
                ViewBag.ErrMessage = "There are error(s) in your input";
                hasErrors = true;
            }
            if (hasErrors)
            {
                return View();
            }
            else
            {
                users.Created = DateTime.Now;
                users.IsDeleted = false;

                users.Email = "hej";
                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", users.UserId);
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FirstName,LastName,Gender,Birthdate,Picture,Phone,Address,ZipCode,City,Country,Email,Info,Created,IsDeleted")] Users users)
        { 
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                if (users.FirstName == null || users.LastName == null || users.Email == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", users.UserId);
            return View(users);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
