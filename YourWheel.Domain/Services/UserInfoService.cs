namespace YourWheel.Domain.Services
{
    using Microsoft.EntityFrameworkCore;
    using YourWheel.Domain.Dto;
    public class UserInfoService : IUserInfoService
    {
        private readonly YourWheelDbContext _context;

        public UserInfoService(YourWheelDbContext context)
        {
            this._context = context;
        }

        public async Task<UserDto> GetUserInfo(Guid userId)
        {
            var user = await this._context.Users
                .Where(c => c.UserId == userId)
                .Select(c => new UserDto()
                {
                    UserId = c.UserId,
                    Login = c.Login,
                    Name = c.Name,
                    Surname = c.Surname,
                    Email = c.Email,
                    Phone = c.Phone
                }).FirstOrDefaultAsync();

            if (user == null) throw new KeyNotFoundException($"Пользователь '{userId}' не найден");

            return user;
        }
    }
}
