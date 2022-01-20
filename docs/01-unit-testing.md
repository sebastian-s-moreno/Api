# Unit testing
In this session, you will extend the 
In this session you learn to cuztomize and improve your api documentation
![customized api](customized-api.PNG)

### Get Weather Details from an external API
Now, we will create an method to retrieve weather data from an external source. We will be using the API from Yr. You can read more about the API here: https://developer.yr.no/doc/

Add the following method to your LocationService. To use the API from yr, you need to pass the coordinates (longitude and latitude) of a place. This method can be used by either coordinates, or the ID of a saved location. If you are using an ID, the location will be retrieved from the database.
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

When testing an method, it is important to test the different possibilities of input. You often start by creating tests for the expected input, then you need to think about what else an user can try to use as input. Add the following tests to your test file. Try to run them. Several of them will fail. Can you see why? Try to edit the GetUpdatedDetails() method, and make sure all the tests pass. 

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
Add the following code to the GetRecommendedLocation method in the LocationService: 

```C#
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
            
 ```
 
 This method should have at least three different unit tests: 
 1. An input from the list of activities
 2. An input which are not from the list, ex: "" (empty string)
 3. Calling the method when there is no available locations in the database.

Try to write these three different tests by yourself. 
Tips: For the latter test, the following line can be useful: 
```C#
_mockRepository.Setup(mr => mr.GetLocations()).Returns(new List<LocationEntity>());
```




Next up - [Integration testing](02-service-layer.md)
