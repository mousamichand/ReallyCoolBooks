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
    public class BooksController: Controller
    {
        private CoolBooksEntities db = new CoolBooksEntities();

        //public int AuthorId { get; private set; }
        //public string Title { get; private set; }

        // GET: Books
        public ActionResult Index()
        {
            // var books = db.Books.Include(b => b.AspNetUsers).Include(b => b.Authors).Include(b => b.Genres);
            if (Session["UserInfo"] != null)
            {
                string sessionId = ((AspNetUsers)Session["UserInfo"]).Id;
                var books =
                from c in db.Books
                where (c.UserId == sessionId)
                select c;
                return View(books.ToList());
            }

            else
                //return RedirectToAction("Home", "Noaccess");
                return Redirect("/Home/Index");
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }




            ViewData["Books"] = db.Books.Find(id);
            
           ViewData["Reviews"] =
                from c in db.Reviews
                where (c.BookId == id)
                select c;





            //if (books == null)
            //{
            //    return HttpNotFound();
            //}
            // return View(books);
            if (ViewData["Reviews"] == null)
            {
                return HttpNotFound();
            }
            return View();
            
        }

        // GET: Books/Create ********
        public ActionResult Create()
        {
            if (Session["UserInfo"] != null)
            { 
                ViewBag.UserId = Session["username"];


                ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FirstName");
                ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name");
                return View();
            }

            return Redirect("/Home/Index");

        }
        /*
            else // Else return to index page
            {
                return RedirectToAction("Home", "Noaccess");
                //return Redirect("../Authors/Index");
                //return Redirect("../Home/Noaccess");
            }
            
        }
        */

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,AuthorId,GenreId,Title,AlternativeTitle,Part,Description,ISBN,PublishDate,ImagePath,Created,IsDeleted")] Books books)
        {

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", books.UserId);
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FirstName", books.AuthorId);
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name", books.GenreId);

            string s1 = Request.Form["n2"];
            
                if (s1.Equals("Save"))
                {
                    Reviews rev = new Reviews();
                    rev.UserId = ((AspNetUsers)Session["UserInfo"]).Id;
                    rev.BookId = Int32.Parse(Request.Form["TxtBookId"]);
                    rev.Text = Request.Form["Comments"];
                    if (Request.Form["star"] != null)
                        rev.Rating = Byte.Parse(Request.Form["star"]);
                    else
                        rev.Rating = 0;
                    rev.Created = DateTime.Now;
                    rev.Title = Request.Form["RevTitle"];
                     rev.IsDeleted = false;
                    db.Reviews.Add(rev);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = rev.BookId });

                }


            if (ModelState.IsValid)
            {
                // books.GenreId = 1;
                string s = Request.Form["n1"];

                if (s.Equals("autofill"))
                {
                    int counter = 0;
                    string line;

                    // Read the file and display it line by line.  
                    System.IO.StreamReader file =
                        new System.IO.StreamReader(@"c:\isbnstuff3.txt");
                    while ((line = file.ReadLine()) != null)
                    {
                        try
                        {


                            Item item = GoogleBooksAPI.SearchBook(line); ;
                            books.UserId = "c2d8fc5a-9218-42be-ba0e-2dcef8621649";
                            books.AuthorId = GetAuthorByName(item.VolumeInfo.Authors[0]);
                            books.Created = DateTime.Now;
                            books.GenreId = GetGenreByName(item.VolumeInfo.Categories[0]);
                            books.IsDeleted = false;
                            books.Title = item.VolumeInfo.Title;
                            books.ISBN = line;
                            books.Description = item.VolumeInfo.Description;
                            books.ImagePath = item.VolumeInfo.ImageLinks.Thumbnail;
                            // books.PublishDate = item.VolumeInfo.PublishedDate;

                            db.Books.Add(books);
                            db.SaveChanges();
                        }


                        catch
                        {
                            int a = 1;
                        }
                        counter++;
                    }

                    file.Close();

                }





                if (s.Equals("Create"))
                {
                    books.UserId = ((AspNetUsers)Session["UserInfo"]).Id;
                    books.Created = DateTime.Now;
                    books.IsDeleted = false;
                    db.Books.Add(books);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
                else
                {
                    // Item  book = GoogleBooksAPI.SearchBook(books.ISBN);

                    //ViewBag.booktitle = book.VolumeInfo.Title;

                    ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", books.UserId);
                    ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FirstName", books.AuthorId);
                    ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name", books.GenreId);




                    return View();
                }

            }

            else
            {
               
                return View(books);
            }
        }

        public int GetGenreByName(string name)
        {
            List<Genres> genres = db.Genres.ToList<Genres>();
            foreach (Genres genre in genres)
                if (genre.Name == name) return genre.Id;
            Genres model = db.Genres.Create();
            model.Name = name;
            model.Created = DateTime.Now;
            model.IsDeleted = false;
            db.Genres.Add(model);
            db.SaveChanges();
            return model.Id;
        }

        public int GetAuthorByName(string name)
        {
            List<Authors> authors = db.Authors.ToList<Authors>();
            foreach (Authors author in authors)
                if ((author.FirstName.ToLower() == name.ToLower()))
                    
                    return author.Id;
            Authors model = db.Authors.Create();
            model.FirstName = name;
            model.LastName = "poop";
            model.Created = DateTime.Now;
            model.IsDeleted = false;
            db.Authors.Add(model);
            db.SaveChanges();
            return model.Id;
        }

        // GET: Books/Edit/5 ****
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books books = db.Books.Find(id);
            if (books == null)
            {
                return HttpNotFound();
            }
          
            if ((string)Session["UserId"] == books.UserId)
            {
                ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", books.UserId);
                ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FirstName", books.AuthorId);
                ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name", books.GenreId);
                return View(books);
            }

            else
                return Redirect("Index");
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,AuthorId,GenreId,Title,AlternativeTitle,Part,Description,ISBN,PublishDate,ImagePath,Created,IsDeleted")] Books books)
        {
            if (ModelState.IsValid)
            {
                db.Entry(books).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", books.UserId);
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FirstName", books.AuthorId);
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name", books.GenreId);
            return View(books);
        }



        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books books = db.Books.Find(id);
            if (books == null)
            {
                return HttpNotFound();
            }
            return View(books);
        }

        // POST: Books/Delete/5 ****
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Books books = db.Books.Find(id);

            if ((string)Session["UserId"] == books.UserId)
            {
                db.Books.Remove(books);
                db.SaveChanges();
            }

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





        //[HttpPost]
        //public ActionResult GetInfoIsbn()
        //{

        //    return View();
        //}

    }
}





