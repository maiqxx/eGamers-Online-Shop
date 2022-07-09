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
using System.Web.UI.HtmlControls;
using System.Web.SessionState;

namespace eGamersShop.Controllers
{
    [SessionState(SessionStateBehavior.Default)]
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

        public ActionResult Registration()
        {
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

        public ActionResult MyCart()
        {
            return View();
        }


        public ActionResult UpdateProduct()
        {
            return View();
        }

        public ActionResult LogIn()
        {
            if (Session["email"] != null)
            {
                return RedirectToAction("Registration", "Home", new { email = Session["email"].ToString() });
            }
            else
            {
                return View();
            }
            //return View();
        }

        [HttpPost]
        public ActionResult LogIn(FormCollection collection)
        {
            var email = Request["txtEmail"];
            var password = Request["txtPassword"];

            try
            {

                Response.Write("<script>alert(email)</script>");

                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM USERTBL WHERE EMAIL = '" + email + "' AND PASSWORD = '" + password + "' ";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {

                            Session["email"] = reader["EMAIL"].ToString();
                            var userEmail = Session["email"].ToString();
                            Response.Write("<script>alert(userEmail)</script>");
                            Response.Redirect("ListAllProducts");
                        }
                        else
                        {
                            Response.Write("<script>alert('Invalid Credentials!')</script>");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Something went wrong...')</script>");
                Response.Write(ex);
            }
            return View();
        }

        

        [HttpPost]
        public ActionResult Registration(FormCollection collection)
        {
            //var data = new List<object>();
            var lastname = Request["txtLastname"];
            var firstname = Request["txtFirstname"];
            var address = Request["txtAddress"];
            var bdate = Request["txtBdate"];
            var contactnum = Request["txtContact"];
            var email = Request["txtEmail"];
            var username = Request["txtUsername"];
            var password = Request["txtPassword"];


            //add try catch
            //add insert sql command
            //get selected value for role

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM USERTBL WHERE USERNAME = '" + username + "' OR EMAIL = '" + email + "' ";
                        SqlDataReader rd = cmd.ExecuteReader();
                        if (rd.HasRows)
                        {
                            Response.Write("<script>alert('This email is already registered.!')</script>");
                            rd.Close();
                        }
                        else
                        {
                            rd.Close();
                            cmd.CommandText = "INSERT INTO USERTBL (LASTNAME,FIRSTNAME,ADDRESS,BIRTHDATE,CONTACTNUM,EMAIL,USERNAME, PASSWORD)"
                                + " VALUES ("
                                + " @LNAME,"
                                + " @FNAME,"
                                + " @ADDRESS,"
                                + " @BDATE,"
                                + " @CONTACT,"
                                + " @EMAIL,"
                                + " @USERNAME,"
                                + " @PSWD)";
                            cmd.Parameters.AddWithValue("@LNAME", lastname);
                            cmd.Parameters.AddWithValue("@FNAME", firstname);
                            cmd.Parameters.AddWithValue("@ADDRESS", address);
                            cmd.Parameters.AddWithValue("@BDATE", bdate);
                            cmd.Parameters.AddWithValue("@CONTACT", contactnum);
                            cmd.Parameters.AddWithValue("@EMAIL", email);
                            cmd.Parameters.AddWithValue("@USERNAME", username);
                            cmd.Parameters.AddWithValue("@PSWD", password);
                            var ctr = cmd.ExecuteNonQuery();

                            if (ctr > 0)
                            {
                                Response.Write("<script>alert('Registered Successfully!')</script>");
                                Response.Redirect("LogIn");
                            }
                            else
                            {
                                Response.Write("<script>alert('Cannot create your account. ')</script>");
                            }
                                
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Sorry, something went wrong.')</script>");
                Response.Write(ex.Message);
            }

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
                            cmd.CommandText = "INSERT INTO ITMTBL (ITMNUM, ITMNAME, ITMDESC, ITMIMG, ITMONHAND, ITMPRICE, ITMDATE)"
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
            var itmcode = Request["itemno"].Trim();
            var qty = Request["qty"];
            var price = "";
            string email = Session["email"].ToString();
            var itmimg = "";
            var itemname = "";
            using (var db1 = new SqlConnection(connDB))
            {
                db1.Open();
                using (var cmd = db1.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM ITMTBL WHERE ITMNUM='" + itmcode + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        itmimg = reader["ITMIMG"].ToString();
                        price = reader["ITMPRICE"].ToString();
                        itemname = reader["ITMNAME"].ToString();

                    }
                }
                db1.Close();

            }
            //Try to add the information in DB
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd2 = db.CreateCommand())
                {
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandText = "SELECT * FROM ORDERTBL WHERE ITMNO ='" + itmcode + "'";
                    SqlDataReader reader = cmd2.ExecuteReader();
                    if (reader.Read())
                    {
                        data.Add(new
                        {
                            exist = true,
                        });
                        return Json(data, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        using (var db2 = new SqlConnection(connDB))
                        {
                            db2.Open();
                            using (var cmd = db2.CreateCommand())
                            {
                                cmd.CommandText = "INSERT INTO ORDERTBL (ITEMCODE, ITMPRICE, ITEMQTY, EMAIL, ITEMNAME, ITEMIMG)"
                                                                            + " VALUES ("
                                                                            + " @NUM,"
                                                                            + " @PRICE,"
                                                                            + " @QTY,"
                                                                            + " @EMAIL,"
                                                                            + " @INAME,"
                                                                            + " @ITEMIMAGE)";
                                cmd.Parameters.AddWithValue("@NUM", itmcode);
                                cmd.Parameters.AddWithValue("@QTY", qty);
                                cmd.Parameters.AddWithValue("@PRICE", price);
                                cmd.Parameters.AddWithValue("@EMAIL", email);
                                cmd.Parameters.AddWithValue("@INAME", itemname);
                                cmd.Parameters.AddWithValue("@ITEMIMAGE", itmimg);
                                var ctr = cmd.ExecuteNonQuery();
                                if (ctr >= 1)
                                {
                                    Response.Write("<script>alert('Add to Cart(hc)')</script>");
                                    data.Add(new
                                    {
                                        price = price,
                                        code = itmcode,
                                        qty = qty


                                    });

                                }
                                else
                                {
                                    Response.Write("<script>alert('Failed to add the item to your cart.')</script>");
                                }
                            }
                        }
                    }




                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SearchItem()
        {
            var data = new List<object>();
            var itmcode = Request["itemcode"];
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM ITMTBL WHERE ITMNUM='" + itmcode + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        data.Add(new
                        {
                            mess = 0,
                            desc = reader["itmname"].ToString(),
                            price = reader["itmprice"].ToString(),
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

        public ActionResult UpdateItem()
        {
            var data = new List<object>();

            var itmcode = Request["itemcode"];
            var itmdesc = Request["itemdesc"];
            var itmprice = Request["itemprice"];

            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE ITMTBL SET "
                        + " ITMNAME ='" + itmdesc + "',"
                        + " ITMPRICE ='" + itmprice + "'"
                        + " WHERE ITMNUM='" + itmcode + "'";
                    var ctr = cmd.ExecuteNonQuery();
                    if (ctr > 0)
                    {
                        data.Add(new
                        {
                            mess = 0
                        });
                    }
                    else
                    {
                        data.Add(new
                        {
                            mess = 1
                        });
                    }
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        
    


    }
}