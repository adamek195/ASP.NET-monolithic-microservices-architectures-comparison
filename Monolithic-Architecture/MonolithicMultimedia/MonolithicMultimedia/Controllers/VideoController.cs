using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonolithicMultimedia.Dtos;
using MonolithicMultimedia.Helpers;
using MonolithicMultimedia.Services;
using MonolithicMultimedia.Services.Interfaces;
using System.Threading.Tasks;
using X.PagedList;

namespace MonolithicMultimedia.Controllers
{
    [Authorize]
    public class VideoController : Controller
    {
        private readonly IVideosService _videosService;
        private readonly IUsersService _usersService;

        public VideoController(IVideosService videosService, IUsersService usersService)
        {
            _videosService = videosService;
            _usersService = usersService;
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Update(int id)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetVideo(int id)
        {
            var videoDto = await _videosService.GetVideo(id);
            byte[] videoBytes = System.IO.File.ReadAllBytes(videoDto.Path);

            return File(videoBytes, "video/mp4");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var videoDto = await _videosService.GetVideo(id);
            var userDto = await _usersService.GetUserById(videoDto.UserId.ToString());
            ViewBag.UserName = userDto.FirstName + " " + userDto.LastName;

            ViewBag.Delete = false;
            if (videoDto.UserId.ToString() == User.GetId())
                ViewBag.Delete = true;

            return View(videoDto);
        }

        [HttpGet]
        public async Task<IActionResult> Hashtag(string hashtag)
        {
            var videos = await _videosService.GetVideosByHashtag(hashtag);

            return View(videos);
        }

        [HttpGet]
        public async Task<IActionResult> AccountVideos(int? page)
        {
            var videos = await _videosService.GetUserVideos(User.GetId());

            var pageNumber = page ?? 1;
            var videosOnPage = videos.ToPagedList(pageNumber, 9);

            return View(videosOnPage);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CommandVideoDto videoDto, IFormFile videoFile)
        {
            if (!ModelState.IsValid)
                return View();

            if (videoFile == null || videoFile.Length == 0)
            {
                ViewBag.VideoError = "You do not upload video.";
                return View();
            }

            if (videoFile.ContentType.ToLower() != "video/mp4" &&
                videoFile.ContentType.ToLower() != "video/mkv")
            {
                ViewBag.VideoError = "You do not upload video.";
                return View();
            }

            using (var stream = videoFile.OpenReadStream())
            {
                await _videosService.CreateVideo(videoDto, stream, User.GetId(), videoFile.FileName);

                return RedirectToAction("Video", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CommandVideoDto videoDto)
        {
            if (!ModelState.IsValid)
                return View();

            await _videosService.UpdateVideo(id, User.GetId(), videoDto);

            return RedirectToAction("Details", new { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _videosService.DeleteVideo(id, User.GetId());

            return RedirectToAction("Video", "Home");
        }
    }
}
