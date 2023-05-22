using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Application.Contracts.Identity;
using Application.Models.Authentication;
using Identity.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ITokenService _tokenService;

        public TokenController(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<JwtSettings> jwtSettings,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        /// <summary>
        /// we implement a refresh endpoint, which gets the user information from the expired access token and validates the refresh token against the user. Once the validation is successful, we generate a new access token and refresh token and the new refresh token is saved against the user in DB.
        /// </summary>
        /// <param name="tokenApiModel"> TokenApiModel </param>
        /// <returns> AuthenticationResponse </returns>
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh(TokenApiModel tokenApiModel)
        {
            if (tokenApiModel is null)
                return BadRequest("Invalid client request");

            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default
            var user = await _userManager.FindByEmailAsync(username);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid client request");

            JwtSecurityToken jwtSecurityToken = await _tokenService.GenerateAccessToken(user);
            var newAccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return Ok(new AuthenticationResponse()
            {
                Id = user.Id,
                Token = newAccessToken,
                RefreshToken = refreshToken,
                //Email = user.Email,
                //UserName = user.UserName,
                IsAuthSuccessful = true
            });
        }

        /// <summary>
        /// We also implement a revoke endpoint that invalidates the refresh token.
        /// </summary>
        /// <returns> NoContent </returns>
        [HttpPost, Authorize]
        [Route("revoke")]
        public async Task<IActionResult> Revoke()
        {
            var username = User.Identity.Name;
            var user = await _userManager.FindByEmailAsync(username);
            if (user == null) return BadRequest();
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return NoContent();
        }
    }
}
