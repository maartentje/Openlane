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
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var conf = builder.Configuration.GetSection("DB");
        if (conf.GetValue<bool>("EnsureDeleted"))
            db.Database.EnsureDeleted();
        if (conf.GetValue<bool>("EnsureCreated"))
            db.Database.EnsureCreated();
    }
    catch (Exception _)
    {
        //if multiple docker instances try to delete, it will throw error, ignore for now
    }
    
}

host.Run();