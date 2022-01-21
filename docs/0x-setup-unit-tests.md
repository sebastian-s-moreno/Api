# Setup for Unit tests

## Create the test project
Create a new project in your solution. Choose the MSTest Test Project template. 

![image](https://user-images.githubusercontent.com/25482321/150508910-02a1e4e2-20d2-42b6-9cc3-d07e55f22aef.png)

To be able to use the classes and methods you have created, these projects must be added as dependencies. 
Inside your new project, right-click on dependencies and choose "Add project reference".  
You can include both Forte.Weather.API, Forte.Weather.Services and Forte.Weather.DataAccess. 

## Create the first test

Look at the automatically created file *UnitTest1.cs*. It shows you have to structure your test file. 
The TestClass attribute denotes a class that contains unit tests. The TestMethod attribute indicates a method is a test method.

It is good practice to structure your tests the same way as you structure your code. Therefore, create a folder named *Services*, and add a file named *LocationServiceTest.cs*

TestInitialize 

## Get started with mocking 

The LocationService is dependent of the LocationRepository which is used in most of the methods. As we want to focus on the LocationService, and not the behaviour of the LocationRepository, we need to isolate the LocationService. We create a "fake" replacement object, which simulates the behaviour of the real one. This is called a Mock. For this we will use the mocking librabry, Moq. 

Again, inside your new project, right-click on dependencies and choose "Manage NuGet Packages". Search for *Moq* and install the latest stable version. 
![image](https://user-images.githubusercontent.com/25482321/150527932-90fbc62b-edfc-43ad-83ef-368242641ba7.png)
Remember to include 
```
using Moq; 
``` 
at the top of your test class. 

### Create a mock
The mock is created by using the following code:
```C#
private Mock<ILocationRepository> _mockRepository;
```

```C#
_mockRepository = new Mock<ILocationRepository>();
```
Where the latter should be inside the TestInitialize method. 

### Instruct the mock
After you have a mock object, you need to instruct it to behave as the object it replaces. This is done by using the method ```Setup()```. 

```C#
// Return all the locations
_mockRepository.Setup(mr => mr.GetLocations()).Returns(locations);

// return a location by Id
_mockRepository.Setup(mr => mr.GetLocation(It.IsAny<string>())).Returns((string i) => locations.Where(x => x.ID == i).Single());
```
We need to give the Mock the attributes which is needed. This means we need to define some locations: 
```C#
var ts1 = JsonConvert.SerializeObject(new TimeSerie
{
    Data = new Data
    {
        Instant = new Instant
        {
            Details = new Details
            {
                Air_pressure_at_sea_level = 1001,
                Air_temperature = 5.6,
                Relative_humidity = 95.6,
                Wind_from_direction = 216.6,
                Wind_speed = 13
            }
        }
    },
    Time = System.DateTimeOffset.Now
});
var ts2 = JsonConvert.SerializeObject(new TimeSerie
{
    Data = new Data
    {
        Instant = new Instant
        {
            Details = new Details
            {
                Air_pressure_at_sea_level = 997.1,
                Air_temperature = -1.6,
                Relative_humidity = 95.9,
                Wind_from_direction = 319.4,
                Wind_speed = 9.3
            }
        }
    },
    Time = System.DateTimeOffset.Now
});
var ts3 = JsonConvert.SerializeObject(new TimeSerie
{
    Data = new Data
    {
        Instant = new Instant
        {
            Details = new Details
            {
                Air_pressure_at_sea_level = 997.1,
                Air_temperature = -1.6,
                Relative_humidity = 95.9,
                Wind_from_direction = 319.4,
                Wind_speed = 9.3
            }
        }
    },
    Time = System.DateTimeOffset.Now
});
List<LocationEntity> locations = new List<LocationEntity>
{
    new LocationEntity  {
        ID= "040958d7-e085-4748-8518-8a23292c114b",
        Name= "Oslo",
        Latitude=59,
        Longitude= 11,
        Timeserie = ts1},
    new LocationEntity  {
        ID= "6db14abf-f819-4816-99c0-3f11592603aa",
        Name= "Trondheim",
        Latitude=63,
        Longitude= 10,
        Timeserie=ts2},
    new LocationEntity  {
        ID= "5e525c01-d4c8-4c35-a7da-f4ad7b19dd59",
        Name= "Bergen",
        Latitude=60,
        Longitude= 5,
        Timeserie=ts3}
};
```

