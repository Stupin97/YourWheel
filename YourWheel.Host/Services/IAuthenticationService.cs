namespace YourWheel.Host.Services
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    /// <summary>
    ///   Сервис для аутентификации пользователей
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        ///   Получение ClaimsIdentity пользователя с верификацией пароля
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <returns>ClaimsIdentity пользователя</returns>
        Task<ClaimsIdentity> GetIdentityAsync(string username, string password);

        /// <summary>
        ///   Получение ClaimsIdentity пользователя без верификации пароля
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <returns>ClaimsIdentity пользователя</returns>
        Task<ClaimsIdentity> GetIdentityAsync(Guid userId);
    }
}
