using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using CRUD_WithoutEF_RazorPages.Models;
using Microsoft.Extensions.Configuration;

namespace CRUD_WithoutEF_RazorPages.Pages
{
    public class DeleteModel : PageModel
    {
        public readonly IConfiguration _configuration;
        private static string connection;
        public DeleteModel(IConfiguration configuration)
        {
            _configuration = configuration;
            connection = _configuration.GetConnectionString("EmployeeDB");
        }

        [BindProperty]
        public EmpClass DisplayRecords { get; set; }
        public void OnGet(int empid)
        {
            EmpClass ec = new EmpClass();
            //string connection = "Data Source=DESKTOP-GORUVUT;Initial Catalog=EMP;Integrated Security=False;User Id=user;Password=user;MultipleActiveResultSets=True";
            //string sqlquery = "SELECT [Empid],[Empname],[Email],[Age],[Salary] FROM [EMP].[dbo].[NewEmployee] WHERE [Empid] = '" + empid + "' ";
            //using (SqlConnection sqlcon = new SqlConnection(connection))
            //{
            //    using (SqlCommand sqlcmd = new SqlCommand(sqlquery, sqlcon))
            //    {
            //        sqlcon.Open();
            //        using (SqlDataReader sdr = sqlcmd.ExecuteReader())
            //        {
            //            while (sdr.Read())
            //            {
            //                ec.Empid = Convert.ToInt32(sdr["Empid"]);
            //                ec.Empname = Convert.ToString(sdr["Empname"]);
            //                ec.Email = Convert.ToString(sdr["Email"]);
            //                ec.Age = Convert.ToInt32(sdr["Age"]);
            //                ec.Salary = Convert.ToInt32(sdr["Salary"]);
            //            }

            //        }
            //    }
            //}
            using (SqlConnection sqlcon = new SqlConnection(connection))
            {
                using (SqlCommand sqlcom = new SqlCommand("[dbo].[GetEmployeeById]", sqlcon))
                {
                    sqlcom.CommandType = CommandType.StoredProcedure;

                    sqlcom.Parameters.Add("@Empid", SqlDbType.VarChar).Value = empid;

                    sqlcon.Open();
                    using (SqlDataReader sdr = sqlcom.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            ec.Empid = Convert.ToInt32(sdr["Empid"]);
                            ec.Empname = Convert.ToString(sdr["Empname"]);
                            ec.Email = Convert.ToString(sdr["Email"]);
                            ec.Age = Convert.ToInt32(sdr["Age"]);
                            ec.Salary = Convert.ToInt32(sdr["Salary"]);
                        }
                    }
                }
            }
            DisplayRecords = ec;
        }

        public IActionResult OnPostAsync(int empid)
        {
            //string connection = "Data Source=DESKTOP-GORUVUT;Initial Catalog=EMP;Integrated Security=False;User Id=user;Password=user;MultipleActiveResultSets=True";
            using (SqlConnection sqlcon = new SqlConnection(connection))
            {
                //string deleteQuery = "DELETE A  FROM[EMP].[dbo].[NewEmployee] A WHERE[Empid] = " + empid;
                //using (SqlCommand sqlcmd = new SqlCommand(deleteQuery, sqlcon))
                //{
                //    sqlcon.Open();
                //    sqlcmd.ExecuteNonQuery();
                //}
                using (SqlCommand sqlcom = new SqlCommand("[dbo].[DeleteEmployeeById]", sqlcon))
                {
                    sqlcom.CommandType = CommandType.StoredProcedure;

                    sqlcom.Parameters.Add("@Empid", SqlDbType.VarChar).Value = empid;

                    sqlcon.Open();
                    sqlcom.ExecuteReader();
                     
                }
            }
            return RedirectToPage("Index");
        }
    }
}