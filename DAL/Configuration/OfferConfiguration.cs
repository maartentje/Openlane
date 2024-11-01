using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configuration;

public class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).ValueGeneratedOnAdd();
    }
}