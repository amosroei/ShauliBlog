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
            if ((((ShauliBlog.Models.Account)Session["user"]).IsAdmin) &&(Comment != null))
            {
                db.Comment.Remove(Comment);
                db.SaveChanges();
            }
        }



        public ActionResult Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                //now's date
                comment.CommentDate = DateTime.Now;
                comment.Account = db.Account.FirstOrDefault(a => a.UserId == comment.AccountId);
                db.Comment.Add(comment);
                db.SaveChanges();
            }
            comment.Account = db.Account.FirstOrDefault(a => a.UserId == comment.AccountId);

            return Json(new
            {
                PostId = comment.PostId,
                //Post = comment.CommentPost,            
                //CommentAuthor = comment.CommentAuthor,
                Account = comment.Account,
                CommentText = comment.CommentText,

                //Text = comment.CommentText,                                
                Date = comment.CommentDate.ToString(),
                id = comment.CommentID
            });
        }

    }
}