using EbuyNetwork.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EbuyNetwork.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index(int? category_id, string keyword, int? startPrice, int? endPrice,int? page)
        {
            SqlConnection con = SQLCon.getConnection();
            SqlCommand q = con.CreateCommand();

            if (page == null)
                page = 0;

            ViewBag.page = page;
            /* categories */
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
            ViewBag.categories = categories;
            result.Close();
            /* latest items */

            string filter = "";

            if (category_id != null && category_id>0) {
                ViewBag.category_id = category_id;
                filter += " AND [Item].category_id="+category_id;
            }

            if (keyword != null) {
                ViewBag.keyword = keyword;
                filter += " AND [Item].name LIKE '%"+keyword+"%'";
            }

            if (startPrice != null && endPrice != null) {
                ViewBag.startPrice = startPrice;
                ViewBag.endPrice = endPrice;
                filter += " AND [Item].price>="+startPrice+" AND [Item].price<="+endPrice+"";
            }




            SqlCommand qTotal = con.CreateCommand();
            qTotal.CommandType = CommandType.Text;
            qTotal.CommandText = "SELECT COUNT([Item].id) AS total FROM [Item] LEFT JOIN [Category] ON [Item].category_id=[Category].id WHERE [Item].sold=0 " + filter + "";

            SqlDataReader resultTotal = qTotal.ExecuteReader();
            resultTotal.Read();

            int totalResults = Int32.Parse(resultTotal["total"].ToString());
            int totalPages =(int)Math.Ceiling((double)totalResults/10);
            ViewBag.totalResult = totalResults;
            ViewBag.totalPages = totalPages;

            resultTotal.Close();


            q.CommandType = CommandType.Text;
            q.CommandText = "SELECT [Category].name AS category_name, [Category].item_count AS category_item_count,[Item].* FROM [Item] LEFT JOIN [Category] ON [Item].category_id=[Category].id WHERE [Item].sold=0 " + filter + " ORDER BY [Item].id DESC OFFSET "+page*10+" ROWS FETCH NEXT 10 ROWS ONLY";



            result = q.ExecuteReader();

            int currentItems = 0;
            List<Item> items = new List<Item>();
            while (result.Read())
            {
                items.Add(new Item
                {
                    id = Int32.Parse(result["id"].ToString()),
                    category = new Category
                    {
                        id = Int32.Parse(result["category_id"].ToString()),
                        item_count = Int32.Parse(result["category_item_count"].ToString()),
                        name = result["category_name"].ToString()

                    },
                    userId = Int32.Parse(result["user_id"].ToString()),
                    price = Double.Parse(result["price"].ToString()),
                    name = result["name"].ToString(),
                    imageFile = result["imagefile"].ToString(),
                    description = result["description"].ToString()

                });
                currentItems++;
            }
            ViewBag.items = items;
            ViewBag.currentItems = currentItems;



            /*pages */
            string longurl = Request.Url.AbsoluteUri;
            var uriBuilder = new UriBuilder(longurl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["page"] = "pageNumber";

            uriBuilder.Query = query.ToString();
            longurl = uriBuilder.ToString();
            ViewBag.qurl = longurl;

            return View();
        }
    }
}