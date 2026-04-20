using MassTransit;
using MassTransit.Futures.Contracts;
using Microsoft.EntityFrameworkCore;
using NDIS.Payment.API.Data;
using NDIS.Payment.API.Repositories;
using NDIS.Payment.API.Repository;
using NDIS.Payment.API.ServiceClient;
using NDIS.Payment.API.Services;
using NDIS.Shared.Common.Middlewares;
using Stripe;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<IOrderServiceClient, OrderServiceClient>(client =>
{
  client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:OrderApi"]!);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapGet("/", () => "Payment API is running");

app.MapControllers();

app.Run();
