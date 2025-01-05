using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DTO.ProductImage
{
    public class ProductImageDTO
    {
        public int ImageId { get; set; }
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
