namespace YourWheel.Host.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    using YourWheel.Domain.Dto;
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

        private readonly IObjectTitlesService _objectTitlesService;

        public AuthController(IJwtService jwtService, IAuthenticationService authenticationService,
            IObjectTitlesService objectTitlesService)
        {
            this._jwtService = jwtService;

            this._authenticationService = authenticationService;

            this._objectTitlesService = objectTitlesService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(string login, string password)
        {
            ClaimsIdentity userClaimsIdentity = await this._authenticationService.GetIdentityAsync(login, password);

            if (userClaimsIdentity == null)
                return BadRequest(new DetailsDto { Details = this._objectTitlesService.GetTitleByTag(ObjectTitles.Constants.TextErrorLoginOrPassword) });

            var jwt = _jwtService.GenerateToken(userClaimsIdentity.Claims);

            var tokenString = _jwtService.GetTokenString(jwt);

            HttpContext.AddToken(tokenString);

            HttpContext.AddSecurityHeaders();

            return NoContent();
        }

        /// <summary>
        ///   Выход
        /// </summary>
        [Authorize]
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            var token = HttpContext.ReadToken();

            if (!string.IsNullOrEmpty(token))
            {
                HttpContext.DeleteToken();

                return NoContent();
            }

            return Unauthorized();
        }
    }
}