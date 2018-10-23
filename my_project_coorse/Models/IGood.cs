namespace preparation.Models
{
    public interface IGood
    {
        string Description { get; set; }
        int Id { get; set; }
        string ImageURL { get; set; }
        string Name { get; set; }
        string Type { get; set; }
    }
}