namespace Application.Models.Authentication
{
    public class RegistrationResponse
    {
        public string? UserId { get; set; }
        public bool IsSuccessfulRegistration { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
