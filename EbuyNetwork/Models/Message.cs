using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbuyNetwork.Models
{
    public class Message
    {
        public int id { get; set; }
        public User user { get; set; }
        public Item item { get; set; }
        public string messageType { get; set; }
        public DateTime addedTime { get; set; }
        public User ouser { get; set; }
    }
}