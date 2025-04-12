namespace YourWheel.Host.Services.Registration
{
    using Microsoft.EntityFrameworkCore;
    using Npgsql;
    using Sprache;
    using System.Security.Claims;
    using YourWheel.Domain;
    using YourWheel.Domain.Dto;
    using YourWheel.Domain.Models;
    using YourWheel.Host.Helpers;

    public class RegistrationServiceFactory : IRegistrationServiceFactory
    {
        private readonly YourWheelDbContext _context;

        private readonly IEnumerable<IRegistrationService> _registrationServices;

        private readonly IHelperRegistrationService _helperRegistrationService;

        private readonly IObjectTitlesService _objectTitlesService;

        public RegistrationServiceFactory(IEnumerable<IRegistrationService> registrationServices, IHelperRegistrationService helperRegistrationService,
                                            IObjectTitlesService objectTitlesService, YourWheelDbContext context)
        {
            this._registrationServices = registrationServices;

            this._helperRegistrationService = helperRegistrationService;

            this._context = context;

            this._objectTitlesService = objectTitlesService;
        }

        public IRegistrationService GetRegistrationService(HelperRegistrationService.RegistrationTypesBy registrationTypesBy)
        {
            IRegistrationService registrationService = this._registrationServices.FirstOrDefault(c => c.RegistrationTypesBy == registrationTypesBy);

            if (registrationService == null)
                throw new ArgumentException($"Отсутствует сервис регистрации для {registrationTypesBy}");

            return registrationService;
        }

        public async ValueTask<Tuple<bool, string>> IsAccountOccupiedByAnotherUser(UserDto userDto)
        {
            bool result = this._helperRegistrationService.TryRegistration(userDto);

            if (!result)
                return Tuple.Create(true, ObjectTitles.Constants.UserIsAlreadyRegisteringAtTheMomentText);

            string login = userDto.Login;

            result = await this._context.Users.AnyAsync(c => c.Login == login || c.Email == login || c.Email == login);

            return Tuple.Create(result, result ? ObjectTitles.Constants.UserAlreadyExistsText : ObjectTitles.Constants.OkText);
        }

        public void RemoveRegistrationAttempt(string login)
        {
            this._helperRegistrationService.RemoveRegistrationAttempt(login);
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
                            SecretHasher.Hash(userDto.Password), userDto.Phone, userDto.Email
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

        public bool IsValidUserData(UserDto userDto, out DetailsDto detailsDto)
        {
            bool isValidUserData = !(String.IsNullOrEmpty(userDto.Login) && String.IsNullOrEmpty(userDto.Password));

            if (!isValidUserData) 
            { 
                detailsDto = new DetailsDto()
                {
                    Details = this._objectTitlesService.GetTitleByTag(ObjectTitles.Constants.EnterRequiredFields,
                                                Guid.Parse(ObjectTitles.Constants.RussianLanguageGuid))
                };

                return false;
            }

            // Вообще минимальную валидацию производить на клиенте match и прочее
            isValidUserData = userDto.Password.Count() > 5;

            detailsDto = new DetailsDto()
            {
                Details = this._objectTitlesService.GetTitleByTag(isValidUserData ? ObjectTitles.Constants.OkText : ObjectTitles.Constants.IncorrectPassword,
                                                                Guid.Parse(ObjectTitles.Constants.RussianLanguageGuid))
            };

            return isValidUserData;
        }

        public bool IsEteredCodeCorrect(string enteredCode, string login)
        {
            return this._helperRegistrationService.IsEteredCodeCorrect(enteredCode, login);
        }

        public UserDto GetRegisteredUser(string login)
        {
            return this._helperRegistrationService.GetRegisteredUser(login);
        }
    }
}
