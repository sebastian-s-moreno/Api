using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forte.Weather.DataAccess.Repository;
using Forte.Weather.DataAccess.Schema;
using Forte.Weather.Services;
using Forte.Weather.Services.Implementation;
using Forte.Weather.Services.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Yr.Facade;
using Yr.Facade.Models;

namespace Forte.Weather.Tests.Services
{
    [TestClass]
    public class LocationServiceTest
    {
        private ILocationService _locationService;
        private Mock<ILocationRepository> _locationRepositoryMock;
        private Mock<IYrFacade> _yrFacadeMock;

        [TestInitialize]
        public void Initialize()
        {
            // create some mock products
            var locations = GetLocations();

            _locationRepositoryMock = new Mock<ILocationRepository>();
            _yrFacadeMock = new Mock<IYrFacade>();

            // Return all the locations
            _locationRepositoryMock.Setup(mr => mr.GetLocations()).Returns(locations);

            // return a location by Id
            _locationRepositoryMock.Setup(mr => mr.GetLocation(
                It.IsAny<string>())).Returns((string i) => locations.Single(x => x.ID == i));

            _locationService = new LocationService(_locationRepositoryMock.Object, _yrFacadeMock.Object);
        }

        [TestMethod]
        public void CreateLocation()
        {
            // Arrange
            var location = new Location
            {
                Name = "Oslo",
                Latitude = 11,
                Longitude = 15
            };

            // Act
            var result = _locationService.AddLocation(location).Result;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DeleteLocation()
        {
            // Act
            var result = _locationService.DeleteLocation("5e525c01-d4c8-4c35-a7da-f4ad7b19dd59");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetDetailsCorrectID()
        {
            // Act
            var result = _locationService.GetUpdatedDetails("5e525c01-d4c8-4c35-a7da-f4ad7b19dd59");

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void GetDetailsCorrectCoordinates()
        {
            // Act
            var result = _locationService.GetUpdatedDetails(5, 5);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetDetailsInvalidString()
        {
            // Act
            var result = _locationService.GetUpdatedDetails("123").Result;

            // Assert
            Assert.IsNull(result);
        }
        [TestMethod]
        public void GetDetailsInvalidCoordinates()
        {
            // Arrange
            _yrFacadeMock.Setup(x => x.GetYrResponse(It.IsAny<string>())).Returns(Task.FromResult<Details?>(null));

            // Act
            var result = _locationService.GetUpdatedDetails(100, 800).Result;

            // Assert
            Assert.IsNull(result.AirPressureAtSeaLevel);
            Assert.IsNull(result.AirTemperature);
            Assert.IsNull(result.RelativeHumidity);
            Assert.IsNull(result.WindFromDirection);
            Assert.IsNull(result.WindSpeed);
        }
        [TestMethod]
        public void GetDetailsNoInput()
        {
            // Act
            var result = _locationService.GetUpdatedDetails(null, null).Result;

            // Assert
            Assert.IsNull(result);
        }

        private static List<LocationEntity> GetLocations()
        {
            return new List<LocationEntity>
            {
                new()
                {
                    ID= "040958d7-e085-4748-8518-8a23292c114b",
                    Name= "Oslo",
                    Latitude=59,
                    Longitude= 11,
                    AirPressureAtSeaLevel = 1001,
                    AirTemperature = 5.6,
                    RelativeHumidity = 95.6,
                    WindFromDirection = 216.6,
                    WindSpeed = 13},
                new()
                {
                    ID= "6db14abf-f819-4816-99c0-3f11592603aa",
                    Name= "Trondheim",
                    Latitude=63,
                    Longitude= 10,
                    AirPressureAtSeaLevel = 997.1,
                    AirTemperature = -1.6,
                    RelativeHumidity = 95.9,
                    WindFromDirection = 319.4,
                    WindSpeed = 9.3},
                new()
                {
                    ID= "5e525c01-d4c8-4c35-a7da-f4ad7b19dd59",
                    Name= "Bergen",
                    Latitude=60,
                    Longitude= 5,
                    AirPressureAtSeaLevel = 997.1,
                    AirTemperature = -1.6,
                    RelativeHumidity = 95.9,
                    WindFromDirection = 319.4,
                    WindSpeed= 9.3}
            };
        }
    }
}
