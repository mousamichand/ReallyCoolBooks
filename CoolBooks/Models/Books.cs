//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoolBooks.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Books
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Books()
        {
            this.Reviews = new HashSet<Reviews>();
        }
    
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        public string AlternativeTitle { get; set; }
        public Nullable<short> Part { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public Nullable<System.DateTime> PublishDate { get; set; }
        [DataType(DataType.Upload)]
        public string ImagePath { get; set; }
        public System.DateTime Created { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Authors Authors { get; set; }
        // public virtual ICollection<Genres> Genres { get; set; }
        public virtual Genres Genres { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reviews> Reviews { get; set; }
       // public Books Books { get; internal set; }
    }
}
