using System;
using System.Collections.Generic;
using System.Linq;
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
                CMemberId = 7,
                COrderStatusId = 1,
                CCuponId = CartOrder.couponSelected,
                CPayTypeId = CartOrder.payType,
                CArrivedAddress = CartOrder.address,
                CTransportMinute = CartOrder.transportTime,
                COrderDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
            };
            BuyFoodDB.TOrders.Add(DbOrder);
            BuyFoodDB.SaveChanges();
            decimal TotalPriceInCart = 0;
            //將訂單內的商品一一存入資料庫中
            foreach (var i in CartOrder.cartOrder)
            {
                TOrderDetail ProductInOrder = new TOrderDetail()
                {
                    CProductId = i.cProductId,
                    COrderId = DbOrder.COrderId,
                    CQuantity = i.QuantityInCart,
                    CPriceAtTheTime = i.cPrice,
                    CScores = null,   //暫時資料會移除
                    CReview = null   //暫時資料會移除
                };
                var InStoreCount = BuyFoodDB.TProducts.FirstOrDefault(x => x.CProductId == i.cProductId);
                InStoreCount.CQuantity -= ProductInOrder.CQuantity;
                BuyFoodDB.TOrderDetails.Add(ProductInOrder);
                TotalPriceInCart += i.QuantityInCart * i.cPrice;
            }
            var MemberDeposit = BuyFoodDB.TMembers.FirstOrDefault(x => x.CMemberId == 7); //從資料庫中取出該物件
            MemberDeposit.CDeposit -= (TotalPriceInCart - CouponPrice);   //直接從欄位更改數值才有效果
            BuyFoodDB.SaveChanges();
            return "已收到訂單，請稍後";
        }

    }
}
