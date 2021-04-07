using BuyFood_Template.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.Controllers
{
    public class yuMemberController : Controller
    {
        擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
        public IActionResult memberView()
        {
            
            return View();
        }
        public JsonResult memberlist()
        {
            var table = db.TMembers.Where(n => n.CName != "管理員").Select(n => n);
            return Json(table);
        }
    }
}
