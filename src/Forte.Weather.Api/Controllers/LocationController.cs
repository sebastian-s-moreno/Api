using Forte.Weather.Services;
using Microsoft.AspNetCore.Mvc;
using Forte.Weather.Services.Models;

namespace Forte.Weather.Api.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly IRecommendationService _recommendationService;

        public LocationController(ILocationService locationService, IRecommendationService recommendationService)
        {
            _locationService = locationService;
            _recommendationService = recommendationService;
        }

        [HttpGet("locations")]
        public ActionResult Get()
        {
            return Ok(_locationService.GetLocations());
        }

        [HttpPost("locations")]
        public async Task<ActionResult> Post([FromBody] Location location)
        {
            if (location == null)
            {
                return BadRequest();
            }
            bool response = await _locationService.AddLocation(location);
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
            bool response = _locationService.DeleteLocation(id);
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
        public async Task<ActionResult<Location>> UpdateLocation(string id, Location location)
        {
            if (location == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool response = await _locationService.UpdateLocation(id,location);
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
            return Ok(await _locationService.GetUpdatedDetails(id));
        }

        [HttpGet("locations/recommended")]
        public ActionResult GetRecommended(Activity preference)
        {
            return Ok(_recommendationService.GetRecommendedLocation(preference));
        }



    }
    
}