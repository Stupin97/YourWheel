namespace YourWheel.Host.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Security.Claims;
    using YourWheel.Domain.Dto;
    using YourWheel.Domain.Services;
    using YourWheel.Host.Extensions;
    using YourWheel.Host.Logging;
    using YourWheel.Host.Services;
    using YourWheel.Host.Services.Registration;

    [Route("api/registration")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        private readonly IObjectTitlesService _objectTitlesService;

        private readonly IJwtService _jwtService;

        private readonly IAppUserService _appUserService;

        private enum RegistrationTypesBy { Login, Email, Phone }

        public RegistrationController(IRegistrationService registrationService, IObjectTitlesService objectTitlesService,
            IJwtService jwtService, IAppUserService appUserService)
        {
            this._registrationService = registrationService;

            this._objectTitlesService = objectTitlesService;

            this._jwtService = jwtService;

            this._appUserService = appUserService;
        }

        /// <summary>
        /// Регистрация через логин и пароль
        /// </summary>
        /// <param name="userDto">Информация о пользователе</param>
        [HttpPost("registration-by-login")]
        public async Task<IActionResult> RegistrationByLoginAsync(UserDto userDto)
        {
            return await RegistrationAsync(userDto, RegistrationTypesBy.Login);
        }

        /// <summary>
        /// Регистрация через почту
        /// </summary>
        /// <param name="userDto">Информация о пользователе</param>
        [HttpPost("registration-by-email")]
        public async Task<IActionResult> RegistrationByEmailAsync(UserDto userDto)
        {
            return await RegistrationAsync(userDto, RegistrationTypesBy.Email);
        }

        /// <summary>
        /// Регистрация через телефон
        /// </summary>
        /// <param name="userDto">Информация о пользователе</param>
        [HttpPost("registration-by-phone")]
        public async Task<IActionResult> RegistrationByPhoneAsync(UserDto userDto)
        {
            return await RegistrationAsync(userDto, RegistrationTypesBy.Phone);
        }

        private async Task<IActionResult> RegistrationAsync(UserDto userDto, RegistrationTypesBy registrationType)
        {
            string enteredField = "";

            try
            {
                Task<ClaimsIdentity> registrationAsync;

                switch (registrationType)
                {
                    case RegistrationTypesBy.Login:

                        enteredField = userDto.Login;

                        registrationAsync = this._registrationService.RegistrationByLoginAsync(userDto);

                        break;
                    case RegistrationTypesBy.Email:

                        enteredField = userDto.Email;

                        registrationAsync = this._registrationService.RegistrationByEmailAsync(userDto);

                        break;
                    case RegistrationTypesBy.Phone:

                        enteredField = userDto.Phone;

                        registrationAsync = this._registrationService.RegistrationByPhoneAsync(userDto);

                        break;
                    default:
                        throw new InvalidOperationException($"Недопустимое состояние: {registrationType}");
                }

                Tuple<bool, string> result = await this._registrationService.IsAccountOccupiedByAnotherUser(enteredField);

                if (result.Item1)
                    return BadRequest(new DetailsDto
                    {
                        Details = this._objectTitlesService.GetTitleByTag(result.Item2,
                        Guid.Parse(ObjectTitles.Constants.RussianLanguageGuid))
                    });

                ClaimsIdentity userClaimsIdentity = await registrationAsync;

                var jwt = _jwtService.GenerateToken(userClaimsIdentity.Claims);

                var tokenString = _jwtService.GetTokenString(jwt);

                Guid userId = this._jwtService.GetUserId(tokenString);

                AppUserDto appUser = await this._appUserService.GetAppUserDtoAsync(this._jwtService.GetUserId(tokenString), this.HttpContext.Connection.RemoteIpAddress?.ToString());

                if (appUser == null)
                    return BadRequest(new DetailsDto { Details = this._objectTitlesService.GetTitleByTag(ObjectTitles.Constants.ErrorText, Guid.Parse(ObjectTitles.Constants.RussianLanguageGuid)) });

                HttpContext.AddToken(tokenString);

                HttpContext.AddSecurityHeaders();

                HttpContext.AddCookieKey(HttpContextExtension.YOURWHEEL_CULTURE, appUser.CurrentLanguageId.ToString());

                Log.Info($"User has successfully registered by {registrationType}", userId);

                return NoContent();
            }
            finally
            {
                this._registrationService.RemoveRegistrationAttempt(enteredField);
            }
        }
    }
}
