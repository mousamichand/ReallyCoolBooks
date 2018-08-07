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

        // GET: AspNetUsers/Signup
        public ActionResult SignUp()
        {
            return View();
        }

        public static string GetPasswordHash(string password)
        {
            byte[] data = Encoding.UTF8.GetBytes(password);
            SHA512 sha = new SHA512Managed();
            data = sha.ComputeHash(data);
            return Encoding.UTF8.GetString(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Include = "Email,UserName")] AspNetUsers aspNetUsers)
        {
            bool hasErrors = false;
            string password = Request.Form["Password"];
            string password2 = Request.Form["Password2"];

            // TODO Validate email?
            // java script for easy checks?
            // email unique?
            // salt? as securitystamp
            // no dropdowns
            // update access failed count? in login?
            // model isvalid?          
            // login in using either username or email?
            // phonenumber etc.
            // fix isbn problem

            if (!ModelState.IsValid)
            {
                ViewBag.ErrMessage = "There are error(s) in your input";
                hasErrors = true;
            }
            if (!this.IsCaptchaValid("Validate your captcha"))
            {
                ViewBag.ErrMessage = "Your captcha answer is incorrect";
                hasErrors = true;
            }
            if (password != password2)
            {
                ViewBag.ErrMessage = "Passwords don't match";
                hasErrors = true;
            }
            if (password.Trim() == "")
            {
                ViewBag.ErrMessage = "Password must be filled in";
                hasErrors = true;
            }
            


            if (hasErrors)
            {
                return View();
            } else { 
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
                db.AspNetUsers.Add(aspNetUsers);
                db.SaveChanges();
                return RedirectToAction("../Users/Profile");
            } 
        }

        // GET: AspNetUsers/Edit/5
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public sealed class SecurePasswordHasher
    {
        /// <summary>
        /// Size of salt
        /// </summary>
        private const int SaltSize = 16;

        /// <summary>
        /// Size of hash
        /// </summary>
        private const int HashSize = 20;

        /// <summary>
        /// Creates a hash from a password
        /// </summary>
        /// <param name="password">the password</param>
        /// <param name="iterations">number of iterations</param>
        /// <returns>the hash</returns>
        /*
        public static string Hash(string password, int iterations)
        {
            //create salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            //create hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            //combine salt and hash
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            //convert to base64
            var base64Hash = Convert.ToBase64String(hashBytes);

            //format hash with extra information
            return string.Format("$MYHASH$V1${0}${1}", iterations, base64Hash);
        }
        /// <summary>
        /// Creates a hash from a password with 10000 iterations
        /// </summary>
        /// <param name="password">the password</param>
        /// <returns>the hash</returns>
        public static string Hash(string password)
        {
            return Hash(password, 10000);
        }

        /// <summary>
        /// Check if hash is supported
        /// </summary>
        /// <param name="hashString">the hash</param>
        /// <returns>is supported?</returns>
        public static bool IsHashSupported(string hashString)
        {
            return hashString.Contains("$MYHASH$V1$");
        }

        /// <summary>
        /// verify a password against a hash
        /// </summary>
        /// <param name="password">the password</param>
        /// <param name="hashedPassword">the hash</param>
        /// <returns>could be verified?</returns>
        public static bool Verify(string password, string hashedPassword)
        {
            //check hash
            if (!IsHashSupported(hashedPassword))
            {
                throw new NotSupportedException("The hashtype is not supported");
            }

            //extract iteration and Base64 string
            var splittedHashString = hashedPassword.Replace("$MYHASH$V1$", "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            //get hashbytes
            var hashBytes = Convert.FromBase64String(base64Hash);

            //get salt
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            //create hash with given salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            //get result
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;


        }
        */
    }


}
