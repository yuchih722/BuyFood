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

namespace BuyFood_Template.Controllers
{
    public class MemberManagementController : Controller
    {
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
            if (str == "All")
            {
                var table = from c in db.TMembers
                            orderby c.CMemberId descending
                            select new
                            {
                                c.CMemberId,
                                c.CPicture,
                                c.CName,
                                c.CGender,
                                cAge = c.CAge.ToString("yyyy/MM/dd"),
                                c.CAddress,
                                c.CPhone,
                                c.CEmail,
                                c.CPassword,
                                c.CBlackList,
                                c.CDeposit,
                            };
                return Json(table);
            }
            else if (str == "male")
            {
                var table = from c in db.TMembers where c.CGender == "男"
                            select new
                            {
                                c.CMemberId,
                                c.CPicture,
                                c.CName,
                                c.CGender,
                                cAge = c.CAge.ToString("yyyy/MM/dd"),
                                c.CAddress,
                                c.CPhone,
                                c.CEmail,
                                c.CPassword,
                                c.CBlackList,
                                c.CDeposit,
                            };
                return Json(table);
            }
            else if(str== "female")
            {
                var table  = from c in db.TMembers where c.CGender == "女"
                             select new
                             {
                                 c.CMemberId,
                                 c.CPicture,
                                 c.CName,
                                 c.CGender,
                                 cAge = c.CAge.ToString("yyyy/MM/dd"),
                                 c.CAddress,
                                 c.CPhone,
                                 c.CEmail,
                                 c.CPassword,
                                 c.CBlackList,
                                 c.CDeposit,
                             };
                return Json(table);
            }
            else if(str== "deposit")
            {
                var table  = from c in db.TMembers where c.CDeposit > 0
                             select new
                             {
                                 c.CMemberId,
                                 c.CPicture,
                                 c.CName,
                                 c.CGender,
                                 cAge = c.CAge.ToString("yyyy/MM/dd"),
                                 c.CAddress,
                                 c.CPhone,
                                 c.CEmail,
                                 c.CPassword,
                                 c.CBlackList,
                                 c.CDeposit,
                             };
                return Json(table);
            }
            else if(str== "Undeposited")
            {
               var table = from c in db.TMembers where c.CDeposit == 0
                           select new
                           {
                               c.CMemberId,
                               c.CPicture,
                               c.CName,
                               c.CGender,
                               cAge = c.CAge.ToString("yyyy/MM/dd"),
                               c.CAddress,
                               c.CPhone,
                               c.CEmail,
                               c.CPassword,
                               c.CBlackList,
                               c.CDeposit,
                           };
                return Json(table);
            }
            else
            {
              var  table = from c in db.TMembers
                        where c.CName.Contains(str)   //???????
                           select new
                           {
                               c.CMemberId,
                               c.CPicture,
                               c.CName,
                               c.CGender,
                               cAge = c.CAge.ToString("yyyy/MM/dd"),
                               c.CAddress,
                               c.CPhone,
                               c.CEmail,
                               c.CPassword,
                               c.CBlackList,
                               c.CDeposit,
                           };
                return Json(table);
            }
            
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
            newMember.CPicture = @"/MemberPhoto/" + photoName;

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
            string photoName = Guid.NewGuid().ToString() + ".jpg";
            if (p.image != null)
            {
                using (var photo = new FileStream(iv_host.WebRootPath + @"\MemberPhoto\" + photoName, FileMode.Create))
                {
                    p.image.CopyTo(photo);
                }
            }
            p.CPicture = @"/MemberPhoto/" + photoName;

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
                    table.CAge = p.CAge;
                    table.CPicture = p.CPicture;  //←要存改照片的話就解開
                    //table.CRegisteredTime = DateTime.Now; //加註冊時間進資料庫
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
            var table = from o in db.TOrders                         
                        join m in db.TMembers on o.CMemberId equals m.CMemberId
                        where m.CMemberId==id
                        select new {cOrderId=o.COrderId,CArrivedAddress = o.CArrivedAddress, cOrderDate = o.COrderDate, cTransportMinute = o.CTransportMinute,
                            cName = m.CName};
          
            return Json(table);
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
