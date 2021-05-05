using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Windows;
using WebApplication1.Models;
//using System.Windows.Forms;

namespace WebApplication1.Controllers
{
    public class AgentsController : Controller
    {
        // GET: Agents
        private lendingTreeEntities1 db = new lendingTreeEntities1();
        private EncryptPassword encryptPassword = new EncryptPassword();
        //public JsonResult IsUserExists(string agentId)
        //{
        //    //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
        //    return Json(!db.Agents.Any(x => x.AgentId == AgentId), JsonRequestBehavior.AllowGet);
        //}

        [Authorize(Roles = "Agent")]
        public ActionResult AgentAccount()
        {
            return View();
        }
        public ActionResult Notification()
        {
            return View();
        }




        public ActionResult AgentCreate()
        {

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgentCreate([Bind(Include = "FirstName,LastName,DoB,Gender,ContactNumber,DepartmentId,AgentId,Password,ConfirmPassword")] Agent agent)
        {
            if (ModelState.IsValid)
            {
                if (!db.Agents.Any(x => x.AgentId == agent.AgentId))
                {
                    var passkey = encryptPassword.Encode(agent.A_Password);
                    agent.A_Password = passkey;
                    db.Agents.Add(agent);
                    db.SaveChanges();
                    MessageBox.Show("New Agent Created Successfully");
                    return RedirectToAction("AgentHome", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Agent Name / ID already exists");
                    return View(agent);
                }
            }
            return View(agent);
        }



        public ActionResult AgentLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgentLogin([Bind(Include = "AgentId,Password")] Agent agent)
        {
            if (agent.AgentId != null)
            {
                if (agent.A_Password != null)
                {
                    string password = encryptPassword.Encode(agent.A_Password);
                    if (db.Agents.Any(b => b.AgentId == agent.AgentId && b.A_Password == password))
                    {
                        FormsAuthentication.SetAuthCookie(agent.AgentId.ToString(), false);
                        return RedirectToAction("AgentAccount", "Agents");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Agent Name / Password is Incorrect");
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



        public ActionResult AgentLogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("AgentHome", "Home");
        }

    }
}