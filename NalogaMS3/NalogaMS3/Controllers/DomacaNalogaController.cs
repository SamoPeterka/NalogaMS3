using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        // GET: DomacaNaloga/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DomacaNaloga/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NalogaID,Naslov,RokZaOddajo")] DomacaNaloga domacaNaloga)
        {
            if (ModelState.IsValid)
            {
                List<Student> studenti = await _context.Studenti.ToListAsync();

                domacaNaloga.NalogaID = Guid.NewGuid();
                _context.Add(domacaNaloga);
                await _context.SaveChangesAsync();

                foreach (var student in studenti)
                {
                    Student_Naloga std_nal = new Student_Naloga();
                    std_nal.Student_NalogaID = Guid.NewGuid();
                    std_nal.Student_ID = student.Student_ID;
                    std_nal.Naloga_ID = domacaNaloga.NalogaID;

                    _context.Add(std_nal);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(domacaNaloga);
        }

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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var domacaNaloga = await _context.DomaceNaloge.FindAsync(id);
            _context.DomaceNaloge.Remove(domacaNaloga);

            List<Student_Naloga> student_naloga = await _context.Studenti_Naloge.Where(x => x.Naloga_ID == domacaNaloga.NalogaID).ToListAsync();
            foreach (Student_Naloga naloga in student_naloga)
            {
                _context.Studenti_Naloge.Remove(naloga);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> PregledOddaj(Guid? id)
        {
            DomacaNaloga NalogaCurrent = await _context.DomaceNaloge.FindAsync(id);

            ViewBag.NalogaNaslov = NalogaCurrent.Naslov;

            if (id == null)
            {
                return NotFound();
            }

            List<Student> studenti = await _context.Studenti.ToListAsync();
            List<Student_Naloga> naloge = await _context.Studenti_Naloge.Where(nal => nal.Naloga_ID == id).ToListAsync();

            IEnumerable<OddajaView> pregledOddaj = from naloga in naloge
                                                   join student in studenti on naloga.Student_ID equals student.Student_ID
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

        public async Task<IActionResult> OddanaNalogaUpdate(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student_Naloga domacaNaloga = await _context.Studenti_Naloge
                .FirstOrDefaultAsync(m => m.Student_NalogaID == id);

            DomacaNaloga naloga = await _context.DomaceNaloge.FindAsync(domacaNaloga.Naloga_ID);
            ViewBag.NaslovNaloge = naloga.Naslov;

            Student student = await _context.Studenti.FindAsync(domacaNaloga.Student_ID);
            ViewBag.ImeStudenta = student.Ime;
            ViewBag.PriimekStudenta = student.Priimek;

            if (domacaNaloga == null)
            {
                return NotFound();
            }

            return View(domacaNaloga);
        }

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

        private bool DomacaNalogaExists(Guid id)
        {
            return _context.DomaceNaloge.Any(e => e.NalogaID == id);
        }
    }
}
