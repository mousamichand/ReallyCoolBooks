using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CoolBooks.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CoolBooks.Controllers
{
    public class BooksController: Controller
    {
        private CoolBooksEntities db = new CoolBooksEntities();

        // GET: Books
        public ActionResult Index()
        {


            List<Books> Books = db.Books.ToList();
            return View(Books);
            //var books = db.Books.Include(b => b.AspNetUsers).Include(b => b.Authors).Include(b => b.Genres);
            //return View(books.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
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
                books.GenreId = 1;

                books.Created = DateTime.Now;
                books.IsDeleted = false;
                //try
                //{


                //    if (books.ImagePath != null)
                //    {
                //        string path = Path.Combine(Server.MapPath("~/BookImaGes"), Path.GetFileName(books.ImagePath));
                //        file.SaveAs(path);

                //    }
                //    ViewBag.FileStatus = "File uploaded successfully.";
                //}
                //catch (Exception)
                //{

                //    ViewBag.FileStatus = "Error while file uploading.";
                //}







                db.Books.Add(books);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", books.UserId);
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "FirstName", books.AuthorId);
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name", books.GenreId);
            return View(books);
        }

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

        // POST: Books/Edit/5
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

        //public static void SearchBook(string isbn)
        //{
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:49905");
        //    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //    HttpResponseMessage response = client.GetAsync("https://www.googleapis.com/books/v1/volumes?q=isbn:" + isbn).Result;

            string jsonData = response.Content.ReadAsStringAsync().Result;
            BookInfos books = JsonConvert.DeserializeObject<BookInfos>(jsonData);
        }
    

    public class BookInfos
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("totalItems")]
        public long TotalItems { get; set; }

        [JsonProperty("items")]
        public Item[] Items { get; set; }
    }

    public class Item
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("selfLink")]
        public string SelfLink { get; set; }

        [JsonProperty("volumeInfo")]
        public VolumeInfo VolumeInfo { get; set; }

        /*
        [JsonProperty("saleInfo")]
        public SaleInfo SaleInfo { get; set; }

        [JsonProperty("accessInfo")]
        public AccessInfo AccessInfo { get; set; }

        [JsonProperty("searchInfo")]
        public SearchInfo SearchInfo { get; set; }
        */
    }


    public class VolumeInfo
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }

        [JsonProperty("authors")]
        public string[] Authors { get; set; }

        [JsonProperty("publishedDate")]
        public DateTimeOffset PublishedDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        /*
        [JsonProperty("industryIdentifiers")]
        public IndustryIdentifier[] IndustryIdentifiers { get; set; }

        [JsonProperty("readingModes")]
        public ReadingModes ReadingModes { get; set; }
        */

        [JsonProperty("pageCount")]
        public long PageCount { get; set; }

        [JsonProperty("printType")]
        public string PrintType { get; set; }

        [JsonProperty("categories")]
        public string[] Categories { get; set; }

        [JsonProperty("averageRating")]
        public double AverageRating { get; set; }

        [JsonProperty("ratingsCount")]
        public long RatingsCount { get; set; }

        [JsonProperty("maturityRating")]
        public string MaturityRating { get; set; }

        [JsonProperty("allowAnonLogging")]
        public bool AllowAnonLogging { get; set; }

        [JsonProperty("contentVersion")]
        public string ContentVersion { get; set; }

        [JsonProperty("imageLinks")]
        public ImageLinks ImageLinks { get; set; }
 
        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("previewLink")]
        public string PreviewLink { get; set; }

        [JsonProperty("infoLink")]
        public string InfoLink { get; set; }

        [JsonProperty("canonicalVolumeLink")]
        public string CanonicalVolumeLink { get; set; }
    }

    public partial class ImageLinks
    {
        [JsonProperty("smallThumbnail")]
        public string SmallThumbnail { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }
    }



}




