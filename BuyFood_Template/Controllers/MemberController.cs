using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BuyFood_Template.Models;
using BuyFood_Template.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BuyFood_Template.ViewModels.forReceiveJson;

namespace BuyFood_Template.Controllers
{
    public class MemberController : Controller
    {
        public void test()
        {
            (new ShareFunction()).sendEmail("always0537@gmail.com", "hihi", "訂單成功", "check your account");

        }
        public string checkLogin()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME)))
                return "1";
            return "0";
        }
        public IActionResult MemberCenter()
        {
            int test = 555;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME)))
            {
                ViewBag.userName = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);
                ViewBag.userPhoto = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERPHOTO);
                ViewBag.memberID = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
                擺腹BuyFoodContext dbcontext = new 擺腹BuyFoodContext();
                TMember data = dbcontext.TMembers.FirstOrDefault(n => n.CMemberId == int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID)));

                return View(new MemberCenterViewModel(data));
            }
            //ViewBag.memberID = 2;

            //擺腹BuyFoodContext ccc = new 擺腹BuyFoodContext();
            //TMember ddd = ccc.TMembers.FirstOrDefault(n => n.CMemberId == 2);
            //return View(new MemberCenterViewModel(ddd));

            else
            {
                HttpContext.Session.SetString(CDictionary.REDIRECT_FROM_MEMBERCENTER, "1");
                return RedirectToAction("登入", "HomePage");
            }
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
        public string savePassword([FromBody]changePassword data)
        {
            擺腹BuyFoodContext dbcontext = new 擺腹BuyFoodContext();
            TMember reviseTarget = dbcontext.TMembers.FirstOrDefault(n => n.CMemberId == int.Parse(data.memberID));

            ShareFunction sf = new ShareFunction();
            SHA1 sha1 = SHA1.Create();
            string SHAoPassword = sf.GetHash(sha1, data.oPassword);
            string SHAnPassword = sf.GetHash(sha1, data.nPassword);

            if (SHAoPassword != reviseTarget.CPassword)
                return "1";
            reviseTarget.CPassword = SHAnPassword;
            dbcontext.SaveChanges();
            return "0";
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

        public void logout()
        {
            HttpContext.Session.Remove(CDictionary.CURRENT_LOGINED_USERID);
            HttpContext.Session.Remove(CDictionary.CURRENT_LOGINED_USERNAME);
            HttpContext.Session.Remove(CDictionary.CURRENT_LOGINED_USERPHOTO);
        }

        public JsonResult QRcode(string id)
        {
            擺腹BuyFoodContext dbcontext = new 擺腹BuyFoodContext();
            TMember targetMember = dbcontext.TMembers.FirstOrDefault(n => n.CMemberId == int.Parse(id));
            string head = $"<h1>推薦碼 : {targetMember.CReferrerCode}</h1>";
            string contenxt = $"https://msit129cwwebapp.azurewebsites.net/Customer/Create?id={targetMember.CReferrerCode}";
            List<string> data = new List<string>();
            data.Add(head);
            data.Add(contenxt);
            return Json(data);
        }

    }

}
