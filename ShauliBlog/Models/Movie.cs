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
        public string MovieDirectorName{ get; set; }
        public int MovieReleaseYear{ get; set; }
        public int MovieGenreId { get; set; }        
    }
}
