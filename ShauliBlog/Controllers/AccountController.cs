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
        // account managment - show all users account 
        public ActionResult Index()
        {
            // Redirects unlogged user to the login page            
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login");
            }
            else if (((ShauliBlog.Models.Account)Session["user"]).IsAdmin)
            {
                using (BlogDBContext db = new BlogDBContext())
                {
                    //select the users account from the DB
                    var accounts = from s in db.Account select s;

                    return View(accounts.ToList());
                }
            }
            else
            {
                // if the user isnt admin - show the posts page
                return RedirectToAction("Index", "Post");
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
            return RedirectToAction("Login");
        }
        //login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        // Called when the user is trying to login
        public ActionResult Login(Account user)
        {
            using (BlogDBContext db = new BlogDBContext())
            {
                
                //regular user
                var usr = db.Account.SingleOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);

                if (usr != null)
                {
                    Session["UserID"] = usr.UserId.ToString();
                    Session["UserName"] = usr.UserName.ToString();
                    Session["User"] = usr;

                    return RedirectToAction("Index", "Post");
                }
                else
                {
                    ModelState.AddModelError("", "UserName or Password is incorrect");
                }
            }
            return View();
        }

        //Login
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

        //delete user
        public ActionResult Delete(int? id)
        {           
            // if id is null, or trying to delete the logged user, prevent action
            if (id == null || ((ShauliBlog.Models.Account)Session["user"]).UserId == id)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Index", "Account");
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
        public ActionResult Edit([Bind(Include = "UserID,FirstName,LastName,Email,UserName,Password,ComfirmPassword,Website,IsAdmin")] Account user)
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

        
        //logout
        public ActionResult LogOut()
        {
            Session["UserID"] = null;
            Session.Clear();
            return RedirectToAction("Login");
        }

    }
}
