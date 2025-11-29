using ChatApp.API.DTOs;
using ChatApp.API.Models;
using ChatApp.API.Repositories;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.API.Services
{
    public class UserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Creates a new AppUser.
        /// Email is currently confirmed by default.
        /// </summary>
        public async Task<CreateUserResult> CreateNewUserAsync(CreateUserRequest dto)
        {
            AppUser newUser = new AppUser
            {
                UserName = dto.Username,
                Email = dto.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newUser, dto.Password);

            if (!result.Succeeded)
            {
                return new CreateUserResult
                {
                    Succeeded = false,
                    Errors = result.Errors
                    .Select(e => e.Description)
                    .ToList()
                };
            }

            return new CreateUserResult
            {
                Succeeded = true,
                User = newUser
            };
        }
    }
}
