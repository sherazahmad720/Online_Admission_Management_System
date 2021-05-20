using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Online_Admission_Management_System.BLL;
using System.IO;

namespace Online_Admission_Management_System.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registration()
        {
            DataTable DT =new DataTable();
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandText = "[spDistrict_List]";
            DT = DBAccess_BLL.ExecuteCommand(Cmd);




            return View(DT);
        }
      
        bool CheckEmail(string Email)
        {
            DataTable DT = new DataTable();
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandText = "spCheckIfEmailExistForRegistration";
            Cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email;
            DT = DBAccess_BLL.ExecuteCommand(Cmd);
            if (DT.Rows.Count > 0)
            {
                if (DT.Rows[0][0].ToString() == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }

               
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        public ActionResult Registration(string Name, string FatherName, string Gender, string DOB, string Nationality, string Country, string District, string Postal_Address, string Permanent_Address, string Phone_Number, string Mobile_Number, string Email, string Province, string Password,HttpPostedFileBase photo,string CNIC)
       
        {
            string full_pathh = "";
            string fileExtention = Path.GetExtension(photo.FileName);
            string path="";
            if (photo.ContentLength != null) 
            {
               
            string pic = System.IO.Path.GetFileName(photo.FileName);
           path  = System.IO.Path.Combine(
                                   Server.MapPath("~/images"), pic);
            full_pathh = "/images/" + pic;

            photo.SaveAs(path);
            }

            ViewBag.NameError = "";

            if (path.ToLower().EndsWith(".jpg") || path.ToLower().EndsWith(".jpeg") || path.ToLower().EndsWith(".png"))
            {

                // file is uploaded
              



                if (CheckEmail(Email))
                {
                    ViewBag.EmailError = "Email is already reserved!";

                    return View();
                }


                else
                {

                    DataTable DT = new DataTable();
                    SqlCommand Cmd = new SqlCommand();
                    Cmd.CommandText = "spStudent_Insert";

                    Cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = Name;
                    Cmd.Parameters.Add("@FatherName", SqlDbType.VarChar).Value = FatherName;
                    Cmd.Parameters.Add("@Gender", SqlDbType.VarChar).Value = Gender;
                    Cmd.Parameters.Add("@DOB", SqlDbType.Date).Value = DOB;
                    Cmd.Parameters.Add("@Nationality", SqlDbType.VarChar).Value = Nationality;
                    Cmd.Parameters.Add("@Country", SqlDbType.VarChar).Value = Country;
                    Cmd.Parameters.Add("@District", SqlDbType.VarChar).Value = District;
                    Cmd.Parameters.Add("@Postal_Address", SqlDbType.VarChar).Value = Postal_Address;
                    Cmd.Parameters.Add("@Permanent_Address", SqlDbType.VarChar).Value = Permanent_Address;
                    Cmd.Parameters.Add("@Phone_Number", SqlDbType.VarChar).Value = Phone_Number;
                    Cmd.Parameters.Add("@Mobile_Number", SqlDbType.VarChar).Value = Mobile_Number;
                    Cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email;
                    Cmd.Parameters.Add("@Province", SqlDbType.VarChar).Value = Province;
                    Cmd.Parameters.Add("@Photo", SqlDbType.VarChar).Value = full_pathh;
                    Cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = Password;
                    Cmd.Parameters.Add("@Role", SqlDbType.VarChar).Value = "Student";
                    Cmd.Parameters.Add("@CNIC", SqlDbType.VarChar).Value = CNIC;

                    DT = DBAccess_BLL.ExecuteCommand(Cmd);
                    if (DT.Rows.Count > 0)
                    {
                        if (DT.Rows[0][0].ToString() == "1")
                        {
                            ViewBag.AdmissionSaved = true;
                        }
                        else
                        {
                            ViewBag.AdmissionSaved = false;
                        }
                    }

                    ViewBag.Registration = "Registerd Successfully";
                    return View("Login");
                }
           
            
            
            }
            else
            {
                ViewBag.photoError = "Please select a jpeg file!";

                return View();
            }
           
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            DataTable DT = new DataTable();
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandText = "[spUser_Login]";
            Cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email;
            Cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = Password;
            DT = DBAccess_BLL.ExecuteCommand(Cmd);
            if (DT.Rows.Count > 0)
                
            {
                String role = DT.Rows[0]["Role"].ToString();
                if (role == "Admin")
                {
                    Session["UserID"] = DT.Rows[0][0].ToString();
                    return RedirectToAction("Index", "Admin");
                }
                else
                {

                    Session["StudentID"] = DT.Rows[0][0].ToString();
                    return RedirectToAction("Index", "Student");

                }
              
                }
            else
            {
                ViewBag.Error = "Email or Password is wrong";
                return View();
            }
        }

        public ActionResult NoticeBoard() {


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

        public ActionResult NoticeView(String Id) {
            DataTable DT = new DataTable();
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandText = "[spNotification_SearchByID]";
            Cmd.Parameters.Add("@NotificationID", SqlDbType.Decimal).Value = Id;
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

        public ActionResult StudentMeritList()
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
       
       

    }
}