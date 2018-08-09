using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoolBooks.Models;
using System.Linq.Dynamic;
using System.Data.Entity;
using System.IO;
using jQueryAjaxInAsp;
using CoolBooks.ViewModels;

namespace CoolBooks.Controllers
{
    public class SearchController : Controller
    {
        //SearchViewModel searchResult = new SearchViewModel();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAllBooks());
        }

        IEnumerable<SearchViewModel> GetAllBooks()
        {
            using (CoolBooksEntities db = new CoolBooksEntities())
            {
                List<Books> booksList = db.Books.ToList();
                SearchViewModel searchVm = new SearchViewModel();
                List<SearchViewModel> searchVmList = booksList.Select(x => new SearchViewModel
                {
                    Title = x.Title,
                    ISBN = x.ISBN,
                    AuthorId = x.AuthorId,
                    AlternativeTitle = x.AlternativeTitle,
                    Created = x.Created,
                    Description = x.Description,
                    FirstName = x.Authors.FirstName,
                    GenreId = x.GenreId,
                    Id = x.Id,
                    ImagePath = x.ImagePath,
                    ImageUpload = x.ImageUpload,
                    IsDeleted = x.IsDeleted,
                    LastName = x.Authors.LastName,
                    Name = x.Genres.Name,
                    Part = x.Part,
                    PublishDate = x.PublishDate,
                    UserId = x.UserId
                }).ToList();
              
                return searchVmList;
            }

        }
        

        public ActionResult BookDetails(int id)
        {
            CoolBooksEntities dc = new CoolBooksEntities();
            Books books = dc.Books.Single(boo => boo.Id == id);
            return View(books);
        }
       
    }

}