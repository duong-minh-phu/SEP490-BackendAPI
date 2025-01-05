﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DTO
{
    public class UpdateProductDTO
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }
        public string Specs { get; set; }
        public int CategoryId { get; set; }
    }
}
