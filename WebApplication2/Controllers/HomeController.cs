using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using System;
using System.Diagnostics;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }
        //public IActionResult admindashboard()
        //{
        //    return View();
        //}
        public async Task<IActionResult> document( int id)
        {

            //HttpContext.Session.SetInt32("req_id", id);

            ViewBag.Id = id;

            //HttpContext.Session.SetString("req_id", id.ToString());
            return _context.Requestwisefiles != null ?
                          View(_context.Requestwisefiles.Where(m => m.Requestid == id).ToList()) : Problem("vchgvytfvtv");
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SubmitRequestOnMe()
        {
            return View();
        }
        public IActionResult SubmitRequestSomeone()
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> patient(Userdata info)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            /*Aspnetuser aspuser = new Aspnetuser
            {
                Usarname = info.first_name,
                Passwordhash = info.last_name,
                Email = info.email,
                Createddate = info.Createddate
            };
            _context.Aspnetusers.Add(aspuser);
            await _context.SaveChangesAsync();
            User user = new User
            {
                Firstname = info.first_name,
                Lastname = info.last_name,
                Email = info.email,
                Mobile = info.phonenumber,
                Street = info.street,
                City = info.city,
                State = info.state,
                Aspnetuserid = aspuser.Id,
                Createdby = info.Createddate.ToShortDateString(),
                Createddate = info.Createddate
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();*/
            return RedirectToAction(nameof(patientlogin), "Home");
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
        public async Task<IActionResult> patientdashboard()
        {
            //var aspnetuser = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Usarname == TempData["Usarname"]);
            //var user = await _context.Users.FirstOrDefaultAsync(m => m.Aspnetuserid == aspnetuser.Id );
            //return _context.Requests != null ? View(await _context.Requests/*.Where(m => m.Userid == user.Userid)*/.ToListAsync()) : Problem("No data");
            var userData = _context.Users.FirstOrDefault(m => m.Email == HttpContext.Session.GetString("UsarEmail"));
            var requestData = _context.Requests.Where(m => m.Userid == userData.Userid).ToList();


            DateOnly date = DateOnly.Parse(DateTime.Parse(userData.Intdate + userData.Strmonth + userData.Intyear).ToString("yyyy-MM-dd"));
            Dictionary<int, int> requestIdCounts = new Dictionary<int, int>();
            foreach (var request in requestData)
            {
                int count = _context.Requestwisefiles.Count(r => r.Requestid == request.Requestid);
                requestIdCounts.Add(request.Requestid, count);
            }
            ViewBag.RequestIdCounts = requestIdCounts;
            profile profile = new();
            profile.Request = requestData;
            profile.User = userData;
            profile.DOB = date;

            return View(profile);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}