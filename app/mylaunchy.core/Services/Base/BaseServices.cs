using Microsoft.Extensions.Logging;

namespace mylaunchy.unittests.Core.Services.Base
{
    public partial class BaseServices
    {
        protected readonly ILogger<BaseServices> _logger;

        public BaseServices(ILogger<BaseServices> logger)
        {
            _logger = logger;
        }
    }
}
