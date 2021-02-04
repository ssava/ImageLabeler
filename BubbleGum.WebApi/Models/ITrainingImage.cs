using System;

namespace BubbleGum.WebApi.Models
{
    public interface ITrainingImage
    {
        Guid Id { get; set; }
        string Path { get; set; }
    }
}
