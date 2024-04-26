using auth_mvc_client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace auth_mvc_client.Controllers
{
    public class HomeController(ILogger<HomeController> logger, HttpClient httpClient) : Controller
    {

        public async Task<IActionResult> Index()
        {

            var token = HttpContext.Session.GetString("jwtToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login");
            }

            // Replace with logic to retrieve token from storage (e.g., cookie)
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync("https://localhost:7101/api/weather");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<WeatherForecastModel[]>();
                // Handle successful response with content
                return View(content);
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
    }
}
