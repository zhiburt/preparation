using System;

namespace preparation.Models
{
    public class Supplier
    {
        public Supplier(string name, string company, string address, string geolocation)
        {
            Name = name;
            Company = company;
            Address = address;
            Geolocation = geolocation;
        }

        public string Name { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string Geolocation { get; set; }

        public override bool Equals(object obj)
        {
            var supplier = obj as Supplier;
            return supplier != null &&
                   Name == supplier.Name &&
                   Company == supplier.Company &&
                   Address == supplier.Address &&
                   Geolocation == supplier.Geolocation;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Company, Address, Geolocation);
        }
    }
}