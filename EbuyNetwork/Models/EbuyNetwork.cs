using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbuyNetwork.Models
{
    public class EbuyNetwork
    {
        public const string itemCode = "EB";

        public static string makeItemCode(int n) {
            return itemCode+ n.ToString().PadLeft(4,'0');
        }

        public static string formatDescription(string des) {
            if (des.Length > 110)
                return des.Substring(0, 110) + "...";
            else
                return des;
        }
    }
}