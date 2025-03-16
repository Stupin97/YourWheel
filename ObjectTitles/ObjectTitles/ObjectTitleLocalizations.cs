namespace ObjectTitles.ObjectTitles
{
    /// <summary>
    /// Класс для локализованных объектов
    /// </summary>
    public sealed class ObjectTitleLocalizations
    {
        /// <summary>
        /// Локализированные объекты
        /// </summary>
        public List<ObjectTitleLocalization> TitleLocalizations { get; set; }

        public string Title
        {
            get
            {
                // Тут нужно будет из настройки пользователя
                return this.GetTitle(Guid.NewGuid());
            }
        }

        public override string ToString()
        {
            return this.Title ?? string.Empty;
        }

        public ObjectTitleLocalizations()
        {
            this.TitleLocalizations = new List<ObjectTitleLocalization>();
        }

        public ObjectTitleLocalizations(List<ObjectTitleLocalization> objectTitleLocalizations) : this()
        {
            this.TitleLocalizations = objectTitleLocalizations;
        }

        /// <summary>
        /// Получить значение объекта
        /// </summary>
        /// <param name="languageGuid">Guid языка</param>
        /// <returns>Локализация</returns>
        public string GetTitle(Guid languageGuid)
        {
            return this.TitleLocalizations.FirstOrDefault(l => l.LanguageGuid == languageGuid)?.Title;
        }

        /// <summary>
        /// Получить объект локализации
        /// </summary>
        /// <param name="languageGuid">Guid языка</param>
        /// <returns>Объект локализации</returns>
        public ObjectTitleLocalization GetObjectTitleLocalization(Guid languageGuid)
        {
            return this.TitleLocalizations.FirstOrDefault(l => l.LanguageGuid == languageGuid);
        }
    }

    /// <summary>
    /// Класс для Текста/Значения объекта
    /// </summary>
    public sealed class ObjectTitleLocalization
    {
        /// <summary>
        /// Гуид языка локализации
        /// </summary>
        public Guid LanguageGuid { get; set; }

        /// <summary>
        /// Значение объекта - локализованный
        /// </summary>
        public string Title { get; set; }

        public ObjectTitleLocalization(Guid languageGuid, string Title)
        {
            this.LanguageGuid = languageGuid;

            this.Title = Title;
        }
    }
}
