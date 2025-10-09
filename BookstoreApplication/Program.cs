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

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// EF Core + Npgsql
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// MVC kontroleri
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (dev)
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
));
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
builder.Services.AddScoped<IAwardRepository, AwardRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IAwardService, AwardService>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddAutoMapper(typeof(BookstoreApplication.Mapping.MappingProfile).Assembly);

builder.Services.AddGlobalExceptionHandling();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseGlobalExceptionHandling();

app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
