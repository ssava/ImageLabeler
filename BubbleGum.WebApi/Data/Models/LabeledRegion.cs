using BubbleGum.WebApi.Models;
using System;

namespace BubbleGum.WebApi.Data.Models
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
    }
}
