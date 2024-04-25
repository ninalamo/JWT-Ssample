using auth_mvc_client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace auth_mvc_client.Controllers
{
    public class HomeController(ILogger<HomeController> logger, HttpClient httpClient) : Controller
    {

        public async Task<IActionResult> Index()
        {
            //var loginResponse = await httpClient.PostAsJsonAsync("https://localhost:7076/login", new LoginViewModel
            //{
            //    Username = "username",
            //    Password = "Password1234!",
            //});  

            //var token = await loginResponse.Content.ReadAsStringAsync();

            var token = HttpContext.Session.GetString("jwtToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login");
            }

            // Replace with logic to retrieve token from storage (e.g., cookie)
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync("https://localhost:7101/api/weather");

            var reason = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<WeatherForecastModel[]>();
                // Handle successful response with content
                return View(content);
            }
            else
            {
                // Handle error response
            }

            return View(Array.Empty<WeatherForecastModel>());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Call Authentication Server to get JWT token
            HttpResponseMessage response;
            try
            {
                response = await httpClient.PostAsJsonAsync("https://your-auth-server/login", model);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "Failed to connect to authentication server.");
                return View(model);
            }

            string token = await response.Content.ReadAsStringAsync();

            // Parse token and construct ClaimsPrincipal
            var claimsPrincipal = ParseToken(token);

            if (claimsPrincipal == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid token received from authentication server.");
                return View(model);
            }

            // Set ClaimsPrincipal as current user's identity
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsPrincipal));

            // Redirect to Index page
            return RedirectToAction("Index", "Home");
        }

        private ClaimsPrincipal ParseToken(string token)
        {
            // Parse token and extract claims
            var claims = new List<Claim>();

            // Example: Extract claims from token string
            // Replace this with your actual token parsing logic
            var payload = token.Split('.')[1];
            var decodedPayload = Encoding.UTF8.GetString(Base64UrlDecode(payload));
            var jwtClaims = JsonSerializer.Deserialize<Dictionary<string, string>>(decodedPayload);

            foreach (var claim in jwtClaims)
            {
                claims.Add(new Claim(claim.Key, claim.Value));
            }

            // Construct ClaimsIdentity and ClaimsPrincipal
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(identity);
        }

        // Helper method to decode base64url-encoded strings
        private byte[] Base64UrlDecode(string input)
        {
            string padded = input.Length % 4 == 0 ? input : input + "====".Substring(input.Length % 4);
            string base64 = padded.Replace("_", "/").Replace("-", "+");
            return Convert.FromBase64String(base64);
        }
    }
}
