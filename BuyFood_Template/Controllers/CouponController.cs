using BuyFood_Template.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.Controllers
{
    public class CouponController : Controller
    {
        //todo
        public JsonResult getCoupons (string id, string used)


        {
            if (used == "0")
            {
                var data = (new 擺腹BuyFoodContext()).TCupons
                .Where(n => n.CMenberId == int.Parse(id)
                && n.CBeUsed == int.Parse(used)
                && n.CValidDate >= DateTime.Today)
                .Select(n => new
                {
                    categoryName = n.CCuponCategory.CategoryName,
                    rdate = n.CReceivedTime.ToString("yyyy/MM/dd"),
                    edate = n.CValidDate.ToString("yyyy/MM/dd"),
                    code = n.CDiscountCode
                }) ;
                return Json(data);
            }
            else
            {
                var data2 = (new 擺腹BuyFoodContext()).TCupons
                .Where(n => n.CMenberId == int.Parse(id)
                && (n.CBeUsed == int.Parse(used)
                || n.CValidDate < DateTime.Today))
                .Select(n => new
                {
                    categoryName = n.CCuponCategory.CategoryName,
                    rdate = n.CReceivedTime.ToString("yyyy/MM/dd"),
                    edate = n.CValidDate.ToString("yyyy/MM/dd"),
                    code = n.CDiscountCode,
                    beused = n.CBeUsed
                });
                return Json(data2);
            }
            
            
        }
    }
}
