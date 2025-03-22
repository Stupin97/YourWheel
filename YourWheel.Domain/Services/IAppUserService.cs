namespace YourWheel.Domain.Services
{
    using YourWheel.Domain.Dto;

    /// <summary>
    /// Сервис для работы с клиентской непубличной информацией
    /// </summary>
    public interface IAppUserService
    {
        /// <summary>
        /// Получение клиентской информации не публичной
        /// </summary>
        /// <param name="userGuid">Id пользователя</param>
        /// <param name="ipAddress">IP пользователя</param>
        /// <returns>Клиентская информация не публичная</returns>
        Task<AppUserDto> GetAppUserDtoAsync(Guid userGuid, string ipAddress);

        /// <summary>
        /// Обновить AppUser после logout
        /// </summary>
        /// <param name="userGuid">Id пользователя</param>
        Task UpdateAppUserAfterLogoutAsync(Guid userGuid);

        /// <summary>
        /// Обновить AppUser после смены языка
        /// </summary>
        /// <param name="userGuid">Id пользователя</param>
        Task UpdateAppUserAfterChangeLanguageAsync(Guid userGuid, Guid languageGuid);

        /// <summary>
        /// Получить текущий язык для пользователя
        /// </summary>
        /// <param name="userGuid">Id пользователя</param>
        /// <returns>Текущий язык пользователя</returns>
        Task<LanguageDto> GetCurrentLanguageForUser(Guid userGuid);

        /// <summary>
        /// Получить язык по идентификатору
        /// </summary>
        /// <param name="languageGuid">Идентификатор языка</param>
        /// <returns>Язык</returns>
        Task<LanguageDto> GetLanguageByGuid(Guid languageGuid);
    }
}
