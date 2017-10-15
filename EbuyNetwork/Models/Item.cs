using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbuyNetwork.Models
{
    public class Item
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string name { get; set; }
        public bool isSold { get; set; }
        public bool isDelivered { get; set; }
        public string description { get; set; }
        public string imageFile { get; set; }
        public double price { get; set; }
        public Category category { get; set; }
        public int purchasedUserId { get; set; }
        public User user { get; set; }

    }
}