using MassTransit;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSharedLogger();
builder.Services.AddMvc();
builder.Services.AddSharedServices();
var connectionString = builder.Configuration.GetConnectionString("Default")!;
builder.Services.AddAppDbContext(connectionString);
builder.Services.AddMassTransit(mt =>
{
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

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseHsts();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();