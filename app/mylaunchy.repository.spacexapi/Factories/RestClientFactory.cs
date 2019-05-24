using RestSharp;

namespace mylaunchy.repository.spacexapi.Factories
{
    public class RestClientFactory : IRestClientFactory
    {
        public RestClient Create(string baseUrl)
        {            
            if (string.IsNullOrEmpty(baseUrl))  baseUrl = "https://api.spacexdata.com/v2/";
            return new RestClient(baseUrl);
        }
    }
}
