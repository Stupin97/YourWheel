
namespace YourWheel.Host.AuthorizationPolitics
{
    using Microsoft.AspNetCore.Authorization;

    /// <summary>
    ///   Требование для авторизации с проверкой на включенную аутентификацию
    /// </summary>
    public class IsAuthenticationEnabledRequirement : IAuthorizationRequirement
    {
    }
}
