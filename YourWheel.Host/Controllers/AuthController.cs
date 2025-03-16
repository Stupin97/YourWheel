namespace YourWheel.Host.Controllers
{
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
            ClaimsIdentity clientClaimsIdentity = await this._authenticationService.GetIdentityAsync(login, password);

            if (clientClaimsIdentity == null)
                return BadRequest(new DetailsDto { Details = this._objectTitlesService.GetTitleByTag(ObjectTitles.Constants.TextErrorLoginOrPassword) });

            var jwt = _jwtService.GenerateToken(clientClaimsIdentity.Claims);

            var tokenString = _jwtService.GetTokenString(jwt);

            HttpContext.AddToken(tokenString);

            HttpContext.AddSecurityHeaders();

            return NoContent();
        }
    }
}