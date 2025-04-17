namespace YourWheel.Host.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    using YourWheel.Domain.Dto.Orders;
    using YourWheel.Domain.Dto.Services;
    using YourWheel.Domain.Services;

    /// <summary>
    /// Контроллер для "ассортимента"
    /// </summary>
    [Route("api/service")]
    [ApiController]
    [Authorize]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            this._serviceService = serviceService;
        }

        /// <summary>
        /// Получить все материалы кож
        /// </summary>
        /// <returns>Dсе материалы кож</returns>
        [HttpGet("get-all-materials")]
        public async Task<ActionResult<List<MaterialDto>>> GetAllMaterialsAsync()
        {
            var result = await this._serviceService.GetAllMaterialAsync();

            return Ok(result);
        }

        /// <summary>
        /// Получить все материалы конерктных цветов и изготовителей
        /// </summary>
        /// <param name="colorIds">Список фильтруемых цветов</param>
        /// <param name="fabricatorIds">Список фильтруемых изготовителей</param>
        /// <returns>Все материалы конерктных цветов и изготовителей </returns>
        [HttpGet("get-all-material-by-colors-and-fabricators")]
        public async Task<ActionResult<List<MaterialDto>>> GetAllMaterialByColorsAndFabricatorsAsync([FromQuery] Guid[] colorIds, [FromQuery] Guid[] fabricatorIds)
        {
            var result = await this._serviceService.GetAllMaterialByColorsAndFabricatorsAsync(colorIds, fabricatorIds);

            return Ok(result);
        }

        /// <summary>
        /// Получить все типы работы
        /// </summary>
        /// <returns>Все типы работы</returns>
        [HttpGet("get-type-works")]
        public async Task<ActionResult<List<TypeWorkDto>>> GetTypeWorksAsync()
        {
            var result = await this._serviceService.GetTypeWorksAsync();

            return Ok(result);
        }
    }
}
