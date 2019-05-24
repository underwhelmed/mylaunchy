using mylaunchy.core.Models.Launchpad;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mylaunchy.core.Repository.Launchpad
{
    public interface ILaunchpadRepository
    {
        Task<LaunchpadViewModel> GetByIdAsync(string id);
        Task<IEnumerable<LaunchpadViewModel>> GetAllAsync();
    }
}
