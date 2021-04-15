using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyFood_Template.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuyFood_Template.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult SearchOrder()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME)))
            {
                ViewBag.USERNAME = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);
                ViewBag.USERPHOTO = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERPHOTO);
                ViewBag.USERUSERID = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
            }

            //{
            //    return RedirectToAction("登入", "HomePage");
            //}




            return View();
        }

        public JsonResult ListOrder(string MemberName)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();

            var memberID = db.TMembers.Where(n => n.CName == MemberName).Select(n => n.CMemberId).FirstOrDefault();


            var AllOrders = db.TOrders.Where(n => n.CMemberId == memberID).Select(n => n);
            //Todo==OrderTime


            //==OrderTIme

            //總金額
            var OrderGroupBy = (from o in db.TOrders
                                join od in db.TOrderDetails on o.COrderId equals od.COrderId
                                where o.CMemberId == memberID
                                group od by od.COrderId into g
                                select new { cOrderID = g.Key, cTotalPrice = g.Sum(n => (n.CQuantity * n.CPriceAtTheTime)) }).OrderByDescending(n => n.cOrderID).Take(10);


            return Json(OrderGroupBy.ToList());
        }

        public JsonResult OrderStatusDate(string MemberName)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var memberID = db.TMembers.Where(n => n.CName == MemberName).Select(n => n.CMemberId).FirstOrDefault();



            var OrderStatusDate = (from o in db.TOrders
                                   where o.CMemberId == memberID
                                   select new { cOrderStatus = o.COrderStatus.COrderStatusName, cOrderDate = o.COrderDate, cOID = o.COrderId }).OrderByDescending(n => n.cOID).Take(10);


            return Json(OrderStatusDate.ToList());
        }

        public JsonResult ListOrderOnGoing(string MemberName)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var memberID = db.TMembers.Where(n => n.CName == MemberName).Select(n => n.CMemberId).FirstOrDefault();


            //總金額
            var OrderGroupBy = from o in db.TOrders
                               join od in db.TOrderDetails on o.COrderId equals od.COrderId
                               where o.CMemberId == memberID && o.COrderStatus.COrderStatusId==1
                               group od by od.COrderId into g
                               select new { cOrderID = g.Key, cTotalPrice = g.Sum(n => (n.CQuantity * n.CPriceAtTheTime)) };


            return Json(OrderGroupBy.ToList());
        }

        public JsonResult OrderStatusDateOnGoing(string MemberName)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var memberID = db.TMembers.Where(n => n.CName == MemberName).Select(n => n.CMemberId).FirstOrDefault();


            var OrderStatusDate = from o in db.TOrders
                                  where o.CMemberId == memberID && o.COrderStatus.COrderStatusId == 1
                                  select new { cOrderStatus = o.COrderStatus.COrderStatusName, cOrderDate = o.COrderDate };


            return Json(OrderStatusDate.ToList());
        }

        public JsonResult ListOrderFinished(string MemberName)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var memberID = db.TMembers.Where(n => n.CName == MemberName).Select(n => n.CMemberId).FirstOrDefault();

            //總金額
            var OrderGroupBy = (from o in db.TOrders
                                join od in db.TOrderDetails on o.COrderId equals od.COrderId
                                where o.CMemberId == memberID && o.COrderStatus.COrderStatusId == 2
                                group od by od.COrderId into g
                                select new { cOrderID = g.Key, cTotalPrice = g.Sum(n => (n.CQuantity * n.CPriceAtTheTime)) }).OrderByDescending(n => n.cOrderID).Take(10);


            return Json(OrderGroupBy.ToList());
        }

        public JsonResult OrderStatusDateFinished(string MemberName)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var memberID = db.TMembers.Where(n => n.CName == MemberName).Select(n => n.CMemberId).FirstOrDefault();

            var OrderStatusDate = (from o in db.TOrders
                                   where o.CMemberId == memberID && o.COrderStatus.COrderStatusId == 2
                                   select new { cOrderStatus = o.COrderStatus.COrderStatusName, cOrderDate = o.COrderDate, cOID = o.COrderId }).OrderByDescending(n => n.cOID).Take(10);


            return Json(OrderStatusDate.ToList());
        }
        public JsonResult getOrderTime()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var OrderIDs = db.TOrders.Select(n => n.COrderId).ToList();
            foreach(var ID in OrderIDs)
            {
                var getOrderTime = db.TOrders.Where(n => n.COrderId == ID).Select(n => n.COrderDate).FirstOrDefault();
                DateTime OrderTime = DateTime.ParseExact(getOrderTime, "yyyy/MM/dd HH:mm:ss",null);
                var getPrepTime = db.TOrderDetails.Where(n => n.COrderId == ID).OrderByDescending(n => n.CProduct.CFinishedTime).Select(n => n.CProduct.CFinishedTime).FirstOrDefault().GetValueOrDefault();
                var TransportTIme = db.TOrders.Where(n => n.COrderId == ID).Select(n => n.CTransportMinute).FirstOrDefault().GetValueOrDefault();
                DateTime TimeOfArrival = OrderTime.AddMinutes((getPrepTime + TransportTIme));

                if (DateTime.Now >= TimeOfArrival)
                {
                    var StatusID=db.TOrders.FirstOrDefault(n => n.COrderId == ID);
                    if (StatusID.COrderStatusId == 1)
                    {
                        StatusID.COrderStatusId = 4;
                        db.SaveChanges();
                    }
                }
                
            }
            return Json(null);

        }

        public void saveTheNewOrders(string userName,string OrderMessage)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            DateTime dn = DateTime.Now;
            TMessageOrder messageOrder = new TMessageOrder
            {
                CMessageOrder = OrderMessage,
                CUserName = userName,
                CIsRead = 0,
                CSaveTime=dn
            };
            db.Add(messageOrder);
            db.SaveChanges();

        }
        public JsonResult newOrdersSignalR()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            DateTime dn = DateTime.Now;

            var orderMessage = db.TMessageOrders.Where(n=>n.CIsRead==0).Select(n => new
            {
                n.CUserName,
                n.CMessageOrder,
                time = dn - n.CSaveTime
            }).ToList();

            return Json(orderMessage);
        }
        public void clearAllnewOrderMessage()
        {
            擺腹BuyFoodContext db = (new 擺腹BuyFoodContext());

            var mesR=db.TMessageOrders.Select(n => n).OrderByDescending(n=>n.CSaveTime).Take(20).ToList();
            foreach(var n in mesR)
            {
                n.CIsRead = 1;
                db.SaveChanges();
            }


        }
    }
}
