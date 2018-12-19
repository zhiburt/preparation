using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace preparation.ViewModels.Supplier
{
    public enum TypePreparation : byte
    {
        Drops,
        Liquid,
        Tablets,
    };

    public class AddPreparationViewModel
    {
        public AddPreparationViewModel()
        {
        }

        public AddPreparationViewModel(string name, string activeIngredient, string type, string description, string imageURL)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ActiveIngredient = activeIngredient ?? throw new ArgumentNullException(nameof(activeIngredient));
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            ImageURL = imageURL ?? throw new ArgumentNullException(nameof(imageURL));
        }

        [Required]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "lenght must be more then 3")]
        public string Name { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "lenght must be more then 3")]
        public string ActiveIngredient { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "lenght must be more then 3")]
        public string Type { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "lenght must be more then 3")]
        public string Description { get; set; }
        [Required]
        [StringLength(400, MinimumLength = 3, ErrorMessage = "lenght must be more then 3")]
        public string ImageURL { get; set; }
    }


}
