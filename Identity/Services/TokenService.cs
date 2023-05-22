using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Application.Contracts.Identity;
using Application.Models.Authentication;
using Identity.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private readonly GoogleAuthSettings _goolgeSettings;
        public TokenService(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<JwtSettings> jwtSettings,
            IOptions<GoogleAuthSettings> googleAuthSettings)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _goolgeSettings = googleAuthSettings.Value;
        }

        /// <summary>
        /// Generate Token By Take User And Prepare Token Include (Roles, RolesClaims, UId, Email, Sub And Jti)
        /// </summary>
        /// <param name="user"> ApplicationUser </param>
        /// <returns> JwtSecurityToken </returns>
        public async Task<JwtSecurityToken> GenerateAccessToken(IdentityUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync((ApplicationUser)user);
            var userInfo = await _userManager.FindByIdAsync(user.Id);
            var roles = await _userManager.GetRolesAsync((ApplicationUser)user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));

                var role = await _roleManager.FindByNameAsync(roles[i]);
                var claimsList = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in claimsList)
                    roleClaims.Add(new Claim("rolesClaims", claim.Value));

            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("fullName", userInfo.FirstName + " " + userInfo.LastName),
                new Claim(ClaimTypes.Name, user.UserName)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        /// <summary>
        /// Generate Refresh Token contains the logic to generate the refresh token. We use the RandomNumberGenerator class to generate a cryptographic random number for this purpose.
        /// </summary>
        /// <returns> string </returns>
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// is used to get the user principal from the expired access token. We make use of the ValidateToken() method of JwtSecurityTokenHandler class for this purpose. This method validates the token and returns the ClaimsPrincipal object.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="SecurityTokenException"></exception>
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        /// <summary>
        /// This method accepts the externalAuth parameter that we send from the client and returns the GoogleJsonWebSignature.Payload object that contains different users’ information. Before we start the validation, we have to create a ValidationSettings object and populate the Audience property with the clientId of our application. With this property, we ensure the token validation is for our application (because we assigned the clientId of our app). Finally, we call the ValidateAsync method and pass the token and the settings parameters. This method validates our token and if valid, returns the user’s data (Name, Family Name, Given Name, Email…).
        /// </summary>
        /// <param name="externalAuth"></param>
        /// <returns></returns>
        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthenticationRequest externalAuth)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { _goolgeSettings.clientId }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);
                return payload;
            }
            catch (Exception ex)
            {
                //log an exception
                return null;
            }
        }
    }
}
