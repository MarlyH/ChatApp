using ChatApp.API.DTOs;
using ChatApp.API.Repositories;
using ChatApp.Domain.Models;

namespace ChatApp.API.Services
{
    public class ChatMessageService
    {
        private readonly ChatMessageRepository _chatMsgRepository;
        private readonly ChatRoomRepository _chatRoomRepository;
        private readonly RoomMemberService _roomMemberService;

        public ChatMessageService(
            ChatMessageRepository chatMsgRepository, 
            ChatRoomRepository chatRoomRepository, 
            RoomMemberService roomMemberService)
        {
            _chatMsgRepository = chatMsgRepository;
            _chatRoomRepository = chatRoomRepository;
            _roomMemberService = roomMemberService;
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

            return new ServiceResult
            {
                Succeeded = true,
                Message = $"Message successfully sent to room {room.Name}."
            };
        }
    }
}