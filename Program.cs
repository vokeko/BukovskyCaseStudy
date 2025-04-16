using Microsoft.EntityFrameworkCore;
using BukovskyCaseStudy.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Runtime.CompilerServices;

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

SqliteConnection m_dbConnection = new SqliteConnection(configuration.GetConnectionString("DefaultConnection"));
m_dbConnection.Open();
string sql = @"CREATE TABLE IF NOT EXISTS Orders (
    Id TEXT PRIMARY KEY,
    ClientName TEXT,
    DateCreated TEXT NOT NULL,
    Status INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS OrderItems (
    Id INTEGER PRIMARY KEY,
    OrderId TEXT NOT NULL,
    ItemName TEXT NOT NULL,
    Price REAL NOT NULL,
    Quantity INTEGER NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE
);";
SqliteCommand command = new SqliteCommand(sql, m_dbConnection);
command.ExecuteNonQuery();
m_dbConnection.Close();

app.Run();
