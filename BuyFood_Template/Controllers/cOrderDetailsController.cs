using BuyFood_Template.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.Controllers
{
    public class cOrderDetailsController : Controller
    {

        擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
        public IActionResult List()
        {
            return View();
        }
        public JsonResult cOrderDetailsJson()
        {
            var table = from orderdetail in db.TOrderDetails
                        join order in db.TOrders on orderdetail.COrderId equals order.COrderId
                        join member in db.TMembers on order.CMemberId equals member.CMemberId
                        join prname in db.TProducts on orderdetail.CProductId equals prname.CProductId
                        join orderstauts in db.TOrderStatuses on order.COrderStatusId equals orderstauts.COrderStatusId
                        select new 
                        {
                            orderdetail.COrderId,
                            orderdetail.COrderDetailId,
                            order.COrderDate,
                            member.CName,
                            prname.CProductName,
                            orderdetail.CPriceAtTheTime,
                            orderdetail.CQuantity,
                           orderstauts.COrderStatusId,
                            orderstauts.COrderStatusName
                        };

            return Json(table.ToList());
        }
        public void orderCancel(int? id)
        {

            if (id != null)
            {

                TOrder l_order被修改 = db.TOrders.FirstOrDefault(n => n.COrderId == id);
                TMember l_member被修改 = db.TMembers.FirstOrDefault(n => n.CMemberId == l_order被修改.CMemberId);
                TCupon l_cupon被修改 = db.TCupons.FirstOrDefault(n => n.CCuponId == l_order被修改.CCuponId);
                TCuponCategory p折扣金額 = db.TCuponCategories.FirstOrDefault(n => n.CCuponCategoryId == l_cupon被修改.CCuponCategoryId);


                IEnumerable<TOrderDetail> table = null;
                table = db.TOrderDetails.Where(n => n.COrderId == id);

                List<TOrderDetail>list = new List<TOrderDetail>();
                foreach (var item in table)
                {
                    list.Add(item);
                }
                decimal p總價 = 0;
                decimal p價格 = 0;
                int p數量 = 0;
                int pID = 0;
                for (int i = 0; i < list.Count(); i++)
                {
                    //消費金額
                    p價格 = list[i].CPriceAtTheTime.Value;
                    p數量 = list[i].CQuantity.Value;
                    p總價 += (p價格 * p數量);

                    //產品庫存
                    pID = list[i].CProductId;
                    TProduct l_product被修改 = db.TProducts.FirstOrDefault(n => n.CProductId == pID);
                    l_product被修改.CQuantity += p數量;

                   
                }

                if (l_order被修改.CPayTypeId.Value == 1)
                {
                    //無折扣
                    if (l_order被修改.CCuponId.Value == 1)
                    {
                        l_order被修改.COrderStatusId = 3;
                        l_member被修改.CDeposit += p總價;
                    }
                    //有折扣
                    else
                    {
                        l_order被修改.COrderStatusId = 3;
                        l_member被修改.CDeposit += (p總價 - p折扣金額.CCutPrice.Value);
                        l_cupon被修改.CBeUsed = 0;
                       
                    }
                }
            db.SaveChanges();
            }
          

        }
        public IActionResult ordercomplete(int? id)
        {
            TOrder l_order被修改 = db.TOrders.FirstOrDefault(n => n.COrderId == id);
            if(id != null)
            {
                l_order被修改.COrderStatusId = 2;
                db.SaveChanges();
            }
            return View("list");
        }

    }
}

