using Microsoft.AspNetCore.Http;

namespace Multimedia.Videos.Dtos
{
    public class CommandVideoFileDto
    {
        public CommandVideoDto CommandVideoDto { get; set; }

        public IFormFile VideoFile { get; set; }
    }
}
