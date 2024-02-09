using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class AspnetusersController : Controller
    {
        private readonly ApplicationContext _context;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePatient(Userdata info)
        {
            User user = new User
            {

                Firstname = info.first_name,
                Lastname = info.last_name,
                Email = info.email,
                Mobile = info.phonenumber,
                Street = info.street,
                City = info.city,
                State = info.state,
                Aspnetuserid = 1,
                Createdby = info.Createddate.ToShortDateString(),
                Createddate = info.Createddate
            };
            Aspnetuser aspuser = new Aspnetuser
            {
                Id = 1,
                Usarname = info.first_name,
                Passwordhash = info.last_name,
                Email = info.email,
                Createddate = info.Createddate
            };
            _context.Aspnetusers.Add(aspuser);
            _context.Users.Add(user);
           await _context.SaveChangesAsync();
            return RedirectToAction(nameof(patientlogin), "Home");
        }

        //Businesss Data Store 

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreatePatientByBusiness(BusinessPatientRequest info)
        {
            User user = new User
            {
                Firstname = info.p_first_name,
                Lastname = info.p_last_name,
                Email = info.p_email,
                Mobile = info.p_phone,
                Street = info.p_street,
                City = info.p_city,
                State = info.p_state,
                Aspnetuserid = 2,
                Createdby = info.Createddate.ToShortDateString(),
                Createddate = info.Createddate
            };
            Aspnetuser aspuser = new Aspnetuser
            {
                Usarname = info.first_name,
                Passwordhash = info.last_name,
                Email = info.email,
                Phonenumber = info.phone,
            };
            _context.Aspnetusers.Add(aspuser);
            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction(nameof(patientlogin), "Home");
        }
        // Concierge Data Store 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePatientByConcierge(ConciergePatientRequest info)
        {
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
            Aspnetuser aspuser = new Aspnetuser
            {
                Usarname = info.first_name,
                Passwordhash = info.last_name,
                Email = info.pemail,
                Phonenumber = info.Phonenumber,
            };
            User user = new User
            {
                Firstname = info.first_name,
                Lastname = info.last_name,
                Email = info.pemail,
                Mobile = info.Phonenumber,
                Street = info.Street,
                City = info.City,
                State = info.State,
                Aspnetuserid = 2,
                Createdby = info.Createddate.ToShortDateString(),
                Createddate = info.Createddate
            };
            _context.Aspnetusers.Add(aspuser);
            _context.Concierges.Add(c);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(patientlogin), "Home");
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

        // GET: Aspnetusers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Usarname,Passwordhash")] Aspnetuser aspnetuser)
        {
            var userobj = await _context.Aspnetusers
            .FirstOrDefaultAsync(m => m.Usarname == aspnetuser.Usarname && m.Passwordhash == aspnetuser.Passwordhash);
            if (userobj == null)
            {
                return RedirectToAction(nameof(patientlogin), "Home");
            }
            return RedirectToAction(nameof(patientdashboard), "Home");
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
