using Microsoft.AspNetCore.SignalR;

namespace Pakhd.Client.Hubs
{
    public class ItemHub : Hub
    {
        public async Task SendItem(int itemId, ItemStatus status)
        {
            await Clients.All.SendAsync("ReceiveItem", itemId, status);
        }
    }
}
