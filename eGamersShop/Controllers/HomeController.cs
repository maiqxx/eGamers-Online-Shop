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
using eGamersShop.Models;

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

        public ActionResult Registration()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Registration(User model)
        {
            DataTable dt = new DataTable();
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@lastname", model.Lastname),
                new SqlParameter("@firstname", model.Firstname),
                new SqlParameter("@address", model.Address),
                new SqlParameter("@birthdate", model.Birthdate),
                new SqlParameter("@address", model.Contactnum),
                new SqlParameter("@address", model.Email),
                new SqlParameter("@address", model.Username),
                new SqlParameter("@address", model.Password),
                new SqlParameter("@address", model.Role)
            };
            dt = SaveData("Registration", param);
            if(dt.Rows.Count > 0)
            {
                if(Convert.ToInt32(dt.Rows[0]["msg"]) == 1)
                {
                    ViewBag.Msg = "Registered Successfully!";
                }
            }
            return View();
        }

        public DataTable SaveData (string ProName, SqlParameter[] Param)
        {
            DataTable dt = new DataTable();
            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    SqlCommand cmd = new SqlCommand(ProName, db);
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach(SqlParameter p in Param)
                    {
                        cmd.Parameters.Add(p);
                    }
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);

                    db.Close();
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            finally
            {
                //
            }
            return dt;
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


        public ActionResult UpdateProduct()
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
            var itemname = "";
            var email = " ";

            //add here the functionalities
            //add database for cart/orders

            try
            {
                using (var db = new SqlConnection(connDB))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT ITMNAME FROM ITMTBL WHERE ITMNUM = '" + itmcode + "' ";
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            data.Add(new
                            {
                                itemname = reader["ITMNAME"].ToString(),

                            });
                        }

                        cmd.CommandText = "INSERT INTO ORDERTBL (ITEMCODE, ITEMNAME, ITEMPRICE, QUANTITY, EMAIL)"
                            + " VALUES ("
                            + " @CODE,"
                            + " @NAME,"
                            + " @PRICE,"
                            + " @QTY,"
                            + " @EMAIL,"
                            + " @DATE)";
                        cmd.Parameters.AddWithValue("@CODE", itmcode);
                        cmd.Parameters.AddWithValue("@NAME", itemname);
                        cmd.Parameters.AddWithValue("@PRICE", price);
                        cmd.Parameters.AddWithValue("@QTY", qty);
                        cmd.Parameters.AddWithValue("@EMAIL", email);
                        cmd.Parameters.AddWithValue("@DATE", DateTime.Now);
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            Response.Write("<script>alert('Item added to cart.')</script>");
                        }
                        else
                        {
                            Response.Write("<script>alert('Failed to adding to cart.')</script>");
                        }
                            



                    }
                }
            }
            catch (Exception ex) 
            {
                Response.Write("<script>alert('Something went wrong...')</script>");
                Response.Write(ex);
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