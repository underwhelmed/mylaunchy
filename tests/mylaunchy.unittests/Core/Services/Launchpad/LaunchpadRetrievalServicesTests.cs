using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using mylaunchy.core.Models.Launchpad;
using mylaunchy.core.Repository.Launchpad;
using mylaunchy.core.Services.Launchpad;
using mylaunchy.unittests.Core.Services.Base;
using mylaunchy.unittests.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace mylaunchy.unittests.Core.Services.Launchpad
{
    public class LaunchpadRetrievalServicesTests
    {
        private Mock<ILaunchpadRepository> _launchpadRepository;
        Mock<ILogger<BaseServices>> _logger;

        public LaunchpadRetrievalServicesTests()
        {
            _launchpadRepository = new Mock<ILaunchpadRepository>();
            _logger = new Mock<ILogger<BaseServices>>();
        }

        LaunchpadRetrievalServices GetService()
        {
            return new LaunchpadRetrievalServices(_launchpadRepository.Object, _logger.Object);
        }
        
        [Fact]
        public async Task GetAll_RetrievesSuccessfully()
        {
            _launchpadRepository.Setup(mock => mock.GetAllAsync()).ReturnsAsync(DataGeneration.GenerateLaunchpadCollection(3));

            var svcResult = await GetService().GetAllAsync();

            svcResult.Count().Should().Be(3);

            VerifyAll();
        }

        [Fact]
        public async Task GetAll_WithKeywordFilterMatch_Success()
        {            
            var launchpads = new List<LaunchpadViewModel>()
            {
                DataGeneration.GenerateLaunchpad(name: "Common Name"),
                DataGeneration.GenerateLaunchpad(name: "This is in Common"),
                DataGeneration.GenerateLaunchpad(name: "Nope, don't return this"),
            };

            _launchpadRepository.Setup(mock => mock.GetAllAsync()).ReturnsAsync(launchpads);

            var svcResult = await GetService().GetAllAsync(new string[] { "common" });

            svcResult.Count().Should().Be(2);

            VerifyAll();
        }

        [Fact]
        public async Task GetAll_WithMultipleKeywordFilterMatch_Success()
        {
            var launchpads = new List<LaunchpadViewModel>()
            {
                DataGeneration.GenerateLaunchpad(name: "Common Name"),
                DataGeneration.GenerateLaunchpad(name: "This is in Common"),
                DataGeneration.GenerateLaunchpad(name: "Maybe this one should return"),
            };

            _launchpadRepository.Setup(mock => mock.GetAllAsync()).ReturnsAsync(launchpads);

            var svcResult = await GetService().GetAllAsync(new string[] { "common", "should" });

            svcResult.Count().Should().Be(3);

            VerifyAll();
        }

        [Fact]
        public async Task GetAll_WithKeywordFilterNoMatch_ReturnsEmpty()
        {
            var launchpads = new List<LaunchpadViewModel>()
            {
                DataGeneration.GenerateLaunchpad(name: "Common Name"),
                DataGeneration.GenerateLaunchpad(name: "This is in Common"),
                DataGeneration.GenerateLaunchpad(name: "Nope, don't return this"),
            };

            _launchpadRepository.Setup(mock => mock.GetAllAsync()).ReturnsAsync(launchpads);

            var svcResult = await GetService().GetAllAsync(new string[] { "tacos" });

            svcResult.Count().Should().Be(0);

            VerifyAll();
        }

        [Fact]
        public async Task GetAll_WithStatusFilterMatch_Success()
        {
            var launchpads = new List<LaunchpadViewModel>()
            {
                DataGeneration.GenerateLaunchpad(status: "active"),
                DataGeneration.GenerateLaunchpad(status: "active"),
                DataGeneration.GenerateLaunchpad(status: "under construction"),
            };

            _launchpadRepository.Setup(mock => mock.GetAllAsync()).ReturnsAsync(launchpads);

            var svcResult = await GetService().GetAllAsync(new string[] { "active" });

            svcResult.Count().Should().Be(2);

            VerifyAll();
        }

        [Fact]
        public async Task GetAll_WithStatusAndNameFilterMatch_Success()
        {
            var launchpads = new List<LaunchpadViewModel>()
            {
                DataGeneration.GenerateLaunchpad(status: "active"),
                DataGeneration.GenerateLaunchpad(status: "active"),
                DataGeneration.GenerateLaunchpad(status: "under construction", name: "Taco and Burrito Launchpad"),
                DataGeneration.GenerateLaunchpad(status: "under construction"),
            };

            _launchpadRepository.Setup(mock => mock.GetAllAsync()).ReturnsAsync(launchpads);

            var svcResult = await GetService().GetAllAsync(new string[] { "active", "taco" });

            svcResult.Count().Should().Be(3);

            VerifyAll();
        }

        [Fact]
        public async Task GetAll_WithStatusFilterNoMatch_ReturnsEmpty()
        {
            var launchpads = new List<LaunchpadViewModel>()
            {
                DataGeneration.GenerateLaunchpad(status: "active"),
                DataGeneration.GenerateLaunchpad(status: "active"),
                DataGeneration.GenerateLaunchpad(status: "under construction"),
            };

            _launchpadRepository.Setup(mock => mock.GetAllAsync()).ReturnsAsync(launchpads);

            var svcResult = await GetService().GetAllAsync(new string[] { "retired" });

            svcResult.Count().Should().Be(0);

            VerifyAll();
        }

        [Fact]
        public async Task GetById_RetrievesSuccessfully()
        {
            const string id = "12345";
            _launchpadRepository.Setup(mock => mock.GetByIdAsync(id)).ReturnsAsync(DataGeneration.GenerateLaunchpad());

            var svcResult = await GetService().GetByIdAsync(id);

            svcResult.Should().NotBeNull();

            VerifyAll();
        }

        void VerifyAll()
        {
            _launchpadRepository.VerifyAll();
        }

    }
}
