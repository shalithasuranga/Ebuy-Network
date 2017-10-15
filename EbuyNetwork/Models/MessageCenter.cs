using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EbuyNetwork.Models
{
    public class MessageCenter
    {
        public static bool addMessage(int userId,int itemId,string messageType) {
            SqlConnection con = SQLCon.getConnection();
            SqlCommand q = con.CreateCommand();
            q.CommandType = CommandType.Text;
            q.CommandText = "INSERT INTO [Message](user_id,item_id,message_type,added_time) VALUES('"+userId+"','"+itemId+"','"+messageType+"','"+DateTime.Now.ToString()+"');";
            if (q.ExecuteNonQuery()>0) {
                return true;
            }

            return false;

        }
    }
}