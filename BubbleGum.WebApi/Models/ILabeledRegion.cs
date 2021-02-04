using System;

namespace BubbleGum.WebApi.Models
{
    public interface ILabeledRegion
    {
        Guid Id { get; set; }
        uint Height { get; set; }
        uint Left { get; set; }
        uint Top { get; set; }
        uint Width { get; set; }

        string Label { get; set; }
        Guid ImageId { get; set; }
    }
}
