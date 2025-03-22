namespace YourWheel.Domain.Services
{
    using Microsoft.EntityFrameworkCore;
    using Npgsql;
    using YourWheel.Domain.Dto;
    using YourWheel.Domain.Models;

    public class AppUserService : IAppUserService
    {
        private readonly YourWheelDbContext _context;

        public AppUserService(YourWheelDbContext context)
        {
            this._context = context;
        }

        public async Task<AppUserDto> GetAppUserDtoAsync(Guid userGuid, string ipAddress)
        {
            try
            {
                var appUser = await this._context.AppUsers.FirstOrDefaultAsync(c => c.UserId == userGuid);

                // Запись отсутствует => создаем
                if (appUser == null) appUser = await this.SetAppUserAsync(userGuid);

                appUser.IsOnline = true;

                appUser.LastConnected = DateTime.Now;

                appUser.LastIPAddress = ipAddress ?? "";

                await this._context.SaveChangesAsync();

                return new AppUserDto()
                {
                    AppUserId = appUser.AppUserId,
                    UserId = appUser.UserId,
                    LastIPAddress = appUser.LastIPAddress,
                    IsOnline = appUser.IsOnline,
                    LastConnected = appUser.LastConnected,
                    LastDisconected = appUser.LastDisconected,
                    CurrentLanguageId = appUser.CurrentLanguageId
                };
            }
            catch (Exception exception)
            {
                // лог

                throw;
            }
        }

        /// <summary>
        /// Создание клиенской информации не публичной
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        private async Task<AppUser> SetAppUserAsync(Guid userGuid)
        {
            Exception lastException = null;

            // 3 попытки создания
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    var appUser = await this._context.SetAndGetUppUserInstance(userGuid).FirstOrDefaultAsync(c => c.UserId == userGuid);

                    if (appUser != null)
                    {
                        // Лог создания + номер попытки

                        return appUser;
                    }
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

        public async Task UpdateAppUserAfterLogoutAsync(Guid userGuid)
        {
            var appUser = await this._context.AppUsers.FirstOrDefaultAsync(c => c.UserId == userGuid);

            if (appUser == null) throw new KeyNotFoundException($"appUser для userId = '{userGuid}' не найден");

            appUser.IsOnline = false;

            appUser.LastDisconected = DateTime.Now;

            await this._context.SaveChangesAsync();
        }

        public async Task UpdateAppUserAfterChangeLanguageAsync(Guid userGuid, Guid languageGuid)
        {
            var appUser = await this._context.AppUsers.FirstOrDefaultAsync(c => c.UserId == userGuid);

            if (appUser == null) throw new KeyNotFoundException($"appUser для userId = '{userGuid}' не найден");

            appUser.CurrentLanguageId = languageGuid;

            await this._context.SaveChangesAsync();
        }

        public async Task<LanguageDto> GetCurrentLanguageForUser(Guid userGuid)
        {
            var appUser = await this._context.AppUsers.FirstOrDefaultAsync(c => c.UserId == userGuid);

            if (appUser == null) throw new KeyNotFoundException($"appUser для userId = '{userGuid}' не найден");

            var language = await this._context.Languages
                .FirstOrDefaultAsync(c => c.LanguageId == appUser.CurrentLanguageId);

            if (language == null) throw new KeyNotFoundException($"Текущий language для User = '{userGuid}' не найден");

            return new LanguageDto() 
            {
                LanguageId = language.LanguageId,
                Name = language.Name
            };
        }

        public async Task<LanguageDto> GetLanguageByGuid(Guid languageGuid)
        {
            var language = await this._context.Languages
                .FirstOrDefaultAsync(c => c.LanguageId == languageGuid);

            if (language == null) throw new KeyNotFoundException($"language = '{languageGuid}' не найден");

            return new LanguageDto()
            {
                LanguageId = language.LanguageId,
                Name = language.Name
            };
        }
    }
}
