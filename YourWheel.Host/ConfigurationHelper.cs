namespace YourWheel.Host
{
    /// <summary>
    /// Конфигурация (на уровне сервиса)
    /// </summary>
    public static class ConfigurationHelper
    {
        public static IConfiguration Configuration;

        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
