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
        public IActionResult admChatView(int? id)
        {
            TempData["get_chat_id"] = id;
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
            var admChatContext = db.TChatRooms.OrderBy(n => n.CSaveTime).Where(n=>n.CMemberId==MemberID&&n.CDifRoomId==MemberID||n.CMemberId==16&&n.CDifRoomId==MemberID).Select(n => n).ToList();
            return Json(admChatContext);
        }
        public void saveContext(string MemberName,string Content,string MessageTime,string foto,int userID,int difChatRoomID)
        {
            if (userID != 16 || difChatRoomID == 16)
            {
                return;
            }
            DateTime theSaveTime = DateTime.Now;


            TChatRoom cr = new TChatRoom()
            {
                CContent = Content,
                CMessageTime = MessageTime,
                CSaveTime = theSaveTime,
                CMemberId = userID,
                CPhoto = foto,
                CDifRoomId = difChatRoomID,
                CReview=1
            };
            db.TChatRooms.Add(cr);
            db.SaveChanges();

        }
    }
}
