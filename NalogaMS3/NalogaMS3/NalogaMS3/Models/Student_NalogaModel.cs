using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NalogaMS3.Models
{
    public class Student_Naloga
    {
        [Key]
        public Guid Student_NalogaID { get; set; }

        public String Student_ID { get; set; }
        public String Naloga_ID { get; set; }

        public DateTime DatumOddaje { get; set; }

        public byte Ocena { get; set; }
    }
}
