using auth_server.service;
using auth_server.service.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace auth_server.Controllers
{
    [ApiController]
    public class AccountController(IAuthService authService) : Controller
    {
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        [ProducesErrorResponseType(typeof(BadRequestObjectResult))]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            if(registerModel.Password != registerModel.ConfirmPassword)
            {
                ModelState.AddModelError("Password", "Password does not match");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = await authService.RegisterAsync(registerModel);

            return Ok(id);
        }


        [Route("login")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(BadRequestObjectResult))]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
#if DEBUG
            loginModel = new LoginModel { Password = "Password1234!", Username = "username" };
#endif
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = await authService.LoginAsync(loginModel);
            return Ok(token);
        }
    }
}
