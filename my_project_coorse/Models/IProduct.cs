namespace preparation.Models
{
    public interface IProduct
    {
        decimal Price { get; set; }
        IGood Product { get; set; }
        ISupplier Supplier { get; set; }
    }
}