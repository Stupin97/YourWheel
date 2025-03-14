namespace YourWheel.Host.Services
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;

    /// <summary>
    /// Сервис для работы с JWT
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        ///   Генерация JwtSecurityToken
        /// </summary>
        /// <param name="claims">Claims для токена</param>
        /// <returns>JwtSecurityToken с заданными Claims</returns>
        JwtSecurityToken GenerateToken(IEnumerable<Claim> claims);

        /// <summary>
        ///   Получение срока годности токена
        /// </summary>
        /// <param name="tokenString">Строка с токеном</param>
        /// <returns>DateTime срока годности</returns>
        DateTime GetTokenExpiryDateTime(string tokenString);

        /// <summary>
        ///   Сериализация jwt в строку
        /// </summary>
        /// <param name="token">JwtSecurityToken</param>
        /// <returns>Строка с токеном</returns>
        string GetTokenString(JwtSecurityToken token);

        /// <summary>
        ///   Получение Id пользователя из токена
        /// </summary>
        /// <param name="tokenString">Строка с токеном</param>
        /// <returns>Id пользователя</returns>
        Guid GetUserId(string tokenString);

        /// <summary>
        ///   Валидация строки с токеном
        /// </summary>
        /// <param name="tokenString">Строка с токеном</param>
        /// <returns>Валидный ли токен</returns>
        bool IsTokenStringValid(string tokenString);
    }
}
