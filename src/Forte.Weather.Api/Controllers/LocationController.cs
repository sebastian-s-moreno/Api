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
        public ActionResult GetRecommended(string preference)
        {
            if (string.IsNullOrEmpty(preference))
            {
                return BadRequest("The preferred activity is not well formed");
            }
            return Ok(_weatherService.GetRecommendedLocation(preference));
        }

        [HttpGet("locations")]
        public ActionResult Get()
        {
            return Ok(_weatherService.GetLocations());
        }

        [HttpPost("locations")]
        public async Task<ActionResult> Post([FromBody] LocationModel location)
        {
            if (location == null)
            {
                return BadRequest();
            }
            bool response = await _weatherService.AddLocation(location);
            if (response)
            {
                return Ok(new { message = "Location added" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error adding data");
            }
            
        }

        [HttpDelete("locations/{id}")]
        public ActionResult Delete(string id)
        {
            bool response = _weatherService.DeleteLocation(id);
            if (response)
            {
                return Ok(new { message = "Location deleted" });
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
            if (location == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool response = await _weatherService.UpdateLocation(id,location);
            if (response)
            {
                return Ok(new { message = "Location updated" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }


        [HttpGet("locations/details")]
        public async Task<ActionResult> GetDetails(string id)
        {
            return Ok(await _weatherService.GetUpdatedDetails(id,null,null));
        }


        
    }
}