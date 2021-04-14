using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BuyFood_Template.Models;
using BuyFood_Template.ViewModels;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BuyFood_Template.ViewModels.forReceiveJson;

namespace BuyFood_Template.Controllers
{
    public class MemberController : Controller
    {
        public JsonResult test()
        {
            //(new ShareFunction()).sendGrid("always0537@gmail.com", "hihi", "訂單成功", "check your account");

            //擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            //var bbb = db.TOrderDetails.OrderByDescending(n => n.COrder.COrderDate).Select(n => new
            //{
            //    n.CProductId,
            //    product = n.CProduct
            //}).Take(100)
            //.GroupBy(n => n.CProductId).Select(n => new
            //{
            //    n.Key,
            //    product = new List<TProduct>(),
            //    count = n.Count()
            //}).OrderByDescending(n => n.count).Take(6).ToList();

            //foreach(var item in bbb)
            //{
            //    TProduct product = db.TProducts.FirstOrDefault(n => n.CProductId == item.Key);
            //    item.product.Add(product);
            //}
            //return Json(bbb);
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

                if (TempData[CDictionary.REDIRECT_FROM_WHERE] != null)
                {
                    // 1:儲值, 2:套餐
                    string goWhere = TempData[CDictionary.REDIRECT_FROM_WHERE].ToString();
                    return View(new MemberCenterViewModel(data, goWhere));
                }
                return View(new MemberCenterViewModel(data,"0" ));
            }

            else
            {
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
            //string SHAoPassword = data.oPassword;
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

        private IHostingEnvironment iv_host;
        public MemberController(IHostingEnvironment p_host)
        {
            iv_host = p_host;
        }
    
        public JsonResult UploadOneFile(IFormFile photo)
        {
            string memberID = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
            
            if (!string.IsNullOrEmpty(memberID)&&photo.Length > 0)
            {
                string photoName = Guid.NewGuid().ToString()+".jpg";
                string photoPath = iv_host.WebRootPath + @"\MemberPhoto\" + photoName;
                using (
                    var addphoto = new FileStream(photoPath, FileMode.Create))
                {
                    photo.CopyTo(addphoto);
                }
                擺腹BuyFoodContext dbcontext = new 擺腹BuyFoodContext();
                TMember target = dbcontext.TMembers.FirstOrDefault(n => n.CMemberId ==int.Parse(memberID));
                target.CPicture = @"/MemberPhoto/"+photoName;
                dbcontext.SaveChanges();
                HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERPHOTO, target.CPicture);

                return Json(new { result = "1",msg="上傳成功",src=target.CPicture});
            }
            return Json(new { result = "0", msg = "上傳失敗" });
        }

        public class dataURLs
        {
            public string dataURL { get; set; }
        }
        //[HttpPost]
        //public string decodeBase64ToImage([FromBody] dataURLs receiveData)
        //{

        //    string imgName = Guid.NewGuid().ToString();
        //    string filename = "";
        //    string base64 = receiveData.dataURL.Substring(receiveData.dataURL.IndexOf(",") + 1);      //将‘，’以前的多余字符串删除
        //    Bitmap bitmap = null;//定义一个Bitmap对象，接收转换完成的图片



        //    try//会有异常抛出，try，catch一下
        //    {
        //        string inputStr = base64;//把纯净的Base64资源扔给inpuStr,这一步有点多余

        //        byte[] arr = Convert.FromBase64String(inputStr);//将纯净资源Base64转换成等效的8位无符号整形数组
        //        MemoryStream ms = new MemoryStream(arr);//转换成无法调整大小的MemoryStream对象
        //        Bitmap bmp = new Bitmap(ms);//将MemoryStream对象转换成Bitmap对象
        //        ms.Close();//关闭当前流，并释放所有与之关联的资源
        //        bitmap = bmp;

        //        filename = iv_host.WebRootPath + @"\cwc\" + imgName + ".jpg";

        //        bitmap.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);//保存到服务器路径

        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message.ToString();
        //    }
        //    return filename;//返回相对路径
        //}

    }

}
