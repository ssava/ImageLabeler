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
        Task<ILabeledRegion> GetRegionByIdAsync(Guid regionId);
    }

    public sealed class RegionService : IRegionService
    {
        private readonly BubbleGumDbContext _context;

        public RegionService(BubbleGumDbContext context) =>
            _context = context;

        public async Task<ILabeledRegion> GetRegionByIdAsync(Guid regionId)
        {
            var region = await _context.Regions
                .Where(r => r.Id == regionId)
                .Select(r => LabeledRegion.CreateDto(r))
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return region;
        }
    }
}
