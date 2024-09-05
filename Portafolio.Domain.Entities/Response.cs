namespace Portafolio.Domain.Entities
{
    public class Response <T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public bool IsValidate { get; set; } = false;
        public string MessageValidate { get; set; } = string.Empty;

    }
}
