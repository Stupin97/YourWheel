namespace YourWheel.Host.AuthorizationPolitics
{
    using Microsoft.AspNetCore.Authorization;

    /// <summary>
    /// Требование для авторизации с проверкой на роль Admin
    /// </summary>
    public class IsAdminRequirement : IAuthorizationRequirement
    {
    }
}
