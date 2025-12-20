using Microsoft.AspNetCore.SignalR;

namespace ChatApp.API.Hubs
{
    public class ChatRoomHub : Hub
    {
        public async Task JoinRoom(string roomSlug, string? username = null, string? guestToken = null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomSlug);

            bool isAuthenticated = Context.User?.Identity?.IsAuthenticated is true;
            string displayName;
            if (isAuthenticated)
            {
                displayName = Context.User?.Identity?.Name!;
            }
            else
            {
                displayName = "Guest User"; 
            }

            await Clients.Group(roomSlug).SendAsync("UserJoined", $"{displayName} has joined the chat.");
        }

        public async Task LeaveRoom(string roomSlug)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomSlug);
        }
    }
}
