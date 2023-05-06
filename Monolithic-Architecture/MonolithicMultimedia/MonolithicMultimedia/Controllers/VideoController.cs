using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonolithicMultimedia.Dtos;
using MonolithicMultimedia.Helpers;
using MonolithicMultimedia.Services.Interfaces;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using X.PagedList;
using MonolithicMultimedia.Exceptions.Filters;

namespace MonolithicMultimedia.Controllers
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
        public async Task<IActionResult> Hashtag(int? page, string hashtag)
        {
            ViewData["Hashtag"] = hashtag;
            var pageNumber = page ?? 1;

            var videos = await _videosService.GetVideosByHashtag(hashtag);

            var videosOnPage = videos.ToPagedList(pageNumber, 9);

            return View(videosOnPage);
        }

        [HttpGet]
        public async Task<IActionResult> Email(int? page, string email)
        {
            ViewData["Email"] = email;
            var pageNumber = page ?? 1;

            if (!String.IsNullOrEmpty(email))
            {
                var videos = await _videosService.GetVideosByEmail(email);

                var videosOnPage = videos.ToPagedList(pageNumber, 9);

                return View(videosOnPage);
            }

            var emptyVideos = new List<VideoDto>();
            return View(emptyVideos.ToPagedList(pageNumber, 9));
        }

        [HttpGet]
        public async Task<IActionResult> UserVideos(int? page)
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
