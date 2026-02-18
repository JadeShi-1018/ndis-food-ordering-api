
using Microsoft.EntityFrameworkCore;
using NDIS.Shared.Common.Middlewares;
using NDISS.UserRebate.API;
using NDISS.UserRebate.API.Data;
using NDISS.UserRebate.API.Repository;
using NDISS.UserRebate.API.Service;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//repositories
builder.Services.AddScoped<IUserRebateRepository,UserRebateRepository>();
builder.Services.AddScoped<IRebateEventRepository, RebateEventRepository>();

//services
builder.Services.AddScoped<IUserRebateService,UserRebateService>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<ITokenRefreshService, TokenRefreshService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));



var app = builder.Build();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
