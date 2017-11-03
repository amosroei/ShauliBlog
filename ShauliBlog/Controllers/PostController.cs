using ShauliBlog.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShauliBlog.Controllers
{
    /// <summary>
    ///  ד
    /// </summary>
    public class PostController : Controller
    {
        private BlogDBContext db = new BlogDBContext();

        //public ActionResult Index()
        //{

        //    if (Session["UserId"] == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    else if (((ShauliBlog.Models.Account)Session["User"]).IsAdmin)
        //    {

        //        var posts = from s in db.Post select s;
        //        ViewBag.TotalPosts = db.Post.Count();
        //        ViewBag.TotalComments = db.Comment.Count();
        //        // ViewBag.TotalAccounts = db.userAccounts.Count();
        //        ViewBag.TotalFans = db.Fan.Count();

        //        return View(posts.ToList());
        //    }
        //    else
        //    {

        //        return RedirectToAction("Index", "Post");

        //    }
        //}

        // GET: Post
        //[HttpPost]

        public ActionResult Index(string SearchTitle, string SearchAuthor)
        {
            // Redirects unlogged user to the login page            
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {

                // Set site statistics properties
                ViewBag.TotalPosts = db.Post.Count();
                ViewBag.TotalComments = db.Comment.Count();
                ViewBag.TotalAccounts = db.Account.Count();
                ViewBag.TotalFans = db.Fan.Count();

                List<Post> posts;

                // generates the search query using the given parameters
                String query = "select * from posts where {0}";
                string select = "";
                string where = "";

                if (!String.IsNullOrEmpty(SearchTitle))
                {
                    select += "PostTitle,";
                    where += "PostTitle like '%" + SearchTitle + "%'";
                }
                if (!String.IsNullOrEmpty(SearchAuthor))
                {
                    select += "PostAuthor ,";

                    if (!String.IsNullOrEmpty(where))
                    {
                        where += "and ";
                    }
                    where += "PostAuthor like '%" + SearchAuthor + "%'";
                }
                
                if (where == "")
                {
                    // removes "where" from the end of the query
                    query = query.Substring(0, query.Length - 10);
                }

                query = String.Format(query, where);

                // returns the matching posts
                posts = (List<Post>)db.Post.SqlQuery(query).ToList();
                return View(posts.ToList());
            }

        }
        
        //
        // GET: /Post/Details/5

        public ActionResult Details(int? id)
        {
            // returns bad request message if id is null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // finds the post by the given id
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //
        // GET: /Post/Create
        public ActionResult Create()
        {
            // Fills the genreitems to be used in the client side
            ViewBag.GenreItems = new SelectList(db.Genre, "GenreId", "GenreName");
            return View();
        }

        //
        // POST: /Post/Create

        [HttpPost]
        public ActionResult Create(Post post)
        {
            // Saves the post to the db
            if (ModelState.IsValid)
            {
                string AttachmentPath = string.Empty;
                bool isSavedSuccessfully = true;
                string fName = "";
                try
                {
                    foreach (string fileName in Request.Files)
                    {
                        HttpPostedFileBase file = Request.Files[fileName];

                        //Save file content goes here
                        fName = file.FileName;
                        if (file != null && file.ContentLength > 0)
                        {
                            string targetFolder = "videos";

                            if (fileName == "image")
                            {
                                targetFolder = "images";
                                post.PostPicturePath = fName;
                            }
                            else
                            {
                                post.PostVideoPath = fName;
                            }

                            //choose a directory to save in : images or videos
                            var originalDirectory = new DirectoryInfo(string.Format("{0}{1}", Server.MapPath(@"\"), targetFolder));

                            string pathString = originalDirectory.ToString();

                            // save the name of a file
                            var fileName1 = Path.GetFileName(file.FileName);

                            bool isExists = System.IO.Directory.Exists(pathString);

                            // create the directory if not exists
                            if (!isExists)
                            {
                                System.IO.Directory.CreateDirectory(pathString);
                            }

                            // save the file in a relative path
                            AttachmentPath = string.Format("{0}\\{1}", pathString, file.FileName);
                            file.SaveAs(AttachmentPath);

                        }
                    }

                }
                catch (Exception ex)
                {
                    isSavedSuccessfully = false;
                }


                if (isSavedSuccessfully)
                {
                    post.Account = db.Account.FirstOrDefault(a => a.UserId == post.AccountId);

                    post.PostDate = DateTime.Now;

                    // save the post to DB
                    db.Post.Add(post);
                    db.SaveChanges();

                    // Run the Apriori algorithm on the data include the new post added to the db
                    var controller = DependencyResolver.Current.GetService<AprioriAlgorithmController>();
                    controller.newDataAddedToDb();

                    return RedirectToAction("Index");
                }
            }

            return View(post);
        }

        //
        // GET: /Post/Edit/5
        public ActionResult Edit(int? id)
        {
            // returns bad request message if id is null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // finds the post by the given id
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //
        // POST: /Post/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                post.PostDate = DateTime.Now;
                // sets state to modified
                db.Entry(post).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        //
        // GET: /Post/Delete/5

        public ActionResult Delete(int? id)
        {
            // returns bad request message if id is null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // finds the post by the given id
            Post post = db.Post.Find(id);

            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //
        // POST: /Post/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // delete the post only if the user is admin
            bool isAdmin = (Boolean)((ShauliBlog.Models.Account)Session["user"]).IsAdmin;
            if (!isAdmin)
            {
                return RedirectToAction("Index");
            }
            Post post = db.Post.Find(id);
            db.Post.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Statistics()
        {
            // group by the users posts
            var query = from i in db.Post
                        group i by i.Account.UserName into g
                        select new { UserName = g.Key, c = g.Count() };
           
            return View(query.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IsImage(string fileType)
        {
            // check the image type
            if (fileType == "image/jpg" || fileType == "image/png")
                return true;

            return false;
        }

    }
}