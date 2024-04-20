using Microsoft.AspNetCore.Mvc;

namespace auth_server.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult GetToken(string userName, string password)
        {
            return View();
        }
    }
}
