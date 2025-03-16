namespace YourWheel.Host.Services
{
    using ObjectTitles;
    using ObjectTitles.ObjectTitles;

    public class ObjectTitlesService : IObjectTitlesService
    {
        public ObjectTitlesService() 
        {
            if (_titles == null)
            {
                _titles = new Dictionary<string, ObjectTitle>();

                this.Initialize();
            }
        }

        private Dictionary<string, ObjectTitle> _titles;

        public Dictionary<string, ObjectTitle> Titles { get { return _titles; } }

        public void Add(string tag, string rus, string eng)
        {
            try
            {
                if (!this.Titles.ContainsKey(tag))
                {
                    ObjectTitleLocalizations objectTitleLocalizations = new ObjectTitleLocalizations();

                    // Передать Guid языка (Последний выбор пользователя. Сохранен в базе)
                    objectTitleLocalizations.TitleLocalizations.Add(new ObjectTitleLocalization(Guid.NewGuid(), rus));

                    objectTitleLocalizations.TitleLocalizations.Add(new ObjectTitleLocalization(Guid.NewGuid(), eng));

                    ObjectTitle objectTitle = new ObjectTitle(tag, objectTitleLocalizations);

                    this.Titles.Add(tag, objectTitle);
                }
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        public string GetTitleByTag(string tag)
        {
            ObjectTitle objectTitle = this.Titles.FirstOrDefault(item => item.Key == tag).Value;

            if (objectTitle != null)
            {
                return objectTitle.Title;
            }
            else
            {
                return this.GetTitleByTag(Constants.UnknownText);
            }
        }

        public void Initialize()
        {
            this.Add(Constants.TextErrorLoginOrPassword, "Неверное имя пользователя или пароль", "Invalid username or password");
            this.Add(Constants.UnknownText, "Не инициализирован", "Not initialized");
        }
    }
}
