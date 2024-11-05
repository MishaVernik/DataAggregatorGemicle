using DAG.Infrastructure.Context;
using DataAggregatorGemicle.Queries.Customer;
using DataAggregatorGemicle.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataAggregatorGemicleContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DataAggregatorGemicleDatabase")));

// Register individual customer queries
builder.Services.AddScoped<IQuietCustomerQuery, Customer2QuietCustomerQuery>();
builder.Services.AddScoped<IQuietCustomerQuery, Customer101QuietCustomerQuery>();
builder.Services.AddScoped<IQuietCustomerQuery, Customer145QuietCustomerQuery>();

builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IQuietCustomerService, QuietCustomerService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
