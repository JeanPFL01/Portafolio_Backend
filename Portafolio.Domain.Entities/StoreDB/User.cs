using System;
using System.Collections.Generic;

namespace Portafolio.Domain.Entities.StoreDB;

public partial class User
{
    public int UserId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string Name { get; set; } = null!;

    public string? Position { get; set; }

    public DateTime? HireDate { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
