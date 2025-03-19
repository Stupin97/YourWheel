using Microsoft.AspNetCore.Authorization;
using YourWheel.Host.AuthorizationPolitics;
using YourWheel.Host.Services;

namespace YourWheel.Host.AuthorizationPolitics
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using YourWheel.Host.Services;

    /// <summary>
    ///   Обработчик требования для авторизации с проверкой на роль Admin
    /// </summary>
    public class IsAdminRequirementHandler : AuthorizationHandler<IsAdminRequirement>
    {
        private readonly IAuthenticationSettings _authSettings;

        public IsAdminRequirementHandler(IAuthenticationSettings authSettings)
        {
            _authSettings = authSettings;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        IsAdminRequirement requirement)
        {
            if (!_authSettings.IsAuthenticationEnabled
                || (context.User.Identities.Any(x => x.IsAuthenticated) && context.User.Claims.Any(c => c.Type == "Role" && c.Value == "Admin")))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}