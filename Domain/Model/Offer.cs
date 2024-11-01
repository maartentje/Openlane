using Domain.Dto;

namespace Domain.Model;

public class Offer
{
    public Guid Id { get; set; }
    public double Price { get; set; }
    public State State { get; set; }
    public Car Car { get; set; } = null!;
    public Guid CarId { get; set; }

    public Offer()
    {
        //used by ef
    }

    public Offer(OfferDto dto)
    {
        Price = dto.Price;
    }
}