using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using preparation.Models;

namespace preparation.Controllers
{
    public class ChangeProductViewModel
    {
        [Required]
        public IEnumerable<IProduct> Products { get; set; }
        [Required]
        public int IndexChanges { get; set; }
        [Required]
        public decimal NewPrice { get; set; }
    }
}