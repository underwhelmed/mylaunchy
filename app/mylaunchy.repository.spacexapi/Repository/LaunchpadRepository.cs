using Microsoft.Extensions.Logging;
using mylaunchy.core.Models.Launchpad;
using mylaunchy.core.Repository.Base;
using mylaunchy.core.Repository.Launchpad;
using mylaunchy.repository.spacexapi.Deserializers;
using mylaunchy.repository.spacexapi.Factories;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace mylaunchy.repository.spacexapi
{
    public class LaunchpadRepository : BaseRepository, ILaunchpadRepository
    {
        private readonly IRestClientFactory _restClientFactory;
        private readonly IRestRequestFactory _restRequestFactory;
        private readonly IJsonResponseDeserializer _deserializer;
        private readonly string _baseUrl;
        private const string SPACEX_API_LAUNCHPAD_FIELD_FILTER = "id,full_name,status";

        public LaunchpadRepository(IRestClientFactory restClientFactory, IRestRequestFactory restRequestFactory, IJsonResponseDeserializer deserializer, ILogger<BaseRepository> logger) : base(logger)
        {
            _restClientFactory = restClientFactory;
            _restRequestFactory = restRequestFactory;
            _deserializer = deserializer;
            _baseUrl = Environment.GetEnvironmentVariable("SPACEX_API_BASE_URL");
        }

        public async Task<IEnumerable<LaunchpadViewModel>> GetAllAsync()
        {            
            var client = _restClientFactory.Create(_baseUrl);
            var request = _restRequestFactory.Create($"launchpads?filter={SPACEX_API_LAUNCHPAD_FIELD_FILTER}", Method.GET);

            var response = await GetRequestAsync(client, request);
            var json = ValidateResponse(response);

            return _deserializer.DeserializeLaunchpadCollectionResponse(json);            
        }

        public async Task<LaunchpadViewModel> GetByIdAsync(string id)
        {
            var client = _restClientFactory.Create(_baseUrl);
            var request = _restRequestFactory.Create($"launchpads/{id}?filter={SPACEX_API_LAUNCHPAD_FIELD_FILTER}", Method.GET);

            var response = await GetRequestAsync(client, request);
            var json = ValidateResponse(response);

            return _deserializer.DeserializeLaunchpadResponse(json);
        }

        #region private methods

        Task<IRestResponse> GetRequestAsync(RestClient client, RestRequest request)
        {
            return Task.Run(() => { return client.Execute(request); });            
        }

        string ValidateResponse(IRestResponse response)
        {
            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return response.Content;                        
                    case HttpStatusCode.NoContent:
                        return null;                        
                    default:
                        throw new Exception($"API Error - HTTP Status Code {(int)response.StatusCode}");                        
                }                
            }
            else
            {
                throw new Exception($"API Request Error - Request {response.ResponseStatus}");
            }
        }
        
#endregion
    }
}
