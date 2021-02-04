using System;

namespace BubbleGum.WebApi.Models
{
    public class TrainingImage : ITrainingImage
    {
        public Guid Id { get; set; }
        public string Path { get; set; }

        public static TrainingImage CreateDto(Data.Models.TrainingImage image) =>
            new TrainingImage
            {
                Id = image.Id,
                Path = image.Path
            };
    }
}
