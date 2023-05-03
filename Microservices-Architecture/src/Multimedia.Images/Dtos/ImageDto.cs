using System;

namespace Multimedia.Images.Dtos
{
    public class ImageDto
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Path { get; set; }

        public string Hashtag { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
