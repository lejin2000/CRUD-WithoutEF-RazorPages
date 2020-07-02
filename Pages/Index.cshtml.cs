using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Data.Sql;
using System.Data.SqlClient;
using CRUD_WithoutEF_RazorPages.Models;
using System.Reflection.Metadata.Ecma335;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace CRUD_WithoutEF_RazorPages.Pages
{
    public class IndexModel : PageModel
    {
        public readonly IConfiguration _configuration;
        private static string connection;
        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
            connection = _configuration.GetConnectionString("EmployeeDB");
        }

        public IEnumerable<EmpClass> GetRecords { get; set; }

        //private readonly ILogger<IndexModel> _logger;

        //public IndexModel(ILogger<IndexModel> logger)
        //{
        //    _logger = logger;
        //}

        public void OnGet()
        {
           
            GetRecords = DisplayRecords();
        }

        public static List<EmpClass> DisplayRecords()
        {
            List<EmpClass> ListObj = new List<EmpClass>();
            //string connection = "Data Source=DESKTOP-GORUVUT;Initial Catalog=EMP;Integrated Security=False;User Id=user;Password=user;MultipleActiveResultSets=True";
            
            using (SqlConnection sqlcon = new SqlConnection(connection))
            {
                using (SqlCommand sqlcom = new SqlCommand("[dbo].[GetEmployee]", sqlcon))
                {
                    sqlcon.Open();
                    using(SqlDataReader sdr = sqlcom.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            EmpClass ec = new  EmpClass();
                            ec.Empid = Convert.ToInt32(sdr["Empid"]);
                            ec.Empname = Convert.ToString(sdr["Empname"]);
                            ec.Email = Convert.ToString(sdr["Email"]);
                            ec.Age = Convert.ToInt32(sdr["Age"]);
                            ec.Salary = Convert.ToDecimal(sdr["Salary"]);
                            ListObj.Add(ec);
                        }
                    }
                }
            }
            return ListObj;
        }

        public IActionResult OnPostAsync(EmpClass ecInsert)
        {
            //string connection = "Data Source=DESKTOP-GORUVUT;Initial Catalog=EMP;Integrated Security=False;User Id=user;Password=user;MultipleActiveResultSets=True";
            using (SqlConnection sqlcon = new SqlConnection(connection))
            {
                //string Insertdata = "Insert into [EMP].[dbo].[NewEmployee] Values('" + ecInsert.Empname + "','" + ecInsert.Email + "','" + ecInsert.Age + "','" + ecInsert.Salary + "')";
                //using(SqlCommand sqlcom = new SqlCommand(Insertdata, sqlcon))
                //{
                //    sqlcon.Open();
                //    sqlcom.ExecuteNonQuery();
                //}

                using (SqlCommand sqlcom = new SqlCommand("[dbo].[AddEmployee]", sqlcon))
                {
                    sqlcom.CommandType = CommandType.StoredProcedure;

                    sqlcom.Parameters.Add("@Empname", SqlDbType.VarChar).Value = ecInsert.Empname;
                    sqlcom.Parameters.Add("@Email", SqlDbType.VarChar).Value = ecInsert.Email;
                    sqlcom.Parameters.Add("@Age", SqlDbType.Int).Value = ecInsert.Age;
                    sqlcom.Parameters.Add("@Salary", SqlDbType.Decimal).Value = ecInsert.Salary;

                    sqlcon.Open();
                    sqlcom.ExecuteNonQuery();
                }
            }
            return RedirectToPage("Index");
        }
    }
}
