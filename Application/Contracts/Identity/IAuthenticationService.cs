using Microsoft.AspNetCore.Mvc;
using Application.Models.Authentication;

namespace Application.Contracts.Identity
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<AuthenticationResponse> TwoStepVerificationAsync(TwoFactorRequest request);
        Task<AuthenticationResponse> ExternalLoginAsync(ExternalAuthenticationRequest request);
        Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);
        Task ForgotPasswordAsync(ForgotPasswordRequest request);
        Task ResetPasswordAsync(ResetPasswordRequest request);
        Task<bool> EmailConfirmationAsync(string email, string token);
    }
}
