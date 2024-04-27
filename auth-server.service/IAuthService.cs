using auth_server.service.Models;

namespace auth_server.service
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginModel loginModel);
        Task<string> RegisterAsync(RegisterModel registerModel);
    }
}
