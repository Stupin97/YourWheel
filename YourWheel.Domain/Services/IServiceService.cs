namespace YourWheel.Domain.Services
{
    using YourWheel.Domain.Dto;
    using YourWheel.Domain.Dto.Orders;
    using YourWheel.Domain.Dto.Services;

    /// <summary>
    /// Сервис для услуги (что-то вроде прейскуранта и списка услуг)
    /// </summary>
    public interface IServiceService
    {
        /// <summary>
        /// Получить все материалы кож
        /// </summary>
        /// <returns></returns>
        Task<List<MaterialDto>> GetAllMaterialAsync();

        /// <summary>
        /// Получить все материалы конерктных цветов и изготовителей
        /// </summary>
        /// <param name="colorIds">Список фильтруемых цветов</param>
        /// <param name="fabricatorIds">Список фильтруемых изготовителей</param>
        /// <returns>Все материалы конерктных цветов и изготовителей </returns>
        Task<List<MaterialDto>> GetAllMaterialByColorsAndFabricatorsAsync(IEnumerable<Guid> colorIds, IEnumerable<Guid> fabricatorIds);

        /// <summary>
        /// Получить все типы работы
        /// </summary>
        /// <returns>Все типы работы</returns>
        Task<List<TypeWorkDto>> GetTypeWorksAsync();
    }
}