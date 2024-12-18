using bioSjenica.CustomMappers;
using bioSjenica.Data;
using bioSjenica.Middleware;
using bioSjenica.Repositories;
using bioSjenica.Repositories.AnimalRepository;
using bioSjenica.Repositories.RegionRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//Prevents circular data
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
builder.Services.AddScoped<IFeedingGroundRepository, FeedingGroundRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
//Custom mappers
builder.Services.AddScoped<IAnimalMapper, AnimalMapper>();
builder.Services.AddScoped<IRegionMapper, RegionMapper>();
builder.Services.AddScoped<IPlantMapper, PlantMapper>();
builder.Services.AddScoped<IFeedingGroundsMapper, FeedingGroundMapper>();
builder.Services.AddScoped<IUserMapper, UserMapper>();

//JWT
// builder.Services.AddAuthentication(c => {
//     c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//     c.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
// }).AddJwtBearer(x => {
//     x.RequireHttpsMetadata = false;
//     x.SaveToken = false;
//     x.TokenValidationParameters = new TokenValidationParameters {
//         ValidateIssuerSigningKey = true,
//         IssuerSigningKey = new SymmetricSecurityKey(
//             Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"] ?? "123")
//         ),
//         ValidateIssuer = false,
//         ValidateAudience = false,
//         ClockSkew = TimeSpan.Zero
//     };
// });

//Middleware
builder.Services.AddTransient<GlobalResponseExceptionMiddleware>();

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

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalResponseExceptionMiddleware>();

app.MapControllers();

app.Run();
