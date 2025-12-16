using ChatApp.API.DTOs;
using ChatApp.API.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ChatApp.API.Services
{
    public class UserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private readonly ILogger<UserService> _logger;

        public UserService(
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            IConfiguration configuration,
            EmailService emailService,
            ILogger<UserService> logger
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailService = emailService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new AppUser and sends a confirmation email.
        /// If email sending fails, the user creation is rolled back.
        /// </summary>
        public async Task<ServiceResult<AppUser>> CreateNewUserAsync(CreateUserRequest dto)
        {
            AppUser newUser = new AppUser
            {
                UserName = dto.Username,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(newUser, dto.Password);
            if (!result.Succeeded)
            {
                return new ServiceResult<AppUser>
                {
                    Succeeded = false,
                    Errors = result.Errors
                    .Select(e => e.Description)
                    .ToList()
                };
            }

            // Send confirmation email and roll back user creation if it fails
            var emailResult = await SendConfirmationEmailAsync(newUser);
            if (!emailResult.Succeeded)
            {
                await _userManager.DeleteAsync(newUser);
                return new ServiceResult<AppUser>
                {
                    Succeeded = false,
                    Errors = new List<string> { emailResult.Message }
                };
            }

            return new ServiceResult<AppUser>
            {
                Succeeded = true,
                Data = newUser
            };
        }

        public async Task<ServiceResult> ResendConfirmation(ResendConfirmationRequest dto)
        {
            AppUser? user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return new ServiceResult
                {
                    Succeeded = false,
                    Message = "User not found."
                };
            }

            return await SendConfirmationEmailAsync(user);
        }

        public async Task<ServiceResult> LoginAsync(LoginRequest dto)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(dto.Username, dto.Password, dto.IsPersistent, false);
            if (!result.Succeeded)
            {
                return new ServiceResult
                {
                    Succeeded = false,
                    Message = "Invalid username or password."
                };
            }

            return new ServiceResult
            {
                Succeeded = true,
                Message = "User successfully logged in."
            };
        }

        public async Task<ServiceResult<GetProfileResponse>> GetUserProfileAsync(ClaimsPrincipal userClaims)
        {
            var appUser = await _userManager.GetUserAsync(userClaims);

            if (appUser is null)
            {
                return new ServiceResult<GetProfileResponse>
                {
                    Succeeded = false,
                    Message = "User not found."
                };
            }
            
            return new ServiceResult<GetProfileResponse>
            {
                Succeeded = true,
                Data = new GetProfileResponse
                {
                    Username = appUser.UserName!,
                    Email = appUser.Email!
                }
            };
        }

        /// <summary>
        /// Sends a confirmation email to the specified user.
        /// </summary>
        public async Task<ServiceResult> SendConfirmationEmailAsync(AppUser user)
        {
            if (user.EmailConfirmed) 
            { 
                return new ServiceResult { Succeeded = false, Message = "Email already confirmed." }; 
            }

            // Generate email confirmation token and link
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var apiUrl = _configuration.GetValue<string>("ApiUrl");
            var confirmLink = $"{apiUrl}/api/user/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            // Send the confirmation email
            try
            {
                await _emailService.SendEmailAsync(
                    user.Email,
                    "Confirm your ChatApp account",
                    $@"
                    <p>Welcome, {user.UserName}!</p>
                    <p>Please confirm your email by clicking the link below:</p>
                    <p><a href=""{confirmLink}"">Confirm Email</a></p>
                    <p>If you didn't create this account, you can ignore this email.</p>"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send confirmation email for user {user.Id}.");
                return new ServiceResult { Succeeded = false, Message = "Failed to send confirmation email." };
            }

            return new ServiceResult { Succeeded = true, Message = "Confirmation email sent." };
        }

        /// <summary>
        /// Confirms a user's email address and verifies their account.
        /// </summary>
        public async Task<ServiceResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResult
                {
                    Succeeded = false,
                    Message = "User not found."
                };
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return new ServiceResult
                {
                    Succeeded = false,
                    Message = "Email confirmation failed."
                };
            }

            return new ServiceResult
            {
                Succeeded = true,
                Message = "Email successfully confirmed."
            };
        }
    }
}
