using System;
using System.Collections.Generic;

namespace BokhandelApp.Models;

public partial class Beställningar
{
    public int Id { get; set; }

    public int? KundId { get; set; }

    public DateOnly? Datum { get; set; }

    public decimal? TotalPris { get; set; }

    public virtual ICollection<Beställningsdetaljer> Beställningsdetaljers { get; set; } = new List<Beställningsdetaljer>();

    public virtual Kunder? Kund { get; set; }
}
