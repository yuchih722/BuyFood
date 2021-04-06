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
            擺腹BuyFoodContext dbcontext = new 擺腹BuyFoodContext();
            ViewBag.memberID = 2;
            TMember data = dbcontext.TMembers.FirstOrDefault(n => n.CMemberId == 2);

            return View(new MemberCenterViewModel(data));
        }

        public JsonResult updateMemberCenter(string id)
        {
            擺腹BuyFoodContext dbcontext = new 擺腹BuyFoodContext();
            var issueCombo = dbcontext.TComboDetails
                    .Where(n => n.CCombo.CMemberId == int.Parse(id))
                    .Select(n => new {
                        comboID = n.CComboId,
                        productID = n.CProduct.CProductId,
                        productOn = n.CProduct.CIsOnSaleId
                    })
                    .GroupBy(n => n.comboID)
                    .Select(n => new
                    {
                        comboID = n.Key,
                        issueItem = n.Count(p => p.productOn == 3)
                    });

            return Json(issueCombo);
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



    }

}
