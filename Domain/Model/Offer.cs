namespace Domain.Model;

public class Offer
{
    public string Id { get; set; } = null!;
    public double Price { get; set; }
    public State State { get; set; }
}