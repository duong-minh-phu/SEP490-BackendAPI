using System;
using System.Collections.Generic;

namespace ClassLibrary1.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public int PaymentMethodId { get; set; }

    public DateTime? PaymentDate { get; set; }

    public decimal AmountPaid { get; set; }

    public string Status { get; set; }

    public virtual Order Order { get; set; }

    public virtual PaymentMethod PaymentMethod { get; set; }
}
