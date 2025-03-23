namespace YourWheel.Host.Services
{
    using ObjectTitles;
    using ObjectTitles.ObjectTitles;

    public class ObjectTitlesService : IObjectTitlesService
    {
        private readonly Guid _rusLanguageGuid;

        private readonly Guid _engLanguageGuid;

        public ObjectTitlesService(Guid rusLanguageGuid, Guid engLanguageGuid) 
        {
            if (this._titles == null)
            {
                this._titles = new Dictionary<string, ObjectTitle>();

                this._rusLanguageGuid = rusLanguageGuid;

                this._engLanguageGuid = engLanguageGuid;

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
                    objectTitleLocalizations.TitleLocalizations.Add(new ObjectTitleLocalization(this._rusLanguageGuid, rus));

                    objectTitleLocalizations.TitleLocalizations.Add(new ObjectTitleLocalization(this._engLanguageGuid, eng));

                    ObjectTitle objectTitle = new ObjectTitle(tag, objectTitleLocalizations);

                    this.Titles.Add(tag, objectTitle);
                }
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        public string GetTitleByTag(string tag, Guid languageGuid, bool isRecursion = false)
        {
            ObjectTitle objectTitle = this.Titles.FirstOrDefault(item => item.Key == tag).Value;

            if (objectTitle != null)
            {
                //return objectTitle.Title;
                return objectTitle.GetTitleByLanguage(languageGuid);
            }
            else
            {
                return isRecursion
                    ? throw new ArgumentException($"Ошибка получения локализации по тегу {tag}; language: {languageGuid}.")
                    : this.GetTitleByTag(Constants.UnknownText, languageGuid, true);
            }
        }

        public void Initialize()
        {
            this.Add(Constants.TextErrorLoginOrPassword, "Неверное имя пользователя или пароль", "Invalid username or password");
            this.Add(Constants.UnknownText, "Не инициализирован", "Not initialized");
            this.Add(Constants.ErrorText, "Произошла ошибка", "An error has occurred");
            this.Add(Constants.UserAlreadyExistsText, "Пользователь уже существует", "The user already exists");
            this.Add(Constants.UserIsAlreadyRegisteringAtTheMomentText, "Пользователь уже проходит регистрацию в данный момент", "The user is already registering at the moment");
            this.Add(Constants.OkText, "Ок", "Ok");
        }
    }
}
