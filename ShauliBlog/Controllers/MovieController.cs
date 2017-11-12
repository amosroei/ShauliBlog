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

        public ActionResult Index(string SearchMovieName, string SearchDirectorName, string SearchYear)
        {
            // Redirects unlogged user to the login page            
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // checks if any search fields were filled
                if ((SearchMovieName != null && SearchMovieName != string.Empty) ||
                    (SearchDirectorName != null && SearchDirectorName != string.Empty) ||
                    (SearchYear != null && SearchYear != string.Empty))
                {
                    // first, filters by movie name and director name
                    List<Movie> filteredMovies = db.Movie.Where(m => m.MovieName.Contains(SearchMovieName) &&
                    m.DirectorName.Contains(SearchDirectorName)).ToList();

                    int filterYear;

                    // try to convert search year to integer and filtered the movies
                    if (SearchYear != string.Empty && int.TryParse(SearchYear, out filterYear))
                    {
                        filteredMovies = filteredMovies.Where(m => m.ReleaseYear == filterYear).ToList();
                    }

                    return View(filteredMovies);
                }

                // if not, return all movies in db
                else
                {
                    return View(db.Movie.ToList());
                }                
            }
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
        // call when prase the button of create movie
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
            ViewBag.GenreItems = new SelectList(db.Genre, "GenreId", "GenreName");
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
        // delete the specific movie id
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

        // group the movies by a genre 
        public ActionResult Statistics()
        {
            // group by the genre
            var query = from i in db.Movie
                        group i by i.Genre into g
                        select new { Genre = g.Key.GenreName, c = g.Count() };
            return View(query.ToList());
        }
    }
}