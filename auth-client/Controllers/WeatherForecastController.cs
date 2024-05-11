using auth_client.service;
using auth_client.service.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth_client_mvc.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WeatherForecastController(IWeatherService weatherService) : ControllerBase
    {

        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        [Route("api/weather")]
        [ProducesResponseType(typeof(IEnumerable<WeatherForecastModel>),StatusCodes.Status200OK)]
        public IEnumerable<WeatherForecastModel> Get()
        {
            var temp = User;
            return weatherService.GetAll();
        }
    }
}
