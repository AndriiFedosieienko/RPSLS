using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RPSLS.Applications.Contracts;
using RPSLS.Applications.Exceptions;
using RPSLS.Applications.Models;

namespace IdentityService.Controllers
{
	[ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenManagementService _tokenManagementService;

        public AuthController(IAuthService authService, ITokenManagementService tokenManagementService)
        {
            _authService = authService;
            _tokenManagementService = tokenManagementService;
        }

        /// <summary>
        ///     Logs in the user with a username/ password and returns a JWT and refresh token.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("~/api/auth/login")]
        [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var authResponse = await _authService.AuthenticateUser(model);

                return Ok(authResponse);
            }
            catch (ArgumentException exception)
            {
                return new BadRequestObjectResult(exception.Message);
            }
            catch (NotAuthorizedException exception)
            {
                return new UnauthorizedObjectResult(exception.Message);
            }
            catch (Exception exception)
            {
                var result = new ObjectResult(exception.Message);
                result.StatusCode = StatusCodes.Status500InternalServerError;
                return result;
            }
        }

        /// <summary>
        /// Using a refresh token, get a new JWT.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("~/api/auth/refresh")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh([FromBody] UserRefreshRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var token = await _tokenManagementService.RefreshJwt(request.RefreshToken, request.Username);
                    return Ok(token);
                }
                catch (ArgumentException exception)
                {
                    return new BadRequestObjectResult(exception.Message);
                }
                catch (NotAuthorizedException exception)
                {
                    return new UnauthorizedObjectResult(exception.Message);
                }
            }

            return BadRequest(ModelState);
        }
    }
}
