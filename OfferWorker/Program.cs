using DAL;
using Microsoft.EntityFrameworkCore;
using OfferWorker;
using Shared.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddSharedLogger();
builder.Services.AddHostedService<Worker>();
builder.Services.AddSharedServices();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default")!;
    var password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
    connectionString = string.Format(connectionString, password);

    options.UseSqlServer(connectionString);
});

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (bool.TryParse(Environment.GetEnvironmentVariable("EnsureDeleted"), out var ensureDeleted) && ensureDeleted)
        db.Database.EnsureDeleted();
    if (bool.TryParse(Environment.GetEnvironmentVariable("EnsureCreated"), out var ensureCreated) && ensureCreated)
        db.Database.EnsureCreated();
}

host.Run();