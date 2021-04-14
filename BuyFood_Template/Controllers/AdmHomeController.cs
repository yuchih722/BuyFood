using BuyFood_Template.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.Controllers
{
    public class AdmHomeController : Controller
    {
        private readonly ILogger<AdmHomeController> _logger;

        public AdmHomeController(ILogger<AdmHomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult AdmHome()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public JsonResult ger_chat_new()
        {
            擺腹BuyFoodContext db = (new 擺腹BuyFoodContext());
            var table = db.TChatRooms.Where(n=>n.CMemberId!=16&&n.CReview==0).GroupBy(n => n.CMemberId).Select(n => n.Key).ToList();

            List<object> list = new List<object>();
            DateTime date =DateTime.Now;
            foreach (var x in table)
            {
                var data_for_Bells = db.TChatRooms.Where(n => n.CMemberId == x && n.CReview == 0).OrderByDescending(n => n.CChatRoomId).Select(n => new {name=n.CMember.CName, img=n.CPhoto,time=date-n.CSaveTime,content=n.CContent ,id=n.CMemberId}).FirstOrDefault();
                list.Add(data_for_Bells);
            }

            return Json(list);
        }
        public void do_all_review_0()
        {
            擺腹BuyFoodContext db = (new 擺腹BuyFoodContext());
            var table = db.TChatRooms.Where(n => n.CReview == 0).Select(n => n).ToList();
            foreach (var item in table)
            {
                item.CReview = 1;
                db.SaveChanges();
            }
        }
        public IActionResult click_num_chat(int? id)
        {
            TempData["get_chat_id"] = id;
            return RedirectToAction("admChatView", "admChat");
        }
    }
}
