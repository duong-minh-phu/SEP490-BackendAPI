using System;
using System.Collections.Generic;

namespace ClassLibrary1.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public string Phone { get; set; }

    public string Address { get; set; }

    public int RoleId { get; set; }

    public bool? IsStudent { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<DiscountCode> DiscountCodes { get; set; } = new List<DiscountCode>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual Role Role { get; set; }

    public virtual ICollection<Shop> Shops { get; set; } = new List<Shop>();

    public virtual ICollection<StudentInfo> StudentInfos { get; set; } = new List<StudentInfo>();
}
