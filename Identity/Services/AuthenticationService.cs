using Application.Contracts.Identity;
using Application.Models.Authentication;
using Identity.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Application.Models.Mail;
using Application.Contracts.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.Metrics;

namespace Identity.Services
{
    public class AuthenticationService: Application.Contracts.Identity.IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AuthenticationService> _logger;
        public AuthenticationService(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<JwtSettings> jwtSettings,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService, 
            IEmailService emailService,
            IEmailSender emailSender,
            ILogger<AuthenticationService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _emailService = emailService;
            _emailSender = emailSender;
            _logger = logger;
        }

        /// <summary>
        /// Register New User And Return With User Id If Success.
        /// And Generate Token To Send By Email To Confirm Email.
        /// </summary>
        /// <param name="request"> RegistrationRequest </param>
        /// <returns> RegistrationResponse </returns>
        /// <exception cref="Exception"> </exception>
        public async Task<RegistrationResponse> RegisterAsync(RegistrationRequest request)
        {
            var existingUser = await _userManager.FindByNameAsync(request.UserName);

            if (existingUser != null)
                return new RegistrationResponse { IsSuccessfulRegistration = false, Errors = new string[] { $"Username '{request.UserName}' already exists." } };
            //throw new Exception($"Username '{request.UserName}' already exists.");

            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                TwoFactorEnabled = true
                //EmailConfirmed = true
            };

            var existingEmail = await _userManager.FindByEmailAsync(request.Email);

            if (existingEmail == null)
            {
                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    //await _userManager.SetTwoFactorEnabledAsync(user, true);

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var param = new Dictionary<string, string?>
                    {
                        {"token", token },
                        {"email", user.Email }
                    };
                    var callback = QueryHelpers.AddQueryString(request.ClientURI, param);
                    var message = new Message(new string[] { user.Email }, "Email Confirmation token", callback, null);
                    await _emailSender.SendEmailAsync(message);

                    return new RegistrationResponse() { IsSuccessfulRegistration = true, UserId = user.Id };
                }
                else
                    return new RegistrationResponse { IsSuccessfulRegistration = false, Errors = result.Errors.Select(e => e.Description) };
                //throw new Exception($"{result.Errors}");
            }
            else
            {
                //throw new Exception($"Email {request.Email } already exists.");
                return new RegistrationResponse { IsSuccessfulRegistration = false, Errors = new string[] { $"Email '{request.Email}' already exists." } };
            }
        }

        /// <summary>
        /// AuthenticateAsync User For Login And Return Token And Info.
        /// Here, we use the IsEmailConfirmedAsync method to check whether the user has an email confirmed. As you can see, we are not using the SigninManager class and the PasswordSignInAsync method that automatically checks this for us. The reason for that is that the PasswordSignInAsync method creates an Identity cookie for authentication purposes. But since we are using JWT for the authentication, we don’t require any cookies for now.
        /// We create a couple of changes here. As you can see, we use the AccessFailedAsync method to increment the number of failed attempts in the database. But this method does even more. If the failed attempts count reaches the threshold (we’ve set this number to three) it will set the LockoutEnd column in the database with the date until the account is locked out. Then, we check if the user is locked out with the IsLockedOutAsync method.If it is, we notify the user by sending the email message with the forgot password link.
        /// The ResetAccessFailedCountAsync method does exactly that, resets the number of failed attempts. Now, if we use the wrong password, the count will increase.But if in a second attempt we enter the correct password, the count resets for sure.
        /// Here, we use the GetTwoFactorEnabledAsync method to check whether the user has two-factor authentication enabled. If they have, we call a private method to generate OTP. Since we don’t have that method, let’s create it
        /// </summary>
        /// <param name="request"> AuthenticationRequest </param>
        /// <returns> AuthenticationResponse </returns>
        /// <exception cref="Exception"> Email not found, Email aren't valid</exception>
        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return new AuthenticationResponse { IsAuthSuccessful = false, ErrorMessage = "Invalid Request." };
            //throw new Exception($"Invalid Request.");
            //throw new Exception($"User with {request.Email} not found.");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return new AuthenticationResponse { IsAuthSuccessful = false, ErrorMessage = $"Email: {user.Email} is not confirmed"};
            //throw new Exception($"Email: {user.Email} is not confirmed" );

            //you can check here if the account is locked out in case the user enters valid credentials after locking the account.
            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                await _userManager.AccessFailedAsync(user);

                if (await _userManager.IsLockedOutAsync(user))
                {
                    var content = $"Your account is locked out. To reset the password click this link: {request.ClientURI}";
                    var message = new Message(new string[] { request.Email },
                        "Locked out account information", content, null);

                    await _emailSender.SendEmailAsync(message);

                    return new AuthenticationResponse { IsAuthSuccessful = false, ErrorMessage = "The account is locked out"};
                    //throw new Exception("The account is locked out");
                }
                return new AuthenticationResponse { IsAuthSuccessful = false, ErrorMessage = "Invalid Authentication"};
                //throw new Exception("Invalid Authentication");
            }

            if (await _userManager.GetTwoFactorEnabledAsync(user))
                return await GenerateOTPFor2StepVerification(user);

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, request.RememberMe, lockoutOnFailure: false);

            if (!result.Succeeded)
                return new AuthenticationResponse { IsAuthSuccessful = false, ErrorMessage = $"Credentials for '{request.Email} aren't valid'." };
            //throw new Exception($"Credentials for '{request.Email} aren't valid'.");

            //JwtSecurityToken jwtSecurityToken = await GenerateToken(user);
            JwtSecurityToken jwtSecurityToken = await _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await _userManager.UpdateAsync(user);

            await _userManager.ResetAccessFailedCountAsync(user);

            AuthenticationResponse response = new AuthenticationResponse
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                RefreshToken = refreshToken,
                //Email = user.Email,
                //UserName = user.UserName,
                //FullName = user.FirstName + ' ' + user.LastName,
                IsAuthSuccessful = true
            };
            
            return response;
        }

        /// <summary>
        /// As you can see, we accept the twoFactorDto object from the client and inspect if the model is valid. If it isn’t we just return a bad request. After that, we try to fetch the user by the provided email. Again, if we can’t find one, we return a bad request. If previous checks pass, we use the VerifyTwoFactorTokenAsync method to verify if the token is valid for our user and the provider. If that’s not true, we return a bad request. Finally, if the verification is valid, we create a new token and send it to the client.
        /// </summary>
        /// <param name="request"> TwoFactorRequest </param>
        /// <returns> AuthenticationResponse </returns>
        public async Task<AuthenticationResponse> TwoStepVerificationAsync(TwoFactorRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return new AuthenticationResponse { IsAuthSuccessful = false, ErrorMessage = "Invalid Request." };

            var validVerification = await _userManager.VerifyTwoFactorTokenAsync(user, request.Provider, request.Token);
            if (!validVerification)
                return new AuthenticationResponse { IsAuthSuccessful = false, ErrorMessage = "Invalid Token Verification" };

            //JwtSecurityToken jwtSecurityToken = await GenerateToken(user);
            JwtSecurityToken jwtSecurityToken = await _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await _userManager.UpdateAsync(user);

            AuthenticationResponse response = new AuthenticationResponse
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                RefreshToken = refreshToken,
                //Email = user.Email,
                //UserName = user.UserName,
                IsAuthSuccessful = true
            };
            return response;
        }

        public async Task<AuthenticationResponse> ExternalLoginAsync(ExternalAuthenticationRequest request)
        {
            var payload = await _tokenService.VerifyGoogleToken(request);
            if (payload == null)
                return new AuthenticationResponse { IsAuthSuccessful = false, ErrorMessage = "Invalid External Authentication." };

            var info = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new ApplicationUser { Email = payload.Email, UserName = payload.Email };
                    await _userManager.CreateAsync(user);
                    //prepare and send an email for the email confirmation
                    await _userManager.AddToRoleAsync(user, "General User");
                    await _userManager.AddLoginAsync(user, info);
                }
                else
                {
                    await _userManager.AddLoginAsync(user, info);
                }
            }
            if (user == null)
                return new AuthenticationResponse { IsAuthSuccessful = false, ErrorMessage = "Invalid External Authentication." };

            //check for the Locked out account
            //JwtSecurityToken jwtSecurityToken = await GenerateToken(user);
            JwtSecurityToken jwtSecurityToken = await _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await _userManager.UpdateAsync(user);

            AuthenticationResponse response = new AuthenticationResponse
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                RefreshToken = refreshToken,
                //Email = user.Email,
                //UserName = user.UserName,
                IsAuthSuccessful = true
            };
            return response;
        }
        /// <summary>
        /// In this action, we fetch the user from the database, create a token, and use the QueryHelpers class to create the URI with two query parameters. After that, we just call the SendEmailAsync method to send an email and return the 200 status code. You can see that if we don’t find a user in the database, we don’t send the NotFound response, but just the BadRequest.This is for security reasons if someone is trying to hack the account and just guessing the email address.
        /// </summary>
        /// <param name="request"> ForgotPasswordRequest </param>
        /// <returns></returns>
        public async Task ForgotPasswordAsync(ForgotPasswordRequest request)
        {
          
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new Exception($"Invalid Request.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string?>
            {
                {"token", token },
                {"email", request.Email }
            };
            var callback = QueryHelpers.AddQueryString(request.ClientURI, param);
            var message = new Message(new string[] { user.Email }, "Reset password token", callback, null);

            try
            {
                await _emailSender.SendEmailAsync(message);
            }
            catch (Exception ex)
            {
                //this shouldn't stop the API from doing else so this can be logged
                _logger.LogError($"Mailing about event {user.Email} failed due to an error with the mail service: {ex.Message}");
            }

            //Sending email notification to admin address
            //var email = new Email() { To = user.Email, Body = $"Reset password token: {callback}", Subject = "Reset password" };

            //try
            //{
            //    await _emailService.SendEmail(email);
            //}
            //catch (Exception ex)
            //{
            //    //this shouldn't stop the API from doing else so this can be logged
            //    _logger.LogError($"Mailing about event {user.Email} failed due to an error with the mail service: {ex.Message}");
            //}

        }

        /// <summary>
        /// if the model is invalid or we can’t find the user by their email, we return BadRequest. If these checks pass, we try resetting the password. If this action is not successful, we collect all the errors and return them to a client. Otherwise, we return 200 status code.
        /// </summary>
        /// <param name="request"> ResetPasswordRequest </param>
        /// <returns></returns>
        /// <exception cref="Exception"> Invalid Request Or errors </exception>
        public async Task ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new Exception($"Invalid Request.");

            var resetPassResult = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (!resetPassResult.Succeeded)
            {
                var errors = resetPassResult.Errors.Select(e => e.Description);
                  throw new Exception($"{new {Errors = errors}}.");
            }
        }

        /// <summary>
        /// In this action, we accept two parameters from the query. These parameters are the same ones we emailed to the user. Next, we try to fetch the user from the database. If we can’t find one, we just return a bad request. Otherwise, we use the ConfirmEmailAsync method to update the user’s data in the database. If this succeeds, the EmailConfirmed column in the AspNetUsers table will be set to true. If it fails, we return a bad request.
        /// The SetLockoutEndDateAsync method will modify the LockoutEnd column in the database. If you set that column’s value to any date in the past, the account will be unlocked. That’s exactly what we do here.
        /// </summary>
        /// <param name="email"> string </param>
        /// <param name="token"> string </param>
        /// <returns> Void Or Execption </returns>
        public async Task<bool> EmailConfirmationAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new Exception("Invalid Email Confirmation Request");

            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded)
                throw new Exception("Invalid Email Confirmation Request");

            await _userManager.SetLockoutEndDateAsync(user, new DateTime(2000, 1, 1));

            return true;
        }

        #region Helper Method
        /// <summary>
        /// Generate Token By Take User And Prepare Token Include (Roles, RolesClaims, UId, Email, Sub And Jti)
        /// </summary>
        /// <param name="user"> ApplicationUser </param>
        /// <returns> JwtSecurityToken </returns>
        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

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
                new Claim("uid", user.Id)
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
        /// GenerateOTPFor2StepVerification In this action, we call the GetValidTwoFactorProviderAsync method to verify if this user has an email provider registered. If this is not the case, we return the Unauthorized response. But if it is true, we generate the OTP with the GenerateTwoFactorTokenAsync method and send that OTP to the user by email. Finally, we return the successful response but with the Is2StepVerificationRequired property set to true and the Provider property set to Email.Since we don’t have these properties in the AuthResponseDto class, 
        /// </summary>
        /// <param name="user"> ApplicationUser </param>
        /// <returns></returns>
        private async Task<AuthenticationResponse> GenerateOTPFor2StepVerification(ApplicationUser user)
        {
            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
            if (!providers.Contains("Email"))
            {
                return new AuthenticationResponse { IsAuthSuccessful = false, ErrorMessage = "Invalid 2 - Step Verification Provider." };
                }

            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            var message = new Message(new string[] { user.Email }, "Authentication token", token, null);

            await _emailSender.SendEmailAsync(message);

            return new AuthenticationResponse { Is2StepVerificationRequired = true, IsAuthSuccessful = true, Provider = "Email" };
        }
        #endregion
    }
}
