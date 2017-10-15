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
    public class AjaxCancelOrderController : Controller
    {
        // GET: AjaxCancelOrder
        [HttpPost]
        public string Index(int order_id,int item_id,int user_id)
        {
            SqlConnection con = SQLCon.getConnection();
            SqlCommand q = con.CreateCommand();
            q.CommandType = CommandType.Text;
            User user = (User)Session["loggedUser"];
            q.CommandText = "UPDATE [Item] SET sold=0,delivered=0 WHERE id='" + item_id + "'";
            if (q.ExecuteNonQuery() > 0) {
                q.CommandText = "DELETE FROM [Order] WHERE user_id='"+user.id+"' AND item_id='"+item_id+"'";
                if (q.ExecuteNonQuery()>0) {
                    /* add message */
                    MessageCenter.addMessage(user_id, item_id, "CANCEL");
                    return "OK";
                }
            }
            return "ERROR";
        }
    }
}