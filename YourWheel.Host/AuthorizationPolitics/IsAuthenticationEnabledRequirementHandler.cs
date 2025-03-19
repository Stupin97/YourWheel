namespace YourWheel.Host.AuthorizationPolitics
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using YourWheel.Host.Services;

    /// <summary>
    ///   Обработчик требования для авторизации с проверкой на включенную аутентификацию
    /// </summary>
    public class IsAuthenticationEnabledRequirementHandler :
        AuthorizationHandler<IsAuthenticationEnabledRequirement>
    {
        private readonly IAuthenticationSettings _authSettings;

        public IsAuthenticationEnabledRequirementHandler(IAuthenticationSettings authSettings)
        {
            _authSettings = authSettings;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            IsAuthenticationEnabledRequirement requirement)
        {
            if (!_authSettings.IsAuthenticationEnabled || context.User.Identities.Any(x => x.IsAuthenticated))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
