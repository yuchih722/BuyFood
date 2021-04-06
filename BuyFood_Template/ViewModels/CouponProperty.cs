using BuyFood_Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModels
{
    public class CouponProperty
    {
        public int CCuponId { get; set; }
        public int CCuponCategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal CCutPrice { get; set; }
    }
}
