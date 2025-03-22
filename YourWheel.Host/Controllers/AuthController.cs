namespace YourWheel.Host.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    using YourWheel.Domain.Dto;
    using YourWheel.Domain.Services;
    using YourWheel.Host.Extensions;
    using YourWheel.Host.Services;

    /// <summary>
    /// Контроллер для аутентификации пользователя
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        private readonly IAuthenticationService _authenticationService;

        private readonly IAppUserService _appUserService;

        private readonly IObjectTitlesService _objectTitlesService;

        public AuthController(IJwtService jwtService, IAuthenticationService authenticationService,
            IObjectTitlesService objectTitlesService, IAppUserService appUserService)
        {
            this._jwtService = jwtService;

            this._authenticationService = authenticationService;

            this._objectTitlesService = objectTitlesService;

            this._appUserService = appUserService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(string login, string password)
        {
            ClaimsIdentity userClaimsIdentity = await this._authenticationService.GetIdentityAsync(login, password);

            if (userClaimsIdentity == null)
                return BadRequest(new DetailsDto { Details = this._objectTitlesService.GetTitleByTag(ObjectTitles.Constants.TextErrorLoginOrPassword, Guid.Parse(ObjectTitles.Constants.RussianLanguageGuid)) });

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
        ///   Выход
        /// </summary>
        [Authorize]
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = HttpContext.ReadToken();

            if (!string.IsNullOrEmpty(token))
            {
                await this._appUserService.UpdateAppUserAfterLogoutAsync(this._jwtService.GetUserId(token));

                HttpContext.DeleteToken();

                return NoContent();
            }

            return Unauthorized();
        }
    }
}