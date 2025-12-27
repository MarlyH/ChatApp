using ChatApp.API.DTOs;
using ChatApp.API.Hubs;
using ChatApp.API.Repositories;
using ChatApp.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.API.Services
{
    public class ChatMessageService
    {
        private readonly ChatMessageRepository _chatMsgRepository;
        private readonly ChatRoomRepository _chatRoomRepository;
        private readonly RoomMemberService _roomMemberService;
        private readonly IHubContext<ChatRoomHub> _hubContext;

        public ChatMessageService(
            ChatMessageRepository chatMsgRepository, 
            ChatRoomRepository chatRoomRepository, 
            RoomMemberService roomMemberService,
            IHubContext<ChatRoomHub> hubContext)
        {
            _chatMsgRepository = chatMsgRepository;
            _chatRoomRepository = chatRoomRepository;
            _roomMemberService = roomMemberService;
            _hubContext = hubContext;
        }

        public async Task<ServiceResult> SendMessageAsync(
                string roomSlug,
                string msgContent,
                Guid? userId = null,
                string? guestToken = null)
        {
            // get room
            ChatRoom? room = await _chatRoomRepository.GetRoomBySlugAsync(roomSlug);
            if (room is null)
            {
                return new ServiceResult
                {
                    Message = "Room must be provided.",
                    Succeeded = false
                };
            }

            // user can be registered or guest, get appopriate RoomMember
            RoomMember? roomMember = await _roomMemberService.ResolveRoomMemberAsync(room!, userId, guestToken);
            if (roomMember is null)
            {
                return new ServiceResult
                {
                    Message = "Room has not been joined by this user/guest.",
                    Succeeded = false
                };
            }

            // send message
            ChatMessage msg = new ChatMessage
            {
                RoomId = room.Id,
                SenderId = roomMember.Id,
                Content = msgContent,
            };
            await _chatMsgRepository.CreateMessageAsync(msg);

            // broadcast to the room's SignalR hub group
            await _hubContext.Clients.Group(roomSlug).SendAsync("MessageReceived", new GetChatMessageResponse
            {
                Id = msg.Id,
                Content = msg.Content,
                SenderUsername = roomMember.SenderName,
                CreatedAt = msg.CreatedAt
            });

            return new ServiceResult
            {
                Succeeded = true,
                Message = $"Message successfully sent to room {room.Name}."
            };
        }

        public async Task<ServiceResult<List<GetChatMessageResponse>>> GetAllRoomMessagesAsync(string roomSlug)
        {
            ChatRoom? room = await _chatRoomRepository.GetRoomWithMessagesBySlugAsync(roomSlug);
            if (room is null)
            {
                return new ServiceResult<List<GetChatMessageResponse>>
                {
                    Message = "Room must be provided.",
                    Succeeded = false
                };
            }

            List<GetChatMessageResponse> messages = room.Messages
                .Select(m => 
                    new GetChatMessageResponse
                    {
                        Id = m.Id,
                        Content = m.Content,
                        CreatedAt = m.CreatedAt,
                        SenderUsername = m.Sender!.SenderName
                    })
                .OrderBy(m => m.CreatedAt)
                .ToList();

            return new ServiceResult<List<GetChatMessageResponse>>
            {
                Succeeded = true,
                Data = messages
            };
        }

        [Authorize]
        public async Task<ServiceResult> DeleteMessageAsync(Guid messageId, string roomSlug)
        {
            var succeeded = await _chatMsgRepository.DeleteMessageAsync(messageId);
            if (!succeeded)
            {
                return new ServiceResult
                {
                    Message = "Message not found",
                    Succeeded = false
                };
            }

            // after successful delete, broadcast to the room's SignalR hub group
            await _hubContext.Clients.Group(roomSlug).SendAsync("MessageDeleted", messageId);

            return new ServiceResult
            {
                Message = "Message successfully deleted",
                Succeeded = true
            };
        }
    }
}