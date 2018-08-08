using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoolBooks.Models;

namespace CoolBooks.ViewModels
{
    public class BookReviewViewModel
    {
        public Books Books { get; set; }
        public Reviews Reviews { get; set; }

    }
}