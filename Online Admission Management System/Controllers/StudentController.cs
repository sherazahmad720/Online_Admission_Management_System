using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Online_Admission_Management_System.BLL;
namespace Online_Admission_Management_System.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            if (Session["StudentID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DataTable DT = new DataTable();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[spStudents_SearchByID]";
                Cmd.Parameters.Add("@StudentID", SqlDbType.Decimal).Value = Session["StudentID"].ToString();
                DT = DBAccess_BLL.ExecuteCommand(Cmd);
                if (DT.Rows.Count > 0)
                {
                    Session["StudentID"] = DT.Rows[0][0].ToString();
                    return View(DT);
                }
                else
                {
                    return View();
                }
            }
        }
        public ActionResult ViewDetails()
        {
            if (Session["StudentID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                string id = Session["StudentID"].ToString();

                DataTable DT = new DataTable();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[spStudents_SearchByID]";
                Cmd.Parameters.Add("@StudentID", SqlDbType.Decimal).Value = id;
                DT = DBAccess_BLL.ExecuteCommand(Cmd);
                if (DT.Rows.Count > 0)
                {
                    return View(DT);
                }
                else
                {
                    return View();
                }
            }
        }




        public ActionResult Logout()
        {
            Session["StudentID"] = null;
            return RedirectToAction("Login", "Home");
        }
        public ActionResult AdmissionForm()
        {

            if (Session["StudentID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {


                DataTable DT = new DataTable();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[spAdmission_data]";
                Cmd.Parameters.Add("@StudentID", SqlDbType.Decimal).Value = Session["StudentID"].ToString();
                DT = DBAccess_BLL.ExecuteCommand(Cmd);


                if (DT.Rows.Count > 0)
                {
                    return View(DT);
                }
                else
                {
                    return View();
                }

            }

        }
        [HttpPost]
        public ActionResult AdmissionForm(string m_school, string SessionID, string m_roll, string m_obtained, string m_total, string m_passing, string m_board,
            string i_college, string i_roll, string i_obtained, string i_total, string i_passing, string i_board,
            string g_uni, string g_roll, string g_obtained, string g_total, string g_passing, string g_board, string disability,
            string programID,  string condition, string disabled,string StudyLevel
            )
        {


            bool Eligible = true;
            float lastDegreePercentage = 0;

            if (StudyLevel=="Inter")
            {
                if (CheckEligible(programID, StudyLevel))
                {
                    Eligible = false;
                }
            }
           
          
                if (StudyLevel=="Bachlor")
                {

                    lastDegreePercentage = (float.Parse(g_obtained) / float.Parse(g_total)) * 100;
                }
              
                else
                {
                    lastDegreePercentage = (float.Parse(i_obtained) / float.Parse(i_total)) * 100;
                }
            
            if (condition == null || i_total == "" || lastDegreePercentage < 45 || Eligible==false)
            {
                DataTable DT = new DataTable();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[spAdmission_data]";
                Cmd.Parameters.Add("@StudentID", SqlDbType.Decimal).Value = Session["StudentID"].ToString();
                DT = DBAccess_BLL.ExecuteCommand(Cmd);


                if (DT.Rows.Count > 0)
                {
                    if (lastDegreePercentage < 45)
                    {
                        ViewBag.error = "You Are Not Eligible Because Your Last Degree Persentage is Less Than 45%";
                    }
                    else if (Eligible==false)
                    {
                        ViewBag.error = "You Are Not Eligible Because Minimal Education Not meet";
                    }
                    else
                    {
                        ViewBag.error = "Please Accept Terms and condition and Enter intermediate Marks Atleast";
                    }
                    return View(DT);
                }
                else
                {
                    return View();
                }

            }
            else
            {

                DataTable DT = new DataTable();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[spAdmission_insert]";
                Cmd.Parameters.Add("@StudentID", SqlDbType.VarChar).Value = Session["StudentID"];
                Cmd.Parameters.Add("@SessionID", SqlDbType.VarChar).Value = SessionID;
                Cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = "Pending";
                Cmd.Parameters.Add("@programID", SqlDbType.VarChar).Value = programID;
           
                Cmd.Parameters.Add("@lastDegree_percentage", SqlDbType.Float).Value = lastDegreePercentage;
               
                Cmd.Parameters.Add("@StudyLevel", SqlDbType.VarChar).Value = StudyLevel;
                Cmd.Parameters.Add("@M_school", SqlDbType.VarChar).Value = m_school;
                Cmd.Parameters.Add("@M_roll", SqlDbType.VarChar).Value = m_roll;
                Cmd.Parameters.Add("@M_total", SqlDbType.VarChar).Value = m_total;
                Cmd.Parameters.Add("@M_obtained", SqlDbType.VarChar).Value = m_obtained;
                Cmd.Parameters.Add("@M_PassingYear", SqlDbType.VarChar).Value = m_passing;
                Cmd.Parameters.Add("@M_board", SqlDbType.VarChar).Value = m_board;
                Cmd.Parameters.Add("@I_college", SqlDbType.VarChar).Value = i_college;
                Cmd.Parameters.Add("@I_roll", SqlDbType.VarChar).Value = i_roll;
                Cmd.Parameters.Add("@I_total", SqlDbType.VarChar).Value = i_total;
                Cmd.Parameters.Add("@I_obtained", SqlDbType.VarChar).Value = i_obtained;
                Cmd.Parameters.Add("@I_PassingYear", SqlDbType.VarChar).Value = i_passing;
                Cmd.Parameters.Add("@I_board", SqlDbType.VarChar).Value = i_board;
                Cmd.Parameters.Add("@G_Uni", SqlDbType.VarChar).Value = g_uni;
                Cmd.Parameters.Add("@G_roll", SqlDbType.VarChar).Value = g_roll;
                Cmd.Parameters.Add("@G_total", SqlDbType.VarChar).Value = g_total;
                Cmd.Parameters.Add("@G_obtained", SqlDbType.VarChar).Value = g_obtained;
                Cmd.Parameters.Add("@G_PassingYear", SqlDbType.VarChar).Value = g_passing;
                Cmd.Parameters.Add("@G_board", SqlDbType.VarChar).Value = g_board;
              
                DT = DBAccess_BLL.ExecuteCommand(Cmd);


                if (DT.Rows.Count > 0)
                {
                    return RedirectToAction("AdmissionStatus", "Student");
                }
                else
                {
                    return View();
                }
            }



        }
        //
        public ActionResult AdmissionStatus()
        {
            if (Session["StudentID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DataTable DT = new DataTable();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[spCheck_Status]";
                Cmd.Parameters.Add("@StudentID", SqlDbType.Decimal).Value = Session["StudentID"];

                DT = DBAccess_BLL.ExecuteCommand(Cmd);


                if (DT.Rows.Count > 0)
                {
                    return View(DT);
                }
                else
                {
                    return View();
                }
            }
        }

        bool CheckEligible(string programID, string StudyLevel)
        {
            DataTable DT = new DataTable();
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandText = "spCheckEligibility";
            Cmd.Parameters.Add("@ProgramID", SqlDbType.VarChar).Value = programID;
            Cmd.Parameters.Add("@StudyLevel", SqlDbType.VarChar).Value = StudyLevel;
            DT = DBAccess_BLL.ExecuteCommand(Cmd);
            if (DT.Rows.Count > 0)
            {
                if (DT.Rows[0][0].ToString() == "1")
                {
                    return false;
                }
                else
                {
                    return true;
                }


            }
            else
            {
                return true;
            }
        }
        public ActionResult EnrollmentStatus(string admissionNo, string sessionId)
        {
            DataTable DT = new DataTable();
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandText = "[spEnrollment]";
            Cmd.Parameters.Add("@AdmissionNo", SqlDbType.Decimal).Value = admissionNo;
            Cmd.Parameters.Add("@Sessionid", SqlDbType.Decimal).Value =sessionId;
            DT = DBAccess_BLL.ExecuteCommand(Cmd);




            return RedirectToAction("AdmissionStatus", "Student");
            
        }

        public ActionResult StudentIdCard(string StudentId, string SessionId)
        {
            if (Session["StudentID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DataTable DT = new DataTable();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[sp_StudentCard]";
                Cmd.Parameters.Add("@StudentID", SqlDbType.VarChar).Value = StudentId;
                Cmd.Parameters.Add("@SessionID", SqlDbType.VarChar).Value = SessionId;
                DT = DBAccess_BLL.ExecuteCommand(Cmd);


                if (DT.Rows.Count > 0)
                {
                    return View(DT);
                }
                else
                {
                    return View();
                }
            }
        }
        // write a method on the top of this line
    }
}