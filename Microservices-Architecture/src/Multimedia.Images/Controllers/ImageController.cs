﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    public class ImageController : ControllerBase
    {
        private readonly IImagesService _imagesService;

        public ImageController(IImagesService imagesService)
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

        [HttpGet]
        [Route("Hashtag")]
        public async Task<IActionResult> GetImagesByHashtag([FromBody] HashtagDto hashtagDto)
        {
            var images = await _imagesService.GetImagesByHashtag(hashtagDto);

            return Ok(images);
        }

        [HttpGet]
        [Route("User")]
        public async Task<IActionResult> GetUserImages([FromBody] UserIdDto userIdDto)
        {
            var images = await _imagesService.GetUserImages(userIdDto);

            return Ok(images);
        }

[HttpPost]
        public async Task<IActionResult> CreateImage([FromForm] CommandImageFileDto commandImageFileDto)
        {
            if (commandImageFileDto.ImageFile == null || commandImageFileDto.ImageFile.Length == 0)
                return BadRequest("You do not upload photo.");

            if (commandImageFileDto.CommandImageDto == null)
                return BadRequest("You do not upload photo information.");

            using (var stream = commandImageFileDto.ImageFile.OpenReadStream())
            {
                var imageDto = await _imagesService.CreateImage(commandImageFileDto.CommandImageDto, stream, System.IO.Path.GetExtension(commandImageFileDto.ImageFile.FileName));

                return Created($"images/{imageDto.Id}", imageDto);
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage([FromRoute] int id, [FromBody] UserIdDto userIdDto)
        {
            await _imagesService.DeleteImage(id, userIdDto);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateImage([FromRoute] int id, [FromBody] CommandImageDto commandImageDto)
        {
            await _imagesService.UpdateImage(id, commandImageDto);

            return NoContent();
        }
    }
}
