using Microsoft.AspNetCore.SignalR;

namespace ChatApp.API.Hubs
{
    public class ChatRoomHub : Hub
    {
        public async Task JoinRoom(Guid roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
        }
    }
}
