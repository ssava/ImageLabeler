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
    [Route("image")]
    public class ImageController : ControllerBase
    {
        private readonly ILogger<ImageController> _logger;
        private readonly ITrainingImageService _imageService;

        public ImageController(ITrainingImageService imageService, ILogger<ImageController> logger)
        {
            _logger = logger;
            _imageService = imageService;
        }

        /// <summary>
        /// Returns a training data set image by its id
        /// </summary>
        /// <param name="id">Image Id</param>
        /// <response code="200">Requested region</response>
        /// <response code="404">If the image id was not found</response>   
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<ITrainingImage>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImageById([FromRoute] Guid id)
        {
            var image = await _imageService.GetImageByIdAsync(id);

            return image == null ? NotFound() : Ok(image);
        }

        /// <summary>
        /// Returns all labeled regions for a given image given its id.
        /// </summary>
        /// <param name="id">Id of the image</param>
        /// <response code="200">Regions contained in the given image</response>
        /// <response code="404">If the image id was not found</response>   
        [HttpGet("{id}/regions")]
        [ProducesResponseType(typeof(IEnumerable<ILabeledRegion>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRegionsForPage([FromRoute] Guid id)
        {
            var regions = await _imageService.GetRegionsByImageIdAsync(id);

            return !regions.Any() ? NotFound() : Ok(regions);
        }
    }
}
