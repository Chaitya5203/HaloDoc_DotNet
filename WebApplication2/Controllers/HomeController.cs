using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult submit_request_screen()
        {
            return View();
        }
        public IActionResult patient()
        {
            return View();
        }
        public IActionResult business()
        {
            return View();
        }
        public IActionResult familyfriend()
        {
            return View();
        }
        public IActionResult concierge()
        {
            return View();
        }
        public IActionResult patientlogin()
        {
            return View();
        }
        public IActionResult patientforgot()
        {
            return View();
        }
        public IActionResult patientdashboard()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}