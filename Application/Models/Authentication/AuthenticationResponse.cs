using System.Security.Claims;

namespace Application.Models.Authentication
{
    public class AuthenticationResponse
    {
        public string? Id { get; set; }
        //public string? UserName { get; set; }
        //public string? Email { get; set; }
        //public string? FullName { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public bool IsAuthSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public bool Is2StepVerificationRequired { get; set; }
        public string? Provider { get; set; }
    }
}
