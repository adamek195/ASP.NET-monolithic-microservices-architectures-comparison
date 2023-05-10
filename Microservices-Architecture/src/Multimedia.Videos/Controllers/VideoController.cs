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
    public class VideoController : ControllerBase
    {
        private readonly IVideosService _videosService;

        public VideoController(IVideosService videosService)
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

        [HttpGet]
        [Route("Hashtag")]
        public async Task<IActionResult> GetVideosByHashtag(HashtagDto hashtagDto)
        {
            var images = await _videosService.GetVideosByHashtag(hashtagDto);

            return Ok(images);
        }

        [HttpGet]
        [Route("User")]
        public async Task<IActionResult> GetUserImages(UserIdDto userIdDto)
        {
            var videos = await _videosService.GetUserVideos(userIdDto);

            return Ok(videos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVideo([FromForm] CommandVideoFileDto commandVideoFileDto)
        {
            if (commandVideoFileDto.VideoFile == null || commandVideoFileDto.VideoFile.Length == 0)
                return BadRequest("You do not upload video.");

            if (commandVideoFileDto.CommandVideoDto == null)
                return BadRequest("You do not upload video information.");

            using (var stream = commandVideoFileDto.VideoFile.OpenReadStream())
            {
                var videoDto = await _videosService.CreateVideo(commandVideoFileDto.CommandVideoDto, stream, commandVideoFileDto.VideoFile.FileName);

                return Created($"videos/{videoDto.Id}", videoDto);
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideo([FromRoute] int id, [FromBody] UserIdDto userIdDto)
        {
            await _videosService.DeleteVideo(id, userIdDto);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVideo([FromRoute] int id, [FromBody] CommandVideoDto commandVideoDto)
        {
            await _videosService.UpdateVideo(id, commandVideoDto);

            return NoContent();
        }
    }
}
