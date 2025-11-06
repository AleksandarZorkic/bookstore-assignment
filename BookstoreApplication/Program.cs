using System;
using BookstoreApplication.Mapping;
using Microsoft.EntityFrameworkCore;
using BookstoreApplication;
using BookstoreApplication.Repositories.Interfaces;
using BookstoreApplication.Repositories.Implementations;
using BookstoreApplication.Services;
using BookstoreApplication.Services.Implementations;
using BookstoreApplication.Services.Interfaces;
using BookstoreApplication.Middleware;
using Serilog;
using AutoMapper;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.OpenApi.Models;
using BookstoreApplication.Models;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// 1) EF Core + Npgsql
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 2) Controllers (jednom) + enum kao string
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// 3) Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bookstore API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer {token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme, Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// 4) CORS za SPA 
builder.Services.AddCors(o => o.AddPolicy("spa", p =>
    p.WithOrigins("http://localhost:5173")
     .AllowAnyHeader()
     .AllowAnyMethod()
     .AllowCredentials()
));

// 5) Repozitorijumi/servisi
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<IAwardRepository, AwardRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IAwardService, AwardService>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddGlobalExceptionHandling();

// 6) Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.Password.RequireDigit = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireUppercase = true;
    opt.Password.RequireNonAlphanumeric = true;
    opt.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,

        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],

        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),

        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role
       
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Ovo je kod koji sam ubacio kako bih generisao password hash za korisnike preko PowerShell-a
#if DEBUG
if (Environment.GetEnvironmentVariable("GEN_HASH") == "1")
{
    string Make(string userName, string password, string? sec = null, string? conc = null)
    {
        var hasher = new PasswordHasher<ApplicationUser>();
        var u = new ApplicationUser
        {
            UserName = userName,
            EmailConfirmed = true,
            SecurityStamp = sec ?? Guid.NewGuid().ToString(),
            ConcurrencyStamp = conc ?? Guid.NewGuid().ToString()
        };
        var hash = hasher.HashPassword(u, password);
        Console.WriteLine($"{userName} PasswordHash: {hash}");
        Console.WriteLine($"{userName} SecurityStamp: {u.SecurityStamp}");
        Console.WriteLine($"{userName} ConcurrencyStamp: {u.ConcurrencyStamp}");
        Console.WriteLine();
        return hash;
    }

    Make("urednik1", "Aa!123456");
    Make("urednik2", "Aa!123456");
    Make("urednik3", "Aa!123456");
    Make("biblio1", "Aa!123456");

    // prekini proces da ne startuje server
    Environment.Exit(0);
}
#endif


// Validacija AutoMapper profila
using (var scope = app.Services.CreateScope())
{
    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
    mapper.ConfigurationProvider.AssertConfigurationIsValid();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseGlobalExceptionHandling();

    app.UseHttpsRedirection();

    app.UseCors("spa");
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
