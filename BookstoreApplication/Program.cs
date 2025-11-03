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
builder.Services.AddSwaggerGen();

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
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IAwardService, AwardService>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddGlobalExceptionHandling();

// 6) Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(opt =>
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

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Validacija AutoMapper profila
using (var scope = app.Services.CreateScope())
{
    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
    mapper.ConfigurationProvider.AssertConfigurationIsValid();
}


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
