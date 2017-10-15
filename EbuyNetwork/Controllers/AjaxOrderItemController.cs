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
    public class AjaxOrderItemController : Controller
    {
        // GET: AjaxOrderItem
        [HttpPost]
        public string Index(int item_id,string payment_code,int user_id)
        {
            SqlConnection con = SQLCon.getConnection();
            SqlCommand q = con.CreateCommand();
            q.CommandType = CommandType.Text;
            User user = (User)Session["loggedUser"];
            q.CommandText = "INSERT INTO [Order](user_id,item_id,ordered_time,payment_code) VALUES('"+user.id+"','"+item_id+"','"+DateTime.Now.ToString()+"','"+payment_code+"')";
            if (q.ExecuteNonQuery()>0) {
                q.CommandText = "UPDATE [Item] SET sold=1 WHERE id='"+item_id+"'";
                if (q.ExecuteNonQuery() > 0)
                    /* add message */
                    MessageCenter.addMessage(user_id, item_id, "PURCHASE");
                    return "OK";
            }

            return "ERROR";
        }
    }
}