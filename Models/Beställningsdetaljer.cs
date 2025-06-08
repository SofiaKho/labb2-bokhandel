using System;
using System.Collections.Generic;

namespace BokhandelApp.Models;

public partial class Beställningsdetaljer
{
    public int BeställningId { get; set; }

    public string Isbn { get; set; } = null!;

    public int? Antal { get; set; }

    public decimal? Pris { get; set; }

    public virtual Beställningar Beställning { get; set; } = null!;

    public virtual Böcker IsbnNavigation { get; set; } = null!;
}
