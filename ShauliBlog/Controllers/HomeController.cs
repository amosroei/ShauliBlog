using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShauliBlog.Models;

namespace ShauliBlog.Controllers
{
    public class HomeController : Controller
    {
        private BlogDBContext db = new BlogDBContext();
        public ActionResult Index()
        {
            return View("index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // Class to JOIN between Comments table and Posts table
        public class CommentsPosts
        {
            public int CommentID { get; set; }
            //public string CommentTitle { get; set; }
            public string Writer { get; set; }
            public string PostTitle { get; set; }
        };

        public ActionResult JoinQueryCommentsPosts()
        {
            // JOIN between Comments and Posts
            var comments = db.Comment.Join(db.Post,
                        c => c.PostId,
                        p => p.PostID,
                        (c, p) =>
                         new
                         {
                             CommentID = c.CommentID,
                             Writer = c.Account.UserName,
                             PostTitle = p.PostTitle
                         });

            var co = new List<CommentsPosts>();
            foreach (var t in co)
            {
                co.Add(new CommentsPosts()
                {
                    CommentID = t.CommentID,
                    Writer = t.Writer,
                    PostTitle = t.PostTitle
                });
            }

            return View(co);
        }

        // Class to JOIN between Posts table and Categories table
        public class PostsGenre
        {
            public int PostID { get; set; }
            public string PostTitle { get; set; }
            public string PostGenre { get; set; }
        };

        public ActionResult JoinQueryPostCat()
        {
            // JOIN between Posts and Genres
            var posts = db.Post.Join(db.Genre,
                                        p => p.GenreId,
                                        c => c.GenreId,
                                        (p, c) =>
                                         new
                                         {
                                             PostID = p.PostID,
                                             PostTitle = p.PostTitle,
                                             PostGenre = c.GenreName
                                         });


            var po = new List<PostsGenre>();
            foreach (var t in po)
            {
                po.Add(new PostsGenre()
                {
                    PostID = t.PostID,
                    PostTitle = t.PostTitle,
                    PostGenre = t.PostGenre
                });
            }
            return View(po);
        }

        // Class to JOIN between Posts table and Movies table
        public class PostsMovies
        {
            public int GenreID { get; set; }
            public string PostTitle { get; set; }
            public string MovieName { get; set; }
            
        };

        public ActionResult JoinQueryPostMovie()
        {
            // JOIN between Posts and Movies by genre
            var posts = db.Post.Join(db.Movie,
                                        p => p.GenreId,
                                        m => m.Genre,
                                        (p, m) =>
                                         new
                                         {
                                             MovieName = m.MovieName,
                                             PostTitle = p.PostTitle
                                         });


            var pm = new List<PostsMovies>();
            foreach (var t in pm)
            {
                pm.Add(new PostsMovies()
                {
                    PostTitle = t.PostTitle,
                    MovieName = t.MovieName
                });
            }
            return View(pm);
        }
    }
}

    
