using ShauliBlog.Models;
using System;
using System.Linq;
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

        
        public void Delete(long ?id)
        {
            // find the comment by id
            if (id != null)
            {
                Comment Comment = db.Comment.Find(id);
                
                // Checks if the comment belongs to the user and deletes and comment
                if ((Comment != null) &&
                    (((ShauliBlog.Models.Account)Session["user"]).IsAdmin ||
                    (((ShauliBlog.Models.Account)Session["user"]).UserId == Comment.AccountId)))
                {
                    db.Comment.Remove(Comment);
                    db.SaveChanges();
                }
            }         
        }

        public ActionResult Create(Comment comment)
        {
            // check if the ccomment is valid
            if (ModelState.IsValid)
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