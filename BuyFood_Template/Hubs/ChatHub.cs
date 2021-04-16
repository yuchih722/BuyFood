using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.Hubs
{
    public class ChatHub :Hub
    {
        //public async Task SendMessage(string user,string message,string adrfoto,int adrMemberIDForChat)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", user, message,adrfoto,adrMemberIDForChat);
        //}

        public async Task AddGroup(string groupName, string username)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

        }
        public Task SendMessageToGroup(string groupName, string username, string message,string adrfoto,int adrMemberIDForChat)
        {
            return Clients.Group(groupName).SendAsync("ReceiveGroupMessage", groupName, username, message, adrfoto,adrMemberIDForChat);
        }
        public Task SendMessageToOrder(string groupName, string username, string message, string adrfoto, int adrMemberIDForChat)
        {
            return Clients.Group(groupName).SendAsync("ReceiveOrderMessage", groupName, username, message, adrfoto, adrMemberIDForChat);
		}

        public async Task updateCombo()
        {
            await Clients.All.SendAsync("receiveComboQty");
        }
    }

}
