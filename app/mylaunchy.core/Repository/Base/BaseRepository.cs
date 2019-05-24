using Microsoft.Extensions.Logging;

namespace mylaunchy.core.Repository.Base
{
    public partial class BaseRepository
    {
        protected readonly ILogger<BaseRepository> _logger;

        public BaseRepository(ILogger<BaseRepository> logger)
        {
            _logger = logger;
        }
    }
}
