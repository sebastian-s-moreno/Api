using Forte.Weather.Api.Controllers;
using Forte.Weather.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Forte.Weather.Services.Models;

namespace Forte.Weather.Tests.Controllers
{
    [TestClass]
    public class LocationControllerTest
    {
        private LocationController _controller;
        private Mock<ILocationService> _locationServiceMock;
        private Mock<IRecommendationService> _recommendationServiceMock;

        [TestInitialize]
        public void Initialize()
        {
            _locationServiceMock = new Mock<ILocationService>();
            _controller = new LocationController(_locationServiceMock.Object, _recommendationServiceMock.Object);

            _locationServiceMock.Setup(x => x.GetLocation("abc")).Returns(new Location { Id = "abc" });
        }


    }
}
