using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YourWheel.Host.Services;

namespace YourWheel.Host.Controllers
{
    /// <summary>
    /// Конроллер для работы администратора
    /// </summary>
    [Route("api/admin")]
    [Authorize(Policy = "RequireAdmin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IObjectTitlesService _objectTitlesServicel;

        public AdminController(IObjectTitlesService objectTitlesService)
        {
            this._objectTitlesServicel = objectTitlesService;
        }

        /// <summary>
        /// Временный метод действия
        /// </summary>
        /// <returns></returns>
        [HttpPost("get-count-users")]

        public async Task<ActionResult<int>> GetCountUsersAsync()
        {
            return Ok(4);
        }
    }
}
