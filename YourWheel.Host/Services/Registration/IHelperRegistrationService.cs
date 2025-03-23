﻿namespace YourWheel.Host.Services.Registration
{
    /// <summary>
    /// Hepler для сервиса регистрации
    /// </summary>
    public interface IHelperRegistrationService
    {
        /// <summary>
        /// Разрешена ли регистрация (не происходит ли одновременно регистарция двух одинаковых пользователей)
        /// </summary>
        /// <param name="keyValue">Активные регистируемые "значения" - это может быть как login/телефон/email</param>
        /// <returns>Разрешена ли регистрация</returns>
        bool IsRegistrationAllowed(string keyValue);

        /// <summary>
        /// Удаление активной регистрации
        /// </summary>
        /// <param name="keyValue">Активные регистируемые "значения"</param>
        void RemoveRegistrationAttempt(string keyValue);

        /// <summary>
        /// Попытка регистрации (Добавление активной регистрации в случае успеха)
        /// </summary>
        /// <param name="keyValue">Активные регистируемые "значения"</param>
        /// <returns>Одновременно регистируется пользователь - теущая попытка не первая</returns>
        bool TryRegistration(string keyValue);
    }
}
