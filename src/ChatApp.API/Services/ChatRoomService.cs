using ChatApp.API.DTOs;
using ChatApp.API.Repositories;
using ChatApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ChatRoom> CreateRoom([FromBody] CreateRoomRequest dto)
        {
            string slug = GenerateSlug(dto.Name);
            if (await _repo.SlugExistsAsync(slug))
            {
                throw new InvalidOperationException("Room name is invalid.");
            }

            ChatRoom newRoom = new ChatRoom()
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Slug = slug,
                IsInviteOnly = dto.IsPrivate
            };

            await _repo.AddRoomAsync(newRoom);

            return newRoom;
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
