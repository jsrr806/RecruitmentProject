using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using ClassLibrary_Recruitment;

namespace Recruitment_Application.Controllers
{
    public class RequirementController : Controller
    {
        public RequirementController()
        {
            ViewBag.Show = true;
        }

        Class1 cl = new Class1();

        public ActionResult DisplayRequests()
        {
            if (Session["currentUID"] != null && Session["currentEType"].ToString() == "Business Unit Head")
            {
                List<proc_display_requrests_Result> temp = cl.DisplayRequests(Convert.ToInt32(Session["p_bid"]));
                return View(temp);

            }
            return RedirectToAction("Login", "Login");
            //TempData["p_bid"] = p_bid;

            //List<proc_display_requrests_Result> temp = cl.DisplayRequests(p_bid);
            //return View(temp);
        }


        public ActionResult ReqInput()
        {
            if(Session["currentUID"]!=null && Session["currentEType"].ToString()== "Business Unit Head")
            {
                return View();
                
            }
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult ReqInput(int p_javareq, int p_dotnetreq, int p_breq)
        {
            cl.InsertRequest(Convert.ToInt32(Session["p_bid"]), p_javareq, p_dotnetreq, p_breq);
            List<proc_user_details_Result> sender=cl.UserDetails(Convert.ToInt32(Session["currentUID"]));

            MailSender(sender[0].EmailId,"hiringmanager1test@gmail.com",sender[0].EPassword, p_javareq, p_dotnetreq, p_breq,"Hiring Manager","BU");

            return RedirectToAction("DisplayRequests","Requirement");
        }
        public void MailSender(string sender,string receiver,string password, int p_javareq, int p_dotnetreq, int p_breq,string rDesignation,string sDesignation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MailMessage m = new MailMessage();
                    SmtpClient sc = new SmtpClient();

                    try
                    {
                        m.From = new MailAddress(sender);
                        m.To.Add(receiver);
                        m.Subject = "BU"+Session["currentUID"]+ " Resource Request";
                        m.IsBodyHtml = true;
                        m.Body = "Hi, "+rDesignation+ "<br /> Business Unit " + Session["currentUID"] + " Head requires "+ p_javareq+ " Java Developers, " + p_dotnetreq + " DotNet Developers and " + p_breq + "  Business Analysts<br /><br /> Kindly assign the following employees on priority. <br /><br />  Regards. <br /> Business Unit "+Session["currentUID"];
                        if (sDesignation == "Hiring Manager")
                        {
                            m.Subject = sDesignation + " Resource Request";
                            m.Body = "Hi, " + rDesignation + "<br /> Hiring Manager requires " + p_javareq + " Java Developers, " + p_dotnetreq + " DotNet Developers and " + p_breq + "  Business Analysts<br /><br /> Kindly hire this many employees on priority. <br /><br />  Regards. <br />Hiring Manager ";
                        }
                        sc.Host = "smtp.gmail.com";
                        sc.Port = 587;
                        sc.Credentials = new System.Net.NetworkCredential(sender, password);

                        sc.EnableSsl = true;
                        sc.Send(m);
                        Response.Write("Email Send successfully");
                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                ViewBag.inner = e.InnerException;
            }
        }

        public ActionResult GetTeamMembers()
        {
            if (Session["currentUID"] != null && Session["currentEType"].ToString() == "Business Unit Head")
            {
                return View(cl.GetTeamMembers(Convert.ToInt32(Session["p_bid"])));

            }
            return RedirectToAction("Login", "Login");
            
        }

        [HttpPost]
        public ActionResult Search_Team_Employee(string p_text)
        {
            if (Session["currentUID"] != null && Session["currentEType"].ToString() == "Business Unit Head")
            {
                return View(cl.SearchTeamEmployee(p_text, Convert.ToInt32(Session["p_bid"])));

            }
            return RedirectToAction("Login", "Login");
            
        }
    }
}