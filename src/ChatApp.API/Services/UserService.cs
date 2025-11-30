using ChatApp.API.DTOs;
using ChatApp.API.Models;
using Microsoft.AspNetCore.Identity;

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
        public async Task<CreateUserResult> CreateNewUserAsync(CreateUserRequest dto)
        {
            AppUser newUser = new AppUser
            {
                UserName = dto.Username,
                Email = dto.Email
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

            // Generate email confirmation token and link
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var apiUrl = _configuration.GetValue<string>("ApiUrl");
            var confirmLink = $"{apiUrl}/api/user/confirm-email?userId={newUser.Id}&token={Uri.EscapeDataString(token)}";

            try
            {
                // Send confirmation email
                await _emailService.SendEmailAsync(
                    newUser.Email,
                    "Confirm your ChatApp account",
                    $@"
                    <p>Welcome, {newUser.UserName}!</p>
                    <p>Please confirm your email by clicking the link below:</p>
                    <p><a href=""{confirmLink}"">Confirm Email</a></p>
                    <p>If you didn't create this account, you can ignore this email.</p>
                "
                );
            }
            catch (Exception ex)
            {
                // Rollback user creation if email fails
                await _userManager.DeleteAsync(newUser);
                _logger.LogError(ex, $"Failed to send confirmation email for user {newUser.Id}.");

                return new CreateUserResult
                {
                    Succeeded = false,
                    Errors = new List<string> { "Something went wrong during registration. Please try again later." }
                };
            }

            return new CreateUserResult
            {
                Succeeded = true,
                User = newUser
            };
        }

        public async Task<SignInResult> LoginAsync(LoginRequest dto)
        {
            return await _signInManager.PasswordSignInAsync(dto.Username, dto.Password, dto.IsPersistent, false);
        }

        public async Task ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                throw new Exception("Email confirmation failed.");
            }
        }
    }
}
