namespace YourWheel.Host.Services.Registration
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using YourWheel.Domain.Dto;
    using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

    public class HelperRegistrationService : IHelperRegistrationService
    {
        /// <summary>
        /// Данные отправленные для регистрации 
        /// </summary>
        private class RegistrationEnteredData
        {
            /// <summary>
            /// Отправленный код
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// Дата отправки кода
            /// </summary>
            public DateTime DateSent { get; set; }

            /// <summary>
            /// Срок годности кода
            /// </summary>
            public static readonly int ExpirationDate = 5;
        }

        public enum RegistrationTypesBy { Login, Email, Phone }

        // (пока без сущности в БД - активные решистрации) 
        private static readonly ConcurrentDictionary<string, Tuple<RegistrationEnteredData, UserDto>> _activeRegistrations = new ConcurrentDictionary<string, Tuple<RegistrationEnteredData, UserDto>>();

        private static Object LockObject = new Object();

        public bool IsRegistrationAllowed(string keyValue)
        {
            return !HelperRegistrationService._activeRegistrations.TryGetValue(keyValue, out var registration);
        }

        public void RemoveRegistrationAttempt(string keyValue)
        {
            HelperRegistrationService._activeRegistrations.TryRemove(keyValue, out _);
        }

        public bool TryRegistration(UserDto userDto)
        {
            lock (HelperRegistrationService.LockObject)
            {
                string login = userDto.Login;

                var expiredCodes = (string key, Tuple<RegistrationEnteredData, UserDto> value) => value.Item1.DateSent.AddMinutes(RegistrationEnteredData.ExpirationDate) < DateTime.Now;

                ICollection<KeyValuePair<string, Tuple<RegistrationEnteredData, UserDto>>> activeRegistrations = HelperRegistrationService._activeRegistrations;

                // Удаление всех регистраций по сроку 
                foreach (var activeRegistration in activeRegistrations)
                {
                    if (expiredCodes(activeRegistration.Key, activeRegistration.Value))
                    {
                        HelperRegistrationService._activeRegistrations.TryRemove(activeRegistration.Key, out _);
                    }
                }

                if (this.IsRegistrationAllowed(login))
                {
                    RegistrationEnteredData registrationEnteredData = new RegistrationEnteredData() { DateSent = DateTime.Now };

                    Tuple<RegistrationEnteredData, UserDto> tupleRegistration = Tuple.Create(registrationEnteredData, userDto);

                    HelperRegistrationService._activeRegistrations.AddOrUpdate(login, tupleRegistration, (k, v) => tupleRegistration);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Генерация 6-ти значного кода для пользователя
        /// </summary>
        /// <returns>6-ти значный код</returns>
        public static string GetSixDigitCode(string login)
        {
            byte[] randomNumber = new byte[4];

            RandomNumberGenerator.Create().GetBytes(randomNumber);

            string code = Math.Abs(BitConverter.ToInt32(randomNumber, 0) % 1000000).ToString("D6");

            lock (HelperRegistrationService.LockObject)
            {
                if (HelperRegistrationService._activeRegistrations.TryGetValue(login, out Tuple<RegistrationEnteredData, UserDto> registrationEnteredData))
                {
                    registrationEnteredData.Item1.Code = code;

                    return code;
                }

                throw new KeyNotFoundException($"При регистрации пользователя, отсутствовала запись активной регистрации: {login}");

            }
        }

        /// <summary>
        /// Корректность введеного кода
        /// </summary>
        /// <param name="enteredCode">Введенный код</param>
        public bool IsEteredCodeCorrect(string enteredCode, string login)
        {
            lock (HelperRegistrationService.LockObject)
            {
                if (HelperRegistrationService._activeRegistrations.TryGetValue(login, out Tuple<RegistrationEnteredData, UserDto> registrationEnteredData))
                {
                    return registrationEnteredData.Item1.Code == enteredCode;
                }

                throw new KeyNotFoundException($"При подтверждении регистрации пользователя, отсутствовала запись активной регистрации: {login}");
            }
        }

        public UserDto GetRegisteredUser(string login)
        {
            lock (HelperRegistrationService.LockObject)
            {
                if (HelperRegistrationService._activeRegistrations.TryGetValue(login, out Tuple<RegistrationEnteredData, UserDto> registrationEnteredData))
                {
                    return registrationEnteredData.Item2;
                }

                throw new KeyNotFoundException($"При получении данных пользователя, отсутствовала запись активной регистрации: {login}");
            }
        }
    }
}
