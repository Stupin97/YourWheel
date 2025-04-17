namespace YourWheel.Domain.Services
{
    using Microsoft.EntityFrameworkCore;
    using System.Linq.Expressions;
    using YourWheel.Domain.Dto;
    using YourWheel.Domain.Models;

    public class UserInfoService : IUserInfoService
    {
        private readonly YourWheelDbContext _context;

        private readonly Expression<Func<User, UserDto>> _userDto = user =>
            new UserDto()
            {
                UserId = user.UserId,
                Login = user.Login,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Phone = user.Phone
            };

        public UserInfoService(YourWheelDbContext context)
        {
            this._context = context;
        }

        public async Task<UserDto> GetUserInfo(Guid userId)
        {
            var user = await this._context.Users
                .Where(c => c.UserId == userId)
                .Select(_userDto)
                .FirstOrDefaultAsync();

            if (user == null) throw new KeyNotFoundException($"Пользователь '{userId}' не найден");

            return user;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users
                .Where(c => c.UserId == userId)
                .Select(_userDto)
                .FirstOrDefaultAsync();
        }
    }
}
