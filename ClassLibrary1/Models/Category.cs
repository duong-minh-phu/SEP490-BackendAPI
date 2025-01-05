using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClassLibrary1.Models;

public partial class Category
{
    [Key]
    public int CategoryId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
