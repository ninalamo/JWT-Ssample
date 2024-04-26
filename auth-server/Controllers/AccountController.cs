using auth_server.service;
using auth_server.service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace auth_server.Controllers
{
    [ApiController]
    public class AccountController(IAuthService authService, IConfiguration configuration) : Controller
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

            var id = await authService.RegisterAsync(registerModel);

            return Ok(id);
        }


        [Route("login")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
#if DEBUG
            loginModel = new LoginModel { Password = "Password1234!", Username = "username" };
#endif
            var token = await authService.LoginAsync(loginModel);


            return Ok(token);

        }
    }
}
