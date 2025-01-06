using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DTO.Cart
{
    public class CartDTO
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public List<CartItemDTO> CartItems { get; set; } = new List<CartItemDTO>();
    }
}
