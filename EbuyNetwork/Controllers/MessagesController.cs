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
    public class MessagesController : Controller
    {
        // GET: Messages
        public ActionResult Index(string keyword,string startDate, string endDate, int? page)
        {
            if (Session["loggedUser"] == null) RedirectToAction("Index", "Login");
            if (page == null)
                page = 0;
            ViewBag.page = page;

            string filter = "";


            if (keyword != null)
            {
                ViewBag.keyword = keyword;
                filter += " AND [Item].name LIKE '%" + keyword + "%'";
            }



            if ((startDate != "" && endDate != "") && (startDate != null && endDate != null))
            {
                ViewBag.startDate = startDate;
                ViewBag.endDate = endDate;
                filter += " AND CAST([Message].added_time AS DATE)>='" + startDate + "' AND CAST([Message].added_time AS DATE)<='" + endDate + "'";
            }


            User user = (User)Session["loggedUser"];
            SqlConnection con = SQLCon.getConnection();


            SqlCommand qTotal = con.CreateCommand();
            qTotal.CommandType = CommandType.Text;
            qTotal.CommandText = "SELECT COUNT([Message].id) AS total FROM [Message] LEFT JOIN [Item] ON [Message].item_id=[Item].id LEFT JOIN [User] ON [Message].user_id=[User].id LEFT JOIN [Category] ON [Item].category_id=[Category].id  LEFT JOIN [Order] ON [Item].id=[Order].item_id LEFT JOIN [User] AS OUser ON [Order].user_id=OUser.id WHERE [Message].user_id=" + user.id + "" + filter;

            SqlDataReader resultTotal = qTotal.ExecuteReader();
            resultTotal.Read();

            int totalResults = Int32.Parse(resultTotal["total"].ToString());
            int totalPages = (int)Math.Ceiling((double)totalResults / 10);
            ViewBag.totalResult = totalResults;
            ViewBag.totalPages = totalPages;

            resultTotal.Close();



            SqlCommand q = con.CreateCommand();
            q.CommandType = CommandType.Text;
            q.CommandText = "SELECT OUser.address AS oaddress,OUser.firstname AS ofirstname,OUser.lastname AS olastname,[User].address,[User].firstname,[User].lastname,[Category].id AS category_id, [Category].item_count AS category_item_count, [Category].name AS category_name, [Item].*, [Message].message_type AS message_type, [Message].added_time FROM [Message] LEFT JOIN [Item] ON [Message].item_id=[Item].id LEFT JOIN [User] ON [Message].user_id=[User].id LEFT JOIN [Category] ON [Item].category_id=[Category].id LEFT JOIN [Order] ON [Item].id=[Order].item_id LEFT JOIN [User] AS OUser ON [Order].user_id=OUser.id  WHERE [Message].user_id=" + user.id + "" + filter + " ORDER BY [Message].id DESC OFFSET " + page*10 + " ROWS FETCH NEXT 10 ROWS ONLY";
            SqlDataReader result = q.ExecuteReader();
            List<Message> messages = new List<Message>();
            while (result.Read())
            {
                messages.Add(new Message
                {
                    item = new Item
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
                        isDelivered = Int32.Parse(result["delivered"].ToString()) == 1,
                        price = Double.Parse(result["price"].ToString()),
                        name = result["name"].ToString(),
                        imageFile = result["imagefile"].ToString(),
                        description = result["description"].ToString()

                    },
                    user = new User {
                        firstName = result["firstname"].ToString(),
                        lastName = result["lastname"].ToString(),
                        address = result["address"].ToString()
                    },
                    ouser = new User
                    {
                        firstName = result["ofirstname"].ToString(),
                        lastName = result["olastname"].ToString(),
                        address = result["oaddress"].ToString()
                    },
                    messageType = result["message_type"].ToString(),
                     addedTime = DateTime.Parse(result["added_time"].ToString())

                });
            }
            ViewBag.messages = messages;
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