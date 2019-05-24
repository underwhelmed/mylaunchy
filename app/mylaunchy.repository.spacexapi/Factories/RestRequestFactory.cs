using RestSharp;

namespace mylaunchy.repository.spacexapi.Factories
{
    public class RestRequestFactory : IRestRequestFactory
    {
        public RestRequest Create(string url, Method method)
        {
            return new RestRequest(url, method);
        }
    }
}
