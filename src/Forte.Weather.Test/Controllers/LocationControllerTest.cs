using Forte.Weather.Api.Controllers;
using Forte.Weather.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Forte.Weather.Tests.Controllers
{
    [TestClass]
    public class LocationControllerTest
    {
        private LocationController _controller;
        private Mock<ILocationService> _mockService;

        [TestInitialize]
        public void Initialize()
        {
            _mockService = new Mock<ILocationService>();
            _controller = new LocationController(_mockService.Object);

            _mockService.Setup(x => x.GetLocation("abc")).Returns(new LocationModel { ID = "abc" });
        }


    }
}
