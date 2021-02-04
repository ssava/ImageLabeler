using BubbleGum.WebApi.Data.Contexts;
using BubbleGum.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BubbleGum.WebApi.Services
{
    public interface ITrainingImageService
    {
        Task<ITrainingImage> GetImageByIdAsync(Guid imageId);
        Task<IEnumerable<ILabeledRegion>> GetRegionsByImageIdAsync(Guid imageId);
        Task<IEnumerable<ILabeledRegion>> GetRegionByImageAsync(ITrainingImage image);
    }

    public sealed class TrainingImageService : ITrainingImageService
    {
        private readonly BubbleGumDbContext _context;

        public TrainingImageService(BubbleGumDbContext context) =>
            _context = context;

        public async Task<ITrainingImage> GetImageByIdAsync(Guid imageId)
        {
            var image = await _context.Images
                .Where(i => i.Id == imageId)
                .Select(i => TrainingImage.CreateDto(i))
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return image;
        }

        public async Task<IEnumerable<ILabeledRegion>> GetRegionsByImageIdAsync(Guid imageId)
        {
            var regions = await _context.Regions
                .Where(r => r.ImageId == imageId)
                .Select(r => LabeledRegion.CreateDto(r))
                .ToListAsync()
                .ConfigureAwait(false);

            return regions;
        }

        public async Task<IEnumerable<ILabeledRegion>> GetRegionByImageAsync(ITrainingImage image)
        {
            var regions = await _context.Regions
                .Where(r => r.ImageId == image.Id)
                .Select(r => LabeledRegion.CreateDto(r))
                .ToListAsync()
                .ConfigureAwait(false);

            return regions;
        }
    }
}
