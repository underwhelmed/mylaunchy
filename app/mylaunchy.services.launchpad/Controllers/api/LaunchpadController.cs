using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mylaunchy.core.Models.Launchpad;
using mylaunchy.core.Services.Launchpad;
using mylaunchy.services.common.Base;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mylaunchy.services.launchpad.Controllers.api
{
    [Route("launchpads")]
    [Produces("application/json")]    
    public class LaunchpadController : BaseController
    {
        private readonly ILaunchpadRetrievalServices _launchpadRetrievalServices;

        public LaunchpadController(ILaunchpadRetrievalServices launchpadRetrievalServices, ILogger<BaseController> logger) : base(logger)
        {
            _launchpadRetrievalServices = launchpadRetrievalServices;    
        }

        [HttpGet]
        [Route("")]        
        [SwaggerResponse(200, "Gets all launchpad information", typeof(IEnumerable<LaunchpadViewModel>))]
        public async Task<ActionResult<IEnumerable<LaunchpadViewModel>>> GetAll(
            [FromQuery, SwaggerParameter("Optional search keywords (comma delimited)")]string keywords = null)
        {
            _logger.LogDebug("Index endpoint requested");
            string[] keywordArray = null;
            if (!string.IsNullOrEmpty(keywords)) keywordArray = keywords.ToLower().Split(",");
            var launchpads = await _launchpadRetrievalServices.GetAllAsync(keywordArray);            
            return Ok(launchpads);
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerResponse(200, "Gets all information for the specified launchpad", typeof(LaunchpadViewModel))]
        [SwaggerResponse(204, "Launchpad could not be found by id")]
        public async Task<ActionResult<LaunchpadViewModel>> GetById(string id)
        {
            _logger.LogDebug("Get endpoint requested");
            var launchpad = await _launchpadRetrievalServices.GetByIdAsync(id);
            if (launchpad != null)
            {
                _logger.LogDebug("Successfully retrieved launchpad", launchpad);
                return Ok(launchpad);
            }
            else
            {
                _logger.LogDebug($"Unable to find launchpad id '{id}'");
                return NoContent();
            }
        }
    }
}