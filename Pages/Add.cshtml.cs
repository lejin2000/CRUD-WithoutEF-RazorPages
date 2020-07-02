using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CRUD_WithoutEF_RazorPages.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CRUD_WithoutEF_RazorPages.Pages
{
    public class AddModel : PageModel
    {
        public readonly IConfiguration _configuration;
        private static string connection;
        public AddModel(IConfiguration configuration)
        {
            _configuration = configuration;
            connection = _configuration.GetConnectionString("EmployeeDB");
        }
        public void OnGet()
        {

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