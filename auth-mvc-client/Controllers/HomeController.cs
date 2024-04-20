using auth_mvc_client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http;

namespace auth_mvc_client.Controllers
{
    public class HomeController(ILogger<HomeController> logger, HttpClient httpClient) : Controller
    {

        public async Task<IActionResult> Index()
        {
            //var token = GetTokenFromStorage(); // Replace with logic to retrieve token from storage (e.g., cookie)
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync("https://localhost:7101/api/weather");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                // Handle successful response with content
            }
            else
            {
                // Handle error response
            }

            return View();
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
