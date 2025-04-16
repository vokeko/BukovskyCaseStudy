using Microsoft.EntityFrameworkCore;
using BukovskyCaseStudy.Data;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<OrderDbContext>(options =>
        options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
