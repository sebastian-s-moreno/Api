using Forte.Weather.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net.Http.Headers;

namespace Forte.Weather.Api.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _weatherService;
        public static List<LocationModel> Locations = new List<LocationModel>();

        public LocationController(ILocationService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("locations/recommended")]
        public async Task<LocationModel?> GetRecommended(string preference)
        {
            return await _weatherService.GetRecommendedLocation(preference);
        }

        [HttpGet("locations")]
        public async Task<List<LocationModel>> Get()
        {
            return await _weatherService.GetLocations();
        }

        [HttpPost("locations")]
        public async Task<ActionResult> Post([FromBody] LocationModel location)
        {
            bool response = await _weatherService.AddLocation(location);
            if (response)
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error adding data");
            }
            
        }

        [HttpPost("locations/delete")]
        public ActionResult Delete([FromBody] string id)
        {
            bool response = _weatherService.DeleteLocation(id);
            if (response)
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }

        [HttpPut("locations/{id}")]
        public async Task<ActionResult<LocationModel>> UpdateLocation(string id, LocationModel location)
        {
            bool response = await _weatherService.UpdateLocation(id,location);
            if (response)
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }


        [HttpGet("locations/details")]
        public async Task<TimeSerie?> GetDetails(string id)
        {
            return await _weatherService.GetUpdatedDetails(id,null,null);
        }


        
    }
}