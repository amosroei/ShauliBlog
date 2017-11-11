//using ShauliBlog.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using System.Net;
//using System.Data.Entity;

//namespace ShauliBlog.Controllers
//{
//    public class FansController : Controller
//    {
//        private BlogDBContext db = new BlogDBContext();

//        public ActionResult Index(string SearchFirst, string SearchLast, string SearchGender)
//        {

//            // Redirects unlogged user to the login page            
//            if (Session["UserId"] == null)
//            {
//                return RedirectToAction("Login", "Account");
//            }
//            else
//            {
//                // constructs select query using the given parametes
//                List<Fan> fans;

//                // generates the search query using the given parameters
//                String query = "select * from fans where {0}";
//                string select = "";
//                string where = "";

//                if (!String.IsNullOrEmpty(SearchFirst))
//                {
//                    select += "FirstName,";
//                    where += "FirstName like '%" + SearchFirst + "%'";
//                }

//                if (!String.IsNullOrEmpty(SearchLast))
//                {
//                    select += "LastName ,";

//                    if (!String.IsNullOrEmpty(where))
//                    {
//                        where += "and ";
//                    }
//                    where += "LastName like '%" + SearchLast + "%'";
//                }


//                if (!String.IsNullOrEmpty(SearchGender))
//                {
//                    select += "gender ,";
//                    if (!String.IsNullOrEmpty(where))
//                    {
//                        where += "and ";
//                    }
//                    where += "gender like '%" + SearchGender + "%'";
//                }
//                if (where == "")
//                {
//                    // removes "where" from the end of the query
//                    query = query.Substring(0, query.Length - 10);
//                }

//                // returns the matching fans
//                query = String.Format(query, where);
//                fans = (List<Fan>)db.Fan.SqlQuery(query).ToList();
//                return View(fans.ToList());
//            }
//        }

//        // GET: Fans/Details/5
//        public ActionResult Details(int? id)
//        {
//            // returns bad request message if id is null
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            // finds the fan by the given id
//            Fan fan = db.Fan.Find(id);
//            if (fan == null)
//            {
//                return HttpNotFound();
//            }
//            return View(fan);
//        }

//        // GET: Fans/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: Fans/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "FanID,FirstName,LastName,Gender,BirthDate,Seniority")] Fan fan)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Fan.Add(fan);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(fan);
//        }

//        // GET: Fans/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            // returns bad request message if id is null
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            // finds the fan by the given id
//            Fan fan = db.Fan.Find(id);
//            if (fan == null)
//            {
//                return HttpNotFound();
//            }
//            return View(fan);
//        }

//        // POST: Fans/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "FanID,FirstName,LastName,Gender,BirthDate,Seniority")] Fan fan)
//        {
//            if (ModelState.IsValid)
//            {
//                // sets state to modified
//                db.Entry(fan).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(fan);
//        }

//        // GET: Fans/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            // check if id exists, and deletes the fan entity
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Fan fan = db.Fan.Find(id);
//            if (fan == null)
//            {
//                return HttpNotFound();
//            }
//            return View(fan);
//        }

//        // POST: Fans/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            Fan fan = db.Fan.Find(id);
//            db.Fan.Remove(fan);
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

//        public ActionResult Statistics()
//        {
//            // group by the genders
//            var query = from i in db.Fan
//                        group i by i.Gender into g
//                        select new { Gender = g.Key, c = g.Count() };
//            return View(query.ToList());
//        }
//    }
//}