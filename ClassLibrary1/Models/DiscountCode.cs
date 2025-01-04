using System;
using System.Collections.Generic;

namespace ClassLibrary1.Models;

public partial class DiscountCode
{
    public int DiscountCodeId { get; set; }

    public string Code { get; set; }

    public decimal DiscountPercentage { get; set; }

    public int MaxUsage { get; set; }

    public int? CurrentUsage { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual User CreatedByNavigation { get; set; }

    public virtual ICollection<OrderDiscount> OrderDiscounts { get; set; } = new List<OrderDiscount>();
}
