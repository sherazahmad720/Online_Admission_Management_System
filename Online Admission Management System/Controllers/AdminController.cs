using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Online_Admission_Management_System.BLL;
using System.Web;
using System.Net.Mail;
using System.Net;
namespace Online_Admission_Management_System.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                //code for getting session id of admin
                DataTable DT = new DataTable();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[spUser_SearchByID]";
                Cmd.Parameters.Add("@UserID", SqlDbType.Decimal).Value = Session["UserID"].ToString();
                DT = DBAccess_BLL.ExecuteCommand(Cmd);
                //code for getting student count
                DataSet ds = new DataSet();
                DataTable DT2 = new DataTable();
                SqlCommand Cmd2 = new SqlCommand();
                Cmd2.CommandText = "[spAdmin_panel]";
                DT2 = DBAccess_BLL.ExecuteCommand(Cmd2);

                DataTable DT3 = new DataTable();
                SqlCommand Cmd3 = new SqlCommand();
                Cmd3.CommandText = "[spSessions]";
                DT3 = DBAccess_BLL.ExecuteCommand(Cmd3);
                ds.Tables.Add(DT2);
                ds.Tables.Add(DT3);
                if (DT2.Rows.Count > 0 && DT3.Rows.Count > 0)
                {
                    Session["UserID"] = DT.Rows[0][0].ToString();

                    return View(ds);
                }
                else
                {
                    return View();
                }
            }
        }





        public ActionResult Logout()
        {
            Session["UserID"] = null;
            return RedirectToAction("Login", "Home");

        }
        public ActionResult StudentList()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DataTable DT = new DataTable();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[spStudents_List]";
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
        string StdID = "0";
        private ActionResult Null;

        public ActionResult StudentView(string Id)
        {
            StdID = Id;
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DataTable DT = new DataTable();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[spStudents_SearchByID]";
                Cmd.Parameters.Add("@StudentID", SqlDbType.Decimal).Value = Id;
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


        public ActionResult Student_Delete(string Id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DataTable DT = new DataTable();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[spStudent_Delete]";
                Cmd.Parameters.Add("@StudentID", SqlDbType.VarChar).Value = Id;
                DBAccess_BLL.ExecuteCommand(Cmd);
                ViewBag.delete = "Student Data is Deleted";
                return RedirectToAction("StudentList", "Admin");
            }
        }



        public ActionResult StudentProfile(string Id, string message)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (message == "suc")
                    ViewBag.EmailError = "";
                else
                    ViewBag.EmailError = message;
                if (Session["UserID"] == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    DataTable DT = new DataTable();
                    SqlCommand Cmd = new SqlCommand();
                    Cmd.CommandText = "[spStudents_SearchByID]";
                    Cmd.Parameters.Add("@StudentID", SqlDbType.Decimal).Value = Id;
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
        }

        bool CheckEmail(string Email, string stdid)
        {
            int id = Convert.ToInt32(stdid);
            DataTable DT = new DataTable();
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandText = "spCheckIfEmailExist";
            Cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email;
            Cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            DT = DBAccess_BLL.ExecuteCommand(Cmd);

            if (DT.Rows[0][0].ToString() == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        [HttpPost]
        public ActionResult Admission_Update(string StudentID, string Name, string FatherName, string Gender, string DOB,
            string Nationality, string Country, string Postal_Address, string Permanent_Address, string Phone_Number,
            string Mobile_Number, string Email, string Province,
               string m_school, string m_roll, string m_obtained, string m_total, string m_passing, string m_board,
               string i_college, string i_roll, string i_obtained, string i_total, string i_passing, string i_board,
               string g_uni, string g_roll, string g_obtained, string g_total, string g_passing, string g_board
               )
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (CheckEmail(Email, StudentID) == true)
                {



                    return RedirectToAction("StudentProfile", "Admin", new { Id = StudentID, message = "Email Already Exists" });

                }

                else
                {
                    DataTable DT = new DataTable();
                    SqlCommand Cmd = new SqlCommand();
                    Cmd.CommandText = "spStudent_Update";
                    Cmd.Parameters.Add("@StudentID", SqlDbType.Decimal).Value = StudentID;
                    Cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = Name;
                    Cmd.Parameters.Add("@FatherName", SqlDbType.VarChar).Value = FatherName;
                    Cmd.Parameters.Add("@Gender", SqlDbType.VarChar).Value = Gender;
                    Cmd.Parameters.Add("@DOB", SqlDbType.Date).Value = DOB;
                    Cmd.Parameters.Add("@Nationality", SqlDbType.VarChar).Value = Nationality;
                    Cmd.Parameters.Add("@Country", SqlDbType.VarChar).Value = Country;

                    Cmd.Parameters.Add("@Postal_Address", SqlDbType.VarChar).Value = Postal_Address;
                    Cmd.Parameters.Add("@Permanent_Address", SqlDbType.VarChar).Value = Permanent_Address;
                    Cmd.Parameters.Add("@Phone_Number", SqlDbType.VarChar).Value = Phone_Number;
                    Cmd.Parameters.Add("@Mobile_Number", SqlDbType.VarChar).Value = Mobile_Number;
                    Cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email;
                    Cmd.Parameters.Add("@Province", SqlDbType.VarChar).Value = Province;
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

                    DBAccess_BLL.ExecuteCommand(Cmd);
                    ViewBag.update = "Updated Successfully";
                    return RedirectToAction("StudentList", "Admin");
                }
            }
        }


        public ActionResult NotificationForm()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult NotificationForm(String Title, String Description, String Status, String Priority)
        {
            DataTable DT = new DataTable();
            SqlCommand Cmd = new SqlCommand();
            String CurrentDate = DateTime.Now.ToString("MM/dd/yyyy");
            Cmd.CommandText = "spCreate_Notification";
            Cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = Title;
            Cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = Description;
            Cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = Status;
            Cmd.Parameters.Add("@Priority", SqlDbType.VarChar).Value = Priority;
            Cmd.Parameters.Add("@CreatedDate", SqlDbType.VarChar).Value = CurrentDate;
            Cmd.Parameters.Add("@PublishedDate", SqlDbType.VarChar).Value = CurrentDate;
            DBAccess_BLL.ExecuteCommand(Cmd);
            ViewBag.notification = "Notification Created SuccessFully!";
            return RedirectToAction("NotificationList", "Admin");
        }
        public ActionResult NotificationList()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DataTable DT = new DataTable();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[spNotification_List]";
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

        public ActionResult NotificationView(string id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DataTable DT = new DataTable();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[spNotification_SearchByID]";
                Cmd.Parameters.Add("@NotificationID", SqlDbType.Decimal).Value = id;
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

        public ActionResult NotificationDelete(string id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DataTable DT = new DataTable();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[spNotification_Delete]";
                Cmd.Parameters.Add("@NotificationID", SqlDbType.VarChar).Value = id;
                DBAccess_BLL.ExecuteCommand(Cmd);
                ViewBag.delete = "Student Data is Deleted";
                return RedirectToAction("NotificationList", "Admin");
            }
        }
        [HttpPost]
        public ActionResult NotificationUpdate(string NotificationID, string Title, string Description, string Status, string Priority)
        {

            DataTable DT = new DataTable();
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandText = "[spNotificatin_Update]";
            Cmd.Parameters.Add("@NotificationId", SqlDbType.VarChar).Value = NotificationID;
            Cmd.Parameters.Add("@NotificationTitle", SqlDbType.VarChar).Value = Title;

            Cmd.Parameters.Add("@NotificationDescription", SqlDbType.VarChar).Value = Description;
            Cmd.Parameters.Add("@NotificationStatus", SqlDbType.VarChar).Value = Status;
            Cmd.Parameters.Add("@NotificationPriority", SqlDbType.VarChar).Value = Priority;
            DT = DBAccess_BLL.ExecuteCommand(Cmd);

            return RedirectToAction("NotificationList", "Admin");

        }

        public ActionResult NotificationUpdate(string id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DataTable DT = new DataTable();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[spNotification_SearchByID]";
                Cmd.Parameters.Add("@NotificationID", SqlDbType.Decimal).Value = id;
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

        public ActionResult NotificationUpdateStatus(string id)
        {
            DataTable DT = new DataTable();
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandText = "[spNotificatin_Update_Status]";
            Cmd.Parameters.Add("@NotificationId", SqlDbType.VarChar).Value = id;

            DT = DBAccess_BLL.ExecuteCommand(Cmd);

            return RedirectToAction("NotificationList", "Admin");
        }
        public ActionResult SessionDetails()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DataTable DT = new DataTable();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[spSessions]";
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
        public ActionResult SessionDetails(String SessionField, String Submitbtn, String CurrentSession)
        {

            if (Submitbtn == "Create")
            {
                if (SessionField == "")
                {
                    ViewBag.Message = "Session Field Must be Filled";
                }
                else
                {
                    DataTable DT1 = new DataTable();
                    SqlCommand Cmd1 = new SqlCommand();
                    Cmd1.CommandText = "[spSession_Insert]";
                    Cmd1.Parameters.Add("@SessionName", SqlDbType.VarChar).Value = SessionField;
                    DT1 = DBAccess_BLL.ExecuteCommand(Cmd1);
                    ViewBag.Message = "Session Created Successfully";
                }


            }
            else if (Submitbtn == "Update")
            {
                DataTable DT2 = new DataTable();
                SqlCommand Cmd2 = new SqlCommand();
                Cmd2.CommandText = "[spSession_Update]";
                Cmd2.Parameters.Add("@SessionName", SqlDbType.VarChar).Value = CurrentSession;
                Cmd2.Parameters.Add("@SessionStatus", SqlDbType.VarChar).Value = "Active";
                DT2 = DBAccess_BLL.ExecuteCommand(Cmd2);
                ViewBag.Update = "Update SuccesssFully";
            }

            DataTable DT = new DataTable();
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandText = "[spSessions]";
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
        [HttpPost]
        public ActionResult AdmissionStatus()
        {
            return null;
        }
        public ActionResult AdmissionList(string admissionmessage, string mailmessage)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DataTable DT = new DataTable();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[spAdmission_List]";
                DT = DBAccess_BLL.ExecuteCommand(Cmd);
                if (DT.Rows.Count > 0)
                {
                    ViewBag.admission = admissionmessage;
                    ViewBag.mail = mailmessage;
                    return View(DT);
                }
                else
                {
                    return View();
                }
            }
        }
        public ActionResult AdmissionStatus(string AdmissionNO, string AdmissionStatus, string Email)
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                string EmailMessage = "";

                if (AdmissionStatus == "Accepted")
                {
                    EmailMessage = "You admission against Admission number(" + AdmissionNO + ") is accepted successfully.For more updates please visit your profile";
                }
                else
                {
                    EmailMessage = "You admission against Admission number(" + AdmissionNO + ") unfortunately rejected.For more query please contact to admin office";
                }

                DataTable DT = new DataTable();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[spAdmission_Update_Status]";
                Cmd.Parameters.Add("@AdmissionNO", SqlDbType.VarChar).Value = AdmissionNO;
                Cmd.Parameters.Add("@AdmissioinStatus", SqlDbType.VarChar).Value = AdmissionStatus;
                DT = DBAccess_BLL.ExecuteCommand(Cmd);
                ViewBag.send = "Email Sent";
                string admissionMsg = "Admission Update Successfully";
                string mailMsg = SendMail(Email, AdmissionStatus, EmailMessage);
                return RedirectToAction("AdmissionList", "Admin", new { admissionmessage = admissionMsg, mailmessage = mailMsg });


            }
        }

        string SendMail(string Email, string Admissionstatus, string EmailMessage)
        {
            try
            {
                MailMessage mm = new MailMessage("sherazahmad720@gmail.com", Email);
                mm.Subject = "Admission " + Admissionstatus;
                mm.Body = EmailMessage;
                mm.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                NetworkCredential nc = new NetworkCredential("sherazahmad720@gmail.com", "ghuganwali786");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = nc;
                smtp.Send(mm);
                return "Mail Sent Successfully";
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public ActionResult MeritLists()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View();
            }
        }
        public ActionResult CreateReport()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DataTable DT = new DataTable();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[spSessions]";
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
        public ActionResult CreateReport(string Session, string Category, string Title, string Description)
        {
            DataTable DT = new DataTable();
            SqlCommand Cmd = new SqlCommand();

            Cmd.CommandText = "spStudentReport_Insert";
            Cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = Title;
            Cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = Description;
            Cmd.Parameters.Add("@Session", SqlDbType.VarChar).Value = Session;
            Cmd.Parameters.Add("@Category", SqlDbType.VarChar).Value = Category;
            DBAccess_BLL.ExecuteCommand(Cmd);

            return RedirectToAction("StudentReportList", "Admin");
        }


        public ActionResult CreateMeritList()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DataSet ds = new DataSet();
                DataTable DT1 = new DataTable();
                DT1.TableName = "Session";
                DataTable DT2 = new DataTable();
                DT2.TableName = "Program";
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[spSessions]";
                DT1 = DBAccess_BLL.ExecuteCommand(Cmd);

                Cmd.CommandText = "[spPrograms]";
                DT2 = DBAccess_BLL.ExecuteCommand(Cmd);
                ds.Tables.Add(DT1);
                ds.Tables.Add(DT2);
                if (DT1.Rows.Count > 0 && DT2.Rows.Count > 0)
                {
                    return View(ds);
                }
                else
                {
                    return View();
                }
            }
        }
        [HttpPost]
        public ActionResult CreateMeritList(string Title, string Description, string Session, string Program, string Province, string Seats)
        {
            DataTable DT = new DataTable();
            SqlCommand Cmd = new SqlCommand();

            Cmd.CommandText = "spMerilList_Insert";
            Cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = Title;
            Cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = Description;
            Cmd.Parameters.Add("@Session", SqlDbType.VarChar).Value = Session;
            Cmd.Parameters.Add("@Program", SqlDbType.VarChar).Value = Program;
            Cmd.Parameters.Add("@Province", SqlDbType.VarChar).Value = Province;
            Cmd.Parameters.Add("@Seats", SqlDbType.VarChar).Value = Seats;
            DBAccess_BLL.ExecuteCommand(Cmd);

            return RedirectToAction("StudentMeritList", "Admin");
        }
        public ActionResult StudentReportList()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DataTable DT = new DataTable();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[spStudentReport_View]";
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
        public ActionResult StudentMeritList()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DataTable DT = new DataTable();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[spMerilList_View]";
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

        public ActionResult PrintStudentAdmissionList(string id, string Title, string Description, string Session, string Category)
        {

            DataTable dt = new DataTable();
            SqlCommand Cmd = new SqlCommand();

            Cmd.CommandText = "sp_Rpt_StudentAdmissionList";
            Cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Convert.ToInt32(id);
            dt = DBAccess_BLL.ExecuteCommand(Cmd);
            ViewBag.category = Category;
            ViewBag.session = Session;
            ViewBag.title = Title;

            return View(dt);
        }
        public ActionResult printstudentMeritList(string id, string Title, string Session, string Province, string Program)
        {
            DataTable dt = new DataTable();
            SqlCommand Cmd = new SqlCommand();

            Cmd.CommandText = "sp_Rpt_MeritList";
            Cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Convert.ToInt32(id);
            dt = DBAccess_BLL.ExecuteCommand(Cmd);

            ViewBag.title = Title;
            ViewBag.session = Session;
            ViewBag.province = Province;
            ViewBag.program = Program;
            return View(dt);
        }

        public ActionResult MeritListUpdateStatus(string Id)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DataTable DT = new DataTable();
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "[spMeritList_Update_Status]";
                Cmd.Parameters.Add("@MeritListId", SqlDbType.VarChar).Value = Id;

                DT = DBAccess_BLL.ExecuteCommand(Cmd);

                return RedirectToAction("StudentMeritList", "Admin");
            }
        }
    }
}