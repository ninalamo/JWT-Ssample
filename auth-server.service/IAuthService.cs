using auth_server.service.Exceptions;
using auth_server.service.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace auth_server.service
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginModel loginModel);
        Task<string> RegisterAsync(RegisterModel registerModel);
    }

    public class AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration) : IAuthService
    {
      
        public async Task<string> LoginAsync(LoginModel loginModel)
        {
#if DEBUG
            loginModel = new LoginModel { Password = "Password1234!", Username = "username" };
#endif
            // Assuming you have retrieved the authenticated user information
            var user = await userManager.Users.SingleOrDefaultAsync(c => c.UserName == loginModel.Username);
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            if (!await signInManager.CanSignInAsync(user))
            {
                throw new SignInNotAllowedException();
            }

            var loginResult = await signInManager.CheckPasswordSignInAsync(user, loginModel.Password!, false);
            if (!loginResult.Succeeded)
            {
                throw new SignInNotAllowedException();
            }

            // Define security key (store securely!)
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
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
                SigningCredentials = credentials,
                Audience = configuration["JWT:ValidAudience"],
                Issuer = configuration["JWT:ValidIssuer"],
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;

        }

        public async Task<string> RegisterAsync(RegisterModel registerModel)
        {
            var user = new IdentityUser(registerModel.Username!);
            var createResult = await userManager.CreateAsync(user, registerModel.Password!);
            if (createResult.Succeeded)
            {
                throw new FailedToRegisterException();
            }

            return user.Id;
        }
    }
}
