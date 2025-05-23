﻿
namespace YourWheel.Domain.Services
{
    using YourWheel.Domain.Dto;

    /// <summary>
    /// Интерфейс для работы с личным кабинетом пользователя
    /// </summary>
    public interface IUserInfoService
    {
        /// <summary>
        /// Получить пользовательские данные
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserDto> GetUserInfo(Guid userId);

        /// <summary>
        /// Получить пользователя по Id
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Пользователь</returns>
        Task<UserDto> GetUserByIdAsync(Guid userId);
    }
}
