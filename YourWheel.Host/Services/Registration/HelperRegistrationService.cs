namespace YourWheel.Host.Services.Registration
{
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using System.Collections.Concurrent;
    using System.Security.Claims;
    using YourWheel.Domain.Dto;

    public class HelperRegistrationService : IHelperRegistrationService
    {
        private readonly ConcurrentDictionary<string, DateTime> _activeRegistrations = new ConcurrentDictionary<string, DateTime>();

        private static Object LockObject = new Object();

        public bool IsRegistrationAllowed(string keyValue)
        {
            return !this._activeRegistrations.TryGetValue(keyValue, out var registration);
        }

        public void RemoveRegistrationAttempt(string keyValue)
        {
            this._activeRegistrations.TryRemove(keyValue, out _);
        }

        public bool TryRegistration(string keyValue)
        {
            lock (HelperRegistrationService.LockObject)
            {
                if (this.IsRegistrationAllowed(keyValue))
                {
                    this._activeRegistrations.AddOrUpdate(keyValue, DateTime.Now, (k, v) => DateTime.Now);

                    return true;
                }
            }

            return false;
        }
    }
}
