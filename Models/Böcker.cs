using System;
using System.Collections.Generic;

namespace BokhandelApp.Models;

public partial class Böcker
{
    public string Isbn { get; set; } = null!;

    public string? Titel { get; set; }

    public string? Språk { get; set; }

    public decimal? Pris { get; set; }

    public DateOnly? Utgivningsdatum { get; set; }

    public int? FörfattareId { get; set; }

    public virtual ICollection<Beställningsdetaljer> Beställningsdetaljers { get; set; } = new List<Beställningsdetaljer>();

    public virtual Författare? Författare { get; set; }

    public virtual ICollection<LagerSaldo> LagerSaldos { get; set; } = new List<LagerSaldo>();

    public virtual ICollection<Kategorier> Kategoris { get; set; } = new List<Kategorier>();
}
