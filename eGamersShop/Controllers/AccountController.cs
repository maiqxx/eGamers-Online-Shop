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
using System.Drawing;
using System.Drawing.Imaging;
 

namespace eGamersShop.Controllers
{
    public class AccountController : Controller
    {
        string connDB = WebConfigurationManager.ConnectionStrings["connDB"].ConnectionString;

        public ActionResult CreateAccount()
        {
            return View();
        }

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

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO USERTBL (LASTNAME, FIRSTNAME, MIDNAME, ADDRESS, CONTACTNO, EMAIL, USERNAME, PASSWORD, ROLE)"
                            + "VALUES ("
                            + "@LNAME,"
                            + "@FNAME,"
                            + "@MNAME,"
                            + "@ADDRS,"
                            + "@CONTACTNUM,"
                            + "@EM,"
                            + "@UNAME,"
                            + "@PSWD,"
                            + "@USER)";
                        cmd.Parameters.AddWithValue("@LNAME", lastname);
                        cmd.Parameters.AddWithValue("@FNAME", firstname);
                        cmd.Parameters.AddWithValue("@MNAME", midname);
                        cmd.Parameters.AddWithValue("@ADDRS", address);
                        cmd.Parameters.AddWithValue("@CONTACTNUM", contactNum);
                        cmd.Parameters.AddWithValue("@EM", email);
                        cmd.Parameters.AddWithValue("@UNAME", username);
                        cmd.Parameters.AddWithValue("@PSWD", password);
                        cmd.Parameters.AddWithValue("@USER", role);
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Response.Write("<script>alert('You've successfully created your account!')</script>");
                        }
                        else
                            Response.Write("<script>alert('Please try again...')</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                //naay error if kni ang maexcecute

                Response.Write("<script>alert('Something went wrong...')</script>");
                
            } 
                return View();
        }

        

    }
}