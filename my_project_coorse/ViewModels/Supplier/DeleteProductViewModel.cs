using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace preparation.ViewModels.Supplier
{
    public class DeleteProductViewModel
    {
        [Required]
        public IEnumerable<Models.IProduct> Products { get; set; }
        [Required]
        public int IndexChanges { get; set; }
    }
}
