using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSharedLogger();
builder.Services.AddMvc();
builder.Services.AddSharedServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseHsts();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();