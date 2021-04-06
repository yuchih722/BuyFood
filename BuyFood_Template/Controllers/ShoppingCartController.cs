using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyFood_Template.Models;
using BuyFood_Template.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BuyFood_Template.Controllers
{
    public class ShoppingCartController : Controller
    {
        public IActionResult CurrentCartItem()
        {
            擺腹BuyFoodContext BuyFoodDB = new 擺腹BuyFoodContext();
            var result = from i in BuyFoodDB.TCupons
                         join b in BuyFoodDB.TCuponCategories
                         on i.CCuponCategoryId equals b.CCuponCategoryId
                         where i.CMenberId == 7 && i.CBeUsed == 0
                         orderby i.CCuponCategoryId
                         select new CouponProperty {
                             CCuponId = i.CCuponId,
                             CCuponCategoryId = i.CCuponCategoryId,
                             CategoryName = b.CategoryName,
                             CCutPrice = (decimal)b.CCutPrice,     
                         };
            //ViewBag.Coupon = result.ToList();
            return View(result.ToList());
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
       
    }
}
