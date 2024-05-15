using System;
using System.Collections.Generic;

namespace StockControl.Models
{
    public partial class Lieferant
    {
        public Lieferant()
        {
            LieferantenWares = new HashSet<LieferantenWare>();
        }

        public int LieferantenId { get; set; }
        public string? Name { get; set; }
        public string? Adresse { get; set; }
        public string? Telefon { get; set; }

        public virtual ICollection<LieferantenWare> LieferantenWares { get; set; }
    }
}
