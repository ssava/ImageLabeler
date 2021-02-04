using BubbleGum.WebApi.Data.Contexts;
using BubbleGum.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BubbleGum.WebApi.Services
{
    public interface IRegionService
    {
        Task<IEnumerable<ILabeledRegion>> GetRegionsByImageIdAsync(Guid imageId);
    }

    public sealed class RegionService : IRegionService
    {
        private readonly RegionDbContext _context;

        public RegionService(RegionDbContext context) =>
            _context = context;

        public async Task<IEnumerable<ILabeledRegion>> GetRegionsByImageIdAsync(Guid imageId)
        {
            var regions = await _context.Regions
                .Where(r => r.ImageId == imageId)
                .Select(r => CreateDto(r))
                .ToListAsync()
                .ConfigureAwait(false);

            return regions;
        }

        private static Models.LabeledRegion CreateDto(Data.Models.LabeledRegion region) =>
            new Models.LabeledRegion
            {
                Id = region.Id,
                Top = region.Top,
                Left = region.Left,
                Width = region.Width,
                Height = region.Height,
                Label = region.Label,
                ImageId = region.ImageId
            };
    }
}
