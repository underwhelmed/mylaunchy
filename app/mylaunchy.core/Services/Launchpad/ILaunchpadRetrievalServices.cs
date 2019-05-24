using mylaunchy.core.Models.Launchpad;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mylaunchy.core.Services.Launchpad
{
    public interface ILaunchpadRetrievalServices
    {
        Task<IEnumerable<LaunchpadViewModel>> GetAllAsync(string[] keywords);        
        Task<LaunchpadViewModel> GetByIdAsync(string id);
    }
}
