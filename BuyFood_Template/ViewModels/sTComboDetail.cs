using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModels
{
    public class sTComboDetail
    {
        public int productID { get; set; }
        public string productName { get; set; }
        public decimal? price { get; set; }

        public int qty { get; set; }
        public int onSale { get; set; }
    }
}
