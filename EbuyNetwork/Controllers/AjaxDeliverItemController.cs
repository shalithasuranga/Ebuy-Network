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
    public class AjaxDeliverItemController : Controller
    {
        // GET: AjaxDeliverItem
        [HttpPost]
        public string Index(int item_id,int user_id)
        {
            SqlConnection con = SQLCon.getConnection();
            SqlCommand q = con.CreateCommand();
            q.CommandType = CommandType.Text;
            User user =(User) Session["loggedUser"];
            q.CommandText = "UPDATE [Item] SET delivered=1 WHERE id='"+item_id+"' AND user_id='"+user.id+"'";
            if (q.ExecuteNonQuery() > 0) {
                /* add message */
                MessageCenter.addMessage(user_id, item_id, "DELIVER");
                return "OK";
            }
            return "ERROR";
        }
    }
}