using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModels
{
    public class CartProductJson  //將CartProduct轉成json可傳遞的格式
    {
        public int cProductId { get; set; }
        public string cProductName { get; set; }
        public decimal cPrice { get; set; }
        public string cPicture { get; set; }
        public int QuantityInCart { get; set; }
        public decimal ProductAmount {get;set; }
        //public int cProductId { get; set; }
        //public string cProductName { get; set; }
        //public int cPrice { get; set; }
        //public string cPicture { get; set; }
        //public int QuantityInCart { get; set; }
        //public int ProductAmount { get; set; }
    }
}
