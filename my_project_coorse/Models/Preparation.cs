using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace preparation.Models
{
    public class Preparation
    {
        public Preparation(string name, string mainIngredient, string description, decimal price, string imageURL, string activeIngredient, string type)
        {
            Name = name;
            ActiveIngredient = mainIngredient;
            Description = description;
            Price = price;
            ImageURL = imageURL;
            ActiveIngredient = activeIngredient;
            Type = type;
        }

        public string Name { get; set; }
        public string ActiveIngredient { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; }
        public Supplier Supplier { get; set; }


        public override bool Equals(object obj)
        {
            var preparation = obj as Preparation;
            return preparation != null &&
                   Name == preparation.Name &&
                   ActiveIngredient == preparation.ActiveIngredient &&
                   Description == preparation.Description &&
                   Price == preparation.Price &&
                   ImageURL == preparation.ImageURL;
        }

        public override int GetHashCode() => HashCode.Combine(Name, ActiveIngredient, Description, Price, ImageURL);
    }
}
