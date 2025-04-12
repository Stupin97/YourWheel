using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YourWheel.Domain.Dto;
using YourWheel.Domain.Services;

namespace YourWheel.Host.Controllers
{
    /// <summary>
    /// Контроллер для информации об аккаунте пользователя
    /// </summary>
    [Authorize]
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserInfoService _userInfoService;

        public AccountController(IUserInfoService userInfoService) 
        {
            this._userInfoService = userInfoService;
        }

        /// <summary>
        /// Поулчить данные пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Данные пользователя</returns>
        [HttpGet("get-user-info", Name = "get-user-info")]

        public async Task<IActionResult> GetUserInfo(Guid userId)
        {
            UserDto userDto = await this._userInfoService.GetUserInfo(userId);

            return Ok(userDto); 
        }
    }
}
