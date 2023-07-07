using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Diagnostics;
using UpworkProject.Models;

namespace UpworkProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IMongoCollection<Image> _imageCollection;

        public HomeController(ILogger<HomeController> logger, IMongoDatabase database)
        {
            _logger = logger;
            _imageCollection = database.GetCollection<Image>("images");
        }

        public IActionResult Index(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return RedirectToAction("Index","Auth");
            }
            ViewBag.SessionId = sessionId;
            return View();
        }
        public IActionResult Images(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
               return RedirectToAction("Index", "Auth");
            }
            ViewBag.SessionId = sessionId;
            var filter = Builders<Image>.Filter.Eq("SessionId", sessionId);
            var images = _imageCollection.Find(filter).ToList();
            return View(images);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] ImageVm image)
        {
            try
            {
                if (image?.file != null && image.file.Length > 0)
                {
                    // Check if the file has a valid image extension
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                    string fileExtension = Path.GetExtension(image.file.FileName).ToLower();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return BadRequest(new { message = "Invalid file extension. Please upload an image with a .jpg, .jpeg, .png, or .gif extension." });

                    }
                    var filename = Guid.NewGuid().ToString() + Path.GetExtension(image.file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", filename);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.file.CopyToAsync(stream);
                    }
                    var img = new Image()
                    {
                        ImageName = filename,
                        SessionId = image.SessionId,
                    };
                    _imageCollection.InsertOne(img);
                    return Ok(img);
                }
                return BadRequest(new { message = "Upload a valid image." });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UploadImage(*)-");
                return BadRequest(ex.Message);
            }

        }

        public IActionResult AllImages(string sessionId)
        {
            ViewBag.SessionId = sessionId;
            var images = _imageCollection.Find(_ => true).ToList();
            return View(images);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}