using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.Hubs
{
    public class ChatHub :Hub
    {
        public async Task SendMessage(string user,string message,string adrfoto,int adrMemberIDForChat)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message,adrfoto,adrMemberIDForChat);
        }

    }
}
