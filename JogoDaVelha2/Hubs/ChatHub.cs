using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace JogoDaVelha2.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendPrivateMessage (string user, string message)
        {
            await Clients.User(user).SendAsync("ReceiveMessage", user, message);
        }

        // Referência: https://docs.microsoft.com/pt-br/javascript/api/@microsoft/signalr/?view=signalr-js-latest
        public async Task SendMessageToCaller(string message)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", message);
        }
    }
}
