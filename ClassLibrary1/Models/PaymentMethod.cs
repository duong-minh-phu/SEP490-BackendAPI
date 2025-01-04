using System;
using System.Collections.Generic;

namespace ClassLibrary1.Models;

public partial class PaymentMethod
{
    public int PaymentMethodId { get; set; }

    public string MethodName { get; set; }

    public string Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
