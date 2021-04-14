using BuyFood_Template.Models;
using BuyFood_Template.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.Controllers
{
    public class adChatRoomController : Controller
    {
        public void getMessage(string MemberName,string Content,string MessageTime,string foto,int userID)
        {
            if (userID == 16)
            {
                return;
            }
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
            DateTime theSaveTime = DateTime.Now;


            TChatRoom cr = new TChatRoom()
            {
                CContent = Content,
                CMessageTime = MessageTime,
                CSaveTime = theSaveTime,
                CMemberId = userID,
                CPhoto=foto,
                CDifRoomId=userID,
                CReview=0
            };
            db.TChatRooms.Add(cr);
            db.SaveChanges();

        }

        public JsonResult ListMessages(int ChannelID)
        {
            擺腹BuyFoodContext db = new 擺腹BuyFoodContext();

            //在一開始就從資料庫抓資料至畫面顯示
            //判斷會員是否是自己
            var MessagesBefore = from mes in db.TChatRooms
                                 where mes.CDifRoomId== ChannelID
                                 join member in db.TMembers on mes.CMemberId equals member.CMemberId
                                 orderby mes.CSaveTime
                                 select new
                                 {
                                     CChatRoomId = mes.CChatRoomId,
                                     CMemberId = mes.CMemberId,
                                     CMemberName = member.CName,
                                     CContent = mes.CContent,
                                     CMessageTime = mes.CMessageTime,
                                     CSaveTime = mes.CSaveTime,
                                     mes.CPhoto,
                                     mes.CDifRoomId
                                 };

            //把ID換成名稱


            return Json(MessagesBefore.ToList());
        }
    }
}
