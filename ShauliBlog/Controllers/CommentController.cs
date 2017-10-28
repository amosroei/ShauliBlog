using ShauliBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShauliBlog.Controllers
{
    public class CommentController : Controller
    {
        BlogDBContext db = new BlogDBContext();
        //
        // GET: /Comment/
        public ActionResult Index()
        {
            return View();
        }

        public void Delete(long id = 0)
        {
            Comment Comment = db.Comment.Find(id);

            // Checks if the user is admin and deletes and comment
            if ((((ShauliBlog.Models.Account)Session["user"]).IsAdmin) &&(Comment != null))
            {
                // deletes the comment
                db.Comment.Remove(Comment);
                db.SaveChanges();
            }            
        }

        public ActionResult Create(Comment comment)
        {
            // check if the ccomment is valid
            if (ModelState.IsValid)
            {
                // sets the comment date to the current date and time
                comment.CommentDate = DateTime.Now;

                db.Comment.Add(comment);
                db.SaveChanges();
                //return RedirectToAction("Index");
            }
            
            // sets the comment author as the account
            comment.Account = db.Account.FirstOrDefault(a => a.UserId == comment.AccountId);

            return Json(new
            {
                PostId = comment.PostId,
                Account = comment.Account,
                CommentText = comment.CommentText,

                Date = comment.CommentDate.ToString(),
                id = comment.CommentID
            });
        }

        //[HttpPost]
        //public ActionResult AddComment(NewComment newComment)
        //{
        //    Comment comment = new Comment();

        //    comment.CommentPost = this.db.Post.Find(newComment.PostId);

        //    int userId;

        //    if (int.TryParse(newComment.UserId, out userId))
        //    {
        //        //comment.CommentAuthor = this.db.user.Find(userId);
        //    }
        //    else
        //    {
        //        comment.CommentTitle = newComment.Name;
        //    }

        //    comment.CommentText = newComment.Text;
        //    //comment.Date = DateTime.Now;

        //    this.db.Comment.Add(comment);
        //    this.db.SaveChanges();

        //    return Json(new
        //    {
        //        Text = comment.CommentText,
        //        PostId = comment.CommentPost.PostID,
        //        //User = comment.User != null ? comment.User.Username : comment.Name,
        //        //Date = comment.Date.ToString(),
        //        id = comment.CommentID
        //    });
        //}
    }
}