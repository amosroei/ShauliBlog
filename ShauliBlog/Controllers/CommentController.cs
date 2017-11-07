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
            // deletes the comment only if the user is admin
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
            if ((ModelState.IsValid)&& (comment.Account != null))
            {
                // sets the comment date to the current date and time
                comment.CommentDate = DateTime.Now;
                comment.Account = db.Account.FirstOrDefault(a => a.UserId == comment.AccountId);
                db.Comment.Add(comment);
                db.SaveChanges();
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

    }
}