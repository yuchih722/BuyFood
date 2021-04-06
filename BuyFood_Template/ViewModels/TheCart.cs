using BuyFood_Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModels
{
    public class TheCart
    {

        public List<CartProductJson> ProductList { get; set; }
        public decimal TotalPriceInCart {
            get
            {
                decimal total = 0;
                foreach(var i in ProductList)
                {
                    total += i.ProductAmount;
                }
                return total;
            } }
    }
}
