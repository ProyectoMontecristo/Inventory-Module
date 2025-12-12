using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using InventoryModule.src.data;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION");
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetConnectionString("Postgres");
}

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try 
    {
        var context = services.GetRequiredService<ApplicationDBContext>();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al conectar con la BD: {ex.Message}");
    }
}

app.UseHttpsRedirection();

app.UseAuthorization(); 

app.MapControllers(); 

app.Run();