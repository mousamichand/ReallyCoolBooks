using CoolBooks.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CoolBooks.ViewModels
{
    public class SearchViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        [DisplayName("Genre")]
        //public virtual ICollection<Genres> Genres { get; set; }
        public string Name { get; set; }
        //public string Text { get; set; }
        [DisplayName("Author")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        public string AlternativeTitle { get; set; }
        public Nullable<short> Part { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public Nullable<System.DateTime> PublishDate { get; set; }
        [DataType(DataType.Upload)]
        [DisplayName("Image")]
        public string ImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }
        public System.DateTime Created { get; set; }
        public bool IsDeleted { get; set; }

       
       
    }
}