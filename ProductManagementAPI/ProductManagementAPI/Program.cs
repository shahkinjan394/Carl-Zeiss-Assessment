using Microsoft.EntityFrameworkCore;
using ProductManagement.Data.Repositories;
using ProductManagement.Data;
using ProductManagement.Service.Interfaces;
using ProductManagement.Service.Services;
using ProductManagementAPI.Helpers;
using ProductManagementAPI.Middlewares;
using Serilog;
using Microsoft.Extensions.Configuration;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    )
);


builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
