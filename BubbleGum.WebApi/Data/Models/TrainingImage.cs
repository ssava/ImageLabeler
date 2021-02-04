using BubbleGum.WebApi.Models;
using System;

namespace BubbleGum.WebApi.Data.Models
{
    public class TrainingImage : ITrainingImage
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
    }
}
