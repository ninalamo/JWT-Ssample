using auth_mvc_client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace auth_mvc_client.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient; // Inject HttpClient for API calls

        public LoginController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Return view with validation errors
            }

            // Convert model to format expected by authentication server
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            // Replace with your authentication server URL and endpoint
            var response = await _httpClient.PostAsync("https://localhost:7076/login", content);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();

                // Store token in session or cookie (if needed)
                HttpContext.Session.SetString("jwtToken", token);

                return RedirectToAction("Index", "Home"); // Redirect to Home on success
            }
            else
            {
                ModelState.AddModelError("", "Invalid login credentials.");
                return View(model); // Return view with error message
            }
        }
    }
}