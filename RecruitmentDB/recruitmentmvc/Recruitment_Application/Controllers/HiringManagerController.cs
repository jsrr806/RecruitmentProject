using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using ClassLibrary_Recruitment;

namespace Recruitment_Application.Controllers
{
    public class HiringManagerController : Controller
    {
        Class1 cl = new Class1();


        public HiringManagerController()
        {
            //ViewBag.Show = true;
        }
        // GET: HiringManager
        
        public ActionResult ReqAllotment()
        {
            if(Session["currentUID"]!=null && Session["currentEType"].ToString()== "Hiring Manager")
            {
                List<int> result = cl.ToBeHired();
                Session["p_hireJava"] = result[0];
                Session["p_hireDotNet"] = result[1];
                Session["p_hireBA"] = result[2];
                return View(cl.DisplayPendingRequests());
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult UpdateHRTable()
        {
            if (Session["currentUID"] != null && Session["currentEType"].ToString() == "Hiring Manager")
            {
                cl.UpdateHRTable(Convert.ToInt32(Session["p_hireJava"]), Convert.ToInt32(Session["p_hireDotNet"]), Convert.ToInt32(Session["p_hireBA"]));
                List<proc_user_details_Result> sender = cl.UserDetails(Convert.ToInt32(Session["currentUID"]));
                MailSender(sender[0].EmailId, "hrunit.test@gmail.com", sender[0].EPassword, Convert.ToInt32(Session["p_hireJava"]), Convert.ToInt32(Session["p_hireDotNet"]), Convert.ToInt32(Session["p_hireBA"]), "HR", "Hiring Manager");
                return RedirectToAction("ReqAllotment");
            }


            return RedirectToAction("Login", "Login");
        }

        public void MailSender(string sender, string receiver, string password, int p_javareq, int p_dotnetreq, int p_breq, string rDesignation, string sDesignation)
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
                        m.Subject = "BU" + Session["currentUID"] + " Resource Request";
                        m.IsBodyHtml = true;
                        m.Body = "Hi, " + rDesignation + "<br /> Business Unit " + Session["currentUID"] + " Head requires " + p_javareq + " Java Developers, " + p_dotnetreq + " DotNet Developers and " + p_breq + "  Business Analysts<br /><br /> Kindly assign the following employees on priority. <br /><br />  Regards. <br /> Business Unit " + Session["currentUID"];
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
        public ActionResult NReqAllotment(int p_bid,int p_rid,int p_jreq,int p_dreq,int p_bareq)
        {
            if (Session["currentUID"] != null && Session["currentEType"].ToString() == "Hiring Manager")
            {
                Session["p_empName"] = null;
                Session["p_bid"] = p_bid;
                Session["p_rid"] = p_rid;
                Session["p_jreq"] = p_jreq;
                Session["p_dreq"] = p_dreq;
                Session["p_bareq"] = p_bareq;
                return RedirectToAction("Allotment");
            }
            return RedirectToAction("Login", "Login");
            
            
        }


        public ActionResult Allotment()/**/
        {
            if (Session["currentUID"] != null && Session["currentEType"].ToString() == "Hiring Manager")
            {
                return View(cl.DisplayAvailableEmployees());
            }
            return RedirectToAction("Login", "Login");
            
        }

        public ActionResult NAllotment(int p_eid,string p_empName, int p_pbid,string p_type,int p_bid, int p_rid, int p_jreq, int p_dreq, int p_bareq)/**/
        {
            if (Session["currentUID"] != null && Session["currentEType"].ToString() == "Hiring Manager")
            {
                Session["p_eid"] = p_eid;
                Session["p_empName"] = p_empName;
                Session["p_type"] = p_type;
                Session["p_bid"] = p_bid;
                Session["p_rid"] = p_rid;
                Session["p_pbid"] = p_pbid;
                Session["p_jreq"] = p_jreq;
                Session["p_dreq"] = p_dreq;
                Session["p_bareq"] = p_bareq;
                if (p_type == "Java")
                {
                    Session["p_jreq"] = p_jreq - 1;
                }
                else if (p_type == "DotNet")
                {
                    Session["p_dreq"] = p_dreq - 1;
                }
                else if (p_type == "BA")
                {
                    Session["p_bareq"] = p_bareq - 1;
                }
                cl.UpdateBusinessRequirements(Convert.ToInt32(Session["p_rid"]), Convert.ToInt32(Session["p_bid"]), Convert.ToInt32(Session["p_eid"]), Convert.ToString(Session["p_type"]));
                cl.InsertAssignedEmployees(Convert.ToInt32(Session["p_eid"]), Convert.ToString(Session["p_empName"]), Convert.ToInt32(Session["p_pbid"]), Convert.ToInt32(Session["p_bid"]), Convert.ToString(Session["p_type"]));
                List<proc_user_details_Result> sender = cl.UserDetails(Convert.ToInt32(Session["currentUID"]));
                List<proc_user_details_Result> receiver = cl.UserDetails(Convert.ToInt32(Session["p_bid"]));
                MailSender(sender[0].EmailId, receiver[0].EmailId, sender[0].EPassword, Convert.ToString(Session["p_empName"]), Convert.ToString(Session["p_type"]));
                return RedirectToAction("Allotment");
            }
            return RedirectToAction("Login", "Login");
            
            
            
        }

        public void MailSender(string sender, string receiver, string password,string EmpName, string EmpType)
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

                        m.Subject ="New Employee Added in BU"+Session["p_bid"];
                        m.IsBodyHtml = true;
                        m.Body = "Hi, Business Unit " + Session["p_bid"] + " Head<br /> " + EmpName+ " having experties in " + EmpType + " has been added to your BU as per your request. <br /><br /> Regards. <br /> Hiring Manager";
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

        [HttpPost]
        public ActionResult Search_Details(string p_search)
        {
            if (Session["currentUID"] != null && Session["currentEType"].ToString() == "Hiring Manager")
            {
                return View(cl.GetSearchResults(p_search));
            }
            return RedirectToAction("Login", "Login");
            
        }


        public ActionResult GetAssignedEmployeeList()
        {
            if (Session["currentUID"] != null && Session["currentEType"].ToString() == "Hiring Manager")
            {
                return View(cl.GetAssignedEmployees());
                
            }
            return RedirectToAction("Login", "Login");
            
        }

        public ActionResult Search_Assigned_Employee_Details(string p_search)
        {
            if (Session["currentUID"] != null && Session["currentEType"].ToString() == "Hiring Manager")
            {
                return View(cl.SearchAssignedEmployee(p_search));

            }
            return RedirectToAction("Login", "Login");
            
        }
    }
}