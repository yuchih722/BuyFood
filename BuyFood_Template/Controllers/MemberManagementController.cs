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
using System.Net.Mail;
using System.Net;

namespace BuyFood_Template.Controllers
{
    public class MemberManagementController : Controller
    {
        private 擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
        public IActionResult List()
        {
            ViewBag.All = db.TMembers.Where(n=>n.CMemberId!=16).Select(n => n).Count();
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
                            where c.CMemberId !=16
                            select new
                            {
                                c.CMemberId,
                                c.CPicture,
                                c.CName,
                                c.CGender,
                                cBirthday = c.CAge.ToString("yyyy/MM/dd"),
                                cAge = DateTime.Now.Year-c.CAge.Year,
                                c.CAddress,
                                c.CPhone,
                                c.CEmail,
                                c.CPassword,
                                c.CBlackList,
                                c.CDeposit,
                                c.CFreezeCount
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
                                cBirthday = c.CAge.ToString("yyyy/MM/dd"),
                                cAge = DateTime.Now.Year - c.CAge.Year,
                                c.CAddress,
                                c.CPhone,
                                c.CEmail,
                                c.CPassword,
                                c.CBlackList,
                                c.CDeposit,
                                c.CFreezeCount
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
                                 cBirthday = c.CAge.ToString("yyyy/MM/dd"),
                                 cAge = DateTime.Now.Year - c.CAge.Year,
                                 c.CAddress,
                                 c.CPhone,
                                 c.CEmail,
                                 c.CPassword,
                                 c.CBlackList,
                                 c.CDeposit,
                                 c.CFreezeCount
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
                                 cBirthday = c.CAge.ToString("yyyy/MM/dd"),
                                 cAge = DateTime.Now.Year - c.CAge.Year,
                                 c.CAddress,
                                 c.CPhone,
                                 c.CEmail,
                                 c.CPassword,
                                 c.CBlackList,
                                 c.CDeposit,
                                 c.CFreezeCount
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
                               cBirthday = c.CAge.ToString("yyyy/MM/dd"),
                               cAge = DateTime.Now.Year - c.CAge.Year,
                               c.CAddress,
                               c.CPhone,
                               c.CEmail,
                               c.CPassword,
                               c.CBlackList,
                               c.CDeposit,
                               c.CFreezeCount
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
                               cBirthday = c.CAge.ToString("yyyy/MM/dd"),
                               cAge = DateTime.Now.Year - c.CAge.Year,
                               c.CAddress,
                               c.CPhone,
                               c.CEmail,
                               c.CPassword,
                               c.CBlackList,
                               c.CDeposit,
                               c.CFreezeCount
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

        public bool changeblacklist(int? id)
        {
            TMember l_黑名單修改 = db.TMembers.FirstOrDefault(n => n.CMemberId == id);
            bool 狀態 = false;
            if (l_黑名單修改 != null)
            {
                if (l_黑名單修改.CBlackList == 1)
                {
                    l_黑名單修改.CBlackList = 0;                   
                }
                else
                {
                    l_黑名單修改.CBlackList = 1;
                    狀態 = true;
                }
            }
            db.SaveChanges();
            return 狀態;
        }
        public bool changefreeze(int? id)
        {
            TMember l_凍結修改 = db.TMembers.FirstOrDefault(n => n.CMemberId == id);
            bool 狀態 = false;
            if (l_凍結修改!=null)
            {
                if (l_凍結修改.CFreezeCount==1)
                {
                    l_凍結修改.CFreezeCount = 0;
                }
                else
                {
                    l_凍結修改.CFreezeCount = 1;
                    狀態 = true;
                }                    
            }
            db.SaveChanges();
            return 狀態;
        }
        



        public void abc()
        {
            sendEmail("s736828@gmail.com","亮","123","test");
        }

        public void sendEmail(string mailtoAddress, string mailtoName, string subject, string body)
        {
            var fromAddress = new MailAddress("sunfengmsit129@gmail.com", "擺腹buyfood");//寄件地址，寄件人
            var toAddress = new MailAddress(mailtoAddress, mailtoName);
            const string fromPassword = "@MSIT129";
            //const string subject = "Subject";
            //const string body = "Body";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        public JsonResult getmemberid(int? id)
        {
            var q = db.TMembers.Where(n => n.CMemberId.Equals(id));
            return Json(q);
        }



        public void Email(int? id)
        {
            MailMessage msg = new MailMessage();
            msg.To.Add("s736828@gmail.com");
            msg.From = new MailAddress("s736828@gmail.com", "小亮", System.Text.Encoding.UTF8);//發件人
            msg.Subject = "測試標題";
            msg.SubjectEncoding = System.Text.Encoding.UTF8; //郵件標題編碼
            msg.Body = "郵件內容測試";
            msg.BodyEncoding = System.Text.Encoding.UTF8;//郵件內容編碼
            msg.Attachments.Add(new Attachment(@"D:\吳亮均.pdf"));//附件
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("s736828@gmail.com", "okdlnrynb1219");
            client.Host = "smtp.gmail.com";
            client.Port = 25;
            client.EnableSsl = true;
            client.Send(msg);
            client.Dispose();
            msg.Dispose();                     
        }

    }
}
