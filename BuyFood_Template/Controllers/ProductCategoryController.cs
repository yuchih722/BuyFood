using BuyFood_Template.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.Controllers
{
    public class ProductCategoryController : Controller
    {
        public JsonResult getAllCategory()
        {
            var data = (new 擺腹BuyFoodContext()).TProductCategories;
            return Json(data);
        }
    }
}
