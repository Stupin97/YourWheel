using System.Runtime.Intrinsics.Arm;

namespace YourWheel.Host.Services
{
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
            // Беру из базы пользователя,
            // + если дойдут руки сразу хранить пароли в БД в зашифрованном виде и после расшифровывать и сравнивать

            return null;
        }

        public async Task<ClaimsIdentity> GetIdentityAsync(Guid userId)
        {
            // Аналогично - проверял на тестовых значениях

            return null;
        }
    }
}
