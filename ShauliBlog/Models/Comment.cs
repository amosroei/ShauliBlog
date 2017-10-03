using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShauliBlog.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        public int PostID { get; set; }
        public string CommentTitle { get; set; }
        public string CommentAuthor { get; set; }
        public string CommentAuthorWebsite { get; set; }
        public string CommentText { get; set; }

        public DateTime CommentDate { get; set; }
        public virtual Post CommentPost { get; set; }
    }

}