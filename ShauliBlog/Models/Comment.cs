using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShauliBlog.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
      
        public string CommentTitle { get; set; }

        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
        public string CommentAuthorWebsite { get; set; }
        public string CommentText { get; set; }

        public DateTime CommentDate { get; set; }

        public int PostId { get; set; }

        [ForeignKey("PostId")]
        public virtual Post CommentPost { get; set; }
    }

}