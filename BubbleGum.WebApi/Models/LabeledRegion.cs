using System;

namespace BubbleGum.WebApi.Models
{
    public class LabeledRegion : ILabeledRegion
    {
        public Guid Id { get; set; }
        public uint Height { get; set; }
        public uint Left { get; set; }
        public uint Top { get; set; }
        public uint Width { get; set; }

        public string Label { get; set; }
        public Guid ImageId { get; set; }
        public virtual ITrainingImage SourceImage { get; set; }

        public static LabeledRegion CreateDto(Data.Models.LabeledRegion region) =>
            new LabeledRegion
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
