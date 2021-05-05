using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
//using System.Windows.Forms;
using System.Web.Security;
using System.Windows;
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
        
        [HttpGet]
        public ActionResult ForgotUserId()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotUserId(ForgotUserId ob)
        {
            string message = "";
            bool Status = false;
            if (ModelState.IsValid)
            {
                var data = db.Users.FirstOrDefault(x => x.ContactNumber == ob.ContactNumber);
                if(data != null)
                {
                    if (string.Compare(ob.Ques1, data.Q1) == 0 && string.Compare(ob.Ques2, data.Q2) == 0 && string.Compare(ob.Ques3, data.Q3) == 0)
                    {
                        Status = true;
                        message = $"User ID is {data.UserId} ";
                    }
                    else
                    {
                        message = "Wrong Answers to the Questions";
                    }
                }
                else
                {
                    message = "Wrong Contact Number";
                }
            }
            ViewBag.Status = Status;
            ViewBag.Message = message;
            return View(ob);
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPassword ob)
        {
            string message = "";
            if (ModelState.IsValid)
            {
                
                var data = db.Users.FirstOrDefault(x => x.UserId == ob.UserId);
                if (data != null)
                {
                    if (string.Compare(ob.Ques1, data.Q1) == 0 && string.Compare(ob.Ques2, data.Q2) == 0 && string.Compare(ob.Ques3, data.Q3) == 0)
                    {
                        return RedirectToAction("ResetPassword", new { UserId = data.UserId});
                    }
                    else
                    {
                        message = "Wrong Answers to the Questions";
                    }
                }
                else
                {
                    message = "User ID does not Exist";
                }
            }
            ViewBag.Message = message;
            return View(ob);
        }

        [HttpGet]
        public ActionResult ResetPassword(string UserId)
        {
            return View(UserId);
        }

        [HttpPost]
        public ActionResult ResetPassword(string UserId, ResetPassword ob)
        {
            string message = "";
            if(ModelState.IsValid)
            {
                var data = db.Users.Find(UserId);
                data.password = ob.NewPassword;
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                message = "Password Reset Sucessfull !!!";
            }
            ViewBag.Message = message;
            return View(ob);
        }
    }
}
