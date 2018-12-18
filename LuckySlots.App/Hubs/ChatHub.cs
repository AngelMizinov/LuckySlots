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

        public static Dictionary<string, string> ConnectedUsers { get; set; } = new Dictionary<string, string>();

        public async Task Send(object sender, string message, string email)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            var isSupport = await this.userManager.IsInRoleAsync(user, GlobalConstants.SupportRoleName);

            if (isSupport)
            {
                await this.Clients.User(user.Id).SendAsync("broadcastMessage", sender, message);
            }
            else
            {
                await this.Clients.Groups("Support").SendAsync("broadcastMessage", sender, message);
            }


            //await Clients.Others.SendAsync("broadcastMessage", sender, message);
        }

        public async Task SendTyping(object sender)
        {
            // Broadcast the typing notification to all clients except the sender
            await this.Clients.Groups("Support").SendAsync("typing", sender);
            //await Clients.Others.SendAsync("typing", sender);
        }

        public override async Task OnConnectedAsync()
        {
            var currentUser = this.Context.User;
            var connedtionId = this.Context.ConnectionId;

            if (!ConnectedUsers.ContainsKey(currentUser.Identity.Name))
            {
                ConnectedUsers.Add(currentUser.Identity.Name, connedtionId);
            }

            ConnectedUsers[currentUser.Identity.Name] = connedtionId;

            if (currentUser.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, "Admins");
                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, "ElevatedUsers");
            }
            else if (currentUser.IsInRole(GlobalConstants.SupportRoleName))
            {
                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, "Support");
                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, "ElevatedUsers");
            }
            else
            {
                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, "Regulars");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var currentUser = this.Context.User;

            if (ConnectedUsers.ContainsKey(currentUser.Identity.Name))
            {
                ConnectedUsers.Remove(currentUser.Identity.Name, out _);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
