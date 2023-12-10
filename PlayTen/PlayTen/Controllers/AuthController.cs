using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlayTen.BLL.DTO;
using PlayTen.BLL.Interfaces;
using PlayTen.BLL.Interfaces.EmailSending;
using PlayTen.DAL.Entities;

namespace PlayTen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly IEmailContentService _emailContentService;
        private readonly IEmailSendingService _emailSendingService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService,
            IJwtService jwtService, IEmailContentService emailContentService, IEmailSendingService emailSendingService, UserManager<User> userManager, IMapper mapper)
        {
            _authService = authService;
            _jwtService = jwtService;
            _emailContentService = emailContentService;
            _emailSendingService = emailSendingService;
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Authentication of existing user
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _authService.FindUserByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    return BadRequest("Даного аккаунту не існує");
                }
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    return BadRequest("Реєстрація не підтверджена. Перевірте вашу пошту і підтвердіть реєстрацію");
                }

                var result = await _authService.SignInAsync(loginDto);
                if (result.IsLockedOut)
                {
                    return BadRequest("Аккаунт заблоковано");
                }
                if (result.Succeeded)
                {
                    var userDto = _mapper.Map<User, UserDTO>(user);
                    var generatedToken = await _jwtService.GenerateJwtTokenAsync(userDto);
                    return Ok(new { token = generatedToken, user = user });
                }
                else
                {
                    return BadRequest("Неправильний пароль");
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// Registration of new user
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Невалідні дані");
            }
            var registeredUser = await _authService.FindUserByEmailAsync(registerDto.Email);
            if (registeredUser != null)
            {
                // Check if user's email is confirmed
                if (await _userManager.IsEmailConfirmedAsync(registeredUser))
                {
                    return BadRequest("Такий користувач вже існує та реєстація підтверджена");
                }

                TimeSpan elapsedTimeFromRegistration = DateTime.Now - registeredUser.RegistredOn;
                if (elapsedTimeFromRegistration < TimeSpan.FromHours(12))
                {
                    return BadRequest("Такий користувач вже існує, але реєстрація не підтверджена");
                }

                await _userManager.DeleteAsync(registeredUser);
            }

            var result = await _authService.CreateUserAsync(registerDto);
            if (!result.Succeeded)
            {
                return BadRequest(new { error = result.Errors });
            }

            registeredUser = await _userManager.FindByEmailAsync(registerDto.Email);
            try
            {
                await SendConfirmationEmail(registeredUser);
            }
            catch (Exception)
            {
                await _userManager.DeleteAsync(registeredUser);
                throw;
            }
            return NoContent();
        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([Required, FromQuery] string userId, [Required, FromQuery] string token)
        {
            var decodedToken = HttpUtility.UrlDecode(token);
            if (!ModelState.IsValid)
            {
                return Redirect($"localhost:5001/api/signin?error={404}");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Redirect($"localhost:5001/api/signin?error={404}");
            }
            TimeSpan elapsedTimeFromRegistration = DateTime.Now - user.RegistredOn;
            if (elapsedTimeFromRegistration >= TimeSpan.FromHours(12))
            {
                // 410 GONE - User should be deleted, because 12hrs elapsed from registration and email is still is not confirmed
                await _userManager.DeleteAsync(user);
                return Redirect($"localhost:5001/api/signin?error={410}");
            }

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error.Description);
                }
                return Redirect($"localhost:5001/api/signin?error={400}");
            }

            await _userManager.AddToRoleAsync(user, "User");

            return Redirect("http://localhost:4200/auth");
        }

        [NonAction]
        public async Task SendConfirmationEmail(User user)
        {
            var reciever = new MailAddress(user.Email, $"{user.Name} {user.Surname}");

            var token = HttpUtility.UrlEncode(await _authService.GetConfirmationTokenAsync(user));

            string url = GetConfirmEmailApiURL(user.Id, token);

            var message = _emailContentService.GetAuthRegisterEmail(url);

            await _emailSendingService.SendEmailAsync(message, reciever);
        }

        private string GetConfirmEmailApiURL(string userId, string token)
        {
            return $"https://localhost:5001/api/Auth/confirmEmail?userId={userId}&token={HttpUtility.UrlEncode(token)}";
        }
    }
}
