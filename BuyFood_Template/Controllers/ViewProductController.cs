using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BuyFood_Template.Models;

namespace BuyFood_Template.Controllers
{
    public class ViewProductController : Controller
    {
        public IActionResult ShowProduct()
        {
            return View();
        }
        public IActionResult GetProductInDB()
        {
            var AllProduct = new 擺腹BuyFoodContext().TProducts.Select(x => x).ToList();
            return Json(AllProduct);
        }

        
    }
}
