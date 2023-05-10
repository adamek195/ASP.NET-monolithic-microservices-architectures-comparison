using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Multimedia.Web.Services.Interfaces;
using Multimedia.Web.Exceptions.Filters;
using Multimedia.Web.Dtos;
using Multimedia.Web.Helpers;
using X.PagedList;

namespace Multimedia.Web.Controllers
{
    [Authorize]
    [TypeFilter(typeof(GlobalExceptionFilter))]
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
            var videoDto = await _videosService.GetVideo<ImageDto>(id, User.GetToken());
            byte[] videoBytes = System.IO.File.ReadAllBytes(videoDto.Path);

            return File(videoBytes, "video/mp4");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var video = await _videosService.GetVideo<VideoDto>(id, User.GetToken());
            var user = await _usersService.GetUserById<UserDto>(new UserIdDto { UserId = video.UserId.ToString() }, User.GetToken());
            ViewBag.UserName = user.FirstName + " " + user.LastName;

            ViewBag.Delete = false;
            if (video.UserId.ToString() == User.GetId())
                ViewBag.Delete = true;

            return View(video);
        }

        [HttpGet]
        public async Task<IActionResult> Hashtag(int? page, string hashtag)
        {
            var pageNumber = page ?? 1;

            var videos = await _videosService.GetVideosByHashtag<List<VideoDto>>(new HashtagDto { Hashtag = hashtag }, User.GetToken());

            var videosOnPage = videos.ToPagedList(pageNumber, 9);

            return View(videosOnPage);
        }

        [HttpGet]
        public async Task<IActionResult> Email(int? page, string email)
        {
            var pageNumber = page ?? 1;

            if (!String.IsNullOrEmpty(email))
            {
                var user = await _usersService.GetUserByEmail<UserDto>(new UserEmailDto { Email = email }, User.GetToken());
                var videos = await _videosService.GetUserVideos<List<VideoDto>>(new UserIdDto { UserId = user.Id.ToString() }, User.GetToken());

                var videosOnPage = videos.ToPagedList(pageNumber, 9);

                return View(videosOnPage);
            }

            var emptyVideos = new List<VideoDto>();
            return View(emptyVideos.ToPagedList(pageNumber, 9));
        }

        [HttpGet]
        public async Task<IActionResult> UserVideos(int? page)
        {
            var videos = await _videosService.GetUserVideos<List<VideoDto>>(new UserIdDto { UserId = User.GetId() }, User.GetToken());

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

            videoDto.UserId = User.GetId();

            using (var stream = videoFile.OpenReadStream())
            {
                await _videosService.CreateVideo(videoDto, stream, videoFile.FileName, User.GetToken());

                return RedirectToAction("Video", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CommandVideoDto videoDto)
        {
            if (!ModelState.IsValid)
                return View();

            videoDto.UserId = User.GetId();

            await _videosService.UpdateVideo<Task>(id, videoDto, User.GetToken());

            return RedirectToAction("Details", new { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _videosService.DeleteVideo<Task>(id, new UserIdDto { UserId = User.GetId() }, User.GetToken());

            return RedirectToAction("Video", "Home");
        }
    }
}
