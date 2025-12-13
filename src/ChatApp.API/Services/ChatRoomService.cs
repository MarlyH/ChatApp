using ChatApp.API.DTOs;
using ChatApp.API.Repositories;
using ChatApp.Domain.Models;

namespace ChatApp.API.Services
{
    public class ChatRoomService
    {
        private readonly ChatRoomRepository _repo;

        public ChatRoomService(ChatRoomRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Creates a new chat room with the specified name and privacy setting.
        /// </summary>
        /// <returns>The newly created <see cref="ChatRoom"/>.</returns>
        public async Task<ServiceResult<ChatRoom>> CreateRoom(CreateRoomRequest dto)
        {
            string slug = GenerateSlug(dto.Name);
            if (await _repo.SlugExistsAsync(slug))
            {
                return new ServiceResult<ChatRoom>
                {
                    Succeeded = false,
                    Message = "Room name is invalid."
                };
            }

            ChatRoom newRoom = new ChatRoom()
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Slug = slug,
                IsInviteOnly = dto.IsPrivate
            };

            await _repo.AddRoomAsync(newRoom);

            return new ServiceResult<ChatRoom>
            {
                Succeeded = true,
                Message = "Chatroom successfully created.",
                Data = newRoom
            };
        }

        /// <summary>
        /// Retrieves a list of all public chat rooms available to users.
        /// </summary>
        /// <returns>A <see cref="ServiceResult{T}"/> containing a list of <see cref="GetRoomsResponse"/> 
        /// objects representing the public chat rooms. The list will be empty if no public rooms are available.
        /// </returns>
        public async Task<ServiceResult<List<GetRoomsResponse>>> GetPublicRooms()
        {
            var publicRooms = await _repo.GetPublicRoomsAsync();

            List<GetRoomsResponse> response = publicRooms.Select(room => new GetRoomsResponse
            {
                Id = room.Id,
                Name = room.Name,
                Slug = room.Slug
            }).ToList();

            return new ServiceResult<List<GetRoomsResponse>>
            {
                Succeeded = true,
                Message = "Chatrooms retrieved successfully.",
                Data = response
            };
        }

        /// <summary>
        /// Retrieves detailed information about a chat room identified by its slug.
        /// </summary>
        public async Task<ServiceResult<GetRoomDetailsResponse>> GetRoomDetails(string slug)
        {
            var room = await _repo.GetRoomBySlugAsync(slug);
            if (room == null)
            {
                return new ServiceResult<GetRoomDetailsResponse>
                {
                    Succeeded = false,
                    Message = "Room not found."
                };
            }

            var response = new GetRoomDetailsResponse
            {
                Name = room.Name,
            };

            return new ServiceResult<GetRoomDetailsResponse>
            {
                Succeeded = true,
                Message = "Room details retrieved successfully.",
                Data = response
            };
        }

        private string GenerateSlug(string roomName)
        {
            return roomName
                .ToLower()
                .Replace(" ", "-")
                .Replace("'", "")
                .Replace("!", "")
                .Replace(".", "");
        }
    }
}
