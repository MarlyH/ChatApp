using ChatApp.API.DTOs;
using ChatApp.API.Models;
using ChatApp.API.Repositories;
using ChatApp.Domain.Models;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ChatApp.API.Services
{
    public class RoomMemberService
    {
        private readonly RoomMemberRepository _roomMemberRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly ChatRoomRepository _chatRoomRepository;

        public RoomMemberService(
            RoomMemberRepository roomMemberRepository, 
            UserManager<AppUser> userManager, 
            ChatRoomRepository chatRoomRepository)
        {
            _roomMemberRepository = roomMemberRepository;
            _userManager = userManager;
            _chatRoomRepository = chatRoomRepository;
        }

        /// <summary>
        /// Joins a registered user to a chat room.
        /// </summary>
        public async Task<ServiceResult> JoinRoomRegisteredAsync(Guid userId, string slug)
        {
            // ensure room exists
            ChatRoom? room = await _chatRoomRepository.GetRoomBySlugAsync(slug);
            if (room is null)
            {
                return new ServiceResult
                {
                    Succeeded = false,
                    Message = "Chat room not found."
                };
            }

            // ensure user has not already joined
            if (await _roomMemberRepository.IsUserRoomMemberAsync(userId, room.Id))
            {
                return new ServiceResult
                {
                    Succeeded = false,
                    Message = "User is already a member of this chat room."
                };
            }

            // create
            RoomMember newMember = new()
            {
                UserId = userId,
                RoomId = room.Id,
                JoinedAt = DateTime.UtcNow
            };
            await _roomMemberRepository.AddRegisteredRoomMemberAsync(newMember);

            return new ServiceResult
            {
                Succeeded = true,
                Message = $"User successfully joined the chat room {room.Name}."
            };
        }
    }
}
