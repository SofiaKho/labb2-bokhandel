using System;
using System.Collections.Generic;

namespace BokhandelApp.Models;

public partial class Kunder
{
    public int Id { get; set; }

    public string? Förnamn { get; set; }

    public string? Efternamn { get; set; }

    public string? Epost { get; set; }

    public string? Telefon { get; set; }

    public virtual ICollection<Beställningar> Beställningars { get; set; } = new List<Beställningar>();
}
