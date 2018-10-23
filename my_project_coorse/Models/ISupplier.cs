namespace preparation.Models
{
    public interface ISupplier
    {
        string Address { get; set; }
        string Company { get; set; }
        string Description { get; set; }
        string Geolocation { get; set; }
        int Id { get; set; }
        string Name { get; set; }
    }
}