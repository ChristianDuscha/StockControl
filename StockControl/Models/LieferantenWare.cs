using System;
using System.Collections.Generic;

namespace StockControl.Models
{
    public partial class LieferantenWare
    {
        public int LieferantenId { get; set; }
        public int WarenId { get; set; }
        public DateTime? Lieferdatum { get; set; }
        public decimal? Preis { get; set; }
        public int? Stückzahl { get; set; }

        public virtual Lieferant Lieferanten { get; set; } = null!;
        public virtual Waren Waren { get; set; } = null!;
    }
}
