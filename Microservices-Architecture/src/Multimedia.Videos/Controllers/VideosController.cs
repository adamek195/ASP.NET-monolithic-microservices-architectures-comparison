using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Multimedia.Videos.Dtos;
using Multimedia.Videos.Exceptions.Filters;
using Multimedia.Videos.Services.Interfaces;
using System.Threading.Tasks;

namespace Multimedia.Videos.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [GlobalExceptionFilter]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VideosController : ControllerBase
    {
        private readonly IVideosService _videosService;

        public VideosController(IVideosService videosService)
        {
            _videosService = videosService;
        }

        [HttpGet]
        public async Task<IActionResult> GetVideos()
        {
            var videos = await _videosService.GetVideos();

            return Ok(videos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVideo([FromRoute] int id)
        {
            var video = await _videosService.GetVideo(id);

            return Ok(video);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVideo([FromForm] CommandVideoDto commandVideoDto, [FromForm] IFormFile videoFile)
        {
            if (videoFile == null || videoFile.Length == 0)
                return BadRequest("You do not upload video.");


            if (videoFile.ContentType.ToLower() != "video/mp4" &&
                videoFile.ContentType.ToLower() != "video/mkv")
                return BadRequest("You do not upload photo.");

            using (var stream = videoFile.OpenReadStream())
            {
                var videoDto = await _videosService.CreateVideo(commandVideoDto, stream, videoFile.FileName);

                return Created($"videos/{videoDto.Id}", videoDto);
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideo([FromRoute] int id, [FromForm] UserDto userDto)
        {
            await _videosService.DeleteVideo(id, userDto);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVideo([FromRoute] int id, [FromForm] CommandVideoDto commandVideoDto)
        {
            await _videosService.UpdateVideo(id, commandVideoDto);

            return NoContent();
        }
    }
}
