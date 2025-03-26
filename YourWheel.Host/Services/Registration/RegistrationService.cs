namespace YourWheel.Host.Services.Registration
{
    using Microsoft.EntityFrameworkCore;
    using Npgsql;
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using YourWheel.Domain;
    using YourWheel.Domain.Dto;
    using YourWheel.Domain.Models;

    public class RegistrationService : IRegistrationService
    {
        private readonly YourWheelDbContext _context;

        private readonly IHelperRegistrationService _helperRegistrationService;

        public RegistrationService(YourWheelDbContext context, IHelperRegistrationService helperRegistrationService) 
        {
            this._helperRegistrationService = helperRegistrationService;

            this._context = context;
        }

        public async ValueTask<Tuple<bool, string>> IsAccountOccupiedByAnotherUser(string login)
        {
            bool result = this._helperRegistrationService.TryRegistration(login);

            if (!result)
                return Tuple.Create(true, ObjectTitles.Constants.UserIsAlreadyRegisteringAtTheMomentText);

            result = await this._context.Users.AnyAsync(c => c.Login == login || c.Email == login || c.Email == login);

            return Tuple.Create(result, result ? ObjectTitles.Constants.UserAlreadyExistsText : ObjectTitles.Constants.OkText);
        }

        public void RemoveRegistrationAttempt(string login)
        {
            this._helperRegistrationService.RemoveRegistrationAttempt(login);
        }

        public async Task<ClaimsIdentity> RegistrationByLoginAsync(UserDto userDto)
        {
            // При регистрации через логин Dto не меняем

            return await this.RegistrationUserAsync(userDto);
        }

        public async Task<ClaimsIdentity> RegistrationByEmailAsync(UserDto userDto)
        {
            userDto.Login = userDto.Email;

            return await this.RegistrationUserAsync(userDto);
        }

        public async Task<ClaimsIdentity> RegistrationByPhoneAsync(UserDto userDto)
        {
            userDto.Login = userDto.Phone;

            return await this.RegistrationUserAsync(userDto);
        }

        public async Task<ClaimsIdentity> RegistrationUserAsync(UserDto userDto)
        {
            Exception lastException = null;

            // 3 попытки создания
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    User user = await this._context.SetUserInstance
                        (
                            userDto.Name, userDto.Surname, userDto.Login,
                            userDto.Password, userDto.Phone, userDto.Email
                        ).FirstOrDefaultAsync();

                    if (user == null)
                        throw new ArgumentException($"При регистрации пользователя произошла ошибка");

                    Role role = await this._context.Roles.FirstOrDefaultAsync(c => c.RoleId == user.RoleId);

                    if (role == null)
                        throw new ArgumentException($"Получение роли для только что зарегистированного пользователя: {user.UserId} прошло с ошибкой");

                    var claims = new List<Claim>
                            {
                                new Claim(Constants.UserIdClaimType, user.UserId.ToString()),
                                new Claim(Constants.UserRoleClaimType, role.Name)
                            };

                    return new ClaimsIdentity(claims, "Token", Constants.UserIdClaimType, Constants.UserRoleClaimType);
                }
                catch (NpgsqlException npgsqlException)
                {
                    // Продумать обработку исключений
                    // 08001 - потерянное соединение с сервером
                    // 42804 - ошибка подключения SQL
                    // 42703 - неопределенный столбец
                }
                catch (TimeoutException ex)
                {
                    // Добавить
                }
                catch (Exception exeption)
                {
                    lastException = exeption;
                }
            }

            throw lastException;
        }
    }
}
