using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using NalogaMS3.Models;

namespace NalogaMS3.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DomacaNaloga> DomaceNaloge { get; set; }
        public DbSet<Student_Naloga> Studenti_Naloge { get; set; }
    }
}
