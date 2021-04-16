using BuyFood_Template.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.Controllers
{
    public class OrderDetailController : Controller
    {
        擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
        public IActionResult ShowOrderDetail(int id)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME)))
            {
                ViewBag.USERNAME = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);
                ViewBag.USERPHOTO = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERPHOTO);
                ViewBag.USERUSERID = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
            }
            return View(id);
        }

        public JsonResult getOrderAdress(int OrderID)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var Orders = db.TOrders.Where(n => n.COrderId == OrderID).Select(n => n);


            return Json(Orders.ToList());
        }
        public JsonResult getMember(int OrderID)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var MemberID = db.TOrders.Where(n => n.COrderId == OrderID).Select(n => n.CMemberId).FirstOrDefault();

            var Member = db.TMembers.Where(n => n.CMemberId == MemberID).Select(n => n);


            return Json(Member.ToList());
        }

        public JsonResult getOrderDetail(int OrderID)
        {
            var Products = db.TOrderDetails.Where(n => n.COrderId == OrderID).Select(n =>
                  new
                  {
                      n.CProduct.CPicture,
                      n.CProduct.CProductName,
                      n.CQuantity,
                      cLittlePrice = (n.CPriceAtTheTime * n.CQuantity),
                      n.COrder.COrderStatus.COrderStatusName,
                      n.CScores,
                      n.CReview,
                      n.COrderDetailId,
                      n.CProductId,
                      n.CFeedBackStatus
                  }
            );

            return Json(Products.ToList());
        }

        public JsonResult getDetailValue(int StarValue, string ReviewInput, int orderdetailID)
        {
            string test = $"{StarValue}分,{ReviewInput},編號{orderdetailID}號";

            var UpdateScore = db.TOrderDetails.Where(n => n.COrderDetailId == orderdetailID).FirstOrDefault();
            UpdateScore.CScores = StarValue;
            UpdateScore.CReview = ReviewInput;
            UpdateScore.CFeedBackStatus = 1;
            db.SaveChanges();

            return Json(UpdateScore);
        }

        public JsonResult getHowToPay(int OrderID)
        {
            //小計
            //折價
            //總計
            //付費型態
            //可以拿前端的來計算也行

            var getHowToPay = db.TOrderDetails.Where(n => n.COrderId == OrderID).Select(n =>
            new
            {
                cCutPrice = n.COrder.CCupon.CCuponCategory.CCutPrice,
                cPayType = n.COrder.CPayType.CPayTypeName
            }
            );


            return Json(getHowToPay.FirstOrDefault());
        }
        public JsonResult getOrderTime(int OrderID)
        {
            var OrderTIme = db.TOrders.Where(n => n.COrderId == OrderID).Select(n => n.COrderDate).FirstOrDefault();

            return Json(OrderTIme);
        }
        public JsonResult getTimeForSalir(string dataTimeOfPreparing, int OrderID)
        {
            DateTime TimeOfOrder = DateTime.ParseExact(dataTimeOfPreparing, "yyyy/MM/dd HH:mm:ss", null);

            var PreparingTime = db.TOrderDetails.Where(n => n.COrderId == OrderID).OrderByDescending(n => n.CProduct.CFinishedTime).Select(n => n.CProduct.CFinishedTime).FirstOrDefault().Value;
            var getTimeOfSalir = TimeOfOrder.AddMinutes(PreparingTime);
            var StrTimeOfSalir = getTimeOfSalir.ToString("yyyy/MM/dd HH:mm:ss");
            return Json(StrTimeOfSalir);
        }
        public JsonResult getTimeOfArrival(string dataTimeOfSalir, int OrderID)
        {
            DateTime TimeOfSalir = DateTime.ParseExact(dataTimeOfSalir, "yyyy/MM/dd HH:mm:ss", null);

            var TransPortTIme = db.TOrders.Where(n => n.COrderId == OrderID).Select(n => n.CTransportMinute).FirstOrDefault().GetValueOrDefault();
            var getTimeOfArrival = TimeOfSalir.AddMinutes(TransPortTIme);
            //===比較時間
 
            DateTime dtNow = DateTime.Now;
            if (dtNow >= getTimeOfArrival)
            {
                var OrderStatus = db.TOrders.FirstOrDefault(n => n.COrderId == OrderID);
                if (OrderStatus.COrderStatusId == 1)
                {
                    OrderStatus.COrderStatusId = 4;
                    db.SaveChanges();
                }
            }

            //===比較時間
            var StrTimeOfArrival = getTimeOfArrival.ToString("yyyy/MM/dd HH:mm:ss");

            return Json(StrTimeOfArrival);


        }
        DateTime nt = DateTime.Now;
        public JsonResult LetBarMove(int OrderID, string ArriveTime)
        {
            var OrderTime = db.TOrders.Where(n => n.COrderId == OrderID).Select(n => n.COrderDate).FirstOrDefault();
            DateTime oTime = DateTime.ParseExact(OrderTime, "yyyy/MM/dd HH:mm:ss", null);
            DateTime aTime = DateTime.ParseExact(ArriveTime, "yyyy/MM/dd HH:mm:ss", null);
            nt = DateTime.Now;
            var BarPercent = (aTime - oTime).TotalSeconds;

            return Json(BarPercent);
        }
        public JsonResult BiggerThanArrival(string ArriveTime)
        {
            DateTime AT = DateTime.ParseExact(ArriveTime, "yyyy/MM/dd HH:mm:ss", null);
            //DateTime nt = DateTime.Now;
            if (nt >= AT)
            {
                return Json("bigger");
            }
            else
            {
                return Json("sm");
            }

        }
        public JsonResult CalTheSecs(string dataTimeOfPreparing)
        {
            DateTime tp = DateTime.ParseExact(dataTimeOfPreparing, "yyyy/MM/dd HH:mm:ss", null);
            DateTime dtc = DateTime.Now;
            var calResult = (dtc - tp).TotalSeconds;
            return Json(calResult);
        }
    }
}
