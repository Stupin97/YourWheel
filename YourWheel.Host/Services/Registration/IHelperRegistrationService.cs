using YourWheel.Domain.Dto;

namespace YourWheel.Host.Services.Registration
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
        /// <param name="userDto">Активные регистируемый пользователь</param>
        /// <returns>Одновременно регистируется пользователь - теущая попытка не первая</returns>
        bool TryRegistration(UserDto userDto);

        /// <summary>
        /// Корректность введеного кода
        /// </summary>
        /// <param name="enteredCode">Введенный код</param>
        /// <param name="login">Логин авторизовывающегося пользователя</param>
        bool IsEteredCodeCorrect(string enteredCode, string login);

        /// <summary>
        /// Получить регистрируемого пользователя
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns>Введенныя информация пользователя</returns>
        public UserDto GetRegisteredUser(string login);
    }
}
