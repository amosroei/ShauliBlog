using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShauliBlog.Models
{
    public class Post 
    {
        [Key]
        public int PostID { get; set; }
        public string PostTitle { get; set; }
        public string PostAuthor { get; set; }
        public string PostAuthorWebsite { get; set; }
        public DateTime PostDate { get; set; }
        public string PostText { get; set; }
        //public string Genre { get; set; }
        [Display(Name = "Genre")]
        public int GenreId { get; set; }
        public string PostPicturePath { get; set; }
        public string PostVideoPath { get; set; }
        //[InverseProperty("CommentPost")]
        public virtual ICollection<Comment> PostComments { get; set; }

        public virtual Genre Genre { get; set; }
    }
}