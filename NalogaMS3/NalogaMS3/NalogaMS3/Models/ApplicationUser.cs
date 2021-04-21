using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NalogaMS3.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string Ime { get; set; }
        public string Priimek { get; set; }
    }
}
