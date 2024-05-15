using System;
using System.Collections.Generic;

namespace StockControl.Models
{
    public partial class Lager
    {
        public int LagerId { get; set; }
        public int? BenutzerId { get; set; }
        public string? Lagername { get; set; }
        public string? Standort { get; set; }
        public decimal? Bestand { get; set; }

        public virtual Benutzer? Benutzer { get; set; }
    }
}
