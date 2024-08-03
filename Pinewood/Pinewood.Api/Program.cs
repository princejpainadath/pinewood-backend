
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Pinewood.Application.Interfaces;
using Pinewood.Application.Services;
using Pinewood.Domain.Interfaces;
using Pinewood.Infrastructure.Context;
using Pinewood.Infrastructure.Mapping;
using Pinewood.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Pinewood API" });
});


builder.Services.AddDbContext<PinewoodDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PinewoodDatabase")));


builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
