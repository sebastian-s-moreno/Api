# Unit testing
In this session, you will create an API based on an empty shell. Download the start point from: **XXXXXXXXX**. While you extend the API with more functionality, you will write unit tests at the same time. The main data source in this API are different locations. A location should have a name, and coordinates, defined by longitude and latitude. 
It should be possible to get locations from the database, add new locations, delete locations and update already saved locations. This is called CRUD-operations (Create, Read, Update, Delete). 

### Create the READ-method
We start by adding code to the Get endpoint in the *LocationController*. 
The endpoint will simply return the response from the Service-layer. 
```C#
[HttpGet("locations")]
public ActionResult Get()
{
    return Ok(_weatherService.GetLocations());
}
``` 
The *LocationService* is connected to the database, and will retrieve the locations and pass them to the Controller: 
```C#
public List<LocationModel> GetLocations()
{
    try
    {
        var locations = _repository.GetLocations();
        return locations.ToModel();
    }
    catch
    {
        return new List<LocationModel>();
    }
}
```
The *LocationRepository* is where the actual database-logic happens: 

```C#
public List<LocationEntity> GetLocations()
{
    return _context.Locations.ToList();
}
```

### Create the CREATE-method
The endpoint where you add data, is a POST endpoint, and this should have more functionality to validate the input from the user. Add this code to the *LocationController*:
```C#
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
        return Ok();
    }
    else
    {
        return StatusCode(StatusCodes.Status500InternalServerError,
            "Error adding data");
    }

}
```
In the *LocationService*, more of the logic happens. An unique ID for the location is created, and some weather details from an external source is added, before the location is saved to the database. As we do not have created the method for retrieving weather data yet, these line is temporary commented out.

```C#
public async Task<bool> AddLocation(LocationModel location)
{
    try
    {
        location.ID = Guid.NewGuid().ToString();
        //TimeSerie? ts = await GetUpdatedDetails(null, location.Longitude, location.Latitude);
        //location.Timeserie = ts;
        _repository.AddLocation(location.FromModel());
    }
    catch
    {
        return false;
    }
    return true;
}
```
Finally, in the *LocationRepository*, add: 
```C#
public void AddLocation(LocationEntity location)
{
    _context.Locations.Add(location);
    _context.SaveChanges();
}
```


### Get Weather Details from an external API
Now, we will create an method to retrieve weather data from an external source. We will be using the API from Yr. You can read more about the API here: https://developer.yr.no/doc/

Add the following code to your *LocationService*. To use the API from yr, you need to pass the coordinates (longitude and latitude) of a place. This method can be used by either coordinates, or the ID of a saved location. If you are using an ID, the location will be retrieved from the database. 
(Now, the two lines in the AddLocation method can be uncommented). 
```C#
public async Task<TimeSerie?> GetUpdatedDetails(string? id, string? longitude, string? latitude)
{
    string elements = "";
    if (id != null)
    {
        LocationModel? location = GetLocation(id);
        if (location != null){
            elements = $"lat={location.Latitude}&lon={location.Longitude}";
        }
    }
    else
    {
        elements = $"lat={latitude}&lon={longitude}";
    }
    var url = "https://api.met.no/weatherapi/locationforecast/2.0/compact?" + elements;
    YrApiResponse? response = null;
    using (var client = new HttpClient())
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var productValue = new ProductInfoHeaderValue("ForteWeatherApp", "1.0");
        request.Headers.UserAgent.Add(productValue);
        var httpResponse = await client.SendAsync(request);
        if (httpResponse.IsSuccessStatusCode)
        {
            response = await httpResponse.Content.ReadFromJsonAsync<YrApiResponse>();
            return response?.Properties?.Timeseries?.FirstOrDefault();
        }
        else
        {
            return null;
        }
    }
}
```

When testing an method, it is important to test the different possibilities of input. You often start by creating tests for the expected input, then you need to think about what else an user can try to use as input. Add the following tests to your test file. Try to run them. Several of them will fail. **Can you see why? Try to edit the GetUpdatedDetails() method, and make sure all the tests pass.**

```C#

[TestMethod]
public void GetDetailsCorrectID()
{
    var result = _service.GetUpdatedDetails("5e525c01-d4c8-4c35-a7da-f4ad7b19dd59", null, null);
    Assert.IsNotNull(result);
}
[TestMethod]
public void GetDetailsCorrectCoordinates()
{
    var result = _service.GetUpdatedDetails(null, "5", "5");
    Assert.IsNotNull(result);
}
[TestMethod]
public void GetDetailsInvalidString()
{
    var result = _service.GetUpdatedDetails("123",null,null).Result;
    Assert.IsNull(result);
}
[TestMethod]
public void GetDetailsInvalidCoordinates()
{
    var result = _service.GetUpdatedDetails(null, "100", "800").Result;
    Assert.IsNull(result);
}
[TestMethod]
public void GetDetailsNoInput()
{
    var result = _service.GetUpdatedDetails(null, null, null).Result;
    Assert.IsNull(result);
}
        
```

### Get recommended location
By using the weather details for all locations, the API should be able to tell the user the recommended location, given a chosen activity. The available activities are: Swimming, Sailing, Skiing, Sightseeing and Unspecified. 
Add the following code to the *LocationService*: 

```C#
public LocationModel? GetRecommendedLocation(string activity)
{
            List<LocationModel> Locations = GetLocations();

            LocationModel? location = null;
            if (Locations.Count() > 0)
            {
                switch (activity)
                {
                    case "Swimming":
                        location = Locations.OrderByDescending(x => x.Timeserie?.Data.Instant.Details.Air_temperature).First();
                        break;
                    case "Sailing":
                        location = Locations.OrderByDescending(x => x.Timeserie?.Data.Instant.Details.Wind_speed).First();
                        break;
                    case "Skiing":
                        location = Locations.OrderBy(x => x.Timeserie?.Data.Instant.Details.Air_temperature).First();
                        break;
                    case "Sightseeing":
                        location = Locations.OrderBy(x => x.Timeserie?.Data.Instant.Details.Air_pressure_at_sea_level).First();
                        break;
                    case "Unspecified":
                        var index = new Random().Next(Locations.Count);
                        location = Locations[index];
                        break;
                    default:
                        return null;
                }
                return location;

            }
            else
            {
                return null;
            }
    }
            
 ```
 
 This method should have at least three different unit tests: 
 1. An input from the list of activities
 2. An input which are not from the list, ex: "" (empty string)
 3. Calling the method when there is no available locations in the database.

**Try to write these three different tests by yourself.**
Tips: For the latter test, the following line can be useful: 
```C#
_mockRepository.Setup(mr => mr.GetLocations()).Returns(new List<LocationEntity>());
```




Next up - [Integration testing](02-service-layer.md)
