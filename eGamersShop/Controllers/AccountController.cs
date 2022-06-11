using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Configuration;
using System.IO;

namespace eGamersShop.Controllers
{
    public class AccountController : Controller
    {
        string connDB = WebConfigurationManager.ConnectionStrings["connDB"].ConnectionString;

        [HttpPost]
        public ActionResult CreateAccount(FormCollection collection)
        {
            var lastname = Request["txtLname"];
            var firstname = Request["txtFname"];
            var midname = Request["txtMname"];
            var address = Request["txtAddress"];
            var contactNum = Request["txtConNum"];
            var email = Request["txtEmail"];
            var username = Request["txtUsername"];
            var password = Request["txtPswd"];
            var role = Request["radRole"];

            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using(var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO USERTBL (LASTNAME, FIRSTNAME, MIDNAME, ADDRESS, CONTACTNO, EMAIL, USERNAME, PASSWORD, ROLE)"
                        + "VALUES ("
                        + "@lname,"
                        + "@fname,"
                        + "@midname,"
                        + "@address,"
                        + "@contactnum,"
                        + "@email,"
                        + "@uname,"
                        + "@pswd,"
                        + "@role)";
                    cmd.Parameters.AddWithValue("@lname", lastname);
                    cmd.Parameters.AddWithValue("@fname", firstname);
                    cmd.Parameters.AddWithValue("@midname", midname);
                    cmd.Parameters.AddWithValue("@address", address);
                    cmd.Parameters.AddWithValue("@contactnum", contactNum);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@uname", username);
                    cmd.Parameters.AddWithValue("@pswd", password);
                    cmd.Parameters.AddWithValue("@role", role);
                    var ctr = cmd.ExecuteNonQuery();
                    if (ctr >= 1)
                    {
                        Response.Write("<script>alert('You've successfully created your account!')</script>");
                    }
                    else
                        Response.Write("<script>alert('Please try again...')</script>");
                
                }
            }
            
                return View();
        }

        
    }
}