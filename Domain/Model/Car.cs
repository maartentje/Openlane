using Domain.Dto;

namespace Domain.Model;

public class Car
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public ICollection<Offer> Offers { get; set; } = new List<Offer>();

    public Car()
    {
        //used by ef
    }

    public Car(CarDto carDto)
    {
        Title = carDto.Title;
    }
}