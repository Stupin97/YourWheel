namespace ObjectTitles.ObjectTitles
{
    /// <summary>
    /// Класс локализации со значениями по конкретному тегу
    /// </summary>
    public class ObjectTitle
    {
        /// <summary>
        /// Локализации для значения
        /// </summary>
        public ObjectTitleLocalizations ObjectTitleLocalizations { get; }

        /// <summary>
        /// Тег для локализации
        /// </summary>
        public string Tag { get; }

        /// <summary>
        /// Конкретное значение с учетом локализации
        /// </summary>
        //public string Title
        //{
        //    get
        //    {
        //        return ObjectTitleLocalizations.Title;
        //    }
        //}

        private ObjectTitle()
        {
            this.ObjectTitleLocalizations = new ObjectTitleLocalizations();
        }

        public ObjectTitle(string tag) : this()
        {
            this.Tag = tag;
        }

        public ObjectTitle(string tag, ObjectTitleLocalizations objectTitleLocalizations) : this(tag)
        {
            this.ObjectTitleLocalizations = objectTitleLocalizations;
        }

        public string GetTitleByLanguage(Guid languageGuid)
        {
            return this.ObjectTitleLocalizations.GetTitle(languageGuid);
        }
    }
}
