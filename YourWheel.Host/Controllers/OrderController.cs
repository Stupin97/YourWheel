namespace YourWheel.Host.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using YourWheel.Domain.Dto;
    using YourWheel.Domain.Dto.Orders;
    using YourWheel.Domain.Dto.Services;
    using YourWheel.Domain.Services;
    using YourWheel.Host.Extensions;
    using YourWheel.Host.Services;

    /// <summary>
    /// Контроллер для работы с заказами + работами
    /// </summary>
    [Route("api/order")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        private readonly IUserInfoService _userInfoService;

        private readonly IObjectTitlesService _objectTitlesService;

        private readonly IServiceService _serviceService;

        public OrderController(IOrderService orderService, IUserInfoService userInfoService,
            IObjectTitlesService objectTitlesService, IServiceService serviceService)
        {
            this._orderService = orderService;

            this._userInfoService = userInfoService;

            this._objectTitlesService = objectTitlesService;

            this._serviceService = serviceService;
        }

        /// <summary>
        /// Добавить работу в заказ
        /// </summary>
        [HttpPost("add-work-to-order")]
        public async Task<ActionResult> AddWorkToOrderAsync([FromBody] CreatedWorkByUserDto workDto)
        {
            var user = await this.GetUserInfo();

            if (user == null) return NotFound(new DetailsDto { Details = "Пользователь не найден" }); // через _objectTitlesService сделать !

            await this._orderService.AddWorkToOrderAsync(user.UserId, workDto);

            return NoContent();
        }

        /// <summary>
        /// Получить все материалы кож
        /// </summary>
        [HttpGet("get-all-material")]
        public async Task<ActionResult<List<MaterialDto>>> GetAllMaterialAsync()
        {
            var materials = await this._serviceService.GetAllMaterialAsync();

            if (materials == null) NotFound(new DetailsDto { Details = "Материалы не найдены" });

            return Ok(materials);
        }

        /// <summary>
        /// Получить все типы работы
        /// </summary>
        /// <returns>Все типы работы</returns>
        [HttpGet("get-types-works")]
        public async Task<ActionResult<List<TypeWorkDto>>> GetTypeWorksAsync()
        {
            var typeWorks = await this._serviceService.GetTypeWorksAsync();

            if (typeWorks == null) NotFound(new DetailsDto { Details = "Типы работы не найдены" });

            return Ok(typeWorks);
        }

        /// <summary>
        /// Получить заказ для пользователя
        /// </summary>
        /// <param name="orderId">Идентификатор заказа</param>
        /// <returns>Заказ для пользователя</returns>
        [HttpGet("get-short-order/{orderId:guid}")]
        public async Task<ActionResult<OrderDto>> GetShortOrderAsync(Guid orderId)
        {
            var user = await this.GetUserInfo();

            if (user == null) return NotFound(new DetailsDto { Details = "Пользователь не найден" });

            var order = await this._orderService.GetShortOrderAsync(user.UserId, orderId);

            if (order == null) NotFound(new DetailsDto { Details = "Заказ не найден" });

            return Ok(order);
        }

        /// <summary>
        /// Получить все заказы пользователя
        /// </summary>
        /// <returns>Все заказы пользователя</returns>
        [HttpPost("get-all-orders")]
        public async Task<ActionResult<List<OrderDto>>> GetAllOrdersAsync()
        {
            var user = await this.GetUserInfo();

            if (user == null) return NotFound(new DetailsDto { Details = "Пользователь не найден" });

            var orders = await this._orderService.GetAllOrdersAsync(user.UserId);

            if (orders == null) NotFound(new DetailsDto { Details = "Заказы не найдены" });

            return Ok(orders);
        }

        /// <summary>
        /// Получить все заказы пользователя по статусу заказа
        /// </summary>
        /// <param name="statusId">Статус заказа</param>
        /// <returns>Все заказы пользователя по статусу заказа</returns>
        [HttpGet("get-all-orders-by-status/{statusId:guid}")]
        public async Task<ActionResult<List<OrderDto>>> GetAllOrdersByStatusAsync(Guid statusId)
        {
            var user = await this.GetUserInfo();

            if (user == null) return NotFound(new DetailsDto { Details = "Пользователь не найден" });

            var orders = await this._orderService.GetAllOrdersByStatusAsync(user.UserId, statusId);

            if (orders == null) NotFound(new DetailsDto { Details = "Заказы не найдены" });

            return Ok(orders);
        }

        /// <summary>
        /// Получить заказы пользователя 
        /// </summary>
        /// <param name="count">Количество возращаемых элементов</param>
        /// <param name="offset">Страница</param>
        /// <returns>Заказы пользователя</returns>
        [HttpGet("get-orders")]
        public async Task<ActionResult<PaginationDto<OrderDto>>> GetOrdersAsync([FromQuery] int count, [FromQuery] int offset)
        {
            var user = await this.GetUserInfo();

            if (user == null) return NotFound(new DetailsDto { Details = "Пользователь не найден" });

            var orders = await this._orderService.GetOrdersAsync(user.UserId, count, offset);

            if (orders == null) NotFound(new DetailsDto { Details = "Заказы не найдены" });

            return Ok(orders);
        }

        /// <summary>
        /// Получить работу для заказа
        /// </summary>
        /// <param name="workId">Идентификатор работы</param>
        /// <returns>Dto работы</returns>
        [HttpGet("get-work/{workId:guid}")]
        public async Task<ActionResult<WorkDto>> GetWorkAsync(Guid workId)
        {
            var user = await this.GetUserInfo();

            if (user == null) return NotFound(new DetailsDto { Details = "Пользователь не найден" });

            var work = await this._orderService.GetWorkAsync(workId);

            if (work == null) NotFound(new DetailsDto { Details = "Работа не найдена" });

            return Ok(work);
        }

        /// <summary>
        /// Удалить работу из заказа
        /// </summary>
        /// <param name="orderId">Идентификатор заказа</param>
        /// <param name="workId">Удаляемый идентификатор работы из заказа</param>
        [HttpPost("{orderId:guid}/remove-work-from-order/{workId:guid}")]
        public async Task<ActionResult> RemoveWorkFromOrderAsync(Guid orderId, Guid workId)
        {
            var user = await this.GetUserInfo();

            if (user == null) return NotFound(new DetailsDto { Details = "Пользователь не найден" });

            await this._orderService.RemoveWorkFromOrderAsync(orderId, workId);

            return NoContent();
        }

        /// <summary>
        /// Получить все работы для заказа
        /// </summary>
        /// <param name="orderId">Идентификатор заказа</param>
        /// <returns>Все работы для заказа</returns>
        [HttpGet("get-all-works-for-order/{orderId:guid}")]
        public async Task<ActionResult<List<WorkDto>>> GetAllWorksForOrderAsync(Guid orderId)
        {
            var user = await this.GetUserInfo();

            if (user == null) return NotFound(new DetailsDto { Details = "Пользователь не найден" });

            var work = await this._orderService.GetAllWorksForOrderAsync(orderId);

            if (work == null) NotFound(new DetailsDto { Details = "Работы не найдены" });

            return Ok(work);
        }

        /// <summary>
        /// Подтвердить заказ
        /// </summary>
        [HttpPost("confirm-order")]
        public async Task<ActionResult> ConfirmOrderAsync()
        {
            var user = await this.GetUserInfo();

            if (user == null) return NotFound(new DetailsDto { Details = "Пользователь не найден" });

            await this._orderService.ConfirmOrderAsync();

            return NoContent();
        }

        /// <summary>
        /// Отменить заказ
        /// </summary>
        /// <param name="orderId">Идентификатор заказа</param>
        [HttpPost("cancel-order/{orderId:guid}")]
        public async Task<ActionResult> CancelOrderAsync(Guid orderId)
        {
            var user = await this.GetUserInfo();

            if (user == null) return NotFound(new DetailsDto { Details = "Пользователь не найден" });

            await this._orderService.CancelOrderAsync(orderId);

            return NoContent();
        }

        /// <summary>
        /// Получить UserDto
        /// </summary>
        /// <returns>userDto</returns>
        private async Task<UserDto> GetUserInfo()
        {
            Guid userId = User.Identity.GetUserId();

            return await _userInfoService.GetUserInfo(userId);
        }
    }
}
