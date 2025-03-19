namespace YourWheel.Host.Services
{
    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    ///   Интерфейс для настроек аутентификации
    /// </summary>
    public interface IAuthenticationSettings
    {
        /// <summary>
        ///   Проверка на включенную аутентификацию
        /// </summary>
        bool IsAuthenticationEnabled { get; }

        /// <summary>
        ///   Строка с Audience для jwt
        /// </summary>
        string JwtAudience { get; }

        /// <summary>
        ///   Строка с Issuer для jwt
        /// </summary>
        string JwtIssuer { get; }

        /// <summary>
        ///   LifeTime для jwt
        /// </summary>
        int JwtLifeTime { get; }

        /// <summary>
        ///   SymmetricSecurityKey для jwt
        /// </summary>
        SymmetricSecurityKey JwtSymmetricSecurityKey { get; }

        /// <summary>
        ///   TokenValidationParameters для jwt
        /// </summary>
        TokenValidationParameters JwtValidationParameters { get; }
    }
}
