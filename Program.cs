using localMarketingSystem.BAL.Interfaces;
using localMarketingSystem.BAL.Services;
using localMarketingSystem.Configuration;
using localMarketingSystem.DAL;
using localMarketingSystem.DAL.Interfaces;
using localMarketingSystem.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Database Connection
builder.Services.AddDbContext<localMarketingSystemDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DBConnection"),
    //options => options.CommandTimeout(999)                   
    options => options.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null)
), ServiceLifetime.Transient);

// -------------- Start JWT --------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secretkey = Encoding.UTF8.GetBytes("DIag0Js8c0k3Ajw3R19kxBsmNqSJFEYf");
        var encryptionkey = Encoding.UTF8.GetBytes("V3Znp4JRTGVWiuGv");

        var validationParameters = new TokenValidationParameters
        {
            ClockSkew = TimeSpan.Zero, // default: 5 min
            RequireSignedTokens = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretkey),

            RequireExpirationTime = true,
            ValidateLifetime = true,

            ValidateAudience = true, //default : false
            ValidAudience = builder.Configuration.GetSection("Auth:Audience").Value,

            ValidateIssuer = true, //default : false
            ValidIssuer = builder.Configuration.GetSection("Auth:Issuer").Value,

            TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey)
        };

        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = validationParameters;
    });
// -------------- End JWT --------------

//-------------- Start Services Register Section --------------

builder.Services.AddTransient<IUserService, UserService>();

//-------------- End Services Section --------------

//-------------- Start Repositories Register Section --------------

builder.Services.AddTransient<IUserRepository, UserRepository>();

//-------------- End Repository Section --------------

//Automapper
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Local Marketing System", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
