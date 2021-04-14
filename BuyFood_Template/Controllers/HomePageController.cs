using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BuyFood_Template.Models;
using BuyFood_Template.ViewModel;
using BuyFood_Template.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuyFood_Template.Controllers
{
    public class HomePageController : Controller
    {
        ShareFunction shareFun = new ShareFunction();

        public IActionResult Home()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME)))
            {
                ViewBag.USERNAME = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);
                ViewBag.USERPHOTO = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERPHOTO);
                ViewBag.USERUSERID = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
            }
            return View();
        }

        [HttpPost]
        public JsonResult getSearchKey(string key,string Category, string Min,string Max,string BreakFast, string Lunch, string Dinner)
        {

            List<TProduct> list = new List<TProduct>();
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            double getMin = Convert.ToDouble(Min);
            double getMax = Convert.ToDouble(Max);
            int getBreakFast = 0;
            int getLunch = 0;
            int getDinner = 0;
            int CategoryID = db.TProductCategories.Where(n => n.CCategoryName == Category).Select(n => n.CProductCategoryId).FirstOrDefault();

            var rowdata = db.TProducts.Select(n => new cTProduct
            {
                TProduct = n,
                count = n.TOrderDetails.Count(x => x.CFeedBackStatus == 1 && x.CScores != null),
                sum = n.TOrderDetails.Where(x => x.CFeedBackStatus == 1 && x.CScores != null).Sum(m => m.CScores)
            });

            var result = rowdata.AsEnumerable().Select(n => n);

            if (BreakFast == "早餐")
                getBreakFast = 1;
            if (Lunch == "午餐")
                getLunch = 1;
            if (Dinner == "晚餐")
                getDinner = 1;
            if (string.IsNullOrEmpty(BreakFast) && string.IsNullOrEmpty(Lunch) && string.IsNullOrEmpty(Dinner))
            {
                getBreakFast = 1;getLunch = 1;getDinner = 1;
            }

            if (string.IsNullOrEmpty(Min))
                getMin = 0;
            
            if (string.IsNullOrEmpty(Max))
                getMax = 10000;
            

            if (string.IsNullOrEmpty(key))
            {
                if (Category == "全部")
                {
                    result=rowdata.AsEnumerable().Where(m=> (double)m.TProduct.CPrice >= getMin && (double)m.TProduct.CPrice <= getMax );
                }
                else
                {
                    result = rowdata.AsEnumerable().Where(m => m.TProduct.CCategoryId == CategoryID &&
                       (double)m.TProduct.CPrice >= getMin && (double)m.TProduct.CPrice <= getMax);

                }
            }

            else
            {

                if (Category=="全部")
                {
                    result=rowdata.AsEnumerable().Where(m=>m.TProduct.CProductName.Any(n=>m.TProduct.CProductName.Contains(key)) &&
                          (double)m.TProduct.CPrice >= getMin && (double)m.TProduct.CPrice <= getMax);
                }

                else
                {

                    result = rowdata.AsEnumerable().Where(m => m.TProduct.CProductName.Any(n => m.TProduct.CProductName.Contains(key)) &&
                      m.TProduct.CCategoryId == CategoryID && (double)m.TProduct.CPrice >= getMin && (double)m.TProduct.CPrice <= getMax);

                }
            }
            //時段判斷
            var product = db.TProducts.AsQueryable();
            var predicate = PredicateBuilder.False<cTProduct>();
            if (getBreakFast == 1)
                predicate = predicate.Or(n => n.TProduct.CIsBreakFast == getBreakFast);
            if (getLunch == 1)
                predicate = predicate.Or(n => n.TProduct.CIsLunch == getLunch);
            if (getDinner == 1)
                predicate = predicate.Or(n => n.TProduct.CIsDinner == getDinner);

            var resultOfTime = result.Where(c => predicate.Invoke(c));

            //var aaa = db.TOrders.Where(n => n.CMemberId == 1).OrderByDescending(n => n.COrderDate).Take(6).GroupBy(n => n.TOrderDetails.);
            //var bbb = db.TOrderDetails.OrderByDescending(n => n.COrder.COrderDate).Select(n=>new { 
            //    n.CProductId,
            //    product = n.CProduct
            //}).Take(100).GroupBy(n => n.CProductId).Select(n => new
            //{
            //    n.Key,
            //    n,
            //    count = n.Count()
            //}).OrderByDescending(n => n.count).Take(6);

            //return Json(result.ToList());
            //return Json(result.Where(n=>n.CIsBreakFast==getBreakFast&&n.CIsLunch==getLunch&&n.CIsDinner==getDinner));
            return Json(resultOfTime.ToList());

        }


        public string getSearchBar()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var nameQuery = db.TProducts.Select(n => n.CProductName);
            List<string> Pnames = new List<string>();

            foreach(var n in nameQuery)
            {
                Pnames.Add(n);
            }
            

            return string.Join(',', Pnames);

        }

        public JsonResult getCategory()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            List<string> list = new List<string>();

            var Categories = db.TProductCategories.Select(n => n.CCategoryName);

            foreach(var CateNames in Categories)
            {
                list.Add(CateNames);
            }

            return Json(list);
        }



            public IActionResult 登入()
        {
            //if (string.IsNullOrEmpty(HttpContext.Session.GetString(CDictionary.LOGIN_AUTHTICATION_CODE)))
            //{
            //    Random r = new Random();
            //    string code = r.Next(0, 10).ToString() +
            //        r.Next(0, 10).ToString() +
            //        r.Next(0, 10).ToString() +
            //        r.Next(0, 10).ToString();
            //    HttpContext.Session.SetString(CDictionary.LOGIN_AUTHTICATION_CODE, code);
            //}

            //ViewData[CDictionary.LOGIN_AUTHTICATION_CODE] = HttpContext.Session.GetString(CDictionary.LOGIN_AUTHTICATION_CODE);
            return PartialView();
        }
        //[HttpPost]
        //public IActionResult 登入(CLoginViewModel loginMember)
        //{
        //    //if (loginMember.txtCode == null)
        //    //{
        //    //    loginMember.txtCode = "";
        //    //}
        //    //if (!loginMember.txtCode.Equals(HttpContext.Session.GetString(CDictionary.LOGIN_AUTHTICATION_CODE)))
        //    //{
        //    //    ViewData[CDictionary.LOGIN_AUTHTICATION_CODE] = HttpContext.Session.GetString(CDictionary.LOGIN_AUTHTICATION_CODE);
        //    //    return View();
        //    //}

        //    TMember CheckMember = (new 擺腹BuyFoodContext()).TMembers.FirstOrDefault(p => p.CEmail.Equals(loginMember.CEmail) && p.CPassword.Equals(loginMember.CPassword));

        //    if (CheckMember != null)
        //    {
        //        HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERNAME, CheckMember.CName);
        //        HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERPHOTO, CheckMember.CPicture);
        //        HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERID, CheckMember.CMemberId.ToString());

        //        return RedirectToAction("Home");
        //    }
        //    //ViewData[CDictionary.LOGIN_AUTHTICATION_CODE] = HttpContext.Session.GetString(CDictionary.LOGIN_AUTHTICATION_CODE);
        //    return PartialView();
        //}
        [HttpPost]
        public JsonResult loginCheck([FromBody] CLoginViewModel loginMember)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var check信箱 = from n in db.TMembers
                          select n.CEmail;

            if(check信箱.Any(n => n == loginMember.CEmail) == true)
            {
                TMember freezeCheck = (from n in db.TMembers
                                       where n.CEmail == loginMember.CEmail
                                       select n).FirstOrDefault();

                var check密碼 = (from n in db.TMembers
                              where n.CEmail == loginMember.CEmail
                              select n).FirstOrDefault();

                SHA1 sha1 = SHA1.Create();

                string pwd解密 = shareFun.GetHash(sha1, loginMember.CPassword);

                if (check密碼.CFreezeCount >=4)
                {
                    return Json("memberFrozed");
                }
                else if(check密碼.CPassword == pwd解密)
                {
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERNAME, check密碼.CName);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERPHOTO, check密碼.CPicture);
                    HttpContext.Session.SetString(CDictionary.CURRENT_LOGINED_USERID, check密碼.CMemberId.ToString());

                    freezeCheck.CFreezeCount = 0;
                    db.SaveChanges();

                    return Json("loginSuccess");
                }
                else
                {
                    if (check密碼.CFreezeCount == 3)
                    {
                    freezeCheck.CFreezeCount += 1;
                    db.SaveChanges();
                    return Json("FrozeComplete");
                    }
                    else
                    {
                        freezeCheck.CFreezeCount += 1;
                        db.SaveChanges();
                        return Json("FrozeCountPlus");
                    }
                }
            }
            else
            {
                return Json("noEmail");
            }
        }
        public bool facebookLogin(string id, string name)
        {
            var test = id + name;

            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            //檢查是否用此帳號登入過

            var checkID = from n in db.TMembers
                          select n.CFacebookId;

            if (checkID.Any(n => n == id) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public JsonResult BCheckLogin()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID)))
            {
                return Json(false);
            }
            else if (int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID)) == 16)
            {
                return Json("isAdm");
            }
            else
            {
                return Json(true);
            }
        }
        public string SLogout()
        {
            HttpContext.Session.Remove(CDictionary.CURRENT_LOGINED_USERNAME);
            HttpContext.Session.Remove(CDictionary.CURRENT_LOGINED_USERPHOTO);
            HttpContext.Session.Remove(CDictionary.CURRENT_LOGINED_USERID);

            return "登出成功";
        }
        public JsonResult 輪播牆()
        {
            var run = (new 擺腹BuyFoodContext()).TActivities.Where(n => n.CStatus == 1 && n.CRank != 99).OrderBy(n => n.CRank).Select(n => n);

            return Json(run);
        }
        public JsonResult getNoodels()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var getNoodels = db.TProducts.Where(n => n.CProductId == 10 || n.CProductId == 17 || n.CProductId == 58).Select(n => n).ToList();
            return Json(getNoodels);
        }
        public JsonResult getArroz()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var getArroz = db.TProducts.Where(n => n.CProductId == 19 || n.CProductId == 20).Select(n => n).ToList();
            return Json(getArroz);
        }
        public JsonResult getDrinks()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var getDrinks = db.TProducts.Where(n => n.CProductId == 5 || n.CProductId == 26 || n.CProductId==24).Select(n => n).ToList();
            return Json(getDrinks);
        }

        public JsonResult getFried()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var getFried = db.TProducts.Where(n => n.CProductId == 14 || n.CProductId == 30).Select(n => n).ToList();
            return Json(getFried);
        }
        public JsonResult getActivity()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var getActivity = db.TActivities.Select(n => n).OrderByDescending(n => n).Where(n => n.CStatus == 1).Take(3).ToList();
            return Json(getActivity);
        }
        public JsonResult get_categorysname() //抓取所有商品顯示在首頁
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var table = db.TProductCategories.Select(n => new {n.CProductCategoryId,n.CCategoryName,
                tProducts = n.TProducts.Select(m => new 
                {
                    tProducts=m,
                    coun=m.TOrderDetails.Count(b=>b.CFeedBackStatus==1&&b.CScores!=null),
                    sum=m.TOrderDetails.Where(m=>m.CFeedBackStatus == 1 && m.CScores != null).Sum(m=>m.CScores)
                })
            });

            return Json(table);
        }

        public JsonResult getBottomList()  
        { 
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            #region 最新商品
            
            var lastProducts = db.TProducts.OrderByDescending(n => n.CProductId).Select(n => n).Take(6);

            #endregion

            #region //好評商品

            var gettopProducts = (from tp in db.TOrderDetails
                              group tp by tp.CProductId into g
                              select new
                              {
                                  g.Key,
                                  AvgScore = g.Sum(n => n.CScores) / g.Count()
                              }).OrderByDescending(n=>n.AvgScore).Select(n=>n.Key).ToList().Take(6);

            List<TProduct> topProducts = new List<TProduct>();
            foreach (var p in gettopProducts)
            {
                topProducts.Add(db.TProducts.Where(n => n.CProductId == p).Select(n => n).FirstOrDefault());
            }

            #endregion

            #region //熱評商品

            var review = (from od in db.TOrderDetails
                         where od.CReview != null
                         group od by od.CProductId into g
                         select new
                         {
                             g.Key,
                             ReviewCounts = g.Count()
                         }).OrderByDescending(n=>n.ReviewCounts).Select(n=>n.Key).ToList().Take(6);

            List<TProduct> ReviewProducts = new List<TProduct>();
            foreach (var p in review)
            {
                ReviewProducts.Add(db.TProducts.Where(n => n.CProductId == p).Select(n => n).FirstOrDefault());
            }

            #endregion


            var table = new { lastProducts = lastProducts, topProducts = topProducts, ReviewProducts = ReviewProducts };
            return Json(table);
        }
        public JsonResult getSideBar_CateNums()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();

            var CategoryGroupBy = db.TProductCategories.Select(n => new
            {
                n.CProductCategoryId,
                n.CCategoryName,
                n.TProducts
            });


            return Json(CategoryGroupBy.ToList());

        }
        public JsonResult DidLogingetBottomList(int MemberID)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            #region 最新商品

            var lastProducts = db.TProducts.OrderByDescending(n => n.CProductId).Select(n => n).Take(6);



            #endregion

            #region //好評商品

            var gettopProducts = (from tp in db.TOrderDetails
                                  group tp by tp.CProductId into g
                                  select new
                                  {
                                      g.Key,
                                      AvgScore = g.Sum(n => n.CScores) / g.Count()
                                  }).OrderByDescending(n => n.AvgScore).Select(n => n.Key).ToList().Take(6);

            List<TProduct> topProducts = new List<TProduct>();
            foreach (var p in gettopProducts)
            {
                topProducts.Add(db.TProducts.Where(n => n.CProductId == p).Select(n => n).FirstOrDefault());
            }

            #endregion

            #region //最常購買

            var ReviewProducts = db.TOrderDetails.OrderByDescending(n => n.COrder.COrderDate).Select(n => new
            {
                n.CProductId,
                product = n.CProduct
            }).Take(100)
            .GroupBy(n => n.CProductId).Select(n => new
            {
                n.Key,
                product = new List<TProduct>(),
                count = n.Count()
            }).OrderByDescending(n => n.count).Take(6).ToList();

            foreach (var item in ReviewProducts)
            {
                TProduct product = db.TProducts.FirstOrDefault(n => n.CProductId == item.Key);
                item.product.Add(product);
            }

            #endregion


            var table = new { lastProducts = lastProducts, topProducts = topProducts, ReviewProducts = ReviewProducts };
            return Json(table);
        }

    }
}
