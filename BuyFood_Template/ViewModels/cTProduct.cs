using BuyFood_Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModels
{
    public class cTProduct
    {
        public TProduct TProduct { get; set; }
        public int count { get; set; }
        public int? sum { get; set; }
    }
}
