using Application.Models.Authentication;
using Application.Contracts.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Asn1.Ocsp;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Register New User
        /// </summary>
        /// <param name="request"> RegistrationRequest </param>
        /// <returns> OK(RegistrationResponse) Or BadRequest With Errors(Request Null , Not Valid, Email Or UserName Exist And Error When Create) </returns>
        [HttpPost("Register")]
        public async Task<ActionResult<RegistrationResponse>> RegisterAsync(RegistrationRequest request)
        {
            //return Ok(await _authenticationService.RegisterAsync(request));
            if (request == null || !ModelState.IsValid)
                return BadRequest();

            var response = await _authenticationService.RegisterAsync(request);
            if (!response.IsSuccessfulRegistration)
                return BadRequest(response);
            else
                return Ok(response);


        }

        /// <summary>
        /// Authenticate User 
        /// </summary>
        /// <param name="request"> AuthenticationRequest </param>
        /// <returns> OK(AuthenticationResponse) Or BadRequest</returns>
        [HttpPost("Authenticate")]
        public async Task<ActionResult<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            if (request == null || !ModelState.IsValid)
                return BadRequest();
            //return Ok(await _authenticationService.AuthenticateAsync(request));
            var response = await _authenticationService.AuthenticateAsync(request);
            if (!response.IsAuthSuccessful)
                return Unauthorized(response);
            else
                return Ok(response);
        }

        /// <summary>
        /// As you can see, we accept the twoFactorDto object from the client and inspect if the model is valid. If it isn’t we just return a bad request. After that, we try to fetch the user by the provided email. Again, if we can’t find one, we return a bad request. If previous checks pass, we use the VerifyTwoFactorTokenAsync method to verify if the token is valid for our user and the provider. If that’s not true, we return a bad request. Finally, if the verification is valid, we create a new token and send it to the client.
        /// </summary>
        /// <param name="request"> TwoFactorRequest </param>
        /// <returns> OK(AuthenticationResponse) Or BadRequest(AuthenticationResponse) Have Error Messagw </returns>
        [HttpPost("TwoStepVerification")]
        public async Task<IActionResult> TwoStepVerification([FromBody] TwoFactorRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = await _authenticationService.TwoStepVerificationAsync(request);
            if (!response.IsAuthSuccessful)
                return BadRequest(response);
            else
                return Ok(response);

        }

        [HttpPost("ExternalLogin")]
        public async Task<IActionResult> ExternalLogin([FromBody] ExternalAuthenticationRequest request)
        {
            var response = await _authenticationService.ExternalLoginAsync(request);
            if (!response.IsAuthSuccessful)
                return BadRequest(response);
            else
                return Ok(response);
        }

        /// <summary>
        /// In this action, we fetch the user from the database, create a token, and use the QueryHelpers class to create the URI with two query parameters. After that, we just call the SendEmailAsync method to send an email and return the 200 status code. You can see that if we don’t find a user in the database, we don’t send the NotFound response, but just the BadRequest.This is for security reasons if someone is trying to hack the account and just guessing the email address.
        /// </summary>
        /// <param name="request"> ForgotPasswordRequest </param>
        /// <returns> Ok() Or BadRequest() </returns>
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _authenticationService.ForgotPasswordAsync(request);
            return Ok();
        }

        /// <summary>
        /// if the model is invalid or we can’t find the user by their email, we return BadRequest. If these checks pass, we try resetting the password. If this action is not successful, we collect all the errors and return them to a client. Otherwise, we return 200 status code.
        /// </summary>
        /// <param name="request"> ResetPasswordRequest </param>
        /// <returns> OK() Or Execption</returns>
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _authenticationService.ResetPasswordAsync(request);
            return Ok();
        }

        /// <summary>
        /// In this action, we accept two parameters from the query. These parameters are the same ones we emailed to the user. Next, we try to fetch the user from the database. If we can’t find one, we just return a bad request. Otherwise, we use the ConfirmEmailAsync method to update the user’s data in the database. If this succeeds, the EmailConfirmed column in the AspNetUsers table will be set to true. If it fails, we return a bad request.
        /// </summary>
        /// <param name="email"> string </param>
        /// <param name="token"> string </param>
        /// <returns> Ok() Or Execption </returns>
        [HttpGet("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            return Ok(await _authenticationService.EmailConfirmationAsync(email, token));
        }

        [HttpGet("Privacy")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Privacy()
        {
            var claims = User.Claims
                .Select(c => new { c.Type, c.Value })
                .ToList();
            return Ok(claims);
        }
    }
}
