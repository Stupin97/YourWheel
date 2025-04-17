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
        private readonly IRegistrationServiceFactory _registrationServiceFactory;

        private readonly IObjectTitlesService _objectTitlesService;

        private readonly IJwtService _jwtService;

        private readonly IAppUserService _appUserService;

        public RegistrationController(IRegistrationServiceFactory registrationServiceFactory, IObjectTitlesService objectTitlesService,
            IJwtService jwtService, IAppUserService appUserService)
        {
            this._registrationServiceFactory = registrationServiceFactory;

            this._objectTitlesService = objectTitlesService;

            this._jwtService = jwtService;

            this._appUserService = appUserService;
        }

        /// <summary>
        /// Регистрация через логин и пароль
        /// </summary>
        /// <param name="userDto">Информация о пользователе</param>
        [HttpPost("registration-by-login")]
        public async Task<IActionResult> RegistrationByLoginAsync([FromBody] UserDto userDto)
        {
            try
            {
                DetailsDto detailsDto;

                if (this._registrationServiceFactory.IsValidUserData(userDto, out detailsDto))
                {
                    Tuple<bool, string> result = await this._registrationServiceFactory.IsAccountOccupiedByAnotherUser(userDto);

                    if (result.Item1)
                        return BadRequest(new DetailsDto
                        {
                            Details = this._objectTitlesService.GetTitleByTag(result.Item2,
                            Guid.Parse(ObjectTitles.Constants.RussianLanguageGuid))
                        });

                    IRegistrationService service = this._registrationServiceFactory.GetRegistrationService(HelperRegistrationService.RegistrationTypesBy.Login);

                    if (await service.TryRegistrationUserAsync(userDto, ""))
                        return await this.RegistrationAsync(userDto);
                }

                return BadRequest(detailsDto);
            }
            catch (Exception exception)
            {
                throw;
            }
            finally
            {
                Log.Info("EXIT Registration by Login");
            }
        }

        /// <summary>
        /// Регистрация через почту
        /// </summary>
        /// <param name="userDto">Информация о пользователе</param>
        [HttpPost("registration-by-email")]
        public async Task<IActionResult> RegistrationByEmailAsync([FromBody] UserDto userDto)
        {
            userDto.Login = userDto.Email;

            DetailsDto detailsDto;

            if (this._registrationServiceFactory.IsValidUserData(userDto, out detailsDto))
            {
                Tuple<bool, string> isAccountOccupied = await this._registrationServiceFactory.IsAccountOccupiedByAnotherUser(userDto);

                if (isAccountOccupied.Item1)
                    return BadRequest(new DetailsDto
                    {
                        Details = this._objectTitlesService.GetTitleByTag(isAccountOccupied.Item2,
                        Guid.Parse(ObjectTitles.Constants.RussianLanguageGuid))
                    });

                IRegistrationService service = this._registrationServiceFactory.GetRegistrationService(HelperRegistrationService.RegistrationTypesBy.Email);

                var scheme = this.Request.Scheme;

                var host = this.Request.Host.Value;

                var fullUrl = $"{scheme}://{host}";

                if (await service.TryRegistrationUserAsync(userDto, fullUrl))
                {
                    return Ok(new DetailsDto()
                    {
                        Details = String.Format(this._objectTitlesService.GetTitleByTag(ObjectTitles.Constants.MessageHasBeenSent,
                                                        Guid.Parse(ObjectTitles.Constants.RussianLanguageGuid)), userDto.Login)
                    });
                }
                else
                {
                    detailsDto = new DetailsDto()
                    { 
                        Details = this._objectTitlesService.GetTitleByTag(ObjectTitles.Constants.ErrorOccurredWhileSendingMessage,
                                    Guid.Parse(ObjectTitles.Constants.RussianLanguageGuid)) 
                    };
                }
            }

            return BadRequest(detailsDto);
        }

        /// <summary>
        /// Ответ на сообщение - ручной ввод
        /// </summary>
        /// <param name="responseAtRegistrationDto">Dto ответа содержащий потенциально отправленный код</param>
        [HttpPost("response-at-registration")]
        public async Task<IActionResult> ResponseAtRegistration([FromBody] ResponseAtRegistrationDto responseAtRegistrationDto)
        {
            if (this._registrationServiceFactory.IsEteredCodeCorrect(responseAtRegistrationDto.Code, responseAtRegistrationDto.Login))
            {
                UserDto userDto = this._registrationServiceFactory.GetRegisteredUser(responseAtRegistrationDto.Login);

                return await this.RegistrationAsync(userDto);
            }

            return BadRequest(new DetailsDto()
            {
                Details = this._objectTitlesService.GetTitleByTag(ObjectTitles.Constants.EnteredCodeIsIncorrectOrExpired,
                                    Guid.Parse(ObjectTitles.Constants.RussianLanguageGuid))
            });
        }

        /// <summary>
        /// Ответ на сообщение - переход по ссылке
        /// </summary>
        /// <param name="responseAtRegistrationDto">Dto ответа содержащий потенциально отправленный код</param>
        [HttpGet("response-at-registration-link")]
        public async Task<IActionResult> ResponseAtRegistrationLink([FromQuery] ResponseAtRegistrationDto responseAtRegistrationDto)
        {
            if (this._registrationServiceFactory.IsEteredCodeCorrect(responseAtRegistrationDto.Code, responseAtRegistrationDto.Login))
            {
                UserDto userDto = this._registrationServiceFactory.GetRegisteredUser(responseAtRegistrationDto.Login);

                return await this.RegistrationAsync(userDto, true);
            }

            return BadRequest(new DetailsDto()
            {
                Details = this._objectTitlesService.GetTitleByTag(ObjectTitles.Constants.EnteredCodeIsIncorrectOrExpired,
                                    Guid.Parse(ObjectTitles.Constants.RussianLanguageGuid))
            });
        }

        private async Task<IActionResult> RegistrationAsync(UserDto userDto, bool isLink = false)
        {
            try
            {
                ClaimsIdentity userClaimsIdentity = await this._registrationServiceFactory.RegistrationUserAsync(userDto);

                var jwt = _jwtService.GenerateToken(userClaimsIdentity.Claims);

                var tokenString = _jwtService.GetTokenString(jwt);

                Guid userId = this._jwtService.GetUserId(tokenString);

                AppUserDto appUser = await this._appUserService.GetAppUserDtoAsync(this._jwtService.GetUserId(tokenString), this.HttpContext.Connection.RemoteIpAddress?.ToString());

                if (appUser == null)
                    return BadRequest(new DetailsDto { Details = this._objectTitlesService.GetTitleByTag(ObjectTitles.Constants.ErrorText, Guid.Parse(ObjectTitles.Constants.RussianLanguageGuid)) });

                HttpContext.AddToken(tokenString);

                HttpContext.AddSecurityHeaders();

                HttpContext.AddCookieKey(HttpContextExtension.YOURWHEEL_CULTURE, appUser.CurrentLanguageId.ToString());

                return isLink ? RedirectToRoute("get-user-info", new { userId = userId }) : NoContent();
            }
            finally
            {
                this._registrationServiceFactory.RemoveRegistrationAttempt(userDto.Login);
            }
        }
    }
}
