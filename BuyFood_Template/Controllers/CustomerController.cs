using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
//using prjBuyFoodCore.Models;
//using prjBuyFoodCore.ViewModel;

namespace BuyFood_Template.Controllers
{
    public class CustomerController : Controller
    {
        private IHostingEnvironment iv_host;
        public CustomerController(IHostingEnvironment p)
        {
            iv_host = p;
        }
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Create()
        {
            //ViewBag.ID = "";
            return View();
        }
        //public IActionResult Create(string id)
        //{
        //    ViewBag.ID = id;
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Create(CCustomerCreateViewModel newMember)
        //{
        //    string photoname = Guid.NewGuid().ToString() + ".jpg";

        //    using (var MemberPhoto = new FileStream(iv_host.WebRootPath + @"\MemberPhoto\" + photoname, FileMode.Create))
        //    {
        //        newMember.img.CopyTo(MemberPhoto);
        //    }

        //    newMember.CPicture = @"~\MemberPhoto\" + photoname;
        //    newMember.CBlackList = 0;
        //    newMember.CFreezeCount = 0;
        //    newMember.CDeposit = 0;

        //    擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
        //    db.TMembers.Add(newMember.member);
        //    db.SaveChanges();

        //    return Redirect("~/Home/Index");
        //}
    }
}

