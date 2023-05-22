using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Application.Models.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Application.Contracts.Identity
{
    public interface ITokenService
    {
        Task<JwtSecurityToken> GenerateAccessToken(IdentityUser user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthenticationRequest externalAuth);
    }
}
