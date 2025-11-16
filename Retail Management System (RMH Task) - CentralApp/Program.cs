using CentralApp.Data;
using CentralApp.Messages;
using CentralApp.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddDbContext<CentralDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ProductCreatedConsumer>();

    //x.UsingRabbitMq((context, cfg) =>
    //{
    //    var host = builder.Configuration.GetValue<string>("RabbitMq:Host") ?? "localhost";
    //    var username = builder.Configuration.GetValue<string>("RabbitMq:Username");
    //    var password = builder.Configuration.GetValue<string>("RabbitMq:Password");

    //    cfg.Host(host, h =>
    //    {
    //        if(!string.IsNullOrWhiteSpace(username)) h.Username(username);
    //        if(!string.IsNullOrWhiteSpace(password)) h.Password(password);
    //    });
    //});

    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", () => "App is running with in-memory transport!");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
