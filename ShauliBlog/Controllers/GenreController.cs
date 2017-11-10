//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using ShauliBlog;
//using ShauliBlog.Models;


////KUSHI
//namespace Blogi.Controllers
//{
//    public class CategoriesController : Controller
//    {
//        private BlogDBContext db = new BlogDBContext();

//        // GET: Categories
//        public ActionResult Index()
//        {
//            return View(db.Genre.ToList());
//        }

//        // GET: Categories/Details/5
//        public ActionResult Details(int? id)
//        {
//            // returns bad request message if id is null
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            // finds the genre by the given id
//            Genre genre = db.Genre.Find(id);
//            if (genre == null)
//            {
//                return HttpNotFound();
//            }
//            return View(genre);
//        }

//        // GET: Categories/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: Categories/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "GenreID,GenreName")] Genre genre)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Genre.Add(genre);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(genre);
//        }

//        // GET: Categories/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            // returns bad request message if id is null
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            // finds the genre by the given id
//            Genre genre = db.Genre.Find(id);
//            if (genre == null)
//            {
//                return HttpNotFound();
//            }
//            return View(genre);
//        }

//        // POST: genre/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "GenreID,GenreName")] Genre genre)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(genre).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(genre);
//        }

//        // GET: genre/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            // returns bad request message if id is null
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            // finds the genre by the given id
//            Genre genre = db.Genre.Find(id);
//            if (genre == null)
//            {
//                return HttpNotFound();
//            }
//            return View(genre);
//        }

//        // POST: Genre/Delete/5
//        // TODO : check if needed
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {

//            Genre genre = db.Genre.Find(id);
//            db.Genre.Remove(genre);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
