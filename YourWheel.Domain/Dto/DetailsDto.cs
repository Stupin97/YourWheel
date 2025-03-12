namespace YourWheel.Domain.Dto
{
    using Newtonsoft.Json;

    /// <summary>
    /// Dto для сообщений
    /// </summary>
    public class DetailsDto
    {
        public string Details { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
