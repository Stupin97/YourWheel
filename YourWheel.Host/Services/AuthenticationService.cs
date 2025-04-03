namespace YourWheel.Host.Services
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using YourWheel.Domain;
    using YourWheel.Host.Helpers;

    public class AuthenticationService : IAuthenticationService
    {
        private readonly YourWheelDbContext _context;

        public AuthenticationService(YourWheelDbContext context)
        {
            this._context = context;
        }

        public async Task<ClaimsIdentity> GetIdentityAsync(string username, string password)
        {
            var user = await this._context.Users.Include(c => c.Role).FirstOrDefaultAsync(x => x.Login == username);

            if (user != null && SecretHasher.Verify(password, user.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(Constants.UserIdClaimType, user.UserId.ToString()),
                    new Claim(Constants.UserRoleClaimType, user.Role.Name)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "Token", Constants.UserIdClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            }

            return null;
        }

        public async Task<ClaimsIdentity> GetIdentityAsync(Guid userId)
        {
            var user = await this._context.Users.Include(c => c.Role).FirstOrDefaultAsync(c => c.UserId == userId);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(Constants.UserIdClaimType, user.UserId.ToString()),
                    new Claim(Constants.UserRoleClaimType, user.Role.Name)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "Token", Constants.UserIdClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            }

            return null;
        }
    }
}
