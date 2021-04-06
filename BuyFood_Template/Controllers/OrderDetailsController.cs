using BuyFood_Template.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.Controllers
{
    public class OrderDetailsController : Controller
    {
        public IActionResult List()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var table = from order in db.TOrderDetails
                        select order;
            return View(table);
        }
    }
}
