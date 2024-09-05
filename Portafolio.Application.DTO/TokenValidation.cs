namespace Portafolio.Application.DTO
{
    public class TokenValidation
    {
        public bool IsValidate { get; set; } = false;
        public bool ExpiredTime { get; set; } = false;
        public string? Message { get; set; }

    }
}
