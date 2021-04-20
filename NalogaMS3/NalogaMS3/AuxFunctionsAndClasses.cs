using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NalogaMS3
{
    public class OddajaView
    {
        public Guid StudentNalogaID { get; set; }
        public string ImeStudenta { get; set; }
        public DateTime DatumOddaje { get; set; }
        public byte Ocena { get; set; }
    }

}
