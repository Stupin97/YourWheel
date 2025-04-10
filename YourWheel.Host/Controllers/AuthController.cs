namespace YourWheel.Host.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    using YourWheel.Domain.Dto;
    using YourWheel.Domain.Services;
    using YourWheel.Host.Extensions;
    using YourWheel.Host.Logging;
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

            Guid userId = this._jwtService.GetUserId(tokenString);

            AppUserDto appUser = await this._appUserService.GetAppUserDtoAsync(userId, this.HttpContext.Connection.RemoteIpAddress?.ToString());

            if (appUser == null)
                return BadRequest(new DetailsDto { Details = this._objectTitlesService.GetTitleByTag(ObjectTitles.Constants.ErrorText, Guid.Parse(ObjectTitles.Constants.RussianLanguageGuid)) });

            this.HttpContext.AddToken(tokenString);

            this.HttpContext.AddSecurityHeaders();

            this.HttpContext.AddCookieKey(HttpContextExtension.YOURWHEEL_CULTURE, appUser.CurrentLanguageId.ToString());

            Log.Info("Success login", userId);

            return NoContent();
        }

        /// <summary>
        ///   Выход
        /// </summary>
        [Authorize]
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = this.HttpContext.ReadToken();

            if (!string.IsNullOrEmpty(token))
            {
                Guid userId = this._jwtService.GetUserId(token);

                await this._appUserService.UpdateAppUserAfterLogoutAsync(userId);

                HttpContext.DeleteToken();

                Log.Info("User logout", userId);

                return NoContent();
            }

            return Unauthorized();
        }
    }
}