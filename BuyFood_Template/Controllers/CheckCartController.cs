using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using BuyFood_Template.Models;
using BuyFood_Template.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuyFood_Template.Controllers
{
    public class CheckCartController : Controller
    {
        // GET: CheckOrderController
        public ActionResult OrderData()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME)))
            {
                ViewBag.USERNAME = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);
                ViewBag.USERPHOTO = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERPHOTO);
                ViewBag.USERUSERID = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
            }
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME)))
            {
                return RedirectToAction("登入", "HomePage");
            }
            return View();
        }
        [HttpPost]
        public JsonResult ProductFinishTime([FromBody] List<int> IdList)
        {
            List<int> pdtTimeList = new List<int>();
            foreach(var i in IdList)
            {
                var result = new 擺腹BuyFoodContext().TProducts.Where(x => x.CProductId == i)
                    .Select(x => x.CFinishedTime).FirstOrDefault();
                pdtTimeList.Add((int)result);
            }
            return Json(pdtTimeList);
        }
        [HttpPost]
        public void GetOpayOrder(OPay OpayData)
        {
            if(OpayData.RtnCode == 1)
            {
                (new ShareFunction()).sendGrid("fg2216875@gmail.com", "hihi", "訂單成功", "查看詳細");
            }
        }
        [HttpPost]
        public string InsertOrderToDB([FromBody] CartOrderJson CartOrder)
        {
            if (CartOrder == null)
                return "購物車內尚無任何商品";
            擺腹BuyFoodContext BuyFoodDB = new 擺腹BuyFoodContext();
            //檢查訂單內的商品數量是否小於現有庫存
            foreach (var i in CartOrder.cartOrder)
            {
                var quantity = BuyFoodDB.TProducts.Where(x => x.CProductId == i.cProductId).FirstOrDefault();
                if (i.QuantityInCart > quantity.CQuantity)
                {
                    return "已超過現有可製作的份量";
                }
            }
            int GetMemberID = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            //檢查會員的餘額是否足夠付款
            decimal TotalPriceInCart = 0;
            foreach (var i in CartOrder.cartOrder)
            {
                TotalPriceInCart += i.QuantityInCart * i.cPrice;
            }
            var MemberDeposit = BuyFoodDB.TMembers.FirstOrDefault(x => x.CMemberId == GetMemberID); //從資料庫中取出該物件 
            if (MemberDeposit.CDeposit - TotalPriceInCart < 0)
            {
                return "您的錢包餘額不足";
            }
            //檢查有無使用優惠券
            decimal CouponPrice = 0;
            var CouponID = BuyFoodDB.TCupons.FirstOrDefault(x => x.CCuponId == CartOrder.couponSelected);
            if (CouponID.CCuponCategoryId != 1)
            {
                CouponPrice = (decimal)BuyFoodDB.TCuponCategories.FirstOrDefault(x => x.CCuponCategoryId == CouponID.CCuponCategoryId).CCutPrice;
                CouponID.CBeUsed = 1;
            }
            //先在資料庫中建立一筆新的訂單
            TOrder DbOrder = new TOrder()
            {
                CMemberId = GetMemberID,
                COrderStatusId = 1,
                CCuponId = CartOrder.couponSelected,
                CPayTypeId = CartOrder.payType,
                CArrivedAddress = CartOrder.address,
                CTransportMinute = CartOrder.transportTime,
                COrderDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
            };
            BuyFoodDB.TOrders.Add(DbOrder);
            BuyFoodDB.SaveChanges();

            //將訂單內的商品一一存入資料庫中
            foreach (var i in CartOrder.cartOrder)
            {
                var InStoreProduct = BuyFoodDB.TProducts.FirstOrDefault(x => x.CProductId == i.cProductId);
                TOrderDetail ProductInOrder = new TOrderDetail()
                {
                    CProductId = i.cProductId,
                    COrderId = DbOrder.COrderId,
                    CQuantity = i.QuantityInCart,
                    CPriceAtTheTime = InStoreProduct.CPrice,
                    CScores = null,   //暫時資料會移除
                    CReview = null   //暫時資料會移除
                };              
                InStoreProduct.CQuantity -= ProductInOrder.CQuantity;
                BuyFoodDB.TOrderDetails.Add(ProductInOrder);
            }
            MemberDeposit.CDeposit -= (TotalPriceInCart - CouponPrice);   //直接從欄位更改數值才有效果
            BuyFoodDB.SaveChanges();
            return "已收到訂單，請稍後";
        }

    }
}
