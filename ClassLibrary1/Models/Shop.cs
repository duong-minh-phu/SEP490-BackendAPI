using System;
using System.Collections.Generic;

namespace ClassLibrary1.Models;

public partial class Shop
{
    public int ShopId { get; set; }

    public int UserId { get; set; }

    public string ShopName { get; set; }

    public string ShopAddress { get; set; }

    public string PhoneNumber { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual User User { get; set; }
}
