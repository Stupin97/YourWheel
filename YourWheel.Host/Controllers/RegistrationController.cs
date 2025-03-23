namespace YourWheel.Host.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;
    using YourWheel.Domain.Dto;
    using YourWheel.Domain.Services;
    using YourWheel.Host.Extensions;
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
            Tuple<bool, string> result = await this._registrationService.IsAccountOccupiedByAnotherUser(userDto.Login);

            if (result.Item1)
                return BadRequest(new DetailsDto 
                {
                    Details = this._objectTitlesService.GetTitleByTag(result.Item2,
                    Guid.Parse(ObjectTitles.Constants.RussianLanguageGuid)) 
                });

            ClaimsIdentity userClaimsIdentity = await this._registrationService.RigistrationByLoginAsync(userDto);

            var jwt = _jwtService.GenerateToken(userClaimsIdentity.Claims);

            var tokenString = _jwtService.GetTokenString(jwt);

            AppUserDto appUser = await this._appUserService.GetAppUserDtoAsync(this._jwtService.GetUserId(tokenString), this.HttpContext.Connection.RemoteIpAddress?.ToString());

            if (appUser == null)
                return BadRequest(new DetailsDto { Details = this._objectTitlesService.GetTitleByTag(ObjectTitles.Constants.ErrorText, Guid.Parse(ObjectTitles.Constants.RussianLanguageGuid)) });

            HttpContext.AddToken(tokenString);

            HttpContext.AddSecurityHeaders();

            HttpContext.AddCookieKey(HttpContextExtension.YOURWHEEL_CULTURE, appUser.CurrentLanguageId.ToString());

            return NoContent();
        }

        /// <summary>
        /// Регистрация через телефон
        /// </summary>
        /// <param name="userDto">Информация о пользователе</param>
        [HttpPost("registration-by-phone")]
        public async Task<IActionResult> RigistrationByPhoneAsync(UserDto userDto)
        {
            return NoContent();
        }

        /// <summary>
        /// Регистрация через почту
        /// </summary>
        /// <param name="userDto">Информация о пользователе</param>
        [HttpPost("registration-by-email")]
        public async Task<IActionResult> RigistrationByEmailAsync(UserDto userDto)
        {
            return NoContent();
        }
    }
}
