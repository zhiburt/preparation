using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace preparation.ViewModels.Supplier
{
    public class AddProductViewModel
    {

        [Required]
        public int ProductId { get; set; }

        public int SupplierId { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
