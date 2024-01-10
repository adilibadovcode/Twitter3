using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using System.Text;
using Twitter.API.Helpers;
using Twitter.Business;
using Twitter.Core.Entities;
using Twitter.Core.Enums;
using Twitter.DAL.Contexts;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})

    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            //2.4 min 20
            ValidIssuer = builder.Configuration.GetSection("Jwt")["Issuer"],
            ValidAudience = builder.Configuration.GetSection("Jwt")["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt")["Key"])),
            LifetimeValidator = (nb, exp, token, _) => token != null ? exp >= DateTime.UtcNow && nb <= DateTime.UtcNow : false

        };
    }
);

builder.Services.AddDbContext<TwitterContext>(opt => opt.UseSqlServer
(builder.Configuration.GetConnectionString("MSSql"))).AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.SignIn.RequireConfirmedEmail = true;
    opt.User.RequireUniqueEmail = true;
    opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._";
    opt.Lockout.MaxFailedAccessAttempts = 10;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 4;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<TwitterContext>();

builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddBusinessLayer();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
    });
}

app.UseHttpsRedirection();
//app.Use(async (context, n) =>
//{
//    var a = context.Connection.RemoteIpAddress;
//    await Console.Out.WriteLineAsync(a.ToString());
//    await n();
//});
app.UseAuthentication();
app.UseAuthorization();
app.UseSeedData();
app.MapControllers();

app.Run();
