using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.SendAsync("broadcastMessage", name, message);
        }

        public void stream_server(byte[] data)
        {
            Clients.All.SendAsync("stream_client", data);
            System.Diagnostics.Debug.WriteLine(Encoding.UTF8.GetString(data, 0, data.Length));

        }
        
        public void listen_desktop_hub()
        {
            Clients.All.SendAsync("ListenDesktop");
        }
        public void stop_listen_desktop_hub()
        {
            Clients.All.SendAsync("StopListenDesktop");
        }

    }
}