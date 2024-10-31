namespace Domain.Model;

public class Car
{
    public Guid Id { get; set; }
    public ICollection<Offer> Offers { get; set; } = new List<Offer>();
}