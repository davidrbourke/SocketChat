using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SocketChat.Hubs
{
    public class ChatHub : Hub
    {
        public Task Send(string message)
        {
            IReadOnlyList<string> me = new List<string> { Context.ConnectionId };
            
            return Clients.AllExcept(me).InvokeAsync("Send", $"{Context.ConnectionId}: {message}");
        }

        public override Task OnConnectedAsync()
        {
            SendMonitoringData("Connected", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        private void SendMonitoringData(string eventType, string connectionId)
        {
            this.AllExceptMe().InvokeAsync("Joined", $"{ Context.ConnectionId}: Has joined the conversation");
        }

    }

    public static class HubExtensions
    {
        public static IClientProxy AllExceptMe(this Hub hub)
        {
            IReadOnlyList<string> me = new List<string> { hub.Context.ConnectionId };
            return hub.Clients.AllExcept(me);
        }
    }
}
