namespace YourWheel.Domain.Services
{
    using YourWheel.Domain.Dto;
    using YourWheel.Domain.Dto.Orders;

    /// <summary>
    /// Сервис для работы с заказами
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Создать активный заказ у пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="orderId">Идентификатор создаваемого заказа</param>
        Task CreateOrderIfNeedAsync(Guid userId, Guid orderId);

        /// <summary>
        /// Есть ли уже активный заказ у пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Есть ли уже активный заказ у пользователя</returns>
        Task<bool> HasDraftOrderAsync(Guid userId);

        /// <summary>
        /// Получить заказ для пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="orderId">Идентификатор заказа</param>
        /// <returns>Заказ для пользователя</returns>
        Task<OrderDto> GetShortOrderAsync(Guid userId, Guid orderId);

        /// <summary>
        /// Получить все заказы пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Все заказы пользователя</returns>
        Task<List<OrderDto>> GetAllOrdersAsync(Guid userId);

        /// <summary>
        /// Получить все заказы пользователя по статусу заказа
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="statusId">Статус заказа</param>
        /// <returns>Все заказы пользователя по статусу заказа</returns>
        Task<List<OrderDto>> GetAllOrdersByStatusAsync(Guid userId, Guid statusId);

        /// <summary>
        /// Получить заказы пользователя 
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="count">Количество возращаемых элементов</param>
        /// <param name="offset">Страница</param>
        /// <returns>Заказы пользователя</returns>
        Task<PaginationDto<OrderDto>> GetOrdersAsync(Guid userId, int count, int offset);

        /// <summary>
        /// Создать работу для заказа
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="workDto">Dto "скелета" работы</param>
        /// <returns>Dto работы</returns>
        Task AddWorkToOrderAsync(Guid userId, CreatedWorkByUserDto workDto);

        /// <summary>
        /// Получить работу для заказа
        /// </summary>
        /// <param name="workId">Идентификатор работы</param>
        /// <returns>Dto работы</returns>
        Task<WorkDto> GetWorkAsync(Guid workId);

        /// <summary>
        /// Удалить работу из заказа
        /// </summary>
        /// <param name="orderId">Идентификатор заказа</param>
        /// <param name="workId">Удаляемый идентификатор работы из заказа</param>
        Task RemoveWorkFromOrderAsync(Guid orderId, Guid workId);

        /// <summary>
        /// Получить все работы для заказа
        /// </summary>
        /// <param name="orderId">Идентификатор заказа</param>
        /// <returns>Все работы для заказа</returns>
        Task<List<WorkDto>> GetAllWorksForOrderAsync(Guid orderId);

        /// <summary>
        /// Подтвердить заказ
        /// </summary>
        /// <returns></returns>
        Task ConfirmOrderAsync();

        /// <summary>
        /// Отменить заказ
        /// </summary>
        /// <param name="orderId">Идентификатор заказа</param>
        /// <returns></returns>
        Task CancelOrderAsync(Guid orderId);
    }
}