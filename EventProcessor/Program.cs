using BL;
using DAL;
using Domain.Model;
using MassTransit;
using EventProcessor.Consumers;
using Shared.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddSharedLogger();
builder.Services.AddSharedServices();
var connectionString = builder.Configuration.GetConnectionString("Default")!;
builder.Services.AddAppDbContext(connectionString);
builder.Services.AddMassTransit(mt =>
{
    mt.AddConsumer<CarConsumer>();
    mt.AddConsumer<OfferConsumer>();
    mt.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.ConfigureEndpoints(ctx);
        cfg.Host("rabbitmq", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });
    });
});

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (bool.TryParse(Environment.GetEnvironmentVariable("SeedDb"), out var seedDb) && seedDb)
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        var carService = scope.ServiceProvider.GetRequiredService<ICarService>();
        carService.AddCar(new Car {Title = "Golf Car"});
    }
    else
    {
        if (bool.TryParse(Environment.GetEnvironmentVariable("EnsureDeleted"), out var ensureDeleted) && ensureDeleted)
            db.Database.EnsureDeleted();
        if (bool.TryParse(Environment.GetEnvironmentVariable("EnsureCreated"), out var ensureCreated) && ensureCreated)
            db.Database.EnsureCreated();
    }
}

host.Run();