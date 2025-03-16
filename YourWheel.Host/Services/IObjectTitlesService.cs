namespace YourWheel.Host.Services
{
    using ObjectTitles.ObjectTitles;

    /// <summary>
    /// Интерфейс локализаций
    /// </summary>
    public interface IObjectTitlesService
    {
        /// <summary>
        /// Список локализаций
        /// </summary>
        Dictionary<string, ObjectTitle> Titles { get; } 

        /// <summary>
        /// Инициализация локализаций
        /// </summary>
        void Initialize();

        /// <summary>
        /// Добавление локализации
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="rus"></param>
        /// <param name="eng"></param>
        void Add(string tag, string rus, string eng);

        /// <summary>
        /// Получить значение объекта с учетом локализации
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        string GetTitleByTag(string tag);
    }
}
