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
    public class EditItemController : Controller
    {
        // GET: EditItem
        public ActionResult Index(int edit_id)
        {
            SqlConnection con = SQLCon.getConnection();
            SqlCommand q = con.CreateCommand();
            q.CommandType = CommandType.Text;
            q.CommandText = "SELECT * FROM [Category]";
            SqlDataReader result = q.ExecuteReader();
            List<Category> categories = new List<Category>();
            while (result.Read())
            {
                categories.Add(new Category
                {
                    id = Int32.Parse(result["id"].ToString()),
                    name = result["name"].ToString(),
                    item_count = Int32.Parse(result["item_count"].ToString())
                });
            }
            result.Close();
            q.CommandText = "SELECT TOP 1 [Category].name AS category_name, [Category].item_count AS category_item_count,[Item].* FROM [Item] LEFT JOIN [Category] ON [Item].category_id=[Category].id WHERE [Item].id='" + edit_id + "'";
            result = q.ExecuteReader();
            result.Read();

            Item item = new Item
            {
                id = Int32.Parse(result["id"].ToString()),
                category = new Category
                {
                    id = Int32.Parse(result["category_id"].ToString()),
                    item_count = Int32.Parse(result["category_item_count"].ToString()),
                    name = result["category_name"].ToString()

                },
                userId = Int32.Parse(result["user_id"].ToString()),
                isSold = Int32.Parse(result["sold"].ToString()) == 1,
                price = Double.Parse(result["price"].ToString()),
                name = result["name"].ToString(),
                imageFile = result["imagefile"].ToString(),
                description = result["description"].ToString()
            };
            ViewBag.item = item;

            ViewBag.categories = categories;


            return View();
        }



        // GET: PostItem
        [HttpPost]
        public ActionResult Index(int edit_id, string name, double price, string description, HttpPostedFileBase file)
        {

            SqlConnection con = SQLCon.getConnection();
            SqlCommand q = con.CreateCommand();
            q.CommandType = CommandType.Text;

            string imagefile = "";
            if (file != null)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + ".jpg";
                    var path = Path.Combine(Server.MapPath("~/Content/Uploads"), fileName);
                    file.SaveAs(path);
                    imagefile = fileName;
                }
            }

            if (imagefile != "")
                q.CommandText = "UPDATE [Item] SET name='" + name.Replace("'", "''") + "',description='" + description.Replace("'", "''") + "',price='" + price + "',imagefile='" + imagefile + "' WHERE id='"+edit_id+"'";
            else
                q.CommandText = "UPDATE [Item] SET name='" + name.Replace("'", "''") + "',description='" + description.Replace("'", "''") + "',price='" + price + "' WHERE id='"+edit_id+"'";

            int a = q.ExecuteNonQuery();


            if (a > 0)
            {
                return RedirectToAction("Index", "Item", new { item_id = edit_id });
            }

            return RedirectToAction("Error", "PostItem");
        }


        public ActionResult Error()
        {
            return View();
        }



    }
}