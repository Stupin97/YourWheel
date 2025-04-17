namespace YourWheel.Domain.Dto.Orders
{
    using YourWheel.Domain.Dto.Services;

    /// <summary>
    /// "Собранная" пользователем работа
    /// </summary>
    public class CreatedWorkByUserDto
    {
        public Guid CreatedWorkByUserDtoId { get; set; }

        public Guid TypeWorkId { get; set; }

        public Guid MaterialId { get; set; }
    }
}
