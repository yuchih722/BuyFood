using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BuyFood_Template.Hubs;
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
        public string usecoupon()
        {
            Random r = new Random();
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var orders = db.TOrders.Select(n => new
            {
                n.COrderId,
                sum = n.TOrderDetails.Sum(m => m.CPriceAtTheTime * m.CQuantity)
            }).Where(n=>n.sum >400).ToList();

            foreach(var order in orders)
            {
                var target = db.TOrders.First(n => n.COrderId == order.COrderId);
                target.CCuponId = r.Next(1, 5);
            }
            db.SaveChanges();
            return ("done");
        }
        
        public JsonResult newOrder()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            int[] memberBox = new int[]
            {
                11,17,21,30,31
            };
            string[] addressBox = new string[]
            {
                "106台北市大安區復興南路一段303號9樓",
                "106台北市大安區信義路三段147巷11弄1號1樓",
                "106台北市大安區瑞安街31巷1號",
                "106台北市大安區通安街115號",
                "106台北市大安區新生南路二段50號",
                "105台北市松山區敦化北路199號",
                "106台北市大安區忠孝東路四段77巷28號",
                "104台北市中山區市民大道三段143號",
                "106台北市大安區安和路二段92號B1F",
                "106台北市大安區復興南路二段102號",
                "106台北市大安區仁愛路三段26-5號",
                "106台北市大安區建國南路二段231號",
                "106台北市大安區通安街115號",
                "106台北市大安區新生南路二段50號",
                "105台北市松山區敦化北路199號",
                "110台北市信義區信義路五段1號",
                "106台北市大安區光復南路180巷11號",
            };
            string[] dateBox = new string[]
            {
                "2021/01/01 10:10:10",
                "2021/02/01 10:10:10",
                "2021/03/01 10:10:10",
                "2021/04/01 10:10:10",
                "2020/01/01 10:10:10",
                "2020/02/01 10:10:10",
                "2020/03/01 10:10:10",
                "2020/04/01 10:10:10",
                "2020/05/01 10:10:10",
                "2020/06/01 10:10:10",
                "2020/07/01 10:10:10",
                "2020/08/01 10:10:10",
                "2020/09/01 10:10:10",
                "2020/10/01 10:10:10",
                "2020/11/01 10:10:10",
                "2020/12/01 10:10:10",
            };

            Random random = new Random();
            var productBox = db.TProducts.ToList();

            for (int i = 0; i < 2000; i++)
            {
                TOrder order = new TOrder()
                {
                    CMemberId = memberBox[random.Next(0, 3)],
                    COrderStatusId = 2,
                    CCuponId = 1,
                    CArrivedAddress = addressBox[random.Next(0, 8)],
                    CPayTypeId = 1,
                    COrderDate = dateBox[random.Next(0, dateBox.Count())],
                    CTransportMinute = random.Next(5, 30)
                };
                db.TOrders.Add(order);
                db.SaveChanges();

                var orderdetailCount = random.Next(1,6);
                for (int p = 0; p < orderdetailCount; p++)
                {
                    var productID = productBox[random.Next(0, productBox.Count())].CProductId;
                    TOrderDetail od = new TOrderDetail()
                    {
                        CProductId = productID,
                        COrderId = order.COrderId,
                        CQuantity = random.Next(1, 4),
                        CPriceAtTheTime = productBox.First(n => n.CProductId == productID).CPrice,
                    };
                    db.TOrderDetails.Add(od);
                }
            }
            db.SaveChanges();

            return Json("done");
        }
        public string checkLogin(string id)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME)))
            {
                if (id != "0")
                    TempData[CDictionary.REDIRECT_FROM_WHERE] = id;
                return "1";
            }

            return "0";
        }
        public IActionResult MemberCenter()
        {
            ViewBag.LOCALWEBSITES = CDictionary.LOCAL_WEBSITES;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME)))
            {
                ViewBag.userName = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);
                ViewBag.userPhoto = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERPHOTO);
                ViewBag.memberID = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
                ViewBag.facebook = string.IsNullOrEmpty(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_FACEBOOK)) ? "0" : "1";
                擺腹BuyFoodContext dbcontext = new 擺腹BuyFoodContext();
                TMember data = dbcontext.TMembers.FirstOrDefault(n => n.CMemberId == int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID)));

                if (TempData[CDictionary.REDIRECT_FROM_WHERE] != null)
                {
                    // 1:儲值, 2:套餐
                    string goWhere = TempData[CDictionary.REDIRECT_FROM_WHERE].ToString();
                    return View(new MemberCenterViewModel(data, goWhere));
                }
                return View(new MemberCenterViewModel(data, "0"));
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
                    .Select(n => new
                    {
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
        public string savePassword([FromBody] changePassword data)
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
            string contenxt = CDictionary.LOCAL_WEBSITES + $"/Customer/Create?id={targetMember.CReferrerCode}";
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

            if (!string.IsNullOrEmpty(memberID) && photo.Length > 0)
            {
                string photoName = Guid.NewGuid().ToString() + ".jpg";
                string photoPath = iv_host.WebRootPath + @"\MemberPhoto\" + photoName;
                using (
                    var addphoto = new FileStream(photoPath, FileMode.Create))
                {
                    photo.CopyTo(addphoto);
                }
                擺腹BuyFoodContext dbcontext = new 擺腹BuyFoodContext();
                TMember target = dbcontext.TMembers.FirstOrDefault(n => n.CMemberId == int.Parse(memberID));
                target.CPicture = @"/MemberPhoto/" + photoName;
                dbcontext.SaveChanges();
                HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERPHOTO, target.CPicture);

                return Json(new { result = "1", msg = "上傳成功", src = target.CPicture });
            }
            return Json(new { result = "0", msg = "上傳失敗" });
        }

        public class dataURLs
        {
            public string dataURL { get; set; }
        }
        public string checkOD()
        {
            int memberID = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var checkdata = db.TOrders.Where(n => n.CMemberId == memberID).ToList();
            if (checkdata.Count() > 0) {
                var haveorder = checkdata.Select(n => new
                {
                    year = int.Parse(n.COrderDate.Substring(0, 4))
                }).OrderByDescending(n => n.year).First().year;
                return haveorder.ToString();
             }
            return "0";
            
        }
        public JsonResult memberRecord(string year)
        {
            int memberID = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();

            var rowdata_orderDetail = db.TOrderDetails.Where(n =>
            n.COrder.CMemberId == memberID &&
            n.COrder.COrderDate.Substring(0, 4) == year).Select(n => new
            {
                n.COrderId,
                n.COrder.COrderDate,
                n.CProduct.CCategoryId,
                n.CProductId,
                n.CProduct.CProductName,
                n.CPriceAtTheTime,
                n.CQuantity
            }).ToList();

            var year_sum = db.TOrders.Where(n => n.CMemberId == memberID)
                .Select(n => new
                {
                    n.COrderId,
                    n.CCupon.CCuponCategory.CCutPrice,
                    Year = n.COrderDate.Substring(0,4)
                }).GroupBy(n=>n.Year).Select(n=> new { 
                n.Key,
                Sum = n.Sum(m=>m.CCutPrice)
                }).OrderByDescending(n=>n.Key).ToList();

            //總消費
            decimal? totalCost = 0;
            var totalSave = year_sum.FirstOrDefault(n => n.Key == year).Sum;
            //訂購前三名
            var productOrderRank = rowdata_orderDetail.GroupBy(n => n.CProductName).Select(n => new
            {
                n.Key,
                Count = n.Count()
            }).OrderByDescending(n => n.Count).Take(3).ToList();

            //每月消費類別分布
            string[] months = new[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            var categorys = db.TProductCategories.ToList();

            ArrayList mth_results = new ArrayList();
            for (int i = 0; i < months.Length; i++)
            {
                Dictionary<string, decimal?> cty_result = new Dictionary<string, decimal?>();
                string str_month = (i + 1) + "月";

                var monthData = rowdata_orderDetail.Where(n => n.COrderDate.Substring(5, 2) == months[i]).ToList();

                var mth_ctys_data = categorys.Select(n => new
                {
                    n.CCategoryName,
                    sum = monthData.Where(m => m.CCategoryId == n.CProductCategoryId).Sum(m => m.CPriceAtTheTime * m.CQuantity)
                }).ToList();

                foreach (var mth_cty in mth_ctys_data)
                {
                    cty_result.Add(mth_cty.CCategoryName, mth_cty.sum);
                    totalCost += mth_cty.sum;
                }

                var mth_result = new
                {
                    month = str_month,
                    ctys_sum = cty_result
                };

                mth_results.Add(mth_result);

            }
            return Json(new
            {
                totalCost = string.Format("{0:0,0}",totalCost),
                totalSave = string.Format("{0:0,0}", totalSave),
                totalPay = string.Format("{0:0,0}", totalCost-totalSave),
                top3 = productOrderRank,
                year_coupon = year_sum,
                mth_data = mth_results
            });

        }
    }
}



