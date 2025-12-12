using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using InventoryModule.src.data;

var builder = WebApplication.CreateBuilder(args);

// 1. Cargar variables de entorno (.env)
Env.Load();

// --- ZONA DE SERVICIOS (Contenedor de Inyección de Dependencias) ---

// A. Agregar soporte para Controladores (¡ESTO ES NUEVO!)
builder.Services.AddControllers();

// B. Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// C. Configurar Base de Datos (PostgreSQL)
var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION");
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetConnectionString("Postgres");
}

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseNpgsql(connectionString));

// D. Servicios de OpenAPI (.NET 9)
builder.Services.AddOpenApi();

var app = builder.Build();

// --- ZONA DE MIDDLEWARE (Pipeline HTTP) ---

// Configuración del entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

// Inicialización de DB (Scope)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try 
    {
        var context = services.GetRequiredService<ApplicationDBContext>();
        // context.Database.EnsureCreated(); // Opcional: si quieres asegurar que se cree
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al conectar con la BD: {ex.Message}");
    }
}

app.UseHttpsRedirection();

app.UseAuthorization(); // Importante si agregas seguridad a futuro

// ¡ESTA LÍNEA MAPEA AUTOMÁTICAMENTE TUS CONTROLADORES!
app.MapControllers(); 

app.Run();