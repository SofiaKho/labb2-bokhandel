using System;
using System.Collections.Generic;

namespace BokhandelApp.Models;

public partial class Kategorier
{
    public int Id { get; set; }

    public string? Namn { get; set; }

    public virtual ICollection<Böcker> Isbns { get; set; } = new List<Böcker>();
}
