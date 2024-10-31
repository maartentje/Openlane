namespace Domain.Model;

public class Offer
{
    public Guid Id { get; set; }
    public string IdOutdated { get; set; } = null!;
    public double Price { get; set; }
    public State State { get; set; }
    public Car Car { get; set; } = null!;
}