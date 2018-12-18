namespace LuckySlots.App.Hubs
{
    using LuckySlots.Data.Models;
    using LuckySlots.Infrastructure;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ChatHub : Hub
    {
        private readonly UserManager<User> userManager;

        public ChatHub(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task Send(object sender, string message)
        {
            await Clients.Others.SendAsync("broadcastMessage", sender, message);
        }

        public async Task SendTyping(object sender)
        {
            await Clients.Others.SendAsync("typing", sender);
        }
    }
}
