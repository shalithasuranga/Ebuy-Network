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
    public class AjaxRemoveItemController : Controller
    {
        // GET: AjaxRemoveItem
        [HttpPost]
        public String Index(int remove_id)
        {
            SqlConnection con = SQLCon.getConnection();
            SqlCommand q = con.CreateCommand();
            q.CommandType = CommandType.Text;


            q.CommandText = "SELECT TOP 1 category_id FROM [Item] WHERE id='"+remove_id+"'";
            SqlDataReader result = q.ExecuteReader();
            if (result.HasRows) {

                result.Read();
                int category_id = Int32.Parse(result["category_id"].ToString());
                result.Close();
                q.CommandText = "DELETE FROM [Item] WHERE id='" + remove_id + "'";
                if (q.ExecuteNonQuery() > 0)
                {
                    q.CommandText = "UPDATE [Category] SET item_count=item_count-1 WHERE id='" + category_id + "'";
                    if (q.ExecuteNonQuery() > 0)
                    {
                        q.CommandText = "DELETE FROM [Order] WHERE item_id='"+remove_id+"'";
                        q.ExecuteNonQuery();
                        return "OK";
                    }
                        
                }

            }


            return "ERROR";
        }
    }
}