using BuyFood_Template.Models;
using BuyFood_Template.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.Controllers
{
    public class admChat : Controller
    {
        擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
        public IActionResult admChatView()
        {
            return View();
        }
        public JsonResult getChatRooms()
        {
            List<adrChatRoomAdm> clist = new List<adrChatRoomAdm>();

            var chatRooms = (from c in db.TChatRooms
                             orderby c.CSaveTime descending
                             where c.CMemberId!=16
                             group c by c.CMemberId into g
                             select new
                             {
                                 cChatRoomID = g.Key
                             }).ToList();

            foreach(var cr in chatRooms)
            {
                var getTime = db.TChatRooms.Where(n => n.CMemberId == cr.cChatRoomID).Select(n => n.CMessageTime).FirstOrDefault();
                var getName = db.TChatRooms.Where(n => n.CMemberId == cr.cChatRoomID).Select(n => n.CMember.CName).FirstOrDefault();
                var getID = db.TChatRooms.Where(n => n.CMemberId == cr.cChatRoomID).Select(n => n.CMemberId).FirstOrDefault().Value;
                var getFoto = db.TChatRooms.Where(n => n.CMemberId == cr.cChatRoomID).Select(n => n.CPhoto).FirstOrDefault();
                clist.Add(new adrChatRoomAdm { cChannelID = getID, cMemberName = getName, cTheLastChatTime = getTime,cFoto=getFoto });
            }

            return Json(clist);
        }

        public JsonResult getContext(int MemberID)
        {
            var admChatContext = db.TChatRooms.OrderBy(n => n.CSaveTime).Where(n=>n.CMemberId==MemberID||n.CMemberId==16).Select(n => n).ToList();
            return Json(admChatContext);
        }
        public IActionResult test()
        {
            return View();
        }
    }
}
