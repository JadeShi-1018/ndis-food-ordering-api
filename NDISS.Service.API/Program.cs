using Microsoft.EntityFrameworkCore;
using NDISS.Service.API.Data;
using NDISS.Service.API.Repositories;
using NDISS.Service.API.Services;
using Serilog;
using NDISS.Service.API.Mappers;
using NDISServiceAPI.DataAcess.Repository.ItemRepository;
using NDISServiceAPI.Services.ItemService;
using FluentValidation.AspNetCore;
using FluentValidation;
using NDIS.Shared.Common.Middlewares;


// Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NDISSService")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfiles));

// Repositories & Services
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IServiceTypeService, ServiceTypeService>();
builder.Services.AddScoped<IProviderServiceService, ProviderServiceService>();
builder.Services.AddScoped<IProviderServiceLocationService, ProviderServiceLocationService>();
builder.Services.AddScoped<IWeeklyPlanService, WeeklyPlanService>();
builder.Services.AddScoped<ISinglePlanService, SinglePlanService>();

//Cors Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Item
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

// Global Exception Handler
app.UseMiddleware<GlobalExceptionHandlerMiddleware>(); 

app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();
