using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class AspnetusersController : Controller
    {
        private readonly ApplicationContext _context;
        public string FileNameOnServer { get; set; }    
        public string FileContentType { get; set; }    
        public long FileContentLength { get; set; } 
        public AspnetusersController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Aspnetusers
        public async Task<IActionResult> Index()
        {
              return _context.Aspnetusers != null ? 
                          View(await _context.Aspnetusers.ToListAsync()) :
                          Problem("Entity set 'ApplicationContext.Aspnetusers'  is null.");
        }
        // GET: Aspnetusers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Aspnetusers == null)
            {
                return NotFound();
            }

            var aspnetuser = await _context.Aspnetusers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspnetuser == null)
            {
                return NotFound();
            }
            return View(aspnetuser);
        }

        // Request on me page When Dashboard Is Open and request is Created 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitRequestOnMe(Userdata info)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == HttpContext.Session.GetString("UsarEmail"));
            if (!ModelState.IsValid)
            {
                return View("../Home/SubmitRequestOnMe", info);
            }
            Request request = new Request
            {
                Requesttypeid = 2,
                Userid = user.Userid,
                Isurgentemailsent = new BitArray(1, false),
                Status = 1,
                Firstname = info.first_name,
                Lastname = info.last_name,
                Email = info.email,
                Phonenumber = info.phonenumber,
                Createddate = info.Createddate,
            };
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            Requestclient requestclient = new Requestclient
            {
                Requestid = request.Requestid,
                Firstname = info.first_name,
                Lastname = info.last_name,
                Email = info.email,
                Phonenumber = info.phonenumber,
                Regionid = 1,
                Street = info.street,
                City = info.city,
                Zipcode = info.zipcode
            };
            _context.Requestclients.Add(requestclient);
            await _context.SaveChangesAsync();

            var file = info.File;
            var uniqueFileName = Path.GetFileName(file.FileName);
            var uploads = Path.Combine("wwwroot", "uploads");
            var filePath = Path.Combine(uploads, uniqueFileName);
            file.CopyTo(new FileStream(filePath, FileMode.Create));
            var addrequestfile = new Requestwisefile
            {
                Createddate = DateTime.Now,
                Filename = uniqueFileName,
                Requestid = request.Requestid
            };
            _context.Requestwisefiles.Add(addrequestfile);
            _context.SaveChanges();
            return RedirectToAction(nameof(patientdashboard), "Home");
        }

        // Request on me page When Dashboard Is Open and request is Created 
        public async Task<IActionResult> SubmitRequestSomeone(FamilyFriendPatientRequest info)
        {
            if (!ModelState.IsValid)
            {
                return View("../Home/SubmitRequestSomeone", info);
            }
            Request request = new Request
            {
                Requesttypeid = 3,
                Isurgentemailsent = new BitArray(1, false),
                Status = 1,
                Firstname = info.f_first_name,
                Lastname = info.f_last_name,
                Email = info.f_email,
                Phonenumber = info.f_phone_number,
                Createddate = info.Createddate,
            };
            _context.Requests.Add(request);
            _context.SaveChanges();

            Requestclient requestclient = new Requestclient
            {
                Requestid = request.Requestid,
                Firstname = info.p_first_name,
                Lastname = info.p_last_name,
                Email = info.f_email,
                Phonenumber = info.p_phonenumber,
                Regionid = 1,
                Street = info.p_street,
                City = info.p_city,
                Zipcode = info.p_zip
            };
            _context.Requestclients.Add(requestclient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(patientdashboard), "Home");
        }

        [HttpPost]
        public IActionResult UploadFile(int id,IFormFile fileToUpload)
        {
            if (fileToUpload != null && fileToUpload.Length > 0)
            {
                // User selected a file
                // Get a temporary path
                //FileNameOnServer = Path.GetTempPath() + "~/wwwroot/UploadedFiles/";
                //FileNameOnServer = "D:/Projects/HelloDOC/MVC/Hallodoc - Copy/wwwroot/UploadedF/";
                var uploads = Path.Combine("wwwroot", "uploads");
                var FileNameOnServer = Path.Combine(uploads, fileToUpload.FileName);
                // Add the file name to the path
                //FileNameOnServer += fileToUpload.FileName;
                // Get the file's length
                FileContentLength = fileToUpload.Length;
                // Get the file's type
                FileContentType = fileToUpload.ContentType;

                // Create a stream to write the file to
                using var stream = System.IO.File.Create(FileNameOnServer);
                // Upload file and copy to the stream
                 
           
                fileToUpload.CopyTo(stream);

                //var userobj = _context.Requests.FirstOrDefaultAsync(m => m.Requestid == (int)TempData["req_id"]);

                Requestwisefile reqclient = new Requestwisefile
                {
                    Requestid = id,
                    Filename = fileToUpload.FileName,
                    Createddate = DateTime.Now,
                };
                _context.Requestwisefiles.Add(reqclient);
                _context.SaveChanges();

                // Return a success page
                return RedirectToAction(nameof(patientdashboard), "Home");
            }
            else
            {
                // User did not select a file
                return RedirectToAction(nameof(patientdashboard), "Home");
            }
        }
        //download File 
        public async Task<IActionResult> DownloadFile(int id)
        {
            var file = _context.Requestwisefiles.Find(id);
            var filepath = "C:\\Users\\pce96\\source\\repos\\WebApplication2 - Copy\\WebApplication2\\wwwroot\\uploads\\" + Path.GetFileName(file.Filename);
            var bytes = System.IO.File.ReadAllBytes(filepath);
            return File(bytes, "application/octet-stream", file.Filename);
        }
        public IActionResult DownloadAll(int id)
        {


            var filesRow = _context.Requestwisefiles.Where(u => u.Requestid == id).ToList();
            MemoryStream ms = new MemoryStream();
            using (ZipArchive zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
                filesRow.ForEach(file =>
                {

                    var path = "C:\\Users\\pce96\\source\\repos\\WebApplication2 - Copy\\WebApplication2\\wwwroot\\uploads\\"+ Path.GetFileName(file.Filename); 
                    ZipArchiveEntry zipEntry = zip.CreateEntry(file.Filename);
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    using (Stream zipEntryStream = zipEntry.Open())
                    {
                        fs.CopyTo(zipEntryStream);
                    }
                });
            return File(ms.ToArray(), "application/zip", "download.zip");
        }

        /// Update Profile 

        [HttpPost]
        [ValidateAntiForgeryToken]

    
        public async Task<IActionResult> Updateprofile(Userdata info)
        {
            var updatedata = await _context.Aspnetusers.FirstOrDefaultAsync(m=>m.Email==HttpContext.Session.GetString("UsarEmail"));
            var updatdata1 = await _context.Users.FirstOrDefaultAsync(m => m.Email == HttpContext.Session.GetString("UsarEmail"));
            if (!ModelState.IsValid)
            {
                    //return View("../Home/patientdashboard", info);
            }

            updatedata.Usarname = info.first_name;
            updatedata.Passwordhash = info.last_name;
            updatedata.Email = info.email;
            updatedata.Createddate = info.Createddate;
            updatdata1.Firstname = info.first_name;
            updatdata1.Lastname = info.last_name;
            updatdata1.Lastname = info.last_name;
            updatdata1.Email = info.email;
            updatdata1.Mobile = info.phonenumber;
            updatdata1.Street = info.street;
            updatdata1.City = info.city;
            updatdata1.City = info.city;
            updatdata1.State = info.state;
            updatdata1.Createddate = info.Createddate;

            _context.Aspnetusers.Update(updatedata);
            _context.Users.Update(updatdata1);
            await _context.SaveChangesAsync();
            
            //User user = new User
            //{
            //    Firstname = info.first_name,
            //    Lastname = info.last_name,
            //    Email = info.email,
            //    Mobile = info.phonenumber,
            //    Street = info.street,
            //    City = info.city,
            //    State = info.state,
            //    Aspnetuserid = updatedata.Id,
            //    Createdby = info.Createddate.ToShortDateString(),
            //    Createddate = info.Createddate
            //};
            //_context.Users.Update(user);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(patientdashboard), "Home");
        }


        //Create Patient By It Self 

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreatePatient(Userdata info)
        {
            var temp = 0;
            var userobj = await _context.Aspnetusers
           .FirstOrDefaultAsync(m => m.Usarname == info.first_name);
            if (!ModelState.IsValid)
            {
                return View("../Home/patient", info);
            }

            int Year = info.dob.Year;
            int Date = info.dob.Day;
            System.Globalization.DateTimeFormatInfo dateformat = new System.Globalization.DateTimeFormatInfo();
            var Month = dateformat.GetMonthName(info.dob.Month).ToString();


            if (userobj == null)                 
            {
                Aspnetuser aspuser = new Aspnetuser
                {
                    Usarname = info.first_name,
                    Passwordhash = info.last_name,
                    Email = info.email,
                    Createddate = info.Createddate
                };
                _context.Aspnetusers.Add(aspuser);
                await _context.SaveChangesAsync();
                temp = aspuser.Id;
            }
            User user = new User
            {
                Firstname = info.first_name,
                Lastname = info.last_name,
                Email = info.email,
                Mobile = info.phonenumber,
                Street = info.street,
                City = info.city,
                State = info.state,
                Aspnetuserid = temp,
                Intdate = Date,
                Intyear = Year,
                Strmonth = Month,
                Zip=info.zipcode,
                //IntYear = model.BirthDate.Year,
                //StrMonth = (model.BirthDate.Month).ToString("MMM"),
                //Intyear = info.IntYear
                //Intdate = info.dob.Day,
                //Strmonth = info.dob.ToString(),


                Createdby = info.Createddate.ToShortDateString(),
                Createddate = info.Createddate
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            Request request = new Request
            {
                Requesttypeid = 2,
                Userid = user.Userid,
                Isurgentemailsent = new BitArray(1, false),
                Status = 1,
                Firstname = info.first_name,
                Lastname = info.last_name,
                Email = info.email,
                Phonenumber = info.phonenumber,
                Createddate = info.Createddate,
            };
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            Requestclient requestclient = new Requestclient
            {
                Requestid = request.Requestid,
                Firstname = info.first_name,
                Lastname = info.last_name,
                Email = info.email,
                Phonenumber= info.phonenumber,
                Regionid = 1,
                Street = info.street,
                City = info.city,
                Intdate = info.dob.Day,
                Intyear = info.dob.Year,
                Strmonth = info.dob.Month.ToString(),
                Zipcode = info.zipcode
            };
            _context.Requestclients.Add(requestclient);
            await _context.SaveChangesAsync();

            var file = info.File;
            var uniqueFileName = Path.GetFileName(file.FileName);
            var uploads = Path.Combine("wwwroot", "uploads");
            var filePath = Path.Combine(uploads, uniqueFileName);
            file.CopyTo(new FileStream(filePath, FileMode.Create));           
            var addrequestfile = new Requestwisefile
            {
                Createddate = DateTime.Now,
                Filename = uniqueFileName,
                Requestid = request.Requestid
            };
            _context.Requestwisefiles.Add(addrequestfile);
            _context.SaveChanges();
            return RedirectToAction(nameof(patientlogin), "Home");        
        }
        //Businesss Data Store On Business Page 

        public IActionResult patientforgot()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> patientforgot(forgot info)
        {
            var mail = "tatva.dotnet.binalmalaviya@outlook.com";
            var password = "binal@2002";
            var receiver = info.email;
            var subject = "Reset Password";
            var message = "Reset Your Password: https://localhost:7050/Home";

            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
            return RedirectToAction(nameof(patientlogin), "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePatientByBusiness(BusinessPatientRequest info)
        {
            Aspnetuser aspuser = _context.Aspnetusers.FirstOrDefault(m => m.Email == info.p_email);
            if (!ModelState.IsValid)
            {
                return View("../Home/business",info);
            }
            int Year = info.dob.Year;
            int Date = info.dob.Day;
            System.Globalization.DateTimeFormatInfo dateformat = new System.Globalization.DateTimeFormatInfo();
            var Month = dateformat.GetMonthName(info.dob.Month).ToString();

            if (aspuser == null)
            {
                var receiver = info.p_email;
                var subject = "Create Account";
                var message = "Tap on link for Create Account: https://localhost:7050/Home/create_patient";
                var mail = "chaityamehta522003@gmail.com";
                var password = "iwbc edlf rgpt oucs";

                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };
                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
            }
            if (aspuser == null)
            {
                Aspnetuser aspuser1 = new Aspnetuser
                {
                    Usarname = info.first_name,
                    Passwordhash = info.last_name,
                    Email = info.email,
                    Phonenumber = info.phone,
                };
                aspuser = aspuser1;
                _context.Aspnetusers.Add(aspuser);
                _context.SaveChanges();
            }
            User user = new User
            {
                Firstname = info.p_first_name,
                Lastname = info.p_last_name,
                Email = info.p_email,
                Zip = info.p_zip_code,
                Mobile = info.p_phone,
                Street = info.p_street,
                City = info.p_city,
                State = info.p_state,
                Aspnetuserid = aspuser.Id,
                Intdate = Date,
                Intyear = Year,
                Strmonth = Month,
                Createdby = info.Createddate.ToShortDateString(),
                Createddate = info.Createddate
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            Business business = new Business()
            {
                Name = info.first_name + ' ' + info.last_name,
                Phonenumber = info.phone,
                Createdby = info.Createddate.ToShortDateString(),
                Createddate = info.Createddate,
            };
            _context.Businesses.Add(business);
            _context.SaveChanges();

            Request request = new Request
            {
                Requesttypeid = 1,
                Userid = user.Userid,
                Isurgentemailsent = new BitArray(1, false),
                Status = 1,
                Firstname = info.first_name,
                Lastname = info.last_name,
                Email = info.email,
                Phonenumber = info.phone,
                Createddate = info.Createddate,
            };
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();  
            Requestclient requestclient = new Requestclient
            {
                Requestid = request.Requestid,
                Firstname = info.p_first_name,
                Lastname = info.p_last_name,
                Email = info.p_email,
                Phonenumber = info.p_phone,
                Regionid = 1,
                Street = info.p_street,
                Intdate = Date,
                Intyear = Year,
                Strmonth = Month,
                City = info.p_city,
                Zipcode = info.p_zip_code
            };
            _context.Requestclients.Add(requestclient); 
            await _context.SaveChangesAsync();

            Requestbusiness requestbusiness = new Requestbusiness
            {
                Requestid = request.Requestid,
                Businessid = business.Businessid,
            };
            _context.Requestbusinesses.Add(requestbusiness);
            _context.SaveChanges();

            return RedirectToAction(nameof(patientlogin), "Home");
        }

        /// Family Friend Data store on Family Friend Page    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePatientByFamilyFriend(FamilyFriendPatientRequest info)
        {
            Aspnetuser aspuser = _context.Aspnetusers.FirstOrDefault(m => m.Email == info.p_email);
            if (!ModelState.IsValid)
            {
                return View("../Home/familyfriend", info);
            }
            int Year = info.dob.Year;
            int Date = info.dob.Day;
            System.Globalization.DateTimeFormatInfo dateformat = new System.Globalization.DateTimeFormatInfo();
            var Month = dateformat.GetMonthName(info.dob.Month).ToString();

            if (aspuser == null)
            {
                var receiver = info.p_email;
                var subject = "Create Account";
                var message = "Tap on link for Create Account: https://localhost:7050/Home/create_patient";
                var mail = "chaityamehta522003@gmail.com";
                var password = "iwbc edlf rgpt oucs";

                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };
                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
            }
            if (aspuser == null)
            {
                Aspnetuser aspuser1 = new Aspnetuser
                {
                    Usarname = info.p_first_name,
                    Passwordhash = info.p_last_name,
                    Email = info.p_email,
                    Phonenumber = info.p_phonenumber,
                };
                aspuser = aspuser1;
                _context.Aspnetusers.Add(aspuser1);
                _context.SaveChanges();
            }
            User user = new User
            {
                Firstname = info.p_first_name,
                Lastname = info.p_last_name,
                Email = info.p_email,
                Zip = info.p_zip,
                Mobile = info.p_phonenumber,
                Street = info.p_street,
                City = info.p_city,
                State = info.p_state,
                Aspnetuserid = aspuser.Id,
                Intdate = Date,
                Intyear = Year,
                Strmonth = Month,
                Createdby = info.Createddate.ToShortDateString(),
                Createddate = info.Createddate
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            Request request = new Request
            {
                Requesttypeid = 3,
                Userid = user.Userid,
                Isurgentemailsent = new BitArray(1, false),
                Status = 1,
                Firstname = info.f_first_name,
                Lastname = info.f_last_name,
                Email = info.f_email,
                Phonenumber = info.f_phone_number,
                Createddate = info.Createddate,
            };
            _context.Requests.Add(request);
            _context.SaveChanges();
            Requestclient requestclient = new Requestclient
            {
                Requestid = request.Requestid,
                Firstname = info.p_first_name,
                Lastname = info.p_last_name,
                Email = info.f_email,
                Intdate = Date,
                Intyear = Year,
                Strmonth = Month,
                Phonenumber = info.p_phonenumber,
                Regionid = 1,
                Street = info.p_street,
                City = info.p_city,
                Zipcode = info.p_zip
            };
            _context.Requestclients.Add(requestclient);
            await _context.SaveChangesAsync();
            var file = info.File;
            var uniqueFileName = Path.GetFileName(file.FileName);
            var uploads = Path.Combine("wwwroot", "uploads");
            var filePath = Path.Combine(uploads, uniqueFileName);
            file.CopyTo(new FileStream(filePath, FileMode.Create));
            var addrequestfile = new Requestwisefile
            {
                Createddate = DateTime.Now,
                Filename = uniqueFileName,
                Requestid = request.Requestid
            };
            _context.Requestwisefiles.Add(addrequestfile);
            _context.SaveChanges();
            return RedirectToAction(nameof(patientlogin), "Home");
        }

        // Concierge Data Store On Concierge Page 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePatientByConcierge(ConciergePatientRequest info)
        {
            Aspnetuser aspuser = _context.Aspnetusers.FirstOrDefault(m => m.Email == info.pemail);
            if (aspuser == null)
            {
                var receiver = info.pemail;
                var subject = "Create Account";
                var message = "Tap on link for Create Account: https://localhost:7050/Home/create_patient";
                var mail = "chaityamehta522003@gmail.com";
                var password = "iwbc edlf rgpt oucs";

                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(mail, password)
                };
                client.SendMailAsync(new MailMessage(from: mail, to: receiver, subject, message));
            }
            if (!ModelState.IsValid)
            {
                return View("../Home/concierge", info);
            }
            int Year = info.dob.Year;
            int Date = info.dob.Day;
            System.Globalization.DateTimeFormatInfo dateformat = new System.Globalization.DateTimeFormatInfo();
            var Month = dateformat.GetMonthName(info.dob.Month).ToString();
            if (aspuser == null)
            {
                Aspnetuser aspuser1 = new Aspnetuser
                {
                    Usarname = info.first_name,
                    Passwordhash = info.last_name,
                    Email = info.pemail,
                    Phonenumber = info.Phonenumber,
                };
                aspuser = aspuser1;
                _context.Aspnetusers.Add(aspuser);
                await _context.SaveChangesAsync();
            }


             Concierge c = new Concierge
             {
                Conciergename = info.cname,
                Regionid = 1,
                Zipcode = info.czip,
                Street = info.cstreet,
                City = info.ccity,
                State = info.cstate,
                Createddate = info.Createddate
             };
            _context.Concierges.Add(c);
            await _context.SaveChangesAsync();
            

            User user = new User
            {
                Firstname = info.first_name,
                Lastname = info.last_name,
                Email = info.pemail,
                Mobile = info.Phonenumber,
                Street = info.Street,
                City = info.City,
                State = info.State,
                //Zip=info.Zip,
                Aspnetuserid = aspuser.Id,
                Intdate = Date,
                Intyear = Year,
                Strmonth = Month,
                Createdby = info.Createddate.ToShortDateString(),
                Createddate = info.Createddate
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            Request request = new Request
            {
                Requesttypeid = 4,
                Isurgentemailsent = new BitArray(1, false),
                Status = 1,
                Userid=user.Userid,
                Firstname = info.cname,
                Lastname = info.clname,
                Email = info.cemail,
                Phonenumber = info.cphone,
                Createddate = info.Createddate,
            };
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();
            Requestclient requestclient = new Requestclient
            {
                Requestid = request.Requestid,
                Firstname = info.first_name,
                Lastname = info.last_name,
                Intdate = Date,
                Intyear = Year,
                Strmonth =Month,
                Email = info.pemail,
                Phonenumber = info.Phonenumber,
                Regionid = 1,
                Street = info.Street,
                City = info.City,
                Zipcode = info.p_zip_code
            };
            _context.Requestclients.Add(requestclient);
            await _context.SaveChangesAsync();

            Requestconcierge requestconcierge = new Requestconcierge
            {
                Requestid = request.Requestid,
                Conciergeid = c.Conciergeid
            };
            _context.Requestconcierges.Add(requestconcierge);
            _context.SaveChanges();
            return RedirectToAction(nameof(patientlogin), "Home");
        }

        //Routing Of The Data and Check if email exist if not then add column password and confirm password 

        [Route("/Home/patient/{email}")]
        [Route("/Home/business/{email}")]
        [Route("/Home/familyfriend/{email}")]
        [Route("/Home/concierge/{email}")]

        [HttpGet]
        public IActionResult CheckEmailExists(string email)
        {
            var emailExists = _context.Aspnetusers.Any(u => u.Email == email);
            return Json(new { exists = emailExists });
        }

        // GET: Aspnetusers/Create
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult patientlogin()
        {
            return View();

        }
        public IActionResult patientdashboard()
        {
            return View();
        }

        // GET: Aspnetusers/Create where the data is check and user can login on it and added session on that page 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Usarname,Passwordhash")] login aspnetuser)
        {
            if (!ModelState.IsValid)
            {
                return View("../Home/patientlogin", aspnetuser);
            }
            var userobj = await _context.Aspnetusers.FirstOrDefaultAsync(m => m.Email == aspnetuser.Usarname && m.Passwordhash == aspnetuser.Passwordhash);
            if (userobj == null)
            {
               
                return RedirectToAction(nameof(patientlogin), "Home");
            }
            else
            {
                HttpContext.Session.SetString("Usarname", userobj.Usarname);
                HttpContext.Session.SetString("UsarEmail", userobj.Email); 
                HttpContext.Session.SetString("Isheader", "unset");

                return RedirectToAction(nameof(patientdashboard), "Home");
            }  
        }
        // POST: Aspnetusers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // GET: Aspnetusers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Aspnetusers == null)
            {
                return NotFound();
            }
            var aspnetuser = await _context.Aspnetusers.FindAsync(id);
            if (aspnetuser == null)
            {
                return NotFound();
            }
            return View(aspnetuser);
        }
        // POST: Aspnetusers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Usarname,Passwordhash,Email,Phonenumber,Ip,Createddate,Modifieddate")] Aspnetuser aspnetuser)
        {
            if (id != aspnetuser.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aspnetuser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspnetuserExists(aspnetuser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(aspnetuser);
        }

        // GET: Aspnetusers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Aspnetusers == null)
            {
                return NotFound();
            }
            var aspnetuser = await _context.Aspnetusers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspnetuser == null)
            {
                return NotFound();
            }

            return View(aspnetuser);
        }
        // POST: Aspnetusers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Aspnetusers == null)
            {
                return Problem("Entity set 'ApplicationContext.Aspnetusers'  is null.");
            }
            var aspnetuser = await _context.Aspnetusers.FindAsync(id);
            if (aspnetuser != null)
            {
                _context.Aspnetusers.Remove(aspnetuser);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool AspnetuserExists(int id)
        {
          return (_context.Aspnetusers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
