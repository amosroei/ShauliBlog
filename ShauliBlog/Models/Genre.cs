using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ShauliBlog.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        [Display(Name = "Genre Name")]
        public string GenreName { get; set; }
        [Display(Name = "Posts")]
        public virtual System.Collections.Generic.ICollection<Post> posts { get; set; }
        public virtual System.Collections.Generic.ICollection<Movie> movies { get; set; }
    }
}
