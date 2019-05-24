using mylaunchy.core.Models.Launchpad;
using System.Collections.Generic;

namespace mylaunchy.repository.spacexapi.Deserializers
{
    public interface IJsonResponseDeserializer
    {
        LaunchpadViewModel DeserializeLaunchpadResponse(string json);
        IEnumerable<LaunchpadViewModel> DeserializeLaunchpadCollectionResponse(string json);
    }
}
