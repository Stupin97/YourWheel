namespace YourWheel.Host.Services.Registration
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using YourWheel.Domain.Dto;

    public interface IRegistrationService
    {
        /// <summary>
        /// Попытка зарегистрировать пользователя
        /// </summary>
        /// <param name="userDto">Информация о пользователе</param>
        ValueTask<bool> TryRegistrationUserAsync(UserDto userDto);

        /// <summary>
        /// Тип регистрации
        /// </summary>
        HelperRegistrationService.RegistrationTypesBy RegistrationTypesBy { get; }
    }
}
