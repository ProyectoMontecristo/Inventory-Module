using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using InventoryModule.src.data;
using InventoryModule.src.model; // Para la clase Productos
using InventoryModule.dtos;      // Para los DTOs

var builder = WebApplication.CreateBuilder(args);

// 1. Cargar variables de entorno (.env) para la conexión a Render
Env.Load();

// 2. Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. Configurar Base de Datos (PostgreSQL)
var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION");
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetConnectionString("Postgres");
}

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseNpgsql(connectionString));

// 4. Servicios de OpenAPI (.NET 9)
builder.Services.AddOpenApi();

var app = builder.Build();

// 5. Configuración del entorno de desarrollo
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
        // Si tienes un Seeder, puedes descomentar la siguiente línea:
        // Seeder.Initialize(services); 
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al conectar con la BD: {ex.Message}");
    }
}

app.UseHttpsRedirection();

// --- ENDPOINTS (Adaptados a tus nombres de variables) ---

// Health Check
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }));

// GET: Listar todos los productos
// Fíjate que usamos 'db.productos' en minúscula, tal como está en tu Context
app.MapGet("/api/productos", async (ApplicationDBContext db) =>
{
    return await db.productos.ToListAsync();
})
.WithName("GetProductos")
.WithOpenApi();

// POST: Crear un nuevo producto
app.MapPost("/api/productos", async (ApplicationDBContext db, CrearProductoDTO dto) =>
{
    // Mapeamos los datos del DTO a tu Entidad Productos
    // Usamos las propiedades en minúscula (sku, nombre) tal como las definiste en tu modelo
    var nuevoProducto = new Productos
    {
        id_producto = Guid.NewGuid().ToString(), // Generamos ID manual porque es string
        
        sku = dto.Sku,
        nombre = dto.Nombre,
        descripcion = dto.Descripcion,
        categoria = dto.Categoria,
        imagen = dto.Imagen,
        pais = dto.Pais,
        precio_venta = dto.PrecioVenta,
        marca = dto.Marca,
        precio_compra = dto.PrecioCompra,
        umbral_alerta_stock_bajo = dto.UmbralAlerta,
        estado = dto.Estado,
        
        // Relaciones (Foreign Key)
        id_proveedor = dto.ProveedorId
    };

    db.productos.Add(nuevoProducto);
    await db.SaveChangesAsync();

    return Results.Created($"/api/productos/{nuevoProducto.id_producto}", nuevoProducto);
})
.WithName("CrearProducto")
.WithOpenApi();

// --- FIN ENDPOINTS ---

app.Run();