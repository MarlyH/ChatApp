using ChatApp.API.DTOs;
using ChatApp.API.Repositories;
using ChatApp.Domain.Models;

namespace ChatApp.API.Services
{
    public class RoomMemberService
    {
        private readonly RoomMemberRepository _roomMemberRepository;
        private readonly ChatRoomRepository _chatRoomRepository;

        public RoomMemberService(RoomMemberRepository roomMemberRepository, ChatRoomRepository chatRoomRepository)
        {
            _roomMemberRepository = roomMemberRepository;
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

        /// <summary>
        /// Resolves the sender’s RoomMember for a room based on either a registered user ID
        /// or a guest access token. 
        /// </summary>
        /// <returns>The corresponding RoomMember. Null if the identity is invalid
        /// or the user is not a member of the room.</returns>
        public async Task<RoomMember?> ResolveRoomMemberAsync(ChatRoom room, Guid? userId = null, string? guestToken = null)
        {
            RoomMember? member = null;

            if ((userId is not null && guestToken is not null)
                || (userId is null && guestToken is null))
            {
                return null;
            }    

            if (userId is not null) // get registered user
            {
                member = await _roomMemberRepository.GetRegisteredRoomMemberAsync(room.Id, (Guid)userId);
            }
            else if (guestToken is not null) // get guest user
            {
                member = await _roomMemberRepository.GetGuestRoomMemberAsync(room.Id, guestToken);
            }

            return member;
        }
    }
}
