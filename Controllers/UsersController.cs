using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows;
//using System.Windows.Forms;
using System.Web.Security;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class UsersController : Controller
    {
        private lendingTreeEntities1 db = new lendingTreeEntities1();

      //  private EncryptPassword ep = new EncryptPassword();

        //GET: Users1
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

      
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,Dob,Gender,ContactNumber,Email,UserId,password,Category,Q1,Q2,Q3,A1,A2,A3")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                MessageBox.Show("New User Created Successfully");
                
                return RedirectToAction("Index","Home");
            }
            else {
                throw new Exception();
            }

            return View(user);
        }

        [HttpGet]
        public ActionResult UserLogin()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserLogin([Bind(Include = "UserId,Password")] User user)
        {
            //if (ModelState.IsValid)
            //{
            if (user.UserId != null)
            {
                if (user.password != null)
                {
                    string password = (user.password);
                    if (db.Users.Any(b => b.UserId.Equals(user.UserId, StringComparison.InvariantCultureIgnoreCase) && b.password.Equals(password, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        ViewBag.Username = User.Identity.Name;
                        FormsAuthentication.SetAuthCookie(user.UserId, false);
                        return RedirectToAction("UserAccount");
                    }
                    else
                    {
                        ModelState.AddModelError("", "User Name / Password is Incorrect");
                        return View();
                    }
                }

                else
                {
                    ModelState.AddModelError("", "Please enter Log In credentials");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Please enter Log In credentials");
                return View();
            }
        }

        
        public ActionResult UserAccount()
        {
            return View();
        }


        // GET: Users1/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FirstName,LastName,Dob,Gender,ContactNumber,Email,UserId,password,Category,Q1,Q2,Q3,A1,A2,A3")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users1/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
    }
}
