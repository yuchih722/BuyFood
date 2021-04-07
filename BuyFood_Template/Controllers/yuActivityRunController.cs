using BuyFood_Template.Models;
using BuyFood_Template.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.Controllers
{
    public class yuActivityRunController : Controller
    {
        擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
        public IActionResult yuActivityRun()
        {
            ViewBag.All = db.TActivities.Where(n => n.CStatus != 0).OrderBy(n => n.CRank).Count();
            ViewBag.Going = db.TActivities.Where(n => n.CStatus != 0 && n.CRank != 99).Count();
            ViewBag.close = db.TActivities.Where(n => n.CStatus != 0 && n.CRank == 99).Count();
            return View();
        }

        public JsonResult ActivityListJson(string str)
        {
            List<yuActivityViewModel> activitie = new List<yuActivityViewModel>();

            IQueryable<TActivity> table = null;

            if (str == "All")
            {
                table = db.TActivities.Where(n => n.CStatus != 0).OrderByDescending(n => n.CActivityId).Select(n => n);
            }
            else if (str == "Going")
            {
                table = db.TActivities.Where(n => n.CStatus != 0 && n.CRank != 99).OrderBy(n => n.CRank).Select(n => n);
            }
            else if (str == "Close")
            {
                table = db.TActivities.Where(n => n.CStatus != 0 && n.CRank == 99).OrderByDescending(n => n.CActivityId).Select(n => n);
            }

            if (table != null)
            {
                foreach (TActivity x in table)
                {
                    activitie.Add(new yuActivityViewModel(x));
                }
            }
            return Json(activitie);
        }
        public void ActionUpRank(int? id)
        {
            if (id != null)
            {
                TActivity table = db.TActivities.FirstOrDefault(n => n.CActivityId == id);
                if (table != null)
                {
                    if (table.CRank == 99)
                    {
                        var newnum = (from x in db.TActivities where x.CRank != 99 && x.CStatus != 0 select x).Count();
                        table.CRank = newnum + 1;
                    }
                    else
                        table.CRank = 99;
                }
                db.SaveChanges();
            }
        }
        
        [HttpPost]
        public void  moveRand([FromBody]List<yupushstrViewModel> Array)
        {
            if (Array != null)
            {
                for (int i = 0; i < Array.Count; i++)
                {
                    TActivity table = db.TActivities.Where(n => n.CActivityId.ToString() == Array[i].strmember).FirstOrDefault();

                    table.CRank = i + 1;
                }
                db.SaveChanges();
            }
        }
    }
}
