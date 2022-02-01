using System.Collections.Generic;
using Forte.Weather.Services;
using Forte.Weather.Services.Implementation;
using Forte.Weather.Services.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Forte.Weather.Tests.Services
{
    [TestClass]
    public class RecommendationServiceTest
    {
        private IRecommendationService _recommendationService;
        private Mock<ILocationService> _locationServiceMock;

        [TestInitialize]
        public void Initialize()
        {
            _locationServiceMock = new Mock<ILocationService>();
            _locationServiceMock.Setup(x => x.GetLocations()).Returns(GetLocations());

            _recommendationService = new RecommendationService(_locationServiceMock.Object);
        }


        [TestMethod]
        public void GetRecommendedLocation_Sailing_ReturnsMostWindyLocation()
        {
            // Arrange

            // Act
            var location = _recommendationService.GetRecommendedLocation(Activity.Sailing);

            // Assert
            Assert.IsNotNull(location);
            Assert.AreEqual("1", location.Id);
        }

        [TestMethod]
        public void GetRecommendationWithLocations()
        {
            var result = _recommendationService.GetRecommendedLocation(Activity.Swimming);
            Assert.AreEqual("Oslo", result?.Name);
        }

        [TestMethod]
        public void GetRecommendationInvalidChoice()
        {
            //var result = _locationService.GetRecommendedLocation(null);
            //Default case slår ut
            //Assert.IsNull(result);
        }

        //[TestMethod]
        //public void GetRecommendationWithNoneLocations()
        //{
        //    _locationRepositoryMock.Setup(mr => mr.GetLocations()).Returns(new List<LocationEntity>());
        //    var result = _locationService.GetRecommendedLocation(Activity.Swimming);
        //    Assert.IsNull(result);
        //}

        private static List<Location> GetLocations()
        {
            return new List<Location>
            {
                new()
                {
                    Id = "1",
                    Latitude = 10,
                    Longitude = 20,
                    Name = "Oslo",
                    WeatherData = new Weather.Services.Models.Weather
                    {
                        AirPressureAtSeaLevel = 40,
                        AirTemperature = 30,
                        RelativeHumidity = 30,
                        WindFromDirection = 180,
                        WindSpeed = 5
                    }
                }
            };
        }
    }
}
