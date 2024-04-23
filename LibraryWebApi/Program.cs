using Microsoft.EntityFrameworkCore;
using LibraryWebApi.Data;
using LibraryWebApi.Mappings;
using LibraryWebApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddDbContext<LibrarySqliteDbContext>(options =>
    options.UseSqlite(builder.Configuration["ConnectionStrings:Default"])
);

builder.Services.AddScoped<IBookRepository, BookRepositoy>();
builder.Services.AddScoped<IBorrowBooksRepository, BorrowBooksRepository>();
builder.Services.AddScoped<IBorrowerRepository, BorrowerRepository>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;
});
app.Run();
