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
    [Authorize(Roles = "admin")]
    public class StudentiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Student
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToArrayAsync());
        }


        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var student = await _context.Users.FindAsync(id);
            _context.Users.Remove(student);

            List<Student_Naloga> student_naloga = await _context.Studenti_Naloge.Where(x => x.Student_ID == student.Id).ToListAsync();
            foreach(Student_Naloga naloga in student_naloga)
            {
                _context.Studenti_Naloge.Remove(naloga);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
