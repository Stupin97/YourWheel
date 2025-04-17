using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourWheel.Host.Services;

namespace YourWheel.Host.Controllers
{
    /// <summary>
    /// Конроллер для работы администратора
    /// </summary>
    [Route("api/admin")]
    [Authorize(Policy = "Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IObjectTitlesService _objectTitlesServicel;

        public AdminController(IObjectTitlesService objectTitlesService)
        {
            this._objectTitlesServicel = objectTitlesService;
        }
    }
}
