using BuyFood_Template.Models;
using BuyFood_Template.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.Controllers
{
    public class yuActivityController : Controller
    {
        [Obsolete]
        private IHostingEnvironment iv_host;

        [Obsolete]
        public yuActivityController(IHostingEnvironment p)
        {
            iv_host = p;
        }

        public IActionResult ActivityList()
        {
            var table = from c in (new 擺腹BuyFoodContext()).TActivities
                        select c;

            List<yuActivityViewModel> activitie = new List<yuActivityViewModel>();
            if (table != null)
            {
                foreach (TActivity x in table)
                {
                    activitie.Add(new yuActivityViewModel(x));
                }
            }
            return View(activitie);
        } 
        public JsonResult ActivityListJson()
        {
            var table = from c in (new 擺腹BuyFoodContext()).TActivities
                        orderby c.CRank ,c.CTime
                        select c;

            List<yuActivityViewModel> activitie = new List<yuActivityViewModel>();
            if (table != null)
            {
                foreach (TActivity x in table)
                {
                    activitie.Add(new yuActivityViewModel(x));
                }
            }
            return Json(activitie);
        }
        public IActionResult ActivityCreat()
        {
            return View(); 
        }
        [HttpPost]
        public IActionResult ActivityCreat(yuActivityViewModel p)
        {
            string pohotoname = Guid.NewGuid().ToString() + ".jpg";

            using (var photo = new FileStream(iv_host.WebRootPath + @"\imgs\" + pohotoname, FileMode.Create))
            {
                p.img.CopyTo(photo);
            }
            
            p.CPicture = @"~/imgs/" + pohotoname;
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            db.TActivities.Add(p.Activity);
            db.SaveChanges();
            return RedirectToAction("ActivityList");
        }
        public IActionResult ActionEdit(int? id)
        {
            if (id != null)
            {
                擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
                TActivity table = db.TActivities.FirstOrDefault(n => n.CActivityId == id);
                if (table != null)
                {
                    return View(new yuActivityViewModel(table));
                }
            }
            return RedirectToAction("ActivityList");
        }
        [HttpPost]
        public IActionResult ActionEdit(yuActivityViewModel p)
        {
            if (p != null)
            {
                if (p.img != null)
                {
                    string pohotoname = Guid.NewGuid().ToString() + ".jpg";

                    using (var photo = new FileStream(iv_host.WebRootPath + @"\imgs\" + pohotoname, FileMode.Create))
                    {
                        p.img.CopyTo(photo);
                    }
                    p.CPicture = @"~/imgs/" + pohotoname;
                }
                擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
                TActivity table = db.TActivities.FirstOrDefault(a => a.CActivityId == p.CActivityId);
                if (table != null)
                {
                    table.CActivityName = p.CActivityName;
                    table.CDescription = p.CDescription;
                    table.CLink = p.CLink;
                    table.CPicture = p.CPicture;
                    table.CRank = p.CRank;
                    table.CStatus = p.CStatus;
                    table.CTime = p.CTime;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("ActivityList");
        }
        public IActionResult ActionDelete(int? id)
        {
            if (id != null)
            {
                TActivity table = (new 擺腹BuyFoodContext()).TActivities.FirstOrDefault(n => n.CActivityId == id);
                if (table != null)
                {
                    //table.狀態 = "已結束";
                }
            }
            return RedirectToAction("ActivityList");
        }
        public IActionResult ActionUpDelete(int? id)
        {
            if (id != null)
            {
                TActivity table = (new 擺腹BuyFoodContext()).TActivities.FirstOrDefault(n => n.CActivityId == id);
                if (table != null)
                {
                    //table.狀態 = "進行中";
                }
            }
            return RedirectToAction("ActivityList");

        }
    }
}
