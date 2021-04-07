using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyFood_Template.Models;
using BuyFood_Template.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BuyFood_Template.Controllers
{
    public class ShoppingCartController : Controller
    {
        public IActionResult CurrentCartItem()
        {
            return View();
        }
        //抓取對應商品的庫存數量
        [HttpPost]
        public JsonResult CurrentItemCount([FromBody] List<int> CartItemID)
        {
            List<CartProductCheckStore> quantityList = new List<CartProductCheckStore>();
            foreach(var i in CartItemID)
            {
                var result = new 擺腹BuyFoodContext().TProducts.Where(x => x.CProductId == i).FirstOrDefault();
                CartProductCheckStore CartItem = new CartProductCheckStore()
                {
                    cProductId = i,
                    quantityInCart = (int)result.CQuantity,
                    finishTime = (int)result.CFinishedTime
                };
                quantityList.Add(CartItem);
            }
            return Json(quantityList);
        }
        [HttpPost]
        public JsonResult CheckCouponCode([FromBody] string Code)
        {
            擺腹BuyFoodContext BuyFoodDB = new 擺腹BuyFoodContext();
            var result = (from i in BuyFoodDB.TCupons
                          join k in BuyFoodDB.TCuponCategories
                          on i.CCuponCategoryId equals k.CCuponCategoryId
                          where i.CDiscountCode == Code && i.CBeUsed == 0
                          select new CouponProperty()
                          {
                              CCuponId = i.CCuponId,
                              CCutPrice = (decimal)k.CCutPrice,
                          }).FirstOrDefault();
            if (result == null)
            {
                return Json("0");
            }
            else
            {
                return Json(result);
            }
        }
    }
}
