using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NalogaMS3.Data;
using NalogaMS3.Models;

namespace NalogaMS3
{
    public class DomacaNalogaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DomacaNalogaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DomacaNaloga
        public async Task<IActionResult> Index()
        {
            return View(await _context.DomaceNaloge.ToListAsync());
        }


        [Authorize(Roles = "admin")]
        // GET: DomacaNaloga/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var domacaNaloga = await _context.DomaceNaloge
                .FirstOrDefaultAsync(m => m.NalogaID == id);
            if (domacaNaloga == null)
            {
                return NotFound();
            }

            return View(domacaNaloga);
        }

        [Authorize(Roles = "admin")]
        // GET: DomacaNaloga/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DomacaNaloga/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NalogaID,Naslov,RokZaOddajo")] DomacaNaloga domacaNaloga)
        {
            if (ModelState.IsValid)
            {
                List<ApplicationUser> studenti = await _context.Users.ToListAsync();
                domacaNaloga.NalogaID = Guid.NewGuid();
                _context.Add(domacaNaloga);
                await _context.SaveChangesAsync();

                foreach (var student in studenti)
                {
                    if(student.Email != "profesor@prof.com")
                    {
                        Student_Naloga std_nal = new Student_Naloga();
                        std_nal.Student_NalogaID = Guid.NewGuid();
                        std_nal.Student_ID = student.Id.ToString();
                        std_nal.Naloga_ID = domacaNaloga.NalogaID.ToString();

                        _context.Add(std_nal);

                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(domacaNaloga);
        }

        [Authorize(Roles = "admin")]
        // GET: DomacaNaloga/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var domacaNaloga = await _context.DomaceNaloge.FindAsync(id);
            if (domacaNaloga == null)
            {
                return NotFound();
            }
            return View(domacaNaloga);
        }

        // POST: DomacaNaloga/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("NalogaID,Naslov,RokZaOddajo")] DomacaNaloga domacaNaloga)
        {
            if (id != domacaNaloga.NalogaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(domacaNaloga);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DomacaNalogaExists(domacaNaloga.NalogaID))
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
            return View(domacaNaloga);
        }

        // GET: DomacaNaloga/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var domacaNaloga = await _context.DomaceNaloge
                .FirstOrDefaultAsync(m => m.NalogaID == id);
            if (domacaNaloga == null)
            {
                return NotFound();
            }

            return View(domacaNaloga);
        }

        // POST: DomacaNaloga/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var domacaNaloga = await _context.DomaceNaloge.FindAsync(id);
            _context.DomaceNaloge.Remove(domacaNaloga);

            List<Student_Naloga> student_naloga = await _context.Studenti_Naloge.Where(x => x.Naloga_ID == domacaNaloga.NalogaID.ToString()).ToListAsync();
            foreach (Student_Naloga naloga in student_naloga)
            {
                _context.Studenti_Naloge.Remove(naloga);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PregledOddaj(Guid? id)
        {
            DomacaNaloga NalogaCurrent = await _context.DomaceNaloge.FindAsync(id);

            ViewBag.NalogaNaslov = NalogaCurrent.Naslov;

            if (id == null)
            {
                return NotFound();
            }

            List<ApplicationUser> studenti = await _context.Users.ToListAsync();
            List<Student_Naloga> naloge = await _context.Studenti_Naloge.Where(nal => nal.Naloga_ID == id.ToString()).ToListAsync();

            IEnumerable<OddajaView> pregledOddaj = from naloga in naloge
                                                   join student in studenti on naloga.Student_ID equals student.Id.ToString()
                                                   select new OddajaView()
                                                   {
                                                       StudentNalogaID = naloga.Student_NalogaID,
                                                       ImeStudenta = $"{student.Ime} {student.Priimek}",
                                                       DatumOddaje = naloga.DatumOddaje,
                                                       Ocena = naloga.Ocena
                                                   };
            if (pregledOddaj == null)
            {
                return NotFound();
            }
            return View(pregledOddaj);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> OddanaNalogaUpdate(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student_Naloga domacaNaloga = await _context.Studenti_Naloge
                .FirstOrDefaultAsync(m => m.Student_NalogaID == id);

            DomacaNaloga naloga = await _context.DomaceNaloge.FindAsync(Guid.Parse(domacaNaloga.Naloga_ID));
            ViewBag.NaslovNaloge = naloga.Naslov;

            ApplicationUser student = await _context.Users.FindAsync(domacaNaloga.Student_ID);
            ViewBag.ImeStudenta = student.Ime;
            ViewBag.PriimekStudenta = student.Priimek;

            if (domacaNaloga == null)
            {
                return NotFound();
            }

            return View(domacaNaloga);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OddanaNalogaUpdate(Guid id, [Bind("Student_NalogaID,Student_ID,Naloga_ID,Ocena")] Student_Naloga domacaNaloga)
        {
            if (id != domacaNaloga.Student_NalogaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(domacaNaloga);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DomacaNalogaExists(domacaNaloga.Student_NalogaID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(PregledOddaj), new { id = domacaNaloga.Naloga_ID });
            }
            return View(domacaNaloga);
        }

        [Authorize(Roles = "student")]

        public async Task<IActionResult> MojeOcene(string id)
        {
            List<Student_Naloga> naloge = await _context.Studenti_Naloge.Where(x => x.Student_ID == id).ToListAsync();
            List<DomacaNaloga> domaceNaloge = await _context.DomaceNaloge.ToListAsync();

            IEnumerable<OddajaView> pregledOddaj = from naloga in naloge
                                                   join domaca in domaceNaloge on naloga.Naloga_ID equals domaca.NalogaID.ToString()
                                                   select new OddajaView()
                                                   {
                                                       NaslovNaloge = domaca.Naslov,
                                                       StudentNalogaID = naloga.Student_NalogaID,
                                                       ImeStudenta = $"",
                                                       DatumOddaje = naloga.DatumOddaje,
                                                       Ocena = naloga.Ocena
                                                   };
            return View(pregledOddaj);
        }

        private bool DomacaNalogaExists(Guid id)
        {
            return _context.DomaceNaloge.Any(e => e.NalogaID == id);
        }
    }
}
