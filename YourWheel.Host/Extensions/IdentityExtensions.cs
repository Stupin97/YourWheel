namespace YourWheel.Host.Extensions
{
    using System.Security.Claims;
    using System.Security.Principal;

    public static class IdentityExtensions
    {
        /// <summary>
        /// Получить UserId из claims
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static Guid GetUserId(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;

            var claim = claimsIdentity?.FindFirst(Constants.UserIdClaimType);

            return claim == null ? Guid.Empty : Guid.Parse(claim.Value);
        }
    }
}
