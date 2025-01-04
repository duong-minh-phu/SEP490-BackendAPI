using System;
using System.Collections.Generic;

namespace ClassLibrary1.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public string Specs { get; set; }

    public int CategoryId { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category Category { get; set; }

    public virtual User CreatedByNavigation { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
}
