using System;
using System.Collections.Generic;

namespace ClassLibrary1.Models;

public partial class OrderDiscount
{
    public int OrderDiscountId { get; set; }

    public int OrderId { get; set; }

    public int DiscountCodeId { get; set; }

    public virtual DiscountCode DiscountCode { get; set; }

    public virtual Order Order { get; set; }
}
