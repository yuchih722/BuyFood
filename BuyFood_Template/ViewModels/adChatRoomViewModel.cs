using BuyFood_Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModels
{
    public class adChatRoomViewModel
    {
        private TChatRoom iv_ChatRoom = null;
        public TChatRoom ChatRoom { get { return iv_ChatRoom; } }
        public adChatRoomViewModel(TChatRoom c)
        {
            iv_ChatRoom = c;
        }
        public adChatRoomViewModel()
        {
            iv_ChatRoom = new TChatRoom();
        }
        擺腹BuyFoodContext db = new 擺腹BuyFoodContext();
        
        public int CChatRoomId { get { return iv_ChatRoom.CChatRoomId; } set { iv_ChatRoom.CChatRoomId = value; } }
        public int? CMemberId { get { return iv_ChatRoom.CMemberId; } set { iv_ChatRoom.CMemberId = value; } }
        public string CMemberName { get; set; }
        public string CContent { get { return iv_ChatRoom.CContent; } set { iv_ChatRoom.CContent = value; } }
        public string CMessageTime { get { return iv_ChatRoom.CMessageTime; } set { iv_ChatRoom.CMessageTime = value; } }

        public DateTime? CSaveTime { get { return iv_ChatRoom.CSaveTime; } set{ iv_ChatRoom.CSaveTime = value; } }


    }
}
