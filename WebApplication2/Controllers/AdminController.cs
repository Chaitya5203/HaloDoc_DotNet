using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using System.Drawing;
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
        public async Task<ActionResult> View_Case(int id)
        {
            //return View();
            var data = await _context.Requestclients.FirstOrDefaultAsync(m => m.Requestid == id);
            DateOnly date = DateOnly.Parse(DateTime.Parse(data.Intyear + data.Strmonth + data.Intdate).ToString("yyyy-MM-dd"));
            var region = await _context.Regions.FirstOrDefaultAsync(m => m.Regionid == data.Regionid);
            requestclientvisedata model = new();
            model.FName = data.Firstname;
            model.LName = data.Lastname;
            model.DOB = date; 
            model.Notes = data.Notes;
            model.Phonenumber = data.Phonenumber;
            model.Email = data.Email;
            model.RegionName = region.Name  ;
            
            model.Address = data.Street + " " + data.City + " " + data.State + " " + data.Zipcode;
            return View(model);
        }
        public IActionResult Adminlogin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SaveNotes(Notes n,int id)
        {
            var user = await _context.Requests.FirstOrDefaultAsync(r => r.Requestid == id);
            var reqnotes = await _context.Requestnotes.Where(x => x.Requestid == id).FirstOrDefaultAsync();
            
            //var tnotes=_context.Requeststatuslogs.Where(x=>x.Requestid==id).FirstOrDefault();
            // if (tnotes != null)
            // {
            //     tnotes.Notes = model.Notes;
            // }
            // else
            // {

            // }
            if (reqnotes != null)
            {
                reqnotes.Adminnotes = n.AdminNotes;
                reqnotes.Physiciannotes = reqnotes.Physiciannotes;
                reqnotes.Createdby = user.Email;
                reqnotes.Modifiedby = user.Email;
                reqnotes.Modifieddate = DateTime.Now;
                _context.Requestnotes.Update(reqnotes);
            }
            else
            {
                Requestnote addreq = new Requestnote
                {
                    Requestid = id,
                    Adminnotes = n.AdminNotes,
                    
                    Createdby = user.Email,
                    Createddate = DateTime.Now,
                };
                _context.Requestnotes.Add(addreq);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(admindashboard));
        }
        public IActionResult Patientrequestadmin()
        {
            return View();
        }
        public IActionResult Cancelcase()
        {
            return View();
        }
        public IActionResult Sendorder()
        {
            return View();
        }
        public IActionResult Adminforgot()
        {
            return View();
        }
        public async Task<ActionResult> View_Note(int id)
        {
            var note = await  _context.Requestnotes.FirstOrDefaultAsync(m => m.Requestid == id);
            var tranfernote = await _context.Requeststatuslogs.Where(m => m.Requestid == id).ToListAsync();
            Notes notes = new();
            Requeststatuslog requeststatuslog = new();
            if(note != null)
            {
                if (note.Physiciannotes == null)
                    notes.phyNotes = "---";
                else
                    notes.phyNotes = note.Physiciannotes;
                if (note.Adminnotes == null)
                    notes.AdminNotes = "---";
                else
                    notes.AdminNotes = note.Adminnotes;
            }
            else
            {
                notes.phyNotes = notes.AdminNotes = "---";
            }
            notes.req_id = id;
            //if (tranfernote != null)
            //{
            //    if (requeststatuslog.Notes == null)
            //        notes.Transfernote = "---";
            //    else
            //        notes.Transfernote = tranfernote.;
            //}
            //else
            //{
            //    notes.Transfernote = tranfernote.
            //}
            return View(notes);
        }

        //public IActionResult View_Note()
        //{
        //    return View();
        //}
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
        public ActionResult New( int id,int check)
        {
            IQueryable data;
            if(check == 0)
            {
                data = from t1 in _context.Requests
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
            }
            else
            {
                data = from t1 in _context.Requests
                join t2 in _context.Requestclients on t1.Requestid equals t2.Requestid
                join t3 in _context.Requesttypes on t1.Requesttypeid equals t3.Requesttypeid
                where t1.Status == id && t1.Requesttypeid == check
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
            }
            if(id == 1) 
                return PartialView("_New", data);
            if(id == 2) 
                return PartialView("_Pending", data);
            if(id == 3) 
                return PartialView("_Active", data);
            if(id == 4) 
                return PartialView("_Conclude", data);
            if(id == 5) 
                return PartialView("_Toclose", data);
            if(id == 6) 
                return PartialView("_Unpaid", data);
            else 
                return PartialView("_New", data);
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
        public async Task<IActionResult> Modalsofnew (int id )
        {
            var data = _context.Requestclients.FirstOrDefault(r => r.Requestid == id);
            TempData["reqid"] = id;

            return PartialView("_Modalsofnew",data);   
        }
        public async Task<IActionResult> Modalofblock(int id)
        {
            var data = _context.Requestclients.FirstOrDefault(r => r.Requestid == id);
            TempData["reqid"] = id;
            return PartialView("_Modalofblock", data);
        }
        public async Task<IActionResult> Modalofassign(int id,int regionid)
        {
            CaseModels model = new();
            if (regionid == 0)
            {
                var physician = _context.Physicians.ToList();
                model.physics = physician;
            }
            else
            {
                var physician = _context.Physicians.Where(m => m.Regionid == regionid).ToList();
                model.physics = physician;
            }
            var region = _context.Regions.ToList();
            model.regions = region;
            TempData["reqid"] = id;
            return PartialView("_Modalofassign", model);
        }
        [HttpPost]
        public ActionResult CancelConfirm(int id, int reasonid, string notes)
        {
            Requeststatuslog statuslog = new Requeststatuslog
            {
                Requestid = id,
                Notes = notes,
                Status = 3,
                Createddate = DateTime.Now,
            };
            _context.Requeststatuslogs.Add(statuslog);
            Request request = _context.Requests.FirstOrDefault(r => r.Requestid == id);
            Casetag reason = _context.Casetags.FirstOrDefault(c => c.Casetagid == reasonid);
            request.Status = 5;
            request.Casetag = reason.Name;
            //request.Physicianid = 0;
            _context.Requests.Update(request);
            _context.SaveChanges();
            return RedirectToAction(nameof(admindashboard));
        }
        [HttpPost]
        public ActionResult AssignConfirm(int id, string notes, string physician, string region )
        {
            Requeststatuslog statuslog = new Requeststatuslog
            {
                Requestid = id,
                Notes = notes,
                Status = 2,
                Createddate = DateTime.Now,
            };
            _context.Requeststatuslogs.Add(statuslog);
            Request request = _context.Requests.FirstOrDefault(r => r.Requestid == id);
            request.Status = 2;
            _context.Requests.Update(request);
            _context.SaveChanges();
            return RedirectToAction(nameof(admindashboard));
        }
        [HttpPost]
        public ActionResult blockConfirm(int id,string notes)
        {
            var data = _context.Requestclients.FirstOrDefault(r => r.Requestid == id);
            Requeststatuslog statuslog = new Requeststatuslog
            {
                Requestid = id,
                Notes = notes,
                Status = 10,
                Createddate = DateTime.Now,
            };
            _context.Requeststatuslogs.Add(statuslog);
            Blockrequest blockrequest = new Blockrequest
            {
                Requestid = id.ToString(),
                Email = data.Email,
                Createddate = DateTime.Now,
                Reason= notes,
                Phonenumber = data.Phonenumber,
            };
            _context.Blockrequests.Add(blockrequest);
            Request request = _context.Requests.FirstOrDefault(r => r.Requestid == id);
            request.Status = 10;
            
            _context.Requests.Update(request);
            _context.SaveChanges();
            return RedirectToAction(nameof(admindashboard));
        }
    }
}