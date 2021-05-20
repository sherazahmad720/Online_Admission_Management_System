using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
namespace Online_Admission_Management_System.BLL
{
    public class DBAccess_BLL
    {
        public static string ConStr = "Data Source=.;Initial Catalog=dbUniSys;Integrated Security=True";
        public static SqlConnection Con = new SqlConnection(ConStr);
        public static void Connection_Open()
        {
            if (Con.State==ConnectionState.Closed)
            {
                Con.Open();
            }
        }
        public static void Connection_Close()
        { 
            if (Con.State == ConnectionState.Open)
            {
                Con.Close();
            }
        }


        public static DataTable ExecuteCommand(SqlCommand Cmd)
        {
            try
            {
                DataTable DT = new DataTable();
                Cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter Adp = new SqlDataAdapter(Cmd);
                Cmd.Connection = Con;
                Connection_Open();
                Adp.Fill(DT);
                Connection_Close();
                return DT;
            }
            catch (Exception ex)
            {
                return null;
                //throw;
            }
}
            
        }
    }