using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModels
{
    public class CartOrderJson
    {
        public List<CartProductJson> cartOrder { get; set; }
        public int couponSelected { get; set; }
        public string address { get; set; }
        public int transportTime { get; set; }
        public int payType { get; set; }
    }
}
