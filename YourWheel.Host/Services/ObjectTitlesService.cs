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
            this.Add(Constants.EnterRequiredFields, "Введите обязательные поля", "Enter the required fields");
            this.Add(Constants.IncorrectPassword, "Некорректный пароль", "Incorrect password");
            this.Add(Constants.MessageHasBeenSent, "Сообщение с кодом было отправлено на {0}", "A message with the code has been sent to {0}");
            this.Add(Constants.ErrorOccurredWhileSendingMessage, "Во время отправки сообщения произошла ошибка. Повторите позже.", "An error occurred while sending the message. Repeat later.");
            this.Add(Constants.InvalidCode, "Неверный код", "Invalid code");
            this.Add(Constants.RegistrationSubjectText, "Подтверждение регистрации", "Invalid code");
            this.Add(Constants.RegistrationBodyText, @"<html><body><h2>Здравствуйте, {0}!</h2>
                                                    <p>Для подтверждения регистрации введите разовый код или перейдите по ссылке<b><a href=""{1}"" target=""_blank"" > подтверждения регистрации</a></b>.</p>
                                                    <p>Ваш разовый код: <b>{2}</b></p></body></html>", "");
            this.Add(Constants.EnteredCodeIsIncorrectOrExpired, "Введенный код неверный или просроченный", "The entered code is incorrect or expired");
        }
    }
}
