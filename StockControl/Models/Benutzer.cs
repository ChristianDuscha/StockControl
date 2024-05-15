using System;
using System.Collections.Generic;

namespace StockControl.Models
{
    public partial class Benutzer
    {
        public Benutzer()
        {
            Lagers = new HashSet<Lager>();
        }

        public int BenutzerId { get; set; }
        public string? Rolle { get; set; }
        public string? Name { get; set; }
        public string? Adresse { get; set; }
        public string? Telefon { get; set; }
        public string? Email { get; set; }

        public virtual ICollection<Lager> Lagers { get; set; }
    }
}
