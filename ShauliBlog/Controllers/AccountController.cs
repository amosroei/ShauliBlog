using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using ShauliBlog.Models;
using System.Net;
using System.Data.Entity;
using System.Collections;
using static ShauliBlog.Models.ApplicationDbContext;

namespace ShauliBlog.Controllers
{

    public class AccountController : Controller
    {
        private BlogDBContext db = new BlogDBContext();
        // GET: Account
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {

                //shuld print into a text box--> " Admin only! login before"
                return RedirectToAction("Login");
            }
            else if (((ShauliBlog.Models.Account)Session["user"]).IsAdmin)
            {
                using (BlogDBContext db = new BlogDBContext())
                {
                    var accounts = from s in db.Account select s;

                    return View(accounts.ToList());
                }
            }
            else
            {
                String s = "You don't have premission. To return home page please click HomePage";
                Console.WriteLine(s);

                return RedirectToAction("Home", "Posts");

            }
        }





        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Account account)
        {
            if (ModelState.IsValid)
            {
                using (BlogDBContext db = new BlogDBContext())
                {
                    db.Account.Add(account);

                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.FirstName + " " + account.LastName + " successfully registered ";
            }
            return View();
        }
        //login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Account user)
        {
            using (BlogDBContext db = new BlogDBContext())
            {
                if ((user.UserName) == "admin" && (user.Password) == "1234")
                {

                    user.IsAdmin = true;
                    user.UserId = 5;
                    user.UserName = "admin";
                    Session["IsAdmin"] = user.IsAdmin.ToString();
                    Session["UserID"] = user.UserId.ToString();
                    Session["UserName"] = user.UserName.ToString();
                    Session["User"] = user;
                    return RedirectToAction("Index", "Post");
                    //return RedirectToAction("LoggedIn");
                }
                var usr = db.Account.SingleOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);

                if (usr != null)

                {
                    Session["UserID"] = usr.UserId.ToString();
                    Session["UserName"] = usr.UserName.ToString();
                    Session["User"] = usr;


                    //return RedirectToAction("LoggedIn");
                    return RedirectToAction("Index", "Post");
                }
                else
                {
                    ModelState.AddModelError("", "UserName or Password is incorrect");
                }
            }
            return View();
        }

        public ActionResult LoggedIn()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }

            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (BlogDBContext db = new BlogDBContext())
            {
                Account user = db.Account.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (BlogDBContext db = new BlogDBContext())
            {
                Account user = db.Account.Find(id);
                db.Account.Remove(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (BlogDBContext db = new BlogDBContext())
            {
                Account user = db.Account.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (BlogDBContext db = new BlogDBContext())
            {
                Account user = db.Account.Find(id);

                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,FirstName,LastName,Email,UserName,Password,ComfirmPassword,IsAdmin")] Account user)
        {
            if (ModelState.IsValid)
            {
                using (BlogDBContext db = new BlogDBContext())
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }

        

        public ActionResult LogOut()
        {
            Session["UserID"] = null;
            Session.Clear();
            return RedirectToAction("Login");
            //return RedirectToAction("Home", "Posts");
        }

    }
}
