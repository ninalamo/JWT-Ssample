using auth_server.service.Exceptions;
using auth_server.service.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace auth_server.service
{
    public class OtherAuthService : IAuthService
    {
        private readonly ICollection<LoginModel> logins = new List<LoginModel>();
        private readonly IConfiguration configuration;

        public OtherAuthService(IConfiguration configuration) {
            this.configuration = configuration;
            logins.Add(new LoginModel
            {
                Username = "testusername",
                Password = "test1234!"
            });

        }
        public async Task<string> LoginAsync(LoginModel loginModel)
        {
           
            if(!logins.Any(i => i.Username == loginModel.Username && i.Password == loginModel.Password))
            {
                throw new UserNotFoundException();
            }

            // Define security key (store securely!)
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define claims (user information)
            //var claims = new List<Claim>()
            //{
            //  new Claim(ClaimTypes.NameIdentifier, user.Id),
            //  new Claim(ClaimTypes.Name,user.UserName),
            //  // Add other relevant claims (e.g., username, role)
            //};

            // Generate the JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30), // Set expiration time
                SigningCredentials = credentials,
                Audience = configuration["JWT:ValidAudience"],
                Issuer = configuration["JWT:ValidIssuer"],
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encodedToken = tokenHandler.WriteToken(token);
             
            return encodedToken;
        }

        public Task<string> RegisterAsync(RegisterModel registerModel)
        {
            throw new NotImplementedException();
        }
    }
}
