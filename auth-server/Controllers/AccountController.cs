using auth_server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
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
    public class AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : Controller
    {


        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            if(registerModel.Password != registerModel.ConfirmPassword)
            {
                ModelState.AddModelError("Password", "Password does not match");
            }

            var user = await userManager.Users.SingleOrDefaultAsync(c => c.UserName == registerModel.Username);
            if (user != null)
            {
                ModelState.AddModelError("Username", "Username is already used.");
            }
            user = new IdentityUser(registerModel.Username!);
            var createResult = await userManager.CreateAsync(user, registerModel.Password!);

            if(createResult.Succeeded)
            {
                return Created("register",user!.Id);
            }

            return BadRequest(ModelState);
        }


        [Route("login")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            // Assuming you have retrieved the authenticated user information
            var user = await userManager.Users.SingleOrDefaultAsync(c => c.UserName == loginModel.Username);
            if(user == null)
            {
                return NotFound();
            }

            if (! await signInManager.CanSignInAsync(user))
            {
                return Forbid();
            }

            var loginResult = await signInManager.CheckPasswordSignInAsync(user, loginModel.Password!, false);
            if(!loginResult.Succeeded)
            {
                return Unauthorized();
            }

            // Define security key (store securely!)
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("d3341f0b-58c3-4c70-bd6f-5ed6e4cf5015"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define claims (user information)
            var claims = new List<Claim>()
            {
              new Claim(ClaimTypes.NameIdentifier, user.Id),
              new Claim(ClaimTypes.Name,user.UserName),
              // Add other relevant claims (e.g., username, role)
            };

            // Generate the JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30), // Set expiration time
                SigningCredentials = credentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encodedToken = tokenHandler.WriteToken(token);

            return Ok(encodedToken);

        }
    }
}
