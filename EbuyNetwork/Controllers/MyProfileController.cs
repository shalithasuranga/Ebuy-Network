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
    public class MyProfileController : Controller
    {
        // GET: MyProfile
        public ActionResult Index()
        {
            if (Session["loggedUser"] == null) return RedirectToAction("Index","Login");
            return View();
        }

        // POST : MyProfile
        [HttpPost]
        public ActionResult Index(string editRequest, string firstname, string lastname, string password, string address, string bankdetails)
        {

            SqlConnection con = SQLCon.getConnection();
            User luser = (User)Session["loggedUser"];
            SqlCommand q1 = con.CreateCommand();
            q1.CommandType = CommandType.Text;
            q1.CommandText = "UPDATE [User] SET password='" + password + "',firstname='" + firstname + "',lastname='" + lastname + "',address='" + address + "',bankdetails='" + bankdetails + "' WHERE id='"+luser.id+"'";
            q1.ExecuteNonQuery();
            SqlCommand q = con.CreateCommand();
            q.CommandType = CommandType.Text;
            q.CommandText = "SELECT TOP 1 * FROM [User] WHERE id='"+luser.id+"'";
            SqlDataReader result = q.ExecuteReader();

            /* update session */
            if (result.HasRows)
            {
                result.Read();
                User user = new User
                {
                    firstName = result["firstname"].ToString(),
                    lastName = result["lastname"].ToString(),
                    email = result["email"].ToString(),
                    password = result["password"].ToString(),
                    address = result["address"].ToString(),
                    bankDetails = result["bankdetails"].ToString(),
                    id = Int32.Parse(result["id"].ToString())
                };
                Session["loggedUser"] = user;
            }
            return RedirectToAction("Index", "MyProfile");
        }
    }
}