using EbuyNetwork.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EbuyNetwork.Controllers
{
    public class PostItemController : Controller
    {
        // GET: PostItem
        public ActionResult Index()
        {
            if (Session["loggedUser"] == null) return RedirectToAction("Index","Login");
            SqlConnection con = SQLCon.getConnection();
            SqlCommand q = con.CreateCommand();
            q.CommandType = CommandType.Text;
            q.CommandText = "SELECT * FROM [Category]";
            SqlDataReader result = q.ExecuteReader();
            List<Category> categories = new List<Category>();
            while (result.Read()) {
                categories.Add(new Category {
                    id=Int32.Parse(result["id"].ToString()),
                    name=result["name"].ToString(),
                    item_count=Int32.Parse(result["item_count"].ToString())
                });
            }
            ViewBag.categories = categories;
            return View();
        }

        // GET: PostItem
        [HttpPost]
        public ActionResult Index(string name,int category,double price, string description,HttpPostedFileBase file)
        {

            SqlConnection con = SQLCon.getConnection();
            SqlCommand q = con.CreateCommand();
            q.CommandType = CommandType.Text;

            string imagefile = "";
            if (file.ContentLength > 0)
            {
                var fileName = Guid.NewGuid().ToString()+".jpg";
                var path = Path.Combine(Server.MapPath("~/Content/Uploads"), fileName);
                file.SaveAs(path);
                imagefile = fileName;
            }

            User loggeduser = (User)Session["loggedUser"];
            q.CommandText = "INSERT INTO [Item](category_id,user_id,name,description,price,imagefile,sold,delivered) VALUES('"+category+"','"+loggeduser.id+"','"+name.Replace("'", "''") + "','"+description.Replace("'","''")+"','"+price+"','"+imagefile+"','0','0');";

            int a = q.ExecuteNonQuery();

            /* increment category table */
            q.CommandText="UPDATE [Category] SET item_count=item_count+1 WHERE id='"+category+"'";
            q.ExecuteNonQuery();


            if (a > 0)
            {
                q.CommandText = "SELECT @@IDENTITY AS new_id";
                int new_id = Int32.Parse(q.ExecuteScalar().ToString());
                return RedirectToAction("Index","MyItems", new { new_id = new_id });
            }

            return RedirectToAction("Error", "PostItem");
        }


        public ActionResult Error() {
            return View();
        }

    }
}