using Microsoft.AspNetCore.Http;

namespace Multimedia.Images.Dtos
{
    public class CommandImageFileDto
    {
        public CommandImageDto CommandImageDto { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
