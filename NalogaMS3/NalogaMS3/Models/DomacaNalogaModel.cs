using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NalogaMS3.Models
{
    public class DomacaNaloga
    {

        [Key]
        public Guid NalogaID { get; set; }

        public string Naslov { get; set; }

        public DateTime RokZaOddajo { get; set; }

    }
}
