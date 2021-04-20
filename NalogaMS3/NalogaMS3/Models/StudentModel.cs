using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NalogaMS3.Models
{
    public class Student
    {
        [Key]
        public Guid Student_ID { get; set; }

        public string Ime { get; set; }

        public string Priimek { get; set; }

        public string Email { get; set; }
    }
}
