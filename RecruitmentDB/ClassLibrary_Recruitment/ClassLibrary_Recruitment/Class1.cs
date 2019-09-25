using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;

namespace ClassLibrary_Recruitment
{
    public class Class1
    {
        RecruitmentDBEntities db = new RecruitmentDBEntities();

        public List<proc_user_details_Result> UserDetails(int id)
        {

            return(db.proc_user_details(id).ToList());
        }

        public List<int> ToBeHired()
        {
            //db.proc_
            ObjectParameter java = new ObjectParameter("reqJava", 0);

            ObjectParameter dotnet = new ObjectParameter("reqDotNet", 0);

            ObjectParameter ba = new ObjectParameter("reqBA", 0);
            db.proc_requiredEmployee(java, dotnet, ba);

            List<int> result = new List<int>();
            result.Add(Convert.ToInt32(java.Value));
            result.Add(Convert.ToInt32(dotnet.Value));
            result.Add(Convert.ToInt32(ba.Value));

            return result;


        }

        public void UpdateHRTable(int java,int dotnet,int ba)
        {
            db.proc_updateHRTable(java,dotnet,ba);
        }

        public void AddBusinessUnit(string p_bname)
        {
            db.proc_add_bunit(p_bname);
        }

        public void AddNewEmplyee(int p_eid, string p_empname, int p_bid, string p_empdes, bool p_empstatus, int p_empexp)
        {
            db.proc_add_employee(p_eid,p_empname,p_bid,p_empdes,p_empstatus,p_empexp);
        }

        public void InsertRequest(int p_bid, int p_jreq, int p_dreq, int p_breq)
        {
            ObjectParameter p_rid = new ObjectParameter("rid", 0);
            db.proc_insert_req(p_rid, p_bid, p_jreq, p_dreq, p_breq);
        }

        public void DeleteBusinessRequirement()
        {
            db.proc_delete_BReqTable();
        }

        public void UpdateBusinessRequirements(int p_rid, int p_bid, int p_eid, string p_etype)
        {
            db.proc_update_tables(p_rid, p_bid, p_eid, p_etype);
        }

        public List<proc_display_requrests_Result> DisplayRequests(int p_bid)
        {
            db.proc_delete_BReqTable();
            return(db.proc_display_requrests(p_bid).ToList());
        }

        public void LoginSignUp(int p_eid, string p_password, string p_etype)
        {
            db.proc_login_signup(p_eid, p_password, p_etype);
        }

        public int? LoginSignIn(int p_eid, string p_password, string p_etype)
        {
            int? result = db.proc_login_signin(p_eid, p_password, p_etype).Single();
            return result;    
        }

        public List<proc_display_pending_requests_Result> DisplayPendingRequests()
        {
            db.proc_delete_BReqTable();
            return db.proc_display_pending_requests().ToList();
        }

        public List<proc_display_available_employees_Result> DisplayAvailableEmployees()
        {
            return db.proc_display_available_employees().ToList();
        }

        public List<Sort_Details_Result> GetSortedDetails(string p_text)
        {
            var result = db.Sort_Details(p_text);
            return result.ToList();
        }

        public List<Search_Details_Result> GetSearchResults(string p_text)
        {
            var result = db.Search_Details(p_text);
            return result.ToList();
        }

        public List<SortExperience_Result> GetSortExpResults(string p_text)
        {
            var result = db.SortExperience(p_text);
            return result.ToList();
        }

        public List<proc_display_team_Result> GetTeamMembers(int p_bid)
        {
            return db.proc_display_team(p_bid).ToList();
        }

        public void InsertAssignedEmployees(int p_eid, string p_empname, int p_pbid, int p_cbid, string p_empdes)
        {
            db.proc_insert_history_table(p_eid, p_empname, p_pbid, p_cbid, p_empdes);
        }

        public List<proc_display_history_table_Result> GetAssignedEmployees()
        {
            return db.proc_display_history_table().ToList();
        }

        public List<proc_Search_Employee_Team_Result> SearchTeamEmployee(string p_text, int p_bid)
        {
            return db.proc_Search_Employee_Team(p_text, p_bid).ToList();
        }

        public List<proc_search_assigned_employee_details_Result> SearchAssignedEmployee(string p_text)
        {
            return db.proc_search_assigned_employee_details(p_text).ToList();
        }
    }
}
