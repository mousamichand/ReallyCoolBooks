using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CoolBooks.Models;
using System.Security.Cryptography;
using CaptchaMvc.HtmlHelpers;
using System.Text;

namespace CoolBooks.Controllers
{
    public class AspNetUsersController: Controller
    {
        private CoolBooksEntities db = new CoolBooksEntities();

        // GET: AspNetUsers
        public ActionResult Index()
        {
            var aspNetUsers = db.AspNetUsers.Include(a => a.Users);
            return View(aspNetUsers.ToList());
        }

        // GET: AspNetUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        public static string GetPasswordHash(string password)
        {
            byte[] data = Encoding.UTF8.GetBytes(password);
            SHA512 sha = new SHA512Managed();
            data = sha.ComputeHash(data);
            return Encoding.UTF8.GetString(data);
        }

        // GET: AspNetUsers/Signup
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Include = "Email,UserName")] AspNetUsers aspNetUsers)
        {
            bool hasErrors = false;
            string password = Request.Form["Password"];
            string password2 = Request.Form["Password2"];

            if (!ModelState.IsValid)
            {
                ViewBag.ErrMessage = "There are error(s) in your input";
                hasErrors = true;
            }
            if (!this.IsCaptchaValid("Validate your captcha"))
            {
                ViewBag.ErrMessage = "Your captcha answer is incorrect.";
                hasErrors = true;
            }
            if (password != password2)
            {
                ViewBag.ErrMessage = "Passwords don't match.";
                hasErrors = true;
            }
            if (password.Trim() == "")
            {
                ViewBag.ErrMessage = "Password must be filled in.";
                hasErrors = true;
            }
            if (aspNetUsers.UserName.Trim() == "")
            {
                ViewBag.ErrMessage = "Username must be filled in.";
                hasErrors = true;
            }
            int count = (from i in db.AspNetUsers
                         where (i.UserName == aspNetUsers.UserName)
                         select i).Count();
            if (0 < count)
            {
                ViewBag.ErrMessage = "User name already exists.";
                hasErrors = true;
            }
            if (!ModelState.IsValid)
            {
                ViewBag.ErrMessage = "There are error(s) in your input";
                hasErrors = true;
            }

            if (hasErrors)
                return View();
            else
            {
                aspNetUsers.Id = Guid.NewGuid().ToString();
                aspNetUsers.EmailConfirmed = true;
                aspNetUsers.PasswordHash = GetPasswordHash(password);
                aspNetUsers.SecurityStamp = "";
                aspNetUsers.PhoneNumber = "";
                aspNetUsers.PhoneNumberConfirmed = false;
                aspNetUsers.TwoFactorEnabled = false;
                aspNetUsers.LockoutEndDateUtc = null;
                aspNetUsers.LockoutEnabled = false;
                aspNetUsers.AccessFailedCount = 0;
                aspNetUsers.AspNetRoles.Add(db.AspNetRoles.Find("2"));
                db.AspNetUsers.Add(aspNetUsers);

                Users users = new Users();
                users.UserId = aspNetUsers.Id;
                users.FirstName = "";
                users.LastName = "";
                users.Email = aspNetUsers.Email;
                users.Created = DateTime.Now;
                users.IsDeleted = false;
                db.Users.Add(users);

                db.SaveChanges();
                Session["UserInfo"] = aspNetUsers;
                Session["UserName"] = aspNetUsers.UserName;

                return RedirectToAction("../Users/Profile");
            }
        }

        // GET: AspNetUsers/Edit
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Users, "UserId", "FirstName", aspNetUsers.Id);
            return View(aspNetUsers);
        }

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] AspNetUsers aspNetUsers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUsers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Users, "UserId", "FirstName", aspNetUsers.Id);
            return View(aspNetUsers);
        }

        /*
        // GET: AspNetUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            if (aspNetUsers == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUsers);
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(aspNetUsers);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
*/
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



