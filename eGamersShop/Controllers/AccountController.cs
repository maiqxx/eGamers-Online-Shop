using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace eGamersShop.Controllers
{
    public class AccountController : Controller
    {
        string connDB = WebConfigurationManager.ConnectionStrings["connDB"].ConnectionString;

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateAccount()
        {
            return View();
        }

        //Method for creating account of users to be saved in DB
        [HttpPost]
        public ActionResult CreateAccount(FormCollection collection)
        {
            var lastname = Request["txtLastname"];
            var firstname = Request["txtFirstname"];
            var midname = Request["txtMidname"];
            var address = Request["txtAddress"];
            var contactNum = Request["txtPhoneNum"];
            var email = Request["txtEmail"];
            var username = Request["txtUsername"];
            var password = Request["txtPassword"];
            var role = Convert.ToString(collection["role"]);

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
                            + "@lname,"
                            + "@fname,"
                            + "@mname,"
                            + "@address,"
                            + "@contactnum,"
                            + "@email,"
                            + "@uname,"
                            + "@pswrd,"
                            + "@role)";
                        cmd.Parameters.AddWithValue("@lname", lastname);
                        cmd.Parameters.AddWithValue("@fname", firstname);
                        cmd.Parameters.AddWithValue("@mname", midname);
                        cmd.Parameters.AddWithValue("@address", address);
                        cmd.Parameters.AddWithValue("@contactnum", contactNum);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@uname", username);
                        cmd.Parameters.AddWithValue("@pswrd", password);
                        cmd.Parameters.AddWithValue("@role", role);
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Response.Write("<script>alert('You've successfully created your account!')</script>");
                            Response.Redirect("Index");

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
                Response.Write(ex);

            }
            return View();
        }

        public ActionResult getSelectedRole()
        {
            var data = new List<object>();
            var role = "";

            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT ROLE as 'USERROLE' FROM USERTBL";
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        data.Add(new
                        {
                            mess = 0,
                            role = reader["USERROLE"].ToString(),
                        });
                    }
                    else
                    {
                        data.Add(new
                        {
                            mess = 1,
                        });
                    }

                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}