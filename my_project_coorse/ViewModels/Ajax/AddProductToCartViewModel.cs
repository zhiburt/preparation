using System.ComponentModel.DataAnnotations;

namespace preparation.ViewModels.Ajax
{
    public class AddProductToCartViewModel
    {
        [Required]
        public string ProductName { get; set; }

        [Required]
        public string Supplier { get; set; }

        [Required]
        public string AddressSupplier { get; set; }
    }
}