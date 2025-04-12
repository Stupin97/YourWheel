namespace YourWheel.Host.Services.Registration
{
    using YourWheel.Domain.Dto;

    public class EmailRegistrationService : IRegistrationService
    {
        private readonly IObjectTitlesService _objectTitlesService;

        private readonly YourWheel.Host.Services.Senders.IEmailSender _emailSender;

        public HelperRegistrationService.RegistrationTypesBy RegistrationTypesBy => HelperRegistrationService.RegistrationTypesBy.Email;

        public EmailRegistrationService(IObjectTitlesService objectTitlesService, YourWheel.Host.Services.Senders.IEmailSender emailSender)
        {
            this._objectTitlesService = objectTitlesService;

            this._emailSender = emailSender;
        }

        public async ValueTask<bool> TryRegistrationUserAsync(UserDto userDto)
        {
            string name = userDto.Name ?? userDto.Surname ?? userDto.Login;

            string code = HelperRegistrationService.GetSixDigitCode(userDto.Login);

            string url = ConfigurationHelper.Configuration.GetValue<string>("ASPNETCORE_URLS").Split(";").First() 
                + $"/api/registration/response-at-registration-link?Code={code}&Login={userDto.Login}";

            // Генерация кода
            string body = String.Format(this._objectTitlesService.GetTitleByTag(ObjectTitles.Constants.RegistrationBodyText,
                                                    Guid.Parse(ObjectTitles.Constants.RussianLanguageGuid)), name, url, code);

            string subject = this._objectTitlesService.GetTitleByTag(ObjectTitles.Constants.RegistrationSubjectText,
                                                    Guid.Parse(ObjectTitles.Constants.RussianLanguageGuid));

            await this._emailSender.SendEmailAsync(userDto.Login, subject, body);

            return true;
        }
    }
}
