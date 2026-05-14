using MassTransit;
using MassTransit.Futures.Contracts;
using Microsoft.EntityFrameworkCore;
using NDIS.Payment.API.Consumers;
using NDIS.Payment.API.Data;
using NDIS.Payment.API.Repositories;
using NDIS.Payment.API.Services;
using NDIS.Payment.API.Services.Outbox;
using NDIS.Shared.Common.Middlewares;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
  x.AddConsumer<OrderCreatedConsumer>();

  x.UsingRabbitMq((context, cfg) =>
  {
    cfg.Host("localhost", "/", h =>
    {
      h.Username("guest");
      h.Password("guest");
    });

    cfg.ReceiveEndpoint("payment-order-created", e =>
    {
      e.UseMessageRetry(r =>
      {
        r.Interval(3, TimeSpan.FromSeconds(5));
      });

      e.ConfigureConsumer<OrderCreatedConsumer>(context);
    });
  });
});
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddHostedService<OutboxProcessor>();
builder.Services.AddScoped<IStripePaymentService, StripePaymentService>();

var stripeSecretKey = builder.Configuration["Stripe:SecretKey"];

if (string.IsNullOrWhiteSpace(stripeSecretKey))
{
  throw new InvalidOperationException("Stripe SecretKey is not configured.");
}

StripeConfiguration.ApiKey = stripeSecretKey;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
