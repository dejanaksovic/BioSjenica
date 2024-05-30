using bioSjenica.CustomMappers;
using bioSjenica.Data;
using bioSjenica.Repositories;
using bioSjenica.Repositories.AnimalRepository;
using bioSjenica.Repositories.RegionRepository;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Custom services
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
builder.Services.AddScoped<IPlantRepository, PlantRepository>();
//Custom mappers
builder.Services.AddScoped<IAnimalMapper, AnimalMapper>();
builder.Services.AddScoped<IRegionMapper, RegionMapper>();
builder.Services.AddScoped<IPlantMapper, PlantMapper>();

builder.Services.AddDbContext<SqlContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Sql"));
});

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
