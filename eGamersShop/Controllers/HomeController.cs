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
    public class HomeController : Controller
    {
        string connDB = WebConfigurationManager.ConnectionStrings["connDB"].ConnectionString;


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ProductEntry()
        {
            return View();
        }

        public ActionResult ListAllProducts()
        {
            return View();
        }

        public ActionResult EntryProduct()
        {
            return View();
        }

        public ActionResult CreateAccount()
        {
            return View();
        }

        //Product entry method
        [HttpPost]
        public ActionResult ProductEntry(FormCollection collection, HttpPostedFileBase uploadImg)
        {
            var itmcde = Convert.ToString(collection["txtCode"]);
            var itmname = Request["txtname"];
            var itmdesc = Request["txtDesc"];
            var itmprce = Request["txtprice"];
            var itmonhand = Request["txtonhand"];
            var date = Request["datepicker"];


            if (uploadImg != null)
            {
                try
                {
                    string filename = Path.GetFileName(uploadImg.FileName);
                    var checkextension = Path.GetExtension(uploadImg.FileName).ToUpper();
                    int filesize = uploadImg.ContentLength;
                    string logPath = "C:\\Uploads";
                    string filepath = Path.Combine(logPath, filename);
                    uploadImg.SaveAs(filepath);
                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "INSERT INTO ITMTBL (ITMNUM,ITMNAME,ITMDESC,ITMIMG,ITMONHAND,ITMPRICE,ITMDATE)"
                                + " VALUES ("
                                + " @NUM,"
                                + " @NAME,"
                                + " @DESC,"
                                + " @IMG,"
                                + " @ONHAND,"
                                + " @PRICE,"
                                + " @DATE)";
                            cmd.Parameters.AddWithValue("@NUM", itmcde);
                            cmd.Parameters.AddWithValue("@NAME", itmname);
                            cmd.Parameters.AddWithValue("@DESC", itmdesc);
                            cmd.Parameters.AddWithValue("@IMG", filename);
                            cmd.Parameters.AddWithValue("@ONHAND", itmonhand);
                            cmd.Parameters.AddWithValue("@PRICE", itmprce);
                            cmd.Parameters.AddWithValue("@DATE", date);
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                Response.Write("<script>alert('Data Save')</script>");
                            }
                            else
                                Response.Write("<script>alert('Data Not Save')</script>");
                        }
                    }

                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Something went wrong...')</script>");
                    Response.Write(ex);
                }
            }

            return View();
        }

        //Method to make an incremental item code during product entry
        public ActionResult getItemCode()
        {
            var data = new List<object>();
            var itmcode = "";
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT MAX(ID) as 'MAXID' FROM ITMTBL";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        data.Add(new
                        {
                            itmcode = reader["MAXID"].ToString(),
                        });
                    }
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //Method to display or show the image that are saved in C:\\Uploads
        [HttpGet]
        public FileResult Image(string filename)
        {
            var folder = "";
            var filepath = "";

            try
            {
                folder = "C:\\Uploads";
                filepath = Path.Combine(folder, filename);
                if (!System.IO.File.Exists(filepath))
                {
                    //image not found here
                }
            }
            catch (Exception) { 
            
                //throw;
            }

            var mime = System.Web.MimeMapping.GetMimeMapping(Path.GetFileName(filepath));
            Response.Headers.Add("Content-Disposition", "Inline");

            return new FilePathResult(filepath, mime);
        }

        //Method/button for add to cart from ListAllProducts.cshtml
        public ActionResult Cart()
        {
            var data = new List<object>();
            var itmcode = Request["ITMNO"].Trim();
            var price = Request["PRICE"].Trim();
            var qty = Request["QTY"].Trim();

            //add here the functionalities
            //add database for cart/orders


            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //Method for creating account of users to be saved in DB
        [HttpPost]
        public ActionResult CreateAccount(FormCollection collection)
        {
            string lastname = Convert.ToString(collection["txtLastname"]);
            string firstname = Convert.ToString(collection["txtFirstname"]);
            string midname = Convert.ToString(collection["txtMidname"]);
            string address = Convert.ToString(collection["txtAddress"]);
            string contactNum = Convert.ToString(collection["txtPhoneNum"]);
            string email = Convert.ToString(collection["txtEmail"]);
            string username = Convert.ToString(collection["txtUsername"]);
            string password = Convert.ToString(collection["txtPassword"]);
            string role = Convert.ToString(collection["radRole"]);

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