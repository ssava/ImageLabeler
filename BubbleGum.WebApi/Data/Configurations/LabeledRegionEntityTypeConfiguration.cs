using BubbleGum.WebApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BubbleGum.WebApi.Data.Configurations
{
    public class LabeledRegionEntityTypeConfiguration : IEntityTypeConfiguration<LabeledRegion>
    {
        public void Configure(EntityTypeBuilder<LabeledRegion> builder)
        {
            builder.ToTable(@"Region");

            /* Id */
            builder.HasKey("Id");
            builder.Property("Id")
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property("Top")
                .HasColumnName("Top")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property("Left")
                .HasColumnName("Left")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property("Width")
                .HasColumnName("Width")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property("Height")
                .HasColumnName("Height")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property("Label")
                .HasColumnName("Label");

            builder.Property("ImageId")
                .HasColumnName("ImageId")
                .IsRequired();
        }
    }
}
