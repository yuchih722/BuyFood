using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyFood_Template.Models;
using BuyFood_Template.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BuyFood_Template.Controllers
{
    public class MemberManagementController : Controller
    {

        public IActionResult List()
        {
            var table = from m in (new 擺腹BuyFoodContext()).TMembers
                        select m;
            List<MemberManagementViewModel> list = new List<MemberManagementViewModel>();
            foreach (TMember t in table)
                list.Add(new MemberManagementViewModel(t));
            return View(list);
        }
        [HttpPost]
        public IActionResult List(string keyword)//關鍵字查詢
        {
            IEnumerable<TMember> table = null;

            if (string.IsNullOrEmpty(keyword))
            {
                table = from m in (new 擺腹BuyFoodContext()).TMembers
                        select m;
            }
            else
            {
                table = from m in (new 擺腹BuyFoodContext()).TMembers
                        where m.CName.Contains(keyword)
                        select m;
            }
            return View(table);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(MemberManagementViewModel p)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            db.TMembers.Add(p.member);
            db.SaveChanges();
            return RedirectToAction("List");
        }
        public IActionResult Edit(int? id)
        {
            //List<MemberManagementViewModel> list = new List<MemberManagementViewModel>();
            TMember table = null;
            if (id != null)
            {
                擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
                table = db.TMembers.FirstOrDefault(a => a.CMemberId == id);        
            }
          return View(table);
        }
        [HttpPost]
        public IActionResult Edit(TMember p)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            TMember table = db.TMembers.Where(x => x.CMemberId == p.CMemberId).Select(p => p).FirstOrDefault();
            if (table != null)
            {
                table.CName = p.CName;
                table.CEmail = p.CEmail;
                table.CPassword = p.CPassword;
                table.CPhone = p.CPhone;
                table.CGender = p.CGender;
                table.CAddress = p.CAddress;
                table.CBlackList = p.CBlackList;
                table.CDeposit = p.CDeposit;
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

        public IActionResult Delete(int? id)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var result = db.TMembers.Where(m => m.CMemberId == id).FirstOrDefault();
            db.TMembers.Remove(result);
            db.SaveChanges();          
            return RedirectToAction("List");
        }

    }
}
