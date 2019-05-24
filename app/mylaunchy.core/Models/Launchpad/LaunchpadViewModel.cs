using Newtonsoft.Json;

namespace mylaunchy.core.Models.Launchpad
{
    public class LaunchpadViewModel
    {  
        [JsonProperty("Launchpad Id")]
        public string Id { get; set; }
        [JsonProperty("Launchpad Name")]
        public string Name { get; set; }
        [JsonProperty("Launchpad Status")]
        public string Status { get; set; }
    }
}
