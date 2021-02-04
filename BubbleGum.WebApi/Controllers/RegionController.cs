using BubbleGum.WebApi.Models;
using BubbleGum.WebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BubbleGum.WebApi.Controllers
{
    [ApiController]
    [Route("region")]
    public class RegionController : ControllerBase
    {
        private readonly ILogger<RegionController> _logger;
        private readonly IRegionService _regionService;

        public RegionController(IRegionService regionService, ILogger<RegionController> logger)
        {
            _logger = logger;
            _regionService = regionService;
        }

        /// <summary>
        /// Returns image regions used by training model.
        /// </summary>
        /// <param name="imageId">Id of the image</param>
        /// <response code="200">Regions contained in a image</response>
        /// <response code="404">If the image id was not found</response>   
        [HttpGet("byImage/{imageId}")]
        [ProducesResponseType(typeof(IEnumerable<ILabeledRegion>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTrainingRegions([FromRoute] Guid imageId)
        {
            var regions = await _regionService.GetRegionsByImageIdAsync(imageId);

            return !regions.Any() ? NotFound() : Ok(regions);
        }
    }
}
