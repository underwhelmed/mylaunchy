using RestSharp;

namespace mylaunchy.repository.spacexapi.Factories
{
    public interface IRestRequestFactory
    {
        RestRequest Create(string url, Method method);
    }
}
