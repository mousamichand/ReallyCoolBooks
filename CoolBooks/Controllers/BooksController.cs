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
            var books = db.Books.Include(b => b.AspNetUsers).Include(b => b.Authors).Include(b => b.Genres);
            return View(books.ToList());
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

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FirstName");
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,AuthorId,GenreId,Title,AlternativeTitle,Part,Description,ISBN,PublishDate,ImagePath,Created,IsDeleted")] Books books)
        {
            if (ModelState.IsValid)
            {
                //books.GenreId = 1;
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


                            Item item = GoogleBooksAPI.SearchBook(line); 
                            books.UserId = "772509fa-cfff-4a68-a573-a62d9c9a0bb6";
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
                books.Created = DateTime.Now;
                books.IsDeleted = false;
                db.Books.Add(books);
                db.SaveChanges();
                return RedirectToAction("Index");
                }
                else if(s.Equals("Save"))
                {
                    Reviews rev = new Reviews();
                    rev.UserId = "mchand";
                    rev.BookId = 21;
                    rev.Text = Request.Form["Comments"];
                    rev.Rating = Byte.Parse(Request.Form["star"]);
                    //  string star2 = Request.Form["star-2"];
                    //string star3 = Request.Form["star-3"];
                    //string star4 = Request.Form["star-4"];
                    //string star5 = RequesForm["star-5"];
                    rev.Created = DateTime.Now;
                    rev.Title= Request.Form["RevTitle"];

                    db.Reviews.Add(rev);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = rev.BookId });

                }
                else
                {
                   // Item  book = GoogleBooksAPI.SearchBook(books.ISBN);

                   //ViewBag.booktitle = book.VolumeInfo.Title;

                    ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", books.UserId);
                    ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FirstName", books.AuthorId);
                    ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name", books.GenreId);


                    //        Books book = new Books();
                    //        book.GenreId = 1 ;
                    //        book.AuthorId = 1;
                    //    book.Title = "crazy .net mvc";
                    //return View(book);


                    Books objbook = new Books
                    {
                        Id = 1,
                        AuthorId = 1,
                        UserId = "mchand",
                        ISBN = "1238",
                        Title = "Priti kumari",
                        Created = DateTime.Now

                    };

                    return View("create", objbook);
                }
                
            }

            else
                return View(books);

        }

        /*
        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult autofill([Bind(Include = "Id,UserId,AuthorId,GenreId,Title,AlternativeTitle,Part,Description,ISBN,PublishDate,ImagePath,Created,IsDeleted")] Books books)
        {
            if (ModelState.IsValid)
            {
                string s = Request.Form["n2"];

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
                            books.UserId = "772509fa-cfff-4a68-a573-a62d9c9a0bb6";
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

                return Index();


            }
            return Index();
        }

            */


        //[HttpPost]
        //public void autofill([Bind(Include = "ISBN")] Books books)
        //{

        //        GoogleBooksAPI.SearchBook(books.ISBN);
        //        books.GenreId = 1;
        //        books.Created = DateTime.Now;
        //        books.IsDeleted = false;




        //}

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
            name = name.Trim();
            string[] names = name.Split();
            string lastName = names.Last<string>();
            string fstName = "";
            for(int i = 0; i < names.Length-1; ++i)
                fstName += names[i] + " ";
            fstName = fstName.Trim();
            List<Authors> authors = db.Authors.ToList<Authors>();
            foreach (Authors author in authors)
                if ((author.FirstName.ToLower() == fstName.ToLower()) 
                    && (author.LastName.ToLower() == lastName.ToLower()))
                    return author.Id;
            Authors model = db.Authors.Create();
            model.FirstName = fstName;
            model.LastName = lastName;
            model.Created = DateTime.Now;
            model.IsDeleted = false;
            db.Authors.Add(model);
            db.SaveChanges();
            return model.Id;
        }

        /*
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


    //        Books book = new Books();
    //        book.GenreId = 1 ;
    //        book.AuthorId = 1;
    //    book.Title = "crazy .net mvc";
    //return View(book);


       Books objbook = new Books
        {
            Id = 1,
            AuthorId=1,
            UserId="mchand",
            ISBN="1238",
            Title = "Priti kumari",
           Created = DateTime.Now

        };

        return View("create",objbook);
    }

}

else
    return View(books);

    }
    return Index();
}
    */

        //[HttpPost]
        //public void autofill([Bind(Include = "ISBN")] Books books)
        //{

        //        GoogleBooksAPI.SearchBook(books.ISBN);
        //        books.GenreId = 1;
        //        books.Created = DateTime.Now;
        //        books.IsDeleted = false;




        //}

        // GET: Books/Edit/5
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
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", books.UserId);
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FirstName", books.AuthorId);
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name", books.GenreId);
            return View(books);
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

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Books books = db.Books.Find(id);
            db.Books.Remove(books);
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





        //[HttpPost]
        //public ActionResult GetInfoIsbn()
        //{

        //    return View();
        //}

    }
}





