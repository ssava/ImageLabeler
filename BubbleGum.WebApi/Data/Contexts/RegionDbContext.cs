using BubbleGum.WebApi.Data.Configurations;
using BubbleGum.WebApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BubbleGum.WebApi.Data.Contexts
{
    public class RegionDbContext : DbContext
    {
        public DbSet<LabeledRegion> Regions { get; set; }

        public RegionDbContext(DbContextOptions<RegionDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LabeledRegionEntityTypeConfiguration).Assembly);
        }
    }
}
