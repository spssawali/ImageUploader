using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using UpworkProject.Models;

namespace UpworkProject.Controllers
{
    public class AuthController : Controller
    {

        // GET: AuthController
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateSession([FromForm] Session session)
        {
            ViewBag.SessionId = session.SessionId;
            return RedirectToAction("Index", "Home", new { sessionId = session.SessionId });
        }
    }
}
