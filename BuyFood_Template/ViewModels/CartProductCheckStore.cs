using BuyFood_Template.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModels
{  
    public class CartProductCheckStore  //放入購物車內的商品
    {
        public int cProductId { get; set; }

        public int quantityInCart { get; set; }  //購物車內的商品數量
        public int finishTime { get; set; }

    }

}
