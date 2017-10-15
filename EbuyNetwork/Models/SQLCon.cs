using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EbuyNetwork.Models
{
    public class SQLCon
    {
        
        public static SqlConnection getConnection() {
            SqlConnection con = new SqlConnection("Data Source=DECASPC\\SQLEXPRESS;Initial Catalog=ebuynetwork;User ID=sa;Password=root");
            con.Open();
            return con; 
        }     

    }
}