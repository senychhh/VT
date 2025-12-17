using Kolbasin.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? "Data Source=menu.db";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// Swagger / OpenAPI
builder.Services.AddOpenApi();
builder.Services.AddControllers();


var app = builder.Build();
Console.WriteLine("DB PATH = " + Path.GetFullPath("menu.db"));


// Заполняем базу начальными данными
await DbInitializer.SeedDataAsync(app);

// Настройка HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();


app.MapControllers();

// Пример тестового эндпоинта
app.MapGet("/categories", async (AppDbContext context) =>
{
    return await context.Categories.ToListAsync();
});

app.MapGet("/dishes", async (AppDbContext context) =>
{
    return await context.Dishes.ToListAsync();
});

app.Run();