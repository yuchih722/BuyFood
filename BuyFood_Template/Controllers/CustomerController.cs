using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BuyFood_Template.Models;
using BuyFood_Template.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using prjBuyFoodCore.Models;
//using prjBuyFoodCore.ViewModel;

namespace BuyFood_Template.Controllers
{

    public class CustomerController : Controller
    {
        ShareFunction shareFun = new ShareFunction();


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
            string referrerSignUpID = HttpContext.Request.Query["id"];

            if(referrerSignUpID != null)
            {
                ViewBag.ReferrerCode = referrerSignUpID;
                return View();
            }
            else
            {
                ViewBag.ReferrerCode = "0";
                return View();
            }
        }
        [HttpPost]
        public JsonResult CreateNewMember([FromBody] CCustomerCreateViewModel newMember)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
          
                var check信箱 = from n in db.TMembers
                              select n.CEmail;

                var check手機 = from n in db.TMembers
                              select n.CPhone;

                if (check信箱.Any(n => n == newMember.CEmail) == true)
                {
                    return Json("EmailRepeat");
                }
                else if (check手機.Any(n => n == newMember.CPhone) == true)
                {
                    return Json("PhoneRepeat");
                }
                else
                {
                    string 邀請碼 = shareFun.產生亂數(6);


                    newMember.CBlackList = 0;
                    newMember.CFreezeCount = 0;
                    newMember.CDeposit = 0;
                    newMember.CRegisteredTime = DateTime.Now;
                    newMember.COpenMember = 0;


                    var check邀請碼 = from n in db.TMembers
                                   select n.CReferrerCode;

                    //抓推薦人
                    if (!string.IsNullOrEmpty(newMember.code))
                    {
                        var 抓推薦人 = from n in db.TMembers
                                   where n.CReferrerCode == newMember.code
                                   select n.CMemberId;

                        int[] 推薦人ID = 抓推薦人.ToArray();
                        newMember.CReferrerID = 推薦人ID[0];
                    }
                    else
                    {
                        newMember.CReferrerID = null;
                    }

                    //防止邀請碼重複
                    while (check邀請碼.Any(n => n == 邀請碼) == true)
                    {
                        邀請碼 = shareFun.產生亂數(6);
                    }

                    newMember.CReferrerCode = 邀請碼;

                    db.TMembers.Add(newMember.member);
                    db.SaveChanges();

                    //註冊折價卷
                    var 新增註冊折價卷 = from n in db.TMembers
                                  where n.CMemberId == newMember.CMemberID
                                  select n.CReferrerId;

                    int?[] 邀請人ID = 新增註冊折價卷.ToArray();


                    if (邀請人ID[0] != null)
                    {

                        TCupon 新增邀請人折價卷 = new TCupon
                        {
                            CCuponCategoryId = 2,
                            CMenberId = (int)邀請人ID[0],
                            CBeUsed = 0,
                            CReceivedTime = DateTime.Now,
                            CValidDate = DateTime.Now.AddDays(60)
                        };
                        TCupon 新增新會員折價卷 = new TCupon
                        {
                            CCuponCategoryId = 3,
                            CMenberId = newMember.CMemberID,
                            CBeUsed = 0,
                            CReceivedTime = DateTime.Now,
                            CValidDate = DateTime.Now.AddDays(60)
                        };
                        db.TCupons.Add(新增邀請人折價卷);
                        db.TCupons.Add(新增新會員折價卷);
                        db.SaveChanges();
                    }

                    //密碼雜湊
                    TMember add密碼雜湊 = (from n in db.TMembers
                                       where n.CMemberId == newMember.CMemberID
                                       select n).FirstOrDefault();

                    SHA1 sha1 = SHA1.Create();

                    string 雜湊密碼 = shareFun.GetHash(sha1, add密碼雜湊.CPassword);

                    add密碼雜湊.CPassword = 雜湊密碼;
                    db.SaveChanges();

                    string val信件內容 = "歡迎加入BuyFood,請點擊以下連結以開通帳號 \n"+CDictionary.LOCAL_WEBSITES+"/Customer/memberConfirm?ID=" + add密碼雜湊.CMemberId;

                    shareFun.sendEmail(add密碼雜湊.CEmail, add密碼雜湊.CName, "BuyFood帳號開通認證信", val信件內容);

                    return Json(true);
                }

        }

        public JsonResult UploadImage(IFormFile photo, string CPhone)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();

            TMember mem新增圖片 = (from n in db.TMembers
                               where n.CPhone == CPhone
                               select n).FirstOrDefault();

            if (photo != null)
            {
                string photoname = Guid.NewGuid().ToString() + ".jpg";

                using (var MemberPhoto = new FileStream(iv_host.WebRootPath + @"\MemberPhoto\" + photoname, FileMode.Create))
                {
                    photo.CopyTo(MemberPhoto);
                }

                mem新增圖片.CPicture = @"/MemberPhoto/" + photoname;
                db.SaveChanges();
                return Json(new { result = true });
            }
            else
            {
                mem新增圖片.CPicture = @"/MemberPhoto/無人頭.jpg";
                db.SaveChanges();
            }

            return Json(new { result = false });
        }

        public IActionResult memberConfirm()
        {
            string memberOpenID = HttpContext.Request.Query["ID"];

            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();

            TMember openMember = (from n in db.TMembers
                                  where n.CMemberId == int.Parse(memberOpenID)
                                  select n).FirstOrDefault();

            openMember.COpenMember = 1;
            db.SaveChanges();

            return Redirect("~/HomePage/Home");
        }

        //public IActionResult getFBIdandName(string id,string name)
        //{
        //    ViewBag.FBUSERID = id;
        //    ViewBag.FBUSERNAME = name;

        //    return Redirect("Customer/CreateFacebookMember");
        //}
        public IActionResult CreateFacebookMember()
        {
            string FaceBookMemberID = HttpContext.Request.Query["id"];
            string FaceBookMemberName = HttpContext.Request.Query["name"];

            ViewBag.FBUSERID = FaceBookMemberID;
            ViewBag.FBUSERNAME = FaceBookMemberName;

            return View();
        }
        [HttpPost]
        public JsonResult CreateNewFacebookMember([FromBody] CCustomerCreateViewModel newMember)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();

            var check信箱 = from n in db.TMembers
                          select n.CEmail;

            var check手機 = from n in db.TMembers
                          select n.CPhone;
            if (check信箱.Any(n => n == newMember.CEmail) == true)
            {
                return Json("EmailRepeat");
            }
            else if (check手機.Any(n => n == newMember.CPhone) == true)
            {
                return Json("PhoneRepeat");
            }
            else
            {
                string 邀請碼 = shareFun.產生亂數(6);


                newMember.CBlackList = 0;
                newMember.CFreezeCount = 0;
                newMember.CDeposit = 0;
                newMember.CRegisteredTime = DateTime.Now;
                newMember.COpenMember = 1;


                var check邀請碼 = from n in db.TMembers
                               select n.CReferrerCode;

                //抓推薦人
                if (!string.IsNullOrEmpty(newMember.code))
                {
                    var 抓推薦人 = from n in db.TMembers
                               where n.CReferrerCode == newMember.code
                               select n.CMemberId;

                    int[] 推薦人ID = 抓推薦人.ToArray();
                    newMember.CReferrerID = 推薦人ID[0];
                }
                else
                {
                    newMember.CReferrerID = null;
                }

                //防止邀請碼重複
                while (check邀請碼.Any(n => n == 邀請碼) == true)
                {
                    邀請碼 = shareFun.產生亂數(6);
                }

                newMember.CReferrerCode = 邀請碼;

                db.TMembers.Add(newMember.member);
                db.SaveChanges();

                //註冊折價卷
                var 新增註冊折價卷 = from n in db.TMembers
                              where n.CMemberId == newMember.CMemberID
                              select n.CReferrerId;

                int?[] 邀請人ID = 新增註冊折價卷.ToArray();


                if (邀請人ID[0] != null)
                {

                    TCupon 新增邀請人折價卷 = new TCupon
                    {
                        CCuponCategoryId = 2,
                        CMenberId = (int)邀請人ID[0],
                        CBeUsed = 0,
                        CReceivedTime = DateTime.Now,
                        CValidDate = DateTime.Now.AddDays(60)
                    };
                    TCupon 新增新會員折價卷 = new TCupon
                    {
                        CCuponCategoryId = 3,
                        CMenberId = newMember.CMemberID,
                        CBeUsed = 0,
                        CReceivedTime = DateTime.Now,
                        CValidDate = DateTime.Now.AddDays(60)
                    };
                    db.TCupons.Add(新增邀請人折價卷);
                    db.TCupons.Add(新增新會員折價卷);
                    db.SaveChanges();
                }

                return Json(true);
            }

        }


        //[HttpPost]
        //public IActionResult CreateFacebookMember(CCustomerCreateViewModel newFacebookMember)
        //{
        //    string photoname = Guid.NewGuid().ToString() + ".jpg";

        //    if (newFacebookMember.img != null)
        //    {
        //        using (var MemberPhoto = new FileStream(iv_host.WebRootPath + @"\MemberPhoto\" + photoname, FileMode.Create))
        //        {
        //            newFacebookMember.img.CopyTo(MemberPhoto);
        //        }
        //    }


        //    string 邀請碼 = shareFun.產生亂數(6);


        //    newFacebookMember.CPicture = @"/MemberPhoto/" + photoname;
        //    newFacebookMember.CBlackList = 0;
        //    newFacebookMember.CFreezeCount = 0;
        //    newFacebookMember.CDeposit = 0;

        //    擺腹BuyFoodContext db = new 擺腹BuyFoodContext();

        //    var check邀請碼 = from n in db.TMembers
        //                   select n.CReferrerCode;

        //    //抓推薦人
        //    if (!string.IsNullOrEmpty(newFacebookMember.code))
        //    {
        //        var 抓邀請人 = from n in db.TMembers
        //                   where n.CReferrerCode == newFacebookMember.code
        //                   select n.CMemberId;

        //        int[] 邀請人ID = 抓邀請人.ToArray();
        //        newFacebookMember.CReferrerID = 邀請人ID[0];
        //    }
        //    else
        //    {
        //        newFacebookMember.CReferrerID = null;
        //    }

        //    //防止邀請碼重複
        //    while (check邀請碼.Any(n => n == 邀請碼) == true)
        //    {
        //        邀請碼 = shareFun.產生亂數(6);
        //    }

        //    newFacebookMember.CReferrerCode = 邀請碼;

        //    db.TMembers.Add(newFacebookMember.member);
        //    db.SaveChanges();

        //    //註冊折價卷
        //    var 新增註冊折價卷 = from n in db.TMembers
        //                  where n.CMemberId == newFacebookMember.CMemberID
        //                  select n.CReferrerId;

        //    if (新增註冊折價卷 != null)
        //    {
        //        int?[] 邀請人ID = 新增註冊折價卷.ToArray();

        //        TCupon 新增邀請人折價卷 = new TCupon
        //        {
        //            CCuponCategoryId = 2,
        //            CMenberId = (int)邀請人ID[0],
        //            CBeUsed = 0,
        //            CReceivedTime = DateTime.Now
        //        };
        //        TCupon 新增新會員折價卷 = new TCupon
        //        {
        //            CCuponCategoryId = 3,
        //            CMenberId = newFacebookMember.CMemberID,
        //            CBeUsed = 0,
        //            CReceivedTime = DateTime.Now
        //        };
        //        db.TCupons.Add(新增邀請人折價卷);
        //        db.TCupons.Add(新增新會員折價卷);
        //        db.SaveChanges();
        //    }

        //    var FB註冊後登入 = (from n in db.TMembers.AsEnumerable()
        //                   where n.CMemberId == newFacebookMember.CMemberID
        //                   select n).FirstOrDefault();

        //    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERNAME, FB註冊後登入.CName);
        //    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERPHOTO, FB註冊後登入.CPicture);

        //    return Redirect("~/HomePage/Home");
        //}

        //[HttpPost]
        //public IActionResult Create(CCustomerCreateViewModel newMember)
        //{

        //    string photoname = Guid.NewGuid().ToString() + ".jpg";

        //    if (newMember.img != null)
        //    {
        //        using (var MemberPhoto = new FileStream(iv_host.WebRootPath + @"\MemberPhoto\" + photoname, FileMode.Create))
        //        {
        //            newMember.img.CopyTo(MemberPhoto);
        //        }
        //    }


        //    string 邀請碼 = shareFun.產生亂數(6);


        //    newMember.CPicture = @"/MemberPhoto/" + photoname;
        //    newMember.CBlackList = 0;
        //    newMember.CFreezeCount = 0;
        //    newMember.CDeposit = 0;
        //    newMember.CRegisteredTime = DateTime.Now;

        //    擺腹BuyFoodContext db = new 擺腹BuyFoodContext();

        //    var check邀請碼 = from n in db.TMembers
        //                   select n.CReferrerCode;

        //    //抓推薦人
        //    if (!string.IsNullOrEmpty(newMember.code))
        //    {
        //        var 抓推薦人 = from n in db.TMembers
        //                   where n.CReferrerCode == newMember.code
        //                   select n.CMemberId;

        //        int[] 推薦人ID = 抓推薦人.ToArray();
        //        newMember.CReferrerID = 推薦人ID[0];
        //    }
        //    else
        //    {
        //        newMember.CReferrerID = null;
        //    }

        //    //防止邀請碼重複
        //    while (check邀請碼.Any(n => n == 邀請碼) == true)
        //    {
        //        邀請碼 = shareFun.產生亂數(6);
        //    }

        //    newMember.CReferrerCode = 邀請碼;

        //    db.TMembers.Add(newMember.member);
        //    db.SaveChanges();

        //    //註冊折價卷
        //    var 新增註冊折價卷 = from n in db.TMembers
        //                  where n.CMemberId == newMember.CMemberID
        //                  select n.CReferrerId;

        //    int?[] 邀請人ID = 新增註冊折價卷.ToArray();


        //    if (邀請人ID[0] != null)
        //    {

        //        TCupon 新增邀請人折價卷 = new TCupon
        //        {
        //            CCuponCategoryId = 2,
        //            CMenberId = (int)邀請人ID[0],
        //            CBeUsed = 0,
        //            CReceivedTime = DateTime.Now,
        //            CValidDate = DateTime.Now.AddDays(60)
        //        };
        //        TCupon 新增新會員折價卷 = new TCupon
        //        {
        //            CCuponCategoryId = 3,
        //            CMenberId = newMember.CMemberID,
        //            CBeUsed = 0,
        //            CReceivedTime = DateTime.Now,
        //            CValidDate = DateTime.Now.AddDays(60)
        //        };
        //        db.TCupons.Add(新增邀請人折價卷);
        //        db.TCupons.Add(新增新會員折價卷);
        //        db.SaveChanges();
        //    }

        //    return Redirect("~/HomePage/登入");
        //}

        //public string 產生邀請碼()
        //{
        //    Random r = new Random();
        //    string 邀請碼 = r.Next(0, 10).ToString() +
        //        r.Next(0, 10).ToString() +
        //        r.Next(0, 10).ToString() +
        //        r.Next(0, 10).ToString() +
        //        r.Next(0, 10).ToString() +
        //        r.Next(0, 10).ToString();

        //    return 邀請碼;
        //}
    }
}

