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
builder.Services.AddSwaggerGen(o => {
    o.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
});

// Custom services
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
builder.Services.AddScoped<IPlantRepository, PlantRepository>();
builder.Services.AddScoped<IFeedingGroundRepository, FeedingGroundRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
//Custom mappers
builder.Services.AddScoped<IAnimalMapper, AnimalMapper>();
builder.Services.AddScoped<IRegionMapper, RegionMapper>();
builder.Services.AddScoped<IPlantMapper, PlantMapper>();
builder.Services.AddScoped<IFeedingGroundsMapper, FeedingGroundMapper>();
builder.Services.AddScoped<IUserMapper, UserMapper>();

//JWT
var jwtSettings = builder.Configuration.GetSection("JWT");
builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt => {
    opt.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("Secret").Value))
    };
    opt.Events = new JwtBearerEvents{
        OnMessageReceived = ctx => {
            ctx.Request.Cookies.TryGetValue("accessToken", out var AT);
            if(!(String.IsNullOrEmpty(AT))) {
                ctx.Token = AT;
            }
        return Task.CompletedTask;
        }
    };
});

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

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalResponseExceptionMiddleware>();

app.MapControllers();

app.Run();
