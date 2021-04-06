using BuyFood_Template.Models;
using BuyFood_Template.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.Controllers
{
    public class yuOrderController : Controller
    {
        public IActionResult yuorderlist()
        {
            var table = from c in (new 擺腹BuyFoodContext()).TOrders
                        select c;
            List<yuOrderViewModel> Order = new List<yuOrderViewModel>();
            if (table != null)
            {
                foreach (TOrder x in table)
                {
                    Order.Add(new yuOrderViewModel(x));
                }
            }
            return View(Order);
        }
        public IActionResult yuOrderDetails(int? id)
        {

            var table = from c in (new 擺腹BuyFoodContext()).TOrderDetails where c.COrderDetailId==id select c;
            return View(table);
        }
        public IActionResult yuorderEdit(yuOrderViewModel p)
        {


            return View();
        }
        public IActionResult yuordeDelete(int? id)
        {
            if (id != null)
            {
                TOrder table = (new 擺腹BuyFoodContext()).TOrders.FirstOrDefault(n => n.COrderId == id);
                if (table != null)
                {
                    //table.狀態 = "已結束";
                }
            }
            return RedirectToAction("yuorderlist");

        }
    }
}
