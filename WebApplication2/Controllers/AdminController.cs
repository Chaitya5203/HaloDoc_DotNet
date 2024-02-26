using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
namespace WebApplication2.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;
        public AdminController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context; 
        }
        public IActionResult admindashboard()
        {

            //if (HttpContext.Session.GetString("Isheader") == "unset")
            //    HttpContext.Session.SetString("Isheader", "true");
            ViewBag.NewCount = _context.Requests.Where(r => r.Status == 1).Count();

            ViewBag.PendingCount = _context.Requests.Where(r => r.Status == 2).Count();
            ViewBag.ActiveCount = _context.Requests.Where(r => r.Status == 3).Count();
            ViewBag.ConcludeCount = _context.Requests.Where(r => r.Status == 4).Count();
            ViewBag.TocloseCount = _context.Requests.Where(r => r.Status == 5).Count();
            ViewBag.UnpaidCount = _context.Requests.Where(r => r.Status == 6).Count();

            return View();
        }
   

        public IActionResult View_Case(int id)
        {
            var data = _context.Requestclients.FirstOrDefault(m => m.Requestid == id);
            //var region = _context.Regions.FirstOrDefault(m => m.Regionid == data.Regionid);
            requestclientvisedata model = new();
            model.FName = data.Firstname;
            model.LName = data.Lastname;
            model.DOB = DateTime.Now;
            model.Notes = data.Notes;
            model.Phonenumber = data.Phonenumber;
            model.Email = data.Email;
            //model.RegionName = region.Name;
            model.Address = data.Street + " " + data.City + " " + data.State + " " + data.Zipcode;


            return View(model);
            //return View(_context.Requestclients.FirstOrDefault(m => m.Requestid == id));
        }
        public IActionResult View_Note()
        {
            return View();
        }
        public IActionResult Access()
        {
            return View();

        }
        public IActionResult Provider()
        {
            return View();
        }
        public IActionResult Myprofile()
        {
            return View();
        }
        public IActionResult Record()
        {
            return View();
        }
        public IActionResult Partner()
        {
            return View();

        }
        public IActionResult Providerlocation()
        {
            return View();
        }
        public IActionResult Adminprofile()
        {
            return PartialView("Adminprofile");
        }
        [HttpPost]
        public ActionResult New( int id)
        {
            var data = from t1 in _context.Requests
                       join t2 in _context.Requestclients on t1.Requestid equals t2.Requestid
                       join t3 in _context.Requesttypes on t1.Requesttypeid equals t3.Requesttypeid
                       where t1.Status == id
                       select new
                       {
                           t1.Requestid,
                           t1.Requesttypeid,
                           t1.Firstname,
                           t1.Lastname,
                           t2.Intdate,
                           t2.Strmonth,
                           t2.Intyear,
                           t1.Phonenumber,
                           t2.Street,
                           t2.City,
                           t2.Notes,
                           t1.Createddate,
                           t3.Name
                       };
            if(id == 1)            return PartialView("_New", data);
            if(id == 2)            return PartialView("_Pending", data);
            if(id == 3)            return PartialView("_Active", data);
            if(id == 4)            return PartialView("_Conclude", data);
            if(id == 5)            return PartialView("_Toclose", data);
            if(id == 6)            return PartialView("_Unpaid", data);
            else          return PartialView("_New", data);
        }

        //[HttpPost]
        //public ActionResult NavigationTabs(int id)
        //{
        //    if (id == 1)
        //    {
        //        HttpContext.Session.SetString("Isheader", "false");
        //        return View("AdminDashboard");
        //    }
        //    //if (id == 1) 
        //    //    return View("admindashboard");
        //    if (id == 2) return View("Providerlocation");
        //    if (id == 3) return View("Myprofile");
        //    if (id == 4) return View("Provider");
        //    if (id == 5) return View("Partner");
        //    if (id == 6) return View("Access");
        //    if (id == 7) return View("Record");
        //    else return View("admindashboard"); 
        //}
        [HttpPost]
        public IActionResult Toclose()
        {
            return PartialView("Toclose");
        }
        [HttpPost]
        public IActionResult Active()
        {
            return PartialView("Active");

        }
        [HttpPost]
        public IActionResult Conclude()
        {
            return PartialView("Conclude");
        }
        [HttpPost]
        public IActionResult Unpaid()
        {
            return PartialView("Unpaid");
        }
    }
}
