using Microsoft.EntityFrameworkCore;
using NDIS.Order.API.DataAccess;
using NDIS.Shared.Common.Middlewares;
using NDIS.Order.API.Mappings;
using NDIS.Order.API.Repositories;
using NDIS.Order.API.Services;
using AutoMapper;
using NDIS.Order.API.ServiceClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System.Text;
using StackExchange.Redis;
using NDIS.Order.API.Service.Idempotency;
using NDIS.Order.API.Repository;
using NDIS.Order.API.Services.Outbox;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//// Redis
//builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
//{
//  var configuration = builder.Configuration.GetConnectionString("Redis");
//  return ConnectionMultiplexer.Connect(configuration!);
//});

var redisConnection = builder.Configuration["Redis:ConnectionString"];

if (!string.IsNullOrWhiteSpace(redisConnection))
{
  builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
      ConnectionMultiplexer.Connect(redisConnection));

  builder.Services.AddScoped<IIdempotencyService, RedisIdempotencyService>();
}
else
{
  builder.Services.AddScoped<IIdempotencyService, NoOpIdempotencyService>();
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  options.SwaggerDoc("v1", new() { Title = "NDIS.Order.API", Version = "v1" });

  // JWT
  options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
  {
    Name = "Authorization",
    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
    Scheme = "bearer",
    BearerFormat = "JWT",
    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
    Description = "Enter JWT token like: Bearer {your token}"
  });

 
  options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
//var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQ");
//builder.Services.AddMassTransit(x =>
//{
//  x.UsingRabbitMq((context, cfg) =>
//  {
//    cfg.Host(
//            rabbitMqSettings["Host"] ?? "localhost",
//            rabbitMqSettings["VirtualHost"] ?? "/",
//            h =>
//            {
//              h.Username(rabbitMqSettings["Username"] ?? "guest");
//              h.Password(rabbitMqSettings["Password"] ?? "guest");
//            });
//  });
//});
var rabbitHost = builder.Configuration["RabbitMQ:Host"];

if (!string.IsNullOrWhiteSpace(rabbitHost))
{
  builder.Services.AddMassTransit(x =>
  {
    x.UsingRabbitMq((context, cfg) =>
    {
      cfg.Host(
        rabbitHost,
        builder.Configuration["RabbitMQ:VirtualHost"] ?? "/",
        h =>
        {
          h.Username(builder.Configuration["RabbitMQ:Username"] ?? "guest");
          h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
        });
    });
  });
}

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NDISOrderService")));
builder.Services.AddAutoMapper(typeof(OrderMappingProfile));
builder.Services.AddHttpContextAccessor();
//builder.Services.AddScoped<IIdempotencyService, RedisIdempotencyService>();
builder.Services.AddScoped<IOrderEventRepository, OrderEventRepository>();
//builder.Services.AddHostedService<OrderEventProcessor>();

builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
  var jwtSettings = builder.Configuration.GetSection("Jwt");

  options.RequireHttpsMetadata = false;
  options.SaveToken = true;

  options.TokenValidationParameters = new TokenValidationParameters
  {
    NameClaimType = ClaimTypes.NameIdentifier,
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = jwtSettings["Issuer"],
    ValidAudience = jwtSettings["Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
      ),
    ClockSkew = TimeSpan.Zero
  };
  options.Events = new JwtBearerEvents
  {
    OnMessageReceived = context =>
    {
      Console.WriteLine("Authorization: " + context.Request.Headers["Authorization"]);
      return Task.CompletedTask;
    },
    OnAuthenticationFailed = context =>
    {
      Console.WriteLine("AUTH FAILED: " + context.Exception.Message);
      return Task.CompletedTask;
    },
    OnChallenge = context =>
    {
      Console.WriteLine("CHALLENGE ERROR: " + context.Error);
      Console.WriteLine("CHALLENGE DESC: " + context.ErrorDescription);
      return Task.CompletedTask;
    },
    OnTokenValidated = context =>
    {
      Console.WriteLine("TOKEN VALIDATED");
      return Task.CompletedTask;
    }
  };


});
builder.Services.AddAuthorization();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddHttpClient<IUserServiceClient, UserServiceClient>(client =>
{
  client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:UserApi"]!);
});

builder.Services.AddHttpClient<IServiceServiceClient, ServiceServiceClient>(client =>
{
  client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ServiceApi"]!);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
