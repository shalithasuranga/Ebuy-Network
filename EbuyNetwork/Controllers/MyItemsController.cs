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
    public class MyItemsController : Controller
    {
        // GET: MyItems
        public ActionResult Index(string keyword,int? status,int? startPrice,int? endPrice,int? category_id,int? page)
        {
            if (Session["loggedUser"] == null) RedirectToAction("Index","Login");

            if (page == null)
                page = 0;
            ViewBag.page = page;

            string filter = "";

            if (category_id != null && category_id > 0)
            {
                ViewBag.category_id = category_id;
                filter += " AND [Item].category_id=" + category_id;
            }

            if (keyword != null)
            {
                ViewBag.keyword = keyword;
                filter += " AND [Item].name LIKE '%" + keyword + "%'";
            }

            ViewBag.status = status;
            if (status == 1)
            {
                filter += " AND [Item].sold=0 ";
            }
            else if (status == 2) {
                filter += " AND [Item].sold=1 ";
            }
            else if (status==3) {
                filter += " AND [Item].delivered=1 ";
            }

            if (startPrice != null && endPrice != null)
            {
                ViewBag.startPrice = startPrice;
                ViewBag.endPrice = endPrice;
                filter += " AND [Item].price>=" + startPrice + " AND [Item].price<=" + endPrice + "";
            }

            User user = (User)Session["loggedUser"];
            SqlConnection con = SQLCon.getConnection();


            SqlCommand qTotal = con.CreateCommand();
            qTotal.CommandType = CommandType.Text;
            qTotal.CommandText = "SELECT COUNT([Item].id) AS total FROM [Item] LEFT JOIN [Category] ON [Item].category_id=[Category].id WHERE [Item].user_id='" + user.id + "' " + filter ;

            SqlDataReader resultTotal = qTotal.ExecuteReader();
            resultTotal.Read();

            int totalResults = Int32.Parse(resultTotal["total"].ToString());
            int totalPages = (int)Math.Ceiling((double)totalResults / 10);
            ViewBag.totalResult = totalResults;
            ViewBag.totalPages = totalPages;

            resultTotal.Close();







            SqlCommand q = con.CreateCommand();
            q.CommandType = CommandType.Text;
            q.CommandText = "SELECT [Order].user_id AS oid, [Category].name AS category_name, [Category].item_count AS category_item_count,[Item].* FROM [Item] LEFT JOIN [Category] ON [Item].category_id=[Category].id LEFT JOIN [Order] ON [Item].id=[Order].item_id WHERE [Item].user_id='"+user.id+"' "+filter+ " ORDER BY [Item].id DESC OFFSET " + page*10 + " ROWS FETCH NEXT 10 ROWS ONLY";
            SqlDataReader result = q.ExecuteReader();
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
                    isSold = Int32.Parse(result["sold"].ToString())==1,
                    isDelivered= Int32.Parse(result["delivered"].ToString()) == 1,
                    price = Double.Parse(result["price"].ToString()),
                    name = result["name"].ToString(),
                    imageFile = result["imagefile"].ToString(),
                    description = result["description"].ToString(),
                    purchasedUserId = Int32.Parse(result["oid"].ToString()==""?"0": result["oid"].ToString())

                });
            }
            ViewBag.items = items;
            ViewBag.item_count = totalResults;


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