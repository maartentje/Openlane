using BL;
using DAL;
using OfferWorker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddTransient<IOfferService, OfferService>();
builder.Services.AddTransient<IOfferRepository, OfferRepository>();

builder.Services.AddSingleton<IQueueService, QueueService>();

var host = builder.Build();
host.Run();