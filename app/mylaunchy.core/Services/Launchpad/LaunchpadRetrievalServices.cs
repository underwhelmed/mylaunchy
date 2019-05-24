using Microsoft.Extensions.Logging;
using mylaunchy.core.Models.Launchpad;
using mylaunchy.core.Repository.Launchpad;
using mylaunchy.unittests.Core.Services.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mylaunchy.core.Services.Launchpad
{
    public class LaunchpadRetrievalServices : BaseServices, ILaunchpadRetrievalServices
    {
        private readonly ILaunchpadRepository _launchpadRepository;

        public LaunchpadRetrievalServices(ILaunchpadRepository launchpadRepository, ILogger<BaseServices> logger) : base(logger)
        {
            _launchpadRepository = launchpadRepository;
        }

        public async Task<IEnumerable<LaunchpadViewModel>> GetAllAsync(string[] keywords = null)
        {
            IEnumerable<LaunchpadViewModel> launchpads = null;
            launchpads = await _launchpadRepository.GetAllAsync(); 
            if (keywords != null) launchpads = launchpads.Where(x => keywords.Any(s => x.Name.ToLower().Contains(s) || x.Status.ToLower().Contains(s)));
            return launchpads;
        }
        
        public async Task<LaunchpadViewModel> GetByIdAsync(string id)
        {
            return await _launchpadRepository.GetByIdAsync(id);
        }
    }
}
