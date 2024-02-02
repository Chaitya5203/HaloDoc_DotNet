using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PatientloginController : Controller
    {
        private readonly ILogger<PatientloginController> _logger;

        public PatientloginController(ILogger<PatientloginController> logger)
        {
            _logger = logger;
        }

        public IActionResult patientlogin()
        {
            return View();
        }

        public IActionResult patientforgot()
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