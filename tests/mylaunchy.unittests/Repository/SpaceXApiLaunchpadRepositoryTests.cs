using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using mylaunchy.core.Repository.Base;
using mylaunchy.repository.spacexapi;
using mylaunchy.repository.spacexapi.Deserializers;
using mylaunchy.repository.spacexapi.Factories;
using mylaunchy.unittests.Helpers;
using RestSharp;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace mylaunchy.unittests.Repository
{
    public class SpaceXApiLaunchpadRepositoryTests
    {
        Mock<ILogger<BaseRepository>> _logger;
        Mock<IRestClientFactory> _restClientFactory;
        Mock<IRestRequestFactory> _restRequestFactory;
        Mock<IJsonResponseDeserializer> _deserializer;

        private const string EXAMPLE_JSON = @"{""content"": ""okay""}";

        public SpaceXApiLaunchpadRepositoryTests()
        {
            _logger = new Mock<ILogger<BaseRepository>>();
            _restClientFactory = new Mock<IRestClientFactory>();
            _restRequestFactory = new Mock<IRestRequestFactory>();
            _deserializer = new Mock<IJsonResponseDeserializer>();
        }

        LaunchpadRepository GetRepository()
        {
            return new LaunchpadRepository(_restClientFactory.Object, _restRequestFactory.Object, _deserializer.Object, _logger.Object);
        }

        #region GetAll 

        [Fact]
        public async Task GetAllAsync_ParsesJsonAndReturnsObject()
        {
            SetupRestAPIReturn();
            _deserializer.Setup(mock => mock.DeserializeLaunchpadCollectionResponse(EXAMPLE_JSON)).Returns(DataGeneration.GenerateLaunchpadCollection(6));

            var launchpads = await GetRepository().GetAllAsync();
            launchpads.Should().NotBeNull();
            launchpads.ToList().Count().Should().Be(6);

            VerifyAll();
        }

        [Fact]
        public void GetAllAsync_APIError_ThrowsException()
        {
            SetupRestAPIReturn(HttpStatusCode.InternalServerError, ResponseStatus.Completed);

            Func<Task> a = async () => await GetRepository().GetAllAsync();
            a.Should().Throw<Exception>()
                .WithMessage("API Error - HTTP Status Code 500");

            VerifyAll();
        }

        [Fact]
        public void GetAllAsync_RequestTimeout_ThrowsException()
        {
            SetupRestAPIReturn(HttpStatusCode.RequestTimeout, ResponseStatus.TimedOut);

            Func<Task> a = async () => await GetRepository().GetAllAsync();
            a.Should().Throw<Exception>()
                .WithMessage("API Request Error - Request TimedOut");

            VerifyAll();
        }

        #endregion

        #region GetById

        [Fact]
        public async Task GetByIdAsync_ParsesJsonAndReturnsObject()
        {
            const string id = "34343";
            SetupRestAPIReturn();
            _deserializer.Setup(mock => mock.DeserializeLaunchpadResponse(EXAMPLE_JSON)).Returns(DataGeneration.GenerateLaunchpad());

            var launchpads = await GetRepository().GetByIdAsync(id);
            launchpads.Should().NotBeNull();
            
            VerifyAll();
        }

        [Fact]
        public void GetByIdAsync_APIError_ThrowsException()
        {
            const string id = "34343";
            SetupRestAPIReturn(HttpStatusCode.InternalServerError, ResponseStatus.Completed);

            Func<Task> a = async () => await GetRepository().GetByIdAsync(id);
            a.Should().Throw<Exception>()
                .WithMessage("API Error - HTTP Status Code 500");

            VerifyAll();
        }

        [Fact]
        public void GetByIdAsync_RequestTimeout_ThrowsException()
        {
            const string id = "34343";
            SetupRestAPIReturn(HttpStatusCode.RequestTimeout, ResponseStatus.TimedOut);

            Func<Task> a = async () => await GetRepository().GetByIdAsync(id);
            a.Should().Throw<Exception>()
                .WithMessage("API Request Error - Request TimedOut");

            VerifyAll();
        }

        #endregion


        void SetupRestAPIReturn(HttpStatusCode statusCode = HttpStatusCode.OK, ResponseStatus responseStatus = ResponseStatus.Completed)
        {
            var response = new RestResponse { StatusCode = statusCode, ResponseStatus = responseStatus};
            if (statusCode == HttpStatusCode.OK && responseStatus == ResponseStatus.Completed) response.Content = EXAMPLE_JSON;
            var restClient = new Mock<RestClient>();
            restClient.Setup(x => x.Execute(It.IsAny<IRestRequest>())).Returns(response);
            _restClientFactory.Setup(mock => mock.Create(null)).Returns(restClient.Object);
            _restRequestFactory.Setup(mock => mock.Create(It.IsAny<string>(), Method.GET)).Returns(new Mock<RestRequest>().Object);
        }

        void VerifyAll()
        {
            _restRequestFactory.VerifyAll();
            _restClientFactory.VerifyAll();
            _deserializer.VerifyAll();
        }

    }
}
