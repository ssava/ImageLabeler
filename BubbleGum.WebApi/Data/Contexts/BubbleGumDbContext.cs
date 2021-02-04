using BubbleGum.WebApi.Data.Configurations;
using BubbleGum.WebApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BubbleGum.WebApi.Data.Contexts
{
    public class BubbleGumDbContext : DbContext
    {
        public DbSet<LabeledRegion> Regions { get; set; }
        public DbSet<TrainingImage> Images { get; set; }

        public BubbleGumDbContext(DbContextOptions<BubbleGumDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LabeledRegionEntityTypeConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrainingImageEntityTypeConfiguration).Assembly);
        }
    }
}
