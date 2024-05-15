using System;
using System.Collections.Generic;

namespace StockControl.Models
{
    public partial class Waren
    {
        public Waren()
        {
            LieferantenWares = new HashSet<LieferantenWare>();
        }

        public int WarenId { get; set; }
        public string? Warennamen { get; set; }
        public string? Warentyp { get; set; }

        public virtual ICollection<LieferantenWare> LieferantenWares { get; set; }
    }
}
