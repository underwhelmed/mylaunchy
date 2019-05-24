using Bogus;
using mylaunchy.core.Models.Launchpad;
using System.Collections.Generic;

namespace mylaunchy.unittests.Helpers
{
    public static class DataGeneration
    {
        public static ICollection<LaunchpadViewModel> GenerateLaunchpadCollection(int count)
        {
            var launchpads = new List<LaunchpadViewModel>();
            for (int i = 0; i < count; i++)
            {
                launchpads.Add(GenerateLaunchpad());
            }
            return launchpads;
        }
        
        public static LaunchpadViewModel GenerateLaunchpad(string name = null, string status = null)
        {
            return new Faker<LaunchpadViewModel>()
                .RuleFor(u => u.Id, f => $"{f.UniqueIndex}")
                .RuleFor(u => u.Name, (f, u) => name ?? f.Lorem.Sentence())
                .RuleFor(u => u.Status, (f, u) => status ?? f.PickRandom(new string[] { "active", "under construction", "retired" }));
        }

    }
}
