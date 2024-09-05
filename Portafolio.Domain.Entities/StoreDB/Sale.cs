﻿using System;
using System.Collections.Generic;

namespace Portafolio.Domain.Entities.StoreDB;

public partial class Sale
{
    public int SaleId { get; set; }

    public int? CustomerId { get; set; }

    public int? UserId { get; set; }

    public DateTime? SaleDate { get; set; }

    public decimal Total { get; set; }

    public decimal? Tax { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();

    public virtual User? User { get; set; }
}
