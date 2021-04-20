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
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Student
        public async Task<IActionResult> Index()
        {
            return View(await _context.Studenti.ToListAsync());
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Studenti
                .FirstOrDefaultAsync(m => m.Student_ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Student_ID,Ime,Priimek,Email")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.Student_ID = Guid.NewGuid();
                _context.Add(student);

                List<DomacaNaloga> naloge = await _context.DomaceNaloge.ToListAsync();
                foreach (var naloga in naloge)
                {
                    Student_Naloga std_nal = new Student_Naloga();
                    std_nal.Student_NalogaID = Guid.NewGuid();
                    std_nal.Student_ID = student.Student_ID;
                    std_nal.Naloga_ID = naloga.NalogaID;

                    _context.Add(std_nal);
                }


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Studenti.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Student_ID,Ime,Priimek,Email")] Student student)
        {
            if (id != student.Student_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Student_ID))
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
            return View(student);
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Studenti
                .FirstOrDefaultAsync(m => m.Student_ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var student = await _context.Studenti.FindAsync(id);
            _context.Studenti.Remove(student);

            List<Student_Naloga> student_naloga = await _context.Studenti_Naloge.Where(x => x.Student_ID == student.Student_ID).ToListAsync();
            foreach(Student_Naloga naloga in student_naloga)
            {
                _context.Studenti_Naloge.Remove(naloga);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(Guid id)
        {
            return _context.Studenti.Any(e => e.Student_ID == id);
        }
    }
}
