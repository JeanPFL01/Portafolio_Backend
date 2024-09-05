﻿using System;
using System.Collections.Generic;

namespace Portafolio.Domain.Entities.StoreDB;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public int? SupplierId { get; set; }

    public int? CategoryId { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();

    public virtual Supplier? Supplier { get; set; }
}
