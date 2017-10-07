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
        //public string PostAuthorWebsite { get; set; }
        public DateTime PostDate { get; set; }
        public string PostText { get; set; }
        public string PostPicturePath { get; set; }
        public string PostVideoPath { get; set; }

        public int AccountId { get; set; }

        //[ForeignKey("AccountId")]
        public virtual Account Account { get; set; }

        //[InverseProperty("CommentPost")]
        public virtual ICollection<Comment> PostComments { get; set; }


    }
}