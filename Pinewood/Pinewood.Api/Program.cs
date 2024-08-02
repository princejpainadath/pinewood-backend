
using Microsoft.EntityFrameworkCore;
using Pinewood.Application.Interfaces;
using Pinewood.Application.Services;
using Pinewood.Domain.Interfaces;
using Pinewood.Infrastructure.Context;
using Pinewood.Infrastructure.Mapping;
using Pinewood.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<PinewoodDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PinewoodDatabase")));


builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();


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
