using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using mylaunchy.core.Models.Launchpad;
using mylaunchy.core.Services.Launchpad;
using mylaunchy.services.common.Base;
using mylaunchy.services.launchpad.Controllers.api;
using mylaunchy.unittests.Helpers;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace mylaunchy.unittests.Microservices.Launchpad
{
    public class LaunchpadControllerTests 
    {        
        Mock<ILogger<BaseController>> _logger;
        Mock<ILaunchpadRetrievalServices> _launchpadRetrievalServices;

        public LaunchpadControllerTests()
        {
            _logger = new Mock<ILogger<BaseController>>();
            _launchpadRetrievalServices = new Mock<ILaunchpadRetrievalServices>();
        }

        LaunchpadController GetController()
        {
            return new LaunchpadController(_launchpadRetrievalServices.Object, _logger.Object);
        }

        [Fact]
        public async void GetAll_ReturnsLaunchpadsResult()
        {            
            _launchpadRetrievalServices.Setup(mock => mock.GetAllAsync(null)).ReturnsAsync(DataGeneration.GenerateLaunchpadCollection(5));
                        
            var response = await GetController().GetAll();
            response.Result.Should().BeOfType<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().NotBeNull();
            var launchpads = (IEnumerable<LaunchpadViewModel>)result.Value;
            launchpads.Count().Should().Be(5);            

            VerifyAll();
        }

        [Fact]
        public async void GetAll_WithKeywords_ReturnsLaunchpadsResult()
        {
            _launchpadRetrievalServices.Setup(mock => mock.GetAllAsync(new string[] { "active" })).ReturnsAsync(DataGeneration.GenerateLaunchpadCollection(2));

            var response = await GetController().GetAll("active");
            response.Result.Should().BeOfType<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().NotBeNull();
            var launchpads = (IEnumerable<LaunchpadViewModel>)result.Value;
            launchpads.Count().Should().Be(2);

            VerifyAll();
        }

        [Fact]
        public async void GetAll_WithMultipleKeywords_ReturnsLaunchpadsResult()
        {
            _launchpadRetrievalServices.Setup(mock => mock.GetAllAsync(new string[] { "active", "air force" })).ReturnsAsync(DataGeneration.GenerateLaunchpadCollection(2));

            var response = await GetController().GetAll("active,air force");
            response.Result.Should().BeOfType<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().NotBeNull();
            var launchpads = (IEnumerable<LaunchpadViewModel>)result.Value;
            launchpads.Count().Should().Be(2);

            VerifyAll();
        }

        [Fact]
        public async void GetAllById_ReturnsSingleLaunchpad()
        {            
            var id = "abc_342";
            LaunchpadViewModel lp = DataGeneration.GenerateLaunchpad();
            lp.Id = id;
            _launchpadRetrievalServices.Setup(mock => mock.GetByIdAsync(id)).ReturnsAsync(lp);

            var response = await GetController().GetById(id);

            response.Result.Should().BeOfType<OkObjectResult>();
            var result = (OkObjectResult)response.Result;
            result.Value.Should().NotBeNull();
            result.Value.Should().BeOfType<LaunchpadViewModel>();
            var launchpad = (LaunchpadViewModel)result.Value;
            launchpad.Id.Should().Be(id);

            VerifyAll();
        }

        [Fact]
        public async void GetAllById_InvalidId_ReturnsNotFound()
        {
            LaunchpadViewModel launchpad = null;
            var id = "NOT FOUND!!!";
            _launchpadRetrievalServices.Setup(mock => mock.GetByIdAsync(id)).ReturnsAsync(launchpad);

            var response = await GetController().GetById(id);

            response.Result.Should().BeOfType<NoContentResult>(); 
            VerifyAll();
        }

        #region private methods        

        void VerifyAll()
        {
            _launchpadRetrievalServices.VerifyAll();
            _logger.VerifyAll();
        }

        #endregion
    }
}
