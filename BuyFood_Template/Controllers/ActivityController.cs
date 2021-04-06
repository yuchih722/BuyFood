using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BuyFood_Template.Models;
using Microsoft.AspNetCore.Mvc;


namespace BuyFood_Template.Controllers
{
    public class ActivityController : Controller
    {
        public IActionResult ShowActivity()
        {
            return View();
        }


        //ettoday 新聞 生活 網頁
        public readonly string RSS_URL = @"http://feeds.feedburner.com/ettoday/health?format=xml";
        public IActionResult ReadRss()
        {
            try
            {
                var webRequest = WebRequest.Create(RSS_URL);
                var reader = new StreamReader(
                    webRequest.GetResponse().GetResponseStream());
                var strContent = reader.ReadToEnd();

                return Content(strContent, "text/xml");
            }
            catch (Exception ex)
            {
                return Content("", "text/xml");
            }
        }
        public JsonResult ActivityPageView()
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            var table = db.TActivities.Where(n => n.CStatus == 1).OrderByDescending(n => n.CActivityId).Select(n => n);
            return Json(table.ToList());
        }
    }
}
