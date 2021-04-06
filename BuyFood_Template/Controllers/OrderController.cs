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
                ViewBag.USERNAME = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);

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
            var OrderGroupBy = from o in db.TOrders
                               join od in db.TOrderDetails on o.COrderId equals od.COrderId
                               where o.CMemberId == memberID
                               group od by od.COrderId into g
                               select new {cOrderID=g.Key, cTotalPrice=g.Sum(n => (n.CQuantity * n.CPriceAtTheTime)) } ;


            return Json(OrderGroupBy.ToList());
        }

        public JsonResult OrderStatusDate(string MemberName)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var memberID = db.TMembers.Where(n => n.CName == MemberName).Select(n => n.CMemberId).FirstOrDefault();



            var AllOrders = db.TOrders.Where(n => n.CMemberId == memberID).Select(n => n);

            var OrderStatusDate = from o in db.TOrders
                                  where o.CMemberId == memberID
                                  select new { cOrderStatus = o.COrderStatus.COrderStatusName, cOrderDate = o.COrderDate };


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
            var OrderGroupBy = from o in db.TOrders
                               join od in db.TOrderDetails on o.COrderId equals od.COrderId
                               where o.CMemberId == memberID && o.COrderStatus.COrderStatusId == 2
                               group od by od.COrderId into g
                               select new { cOrderID = g.Key, cTotalPrice = g.Sum(n => (n.CQuantity * n.CPriceAtTheTime)) };


            return Json(OrderGroupBy.ToList());
        }

        public JsonResult OrderStatusDateFinished(string MemberName)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var memberID = db.TMembers.Where(n => n.CName == MemberName).Select(n => n.CMemberId).FirstOrDefault();

            var OrderStatusDate = from o in db.TOrders
                                  where o.CMemberId == memberID && o.COrderStatus.COrderStatusId == 2
                                  select new { cOrderStatus = o.COrderStatus.COrderStatusName, cOrderDate = o.COrderDate };


            return Json(OrderStatusDate.ToList());
        }
    }
}
