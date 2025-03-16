namespace YourWheel.Host.Services
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using YourWheel.Domain;

    public class AuthenticationService : IAuthenticationService
    {
        private readonly YourWheelDbContext _context;

        public AuthenticationService(YourWheelDbContext context)
        {
            _context = context;
        }

        public async Task<ClaimsIdentity> GetIdentityAsync(string username, string password)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Login == username);

            // + Проверка на пароль !доработать!
            if (client != null && this.VerifyPassword(password, client.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(Constants.UserIdClaimType, client.ClientId.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, "Token", Constants.UserIdClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            }

            return null;
        }

        public async Task<ClaimsIdentity> GetIdentityAsync(Guid userId)
        {
            // Аналогично - проверял на тестовых значениях

            return null;
        }

        private bool VerifyPassword(string enteredPassword, string hashedPassword)
        {
            return true;
        }
    }
}
