using ShauliBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity;

namespace ShauliBlog.Controllers
{
    public class MovieController : Controller
    {
        private BlogDBContext db = new BlogDBContext();

        public ViewResult Index(string SearchMovieName, string SearchDirectorName, string SearchYear)
        {
            // constructs select query using the given parametes
            List<Movie> movies;

            // generates the search query using the given parameters
            String query = "select * from movies where {0}";
            string select = "";
            string where = "";

            if (!String.IsNullOrEmpty(SearchMovieName))
            {
                select += "MovieName,";
                where += "MovieName like '%" + SearchMovieName + "%'";
            }

            if (!String.IsNullOrEmpty(SearchDirectorName))
            {
                select += "DirectorName ,";

                if (!String.IsNullOrEmpty(where))
                {
                    where += "and ";
                }
                where += "DirectorName like '%" + SearchDirectorName + "%'";
            }


            if (!String.IsNullOrEmpty(SearchYear))
            {
                select += "ReleaseYear ,";
                if (!String.IsNullOrEmpty(where))
                {
                    where += "and ";
                }
                where += "ReleaseYear like '%" + SearchYear + "%'";
            }
            if (where == "")
            {
                // removes "where" from the end of the query
                query = query.Substring(0, query.Length - 10);
            }

            // returns the matching movies
            query = String.Format(query, where);
            movies = (List<Movie>)db.Movie.SqlQuery(query).ToList();
            return View(movies.ToList());
        }


        // GET: Movie/Details/5
        public ActionResult Details(int? id)
        {
            // returns bad request message if id is null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // finds the movie by the given id
            Movie movie = db.Movie.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: Movie/Create
        public ActionResult Create()
        {
            // Fills the genreitems to be used in the client side
            ViewBag.GenreItems = new SelectList(db.Genre, "GenreId", "GenreName");
            return View();
        }

        // POST: Movie/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Movie.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // GET: Movie/Edit/5
        public ActionResult Edit(int? id)
        {
            // returns bad request message if id is null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // finds the movie by the given id
            Movie movie = db.Movie.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            ViewBag.GenreItems = new SelectList(db.Genre, "GenreId", "GenreName");
            return View(movie);
        }

        // POST: Movie/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                // sets state to modified
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movie/Delete/5
        public ActionResult Delete(int? id)
        {
            // check if id exists, and deletes the movie entity
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movie.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movie.Find(id);
            db.Movie.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Statistics()
        {
            // group by the year
            var query = from i in db.Movie
                        group i by i.ReleaseYear into g
                        select new { Year = g.Key, c = g.Count() };
            return View(query.ToList());
        }
    }
}