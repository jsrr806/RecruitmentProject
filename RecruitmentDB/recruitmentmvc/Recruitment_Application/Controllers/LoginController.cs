using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassLibrary_Recruitment;

namespace Recruitment_Application.Controllers
{
    public class LoginController : Controller
    {
        public LoginController()
        {
            ViewBag.Show = false;
        }

        Class1 cl = new Class1();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string p_etype, int p_eid, string p_pass)
        { 
            int? temp = cl.LoginSignIn(p_eid, p_pass, p_etype);
            Session["currentUID"] = null;
            Session["currentEType"] = null;
            if (temp == 1 && p_etype == "Hiring Manager")
            {
                Session["currentUID"] = p_eid;
                Session["currentEType"] = p_etype;
                return RedirectToAction("ReqAllotment", "HiringManager");
            }
                
            else if (temp == 1 && p_etype == "Business Unit Head")
            {
                Session["p_bid"] = p_eid;
                Session["currentUID"] = p_eid;
                Session["currentEType"] = p_etype;
                return RedirectToAction("DisplayRequests", "Requirement");
            }
            else if (temp == 1 && p_etype == "HR")
                return RedirectToAction("Login", "Login");
            else return RedirectToAction("LoginInvalid", "Login");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            //Session.Abandon();
            return RedirectToAction("Login", "Login");
        }

        public ActionResult LoginInvalid()
        {
            return View();
        }
    }
}