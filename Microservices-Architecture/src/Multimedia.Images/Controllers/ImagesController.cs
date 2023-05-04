using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Multimedia.Images.Dtos;
using Multimedia.Images.Exceptions.Filters;
using Multimedia.Images.Services.Interfaces;
using System.Threading.Tasks;

namespace Multimedia.Images.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [GlobalExceptionFilter]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ImagesController : ControllerBase
    {
        private readonly IImagesService _imagesService;

        public ImagesController(IImagesService imagesService)
        {
            _imagesService = imagesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetImages()
        {
            var images = await _imagesService.GetImages();

            return Ok(images);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage([FromRoute] int id)
        {
            var image = await _imagesService.GetImage(id);

            return Ok(image);
        }

        [HttpPost]
        public async Task<IActionResult> CreateImage([FromForm] CommandImageDto commandImageDto, [FromForm] IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("You do not upload photo.");


            if (imageFile.ContentType.ToLower() != "image/jpeg" &&
                imageFile.ContentType.ToLower() != "image/jpg" &&
                imageFile.ContentType.ToLower() != "image/png")
                return BadRequest("You do not upload photo.");

            using (var stream = imageFile.OpenReadStream())
            {
                var imageDto = await _imagesService.CreateImage(commandImageDto, stream, imageFile.FileName);

                return Created($"images/{imageDto.Id}", imageDto);
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage([FromRoute] int id, [FromForm] UserDto userDto)
        {
            await _imagesService.DeleteImage(id, userDto);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateImage([FromRoute] int id, [FromForm] CommandImageDto commandImageDto)
        {
            await _imagesService.UpdateImage(id, commandImageDto);

            return NoContent();
        }
    }
}
