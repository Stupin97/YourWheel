using System.Security.Claims;
using YourWheel.Domain.Dto;

namespace YourWheel.Host.Services.Registration
{
    /// <summary>
    /// Фабрика для регистрации
    /// </summary>
    public interface IRegistrationServiceFactory
    {
        /// <summary>
        /// Получить тип регистрации
        /// </summary>
        /// <param name="registrationTypesBy"></param>
        /// <returns></returns>
        IRegistrationService GetRegistrationService(HelperRegistrationService.RegistrationTypesBy registrationTypesBy);

        /// <summary>
        /// Являются ли действительными пользовательские введенные данные
        /// </summary>
        /// <param name="userDto">Вводимые данные пользователем</param>
        /// <param name="detailsDto">Детали валидации</param>
        /// <returns>Валидны ли значения</returns>
        bool IsValidUserData(UserDto userDto, out DetailsDto detailsDto);

        /// <summary>
        /// Проверка на занятость аккаунта другим пользователем
        /// </summary>
        /// <param name="userDto">телефон/логин/email + данные</param>
        /// <returns>Занят ли аккаунт другим пользователем</returns>
        ValueTask<Tuple<bool, string>> IsAccountOccupiedByAnotherUser(UserDto userDto);

        /// <summary>
        /// Попытка зарегистрировать пользователя
        /// </summary>
        /// <param name="userDto">Информация о пользователе</param>
        Task<ClaimsIdentity> RegistrationUserAsync(UserDto userDto);

        /// <summary>
        /// Удаление активной регистрации
        /// </summary>
        /// <param name="login">телефон/логин/email</param>
        void RemoveRegistrationAttempt(string login);

        /// <summary>
        /// Корректность введеного кода
        /// </summary>
        /// <param name="enteredCode">Введенный код</param>
        /// <param name="login">Логин авторизовывающегося пользователя</param>
        bool IsEteredCodeCorrect(string enteredCode, string login);

        /// <summary>
        /// Получить регистрируемого пользователя
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns>Введенныя информация пользователя</returns>
        public UserDto GetRegisteredUser(string login);
    }
}
