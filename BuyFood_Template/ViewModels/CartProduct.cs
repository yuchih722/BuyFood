using BuyFood_Template.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModels
{  
    public class CartProduct  //放入購物車內的商品
    {
        private readonly TProduct tproduct = null;
        [JsonConstructor]
        public CartProduct(TProduct p)
        {
            tproduct = p;
        }
 
        public int CProductId 
        { 
            get {return tproduct.CProductId;} 
            set {tproduct.CProductId = value;}
        }
        public string CProductName
        {
            get { return tproduct.CProductName; }
            set { tproduct.CProductName = value; }
        }
        public decimal CPrice
        {
            get { return (decimal)tproduct.CPrice; }
            set { tproduct.CPrice = value; }
        }
        public string CPicture
        {
            get { return tproduct.CPicture; }
            set { tproduct.CPicture = value; }
        }
        int count = 1;
        public int QuantityInCart { get { return count; } set { count = value; } }  //購物車內的商品數量
        public decimal ProductAmount   //單項商品總計
        { 
            get { return CPrice * QuantityInCart; }
        }
        public CartProductJson TransferJson(CartProductJson pdtjson)  //將CartProduct的屬性值轉存至CartProductJson(轉Json)
        {
            pdtjson.cProductId = tproduct.CProductId;
            pdtjson.cProductName = tproduct.CProductName;
            pdtjson.cPrice = (decimal)tproduct.CPrice;
            pdtjson.cPicture = tproduct.CPicture;
            pdtjson.QuantityInCart = QuantityInCart;
            return pdtjson;
        }

    }

}
