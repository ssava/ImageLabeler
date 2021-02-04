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
        /// Returns a labeled region by its id
        /// </summary>
        /// <param name="id">Region Id</param>
        /// <response code="200">Requested region</response>
        /// <response code="404">If the region id was not found</response>   
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<ILabeledRegion>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRegionFrom([FromRoute] Guid id)
        {
            var region = await _regionService.GetRegionByIdAsync(id);

            return region == null ? NotFound() : Ok(region);
        }
    }
}
