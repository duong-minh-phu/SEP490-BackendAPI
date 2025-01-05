using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DTO.Category
{
    public class CreateCategoryDTO
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
