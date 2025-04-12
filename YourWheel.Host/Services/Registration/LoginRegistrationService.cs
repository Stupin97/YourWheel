namespace YourWheel.Host.Services.Registration
{
    using YourWheel.Domain.Dto;

    public class LoginRegistrationService : IRegistrationService
    {
        public HelperRegistrationService.RegistrationTypesBy RegistrationTypesBy => HelperRegistrationService.RegistrationTypesBy.Login;

        public async ValueTask<bool> TryRegistrationUserAsync(UserDto userDto)
        {
            // Логика для регистрации пуста, поскольку данные идут сразу на прямую на сохранение
            return true;
        }
    }
}
