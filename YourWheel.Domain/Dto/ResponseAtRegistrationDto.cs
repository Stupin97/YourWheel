namespace YourWheel.Domain.Dto
{
    /// <summary>
    /// Класс ответ при регистрации
    /// </summary>
    public class ResponseAtRegistrationDto
    {
        /// <summary>
        /// Код из сообщения
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Логин это только - (Email / телефон)
        /// </summary>
        public string Login { get; set; }
    }
}
