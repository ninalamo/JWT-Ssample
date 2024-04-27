using auth_mvc_client.Extenstions;
using auth_mvc_client.httpservice;
using auth_mvc_client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace auth_mvc_client.Controllers
{
    public class HomeController( IHttpService httpService) : Controller
    {

        public async Task<IActionResult> Index()
        {

            var token = HttpContext.Session.GetString("jwtToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login");
            }
            var weatherForecast = await httpService.GetWeatherForecasts(token);

            return View(weatherForecast.ToDto());
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
