using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ShauliBlog.Models
{
    public class Movie
    {
        [Key]
        public int MovieID { get; set; }
        public string MovieName { get; set; }
        public string DirectorName{ get; set; }
        public int ReleaseYear{ get; set; }

        [Display(Name = "Genre")]
        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
