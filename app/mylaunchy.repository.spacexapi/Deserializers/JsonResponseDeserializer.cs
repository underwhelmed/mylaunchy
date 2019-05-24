using mylaunchy.core.Models.Launchpad;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace mylaunchy.repository.spacexapi.Deserializers
{
    public class JsonResponseDeserializer : IJsonResponseDeserializer
    {

        public IEnumerable<LaunchpadViewModel> DeserializeLaunchpadCollectionResponse(string json)
        {
            var launchpads = new List<LaunchpadViewModel>();
            if (!string.IsNullOrEmpty(json))
            {
                foreach (var launchpad in JsonConvert.DeserializeObject<IEnumerable<Launchpad>>(json))
                {
                    launchpads.Add(ConvertToLaunchpadViewModel(launchpad));
                }
            }
            return launchpads;
        }

        public LaunchpadViewModel DeserializeLaunchpadResponse(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                var jsonObject = JsonConvert.DeserializeObject<Launchpad>(json);
                return ConvertToLaunchpadViewModel(jsonObject);
            }
            else
            {
                return null;
            }
        }

        // TODO: move to mapping package (e.g.: automapper) based on performance/need
        LaunchpadViewModel ConvertToLaunchpadViewModel(Launchpad launchpad)
        {
            var l = new LaunchpadViewModel() { Id = launchpad.id, Name = launchpad.full_name, Status = launchpad.status };
            return string.IsNullOrEmpty(l.Id) ? null : l;
        }

        private class Launchpad
        {
            public string id { get; set; }
            public string full_name { get; set; }
            public string status { get; set; }
        }
    }
}
