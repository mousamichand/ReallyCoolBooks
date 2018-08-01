using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using CoolBooks.Models;
using Newtonsoft.Json;

namespace CoolBooks.Controllers
{
    public class BooksController : Controller
    {
        // GET: Books
        private CoolBooksEntities db = new CoolBooksEntities();

        public ActionResult Index()
            
        {
            //ViewBag.Generid = new SelectList(db.Genres, "Id", "Name");
            List <Genres> genres= db.Genres.ToList();
            return View(genres);
        }

        // GET: Books/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
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

        // GET: Books/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Books/Edit/5
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

        // GET: Books/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Books/Delete/5
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

        public static void SearchBook(string isbn)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:49905");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync("https://www.googleapis.com/books/v1/volumes?q=isbn:" + isbn).Result;

            string jsonData = response.Content.ReadAsStringAsync().Result;

            Books book = JsonConvert.DeserializeObject<Books>(jsonData);

        }

    }
}
