using RestSharp;

namespace mylaunchy.repository.spacexapi.Factories
{
    public interface IRestClientFactory
    {
        RestClient Create(string baseUrl);
    }
}
