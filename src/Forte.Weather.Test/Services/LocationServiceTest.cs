using Forte.Weather.DataAccess.Repository;
using Forte.Weather.DataAccess.Schema;
using Forte.Weather.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Forte.Weather.Test.Services
{
    [TestClass]
    public class LocationServiceTest
    {
        private ILocationService _service;
        private Mock<ILocationRepository> _mockRepository;

        [TestInitialize]
        public void Initialize()
        {
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
            // create some mock products
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

            
            _mockRepository = new Mock<ILocationRepository>();

            // Return all the locations
            _mockRepository.Setup(mr => mr.GetLocations()).Returns(locations);

            // return a location by Id
            _mockRepository.Setup(mr => mr.GetLocation(
                It.IsAny<string>())).Returns((string i) => locations.Where(
                x => x.ID == i).Single());

            _service = new LocationService(_mockRepository.Object);
        }

        [TestMethod]
        public void CreateLocation()
        {
            LocationModel location = new LocationModel
            {
                Name = "Oslo",
                Latitude = 11,
                Longitude = 15
            };
            var result = _service.AddLocation(location).Result;
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void DeleteLocation()
        {
            var result = _service.DeleteLocation("5e525c01-d4c8-4c35-a7da-f4ad7b19dd59");
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void GetRecommendationWithLocation()
        {
            var result = _service.GetRecommendedLocation("Swimming");
            Assert.AreEqual("Oslo",result?.Name);
        }

        [TestMethod]
        public void GetRecommendationWithNoneLocations()
        {
            _mockRepository.Setup(mr => mr.GetLocations()).Returns(new List<LocationEntity>());
            var result = _service.GetRecommendedLocation("Swimming");
            Assert.IsNull(result);
        }
        [TestMethod]
        public void GetRecommendationInvalidChoice()
        {
            var result = _service.GetRecommendedLocation("");
            //Default case slå ut
            Assert.IsNull(result);
        }

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


    }
}
