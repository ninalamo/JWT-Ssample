using auth_server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace auth_server.Controllers
{
    [ApiController]
    public class AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : Controller
    {


        [HttpGet]
        [Route("register")]
        public async Task<IActionResult> Register(string username, string password, string confirmapassword)
        {
            if(password != confirmapassword)
            {
                throw new 
            }
        }


        [Route("login")]
        [ProducesResponseType(typeof(string), 200)]
        [HttpGet]
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
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define claims (user information)
            var claims = new List<Claim>()
            {
              new Claim(ClaimTypes.NameIdentifier, user.Id),
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
