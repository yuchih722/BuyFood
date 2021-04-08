using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyFood_Template.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BuyFood_Template.ViewModels;
using BuyFood_Template.ViewModel;
using Microsoft.AspNetCore.Hosting; //圖片
using System.IO;

namespace adminCode.Controllers
{
    public class MemberManagementController : Controller
    {
        //public IActionResult List() //列資料表
        //{
        //    var table = from m in (new 擺腹BuyFoodContext()).TMembers
        //                select m;
        //    List<MemberManagementViewModel> list = new List<MemberManagementViewModel>();
        //    foreach (TMember t in table)
        //        list.Add(new MemberManagementViewModel(t));
        //    return View(list);
        //}
        private 擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
        public IActionResult List()
        {
            ViewBag.All = db.TMembers.Select(n => n).Count();
            ViewBag.male = db.TMembers.Where(n => n.CGender == "男").Select(n => n).Count();
            ViewBag.female = db.TMembers.Where(n => n.CGender == "女").Select(n => n).Count();
            ViewBag.deposit = db.TMembers.Where(n => n.CDeposit > 0).Select(n => n).Count();//未儲值
            ViewBag.Undeposited = db.TMembers.Where(n => n.CDeposit == 0).Select(n => n).Count();//已儲值
            return View();
        }
        public JsonResult jsonList(string str)
        {
            List<MemberManagementViewModel> member =new List<MemberManagementViewModel>();
            IEnumerable<TMember> table = null;
            if (str == "All")
            {
                table = from c in db.TMembers
                        orderby c.CMemberId descending
                        select c;
            }
            else if (str == "male")
            {
                table = from c in db.TMembers where c.CGender == "男" select c;
            }
            else if(str== "female")
            {
                table = from c in db.TMembers where c.CGender == "女" select c;              
            }
            else if(str== "deposit")
            {
                table = from c in db.TMembers where c.CDeposit > 0 select c;
            }
            else if(str== "Undeposited")
            {
                table = from c in db.TMembers where c.CDeposit == 0 select c;
            }
            else
            {
                table = from p in db.TMembers
                        where p.CName.Contains(str)   //???????
                        select p;
            }
            if (table != null) //?????
            {
                foreach(TMember x in table)
                {
                    member.Add(new MemberManagementViewModel(x));
                }
            }
            return Json(member);
        }
        //以上新增圖片
        private IHostingEnvironment iv_host;
        public MemberManagementController(IHostingEnvironment p)
        {
            iv_host = p;
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(MemberManagementViewModel newMember)
        {
            string photoName = Guid.NewGuid().ToString() + ".jpg";
            if (newMember.image != null)
            {
                using (var photo = new FileStream(iv_host.WebRootPath + @"\MemberPhoto\" + photoName, FileMode.Create))
                {
                    newMember.image.CopyTo(photo);
                }
            }            
            newMember.CPicture = @"~/MemberPhoto/" + photoName;

            db.TMembers.Add(newMember.member);
            db.SaveChanges();
            return RedirectToAction("List");
        }
        public IActionResult Edit(int? id)
        {           
            if (id != null)
            {                
                TMember table = db.TMembers.FirstOrDefault(a => a.CMemberId == id);
                if (table != null)
                {
                    return View(new MemberManagementViewModel(table));
                }
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public IActionResult Edit(MemberManagementViewModel p)
        {
            if (p != null)
            {                       
                TMember table = db.TMembers.FirstOrDefault(t => t.CMemberId == p.CMemberId);
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
                    table.CPicture = p.CPicture;
                    db.SaveChanges();
                }
            }            
            return RedirectToAction("List");
        }        

        public IActionResult Delete(int? id)
        {        
            var result = db.TMembers.Where(m => m.CMemberId == id).FirstOrDefault();
            db.TMembers.Remove(result);
            db.SaveChanges();          
            return RedirectToAction("List");
        }
        
        public JsonResult detail(int? id)
        {            
            //var table = from o in db.TOrders
            //            join m in db.TOrderStatuses on o.CMemberId equals id
            //            select new { CArrivedAddress = o.CArrivedAddress, cOrderDate = o.COrderDate, cTransportMinute = o.CTransportMinute, cOrderStatusName = m.COrderStatusName };

            var table = from o in db.TOrders
                        join m in db.TMembers on o.CMemberId equals m.CMemberId
                        where m.CMemberId==id
                        select new { CArrivedAddress = o.CArrivedAddress, cOrderDate = o.COrderDate, cTransportMinute = o.CTransportMinute, cPicture = m.CPicture };
            //var table = db.TOrders.Where(n => n.CMemberId == id)
            return Json(table);
        }

        public IActionResult Getemail()
        {
            return View();
        }
        public ActionResult GetImage(int? id)
        {
            //參考網址 https://kevintsengtw.blogspot.com/2013/10/aspnet-mvc-image.html
            var mberImage = db.TMembers.FirstOrDefault(m => m.CMemberId == id);
            if(mberImage!=null && mberImage.CPicture != null)
            {
                using(MemoryStream ms=new MemoryStream())
                {
                    
                }
            }
            return new EmptyResult();               
        }

    }
}
