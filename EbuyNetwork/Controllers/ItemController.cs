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
    public class ItemController : Controller
    {

        // GET: Item
        public ActionResult Index(int item_id)
        {
            SqlConnection con = SQLCon.getConnection();
            SqlCommand q = con.CreateCommand();
            q.CommandType = CommandType.Text;
            q.CommandText = "SELECT TOP 1 [User].id AS seller_id, [User].firstname,[User].lastname, [User].email,[User].address,[User].bankdetails,[Category].name AS category_name, [Category].item_count AS category_item_count,[Item].* FROM [Item] LEFT JOIN [Category] ON [Item].category_id=[Category].id LEFT JOIN [User] ON [Item].user_id=[User].id WHERE [Item].id='"+item_id+"'";
            SqlDataReader result = q.ExecuteReader();
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
                description = result["description"].ToString(),
                user = new User
                {
                    id = Int32.Parse(result["seller_id"].ToString()),
                    email = result["email"].ToString(),
                    firstName = result["firstname"].ToString(),
                    lastName = result["lastname"].ToString(),
                    address = result["address"].ToString(),
                    bankDetails = result["bankdetails"].ToString()
                }
            };
            ViewBag.item = item;
            result.Close();

            /* get if item is sold */
            if (item.isSold)
            {
                q.CommandText = "SELECT TOP 1 [User].*,[Order].id AS oid,[Order].ordered_time AS ordered_time, [Order].payment_code AS payment_code FROM [Order] LEFT JOIN [User] ON [Order].user_id=[User].id WHERE [Order].item_id='"+item.id+"'";
                result = q.ExecuteReader();
                if (result.HasRows)
                {
                    result.Read();
                    Order order = new Order
                    {
                        id = Int32.Parse(result["oid"].ToString()),
                        orderedTime = DateTime.Parse(result["ordered_time"].ToString()),
                        paymentCode= result["payment_code"].ToString(),
                        item = item,
                        user = new User
                        {
                            id = Int32.Parse(result["id"].ToString()),
                            email = result["email"].ToString(),
                            firstName = result["firstname"].ToString(),
                            lastName = result["lastname"].ToString(),
                            address = result["address"].ToString(),
                            bankDetails = result["bankdetails"].ToString()
                        }


                    };

                    ViewBag.order = order;
                   
                }
                
            }


            return View();
        }
    }
}