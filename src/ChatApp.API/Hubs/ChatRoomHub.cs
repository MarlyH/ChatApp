using Microsoft.AspNetCore.SignalR;

namespace ChatApp.API.Hubs
{
    public class ChatRoomHub : Hub
    {
        public async Task JoinRoom(string roomSlug)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomSlug);
        }

        public async Task LeaveRoom(string roomSlug)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomSlug);
        }
    }
}
