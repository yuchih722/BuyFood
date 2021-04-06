using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyFood_Template.Models;
using BuyFood_Template.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BuyFood_Template.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult MemberCenter()
        {
            ViewBag.memberID = 2;
            TMember data = (new 擺腹BuyFoodContext()).TMembers.FirstOrDefault(n => n.CMemberId == 2);
            return View(new MemberCenterViewModel(data));
        }

        [HttpPost]
        public JsonResult saveProfile([FromBody] TMember member)
        {
            擺腹BuyFoodContext dbcontext = new 擺腹BuyFoodContext();
            TMember data = dbcontext.TMembers.FirstOrDefault(n => n.CMemberId == member.CMemberId);
            data.CName = member.CName;
            dbcontext.SaveChanges();
            return Json(data);
        }

        public JsonResult getDeposits(string id)
        {
            var data = (new 擺腹BuyFoodContext()).TDeposits.Where(n => n.CMemberId == int.Parse(id));
            return Json(data);
        }

    }

}
