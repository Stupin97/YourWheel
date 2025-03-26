namespace YourWheel.Host.Services.Registration
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using YourWheel.Domain.Dto;

    public interface IRegistrationService
    {
        /// <summary>
        /// Проверка на занятость аккаунта другим пользователем
        /// </summary>
        /// <param name="login">телефон/логин/email</param>
        /// <returns>Занят ли аккаунт другим пользователем</returns>
        ValueTask<Tuple<bool, string>> IsAccountOccupiedByAnotherUser(string login);

        /// <summary>
        /// Попытка зарегистрировать пользователя
        /// </summary>
        /// <param name="userDto">Информация о пользователе</param>
        Task<ClaimsIdentity> RegistrationUserAsync(UserDto userDto);

        /// <summary>
        /// Удаление активной регистрации
        /// </summary>
        /// <param name="login">телефон/логин/email</param>
        void RemoveRegistrationAttempt(string login);

        /// <summary>
        /// Регистрация через логин и пароль
        /// </summary>
        /// <param name="userDto">Информация о пользователе</param>
        Task<ClaimsIdentity> RegistrationByLoginAsync(UserDto userDto);

        /// <summary>
        /// Регистрация через телефон (https://smsc.ru/)
        /// </summary>
        /// <param name="userDto">Информация о пользователе</param>
        Task<ClaimsIdentity> RegistrationByPhoneAsync(UserDto userDto);

        /// <summary>
        /// Регистрация через почту
        /// </summary>
        /// <param name="userDto">Информация о пользователе</param>
        Task<ClaimsIdentity> RegistrationByEmailAsync(UserDto userDto);
    }
}
