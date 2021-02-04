using BubbleGum.WebApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BubbleGum.WebApi.Data.Configurations
{
    public class TrainingImageEntityTypeConfiguration : IEntityTypeConfiguration<TrainingImage>
    {
        public void Configure(EntityTypeBuilder<TrainingImage> builder)
        {
            builder.ToTable(@"Image");

            /* Id */
            builder.HasKey("Id");
            builder.Property("Id")
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property("Path")
                .HasColumnName("Path")
                .IsRequired();
        }
    }
}
