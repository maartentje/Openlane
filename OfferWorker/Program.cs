using BL;
using DAL;
using Microsoft.EntityFrameworkCore;
using OfferWorker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddTransient<IOfferService, OfferService>();
builder.Services.AddTransient<IOfferRepository, OfferRepository>();

builder.Services.AddSingleton<IQueueService, QueueService>();

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
    db.Database.EnsureCreated();
}

host.Run();