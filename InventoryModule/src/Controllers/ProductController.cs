using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryModule.src.data;
using InventoryModule.src.model;
using InventoryModule.dtos;

namespace InventoryModule.src.controllers
{
    [Route("api/[controller]")] // Ruta base: api/productos
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ProductosController(ApplicationDBContext context)
        {
            _context = context;
        }

        // 1. GET: api/productos (Listar todos)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Productos>>> GetProductos()
        {
            return await _context.productos.ToListAsync();
        }

        // 2. GET: api/productos/{sku} (Buscar por SKU)
        [HttpGet("{sku}")]
        public async Task<ActionResult<Productos>> GetProductoBySku(string sku)
        {
            var producto = await _context.productos.FirstOrDefaultAsync(p => p.sku == sku);

            if (producto == null)
            {
                return NotFound(new { mensaje = $"No se encontró el producto con SKU: {sku}" });
            }

            return Ok(producto);
        }

        // GET: api/productos/id/{id}
// Ejemplo de llamada: /api/productos/id/3fe387e3-d1e8-491c-a7af-9ec26b745c52
[HttpGet("id/{id}")] 
public async Task<ActionResult<Productos>> GetProductoById(string id)
{
    // Aquí hacemos el match EXACTO con tu columna 'id_producto'
    var producto = await _context.productos.FirstOrDefaultAsync(p => p.id_producto == id);

    if (producto == null)
    {
        return NotFound(new { mensaje = $"No se encontró el producto con el ID interno: {id}" });
    }

    return Ok(producto);
}

        // 4. POST: api/productos (Crear)
        [HttpPost]
        public async Task<ActionResult<Productos>> CreateProducto(CrearProductoDTO dto)
        {
            // Validar si el SKU ya existe para no duplicar
            var existe = await _context.productos.AnyAsync(p => p.sku == dto.Sku);
            if (existe)
            {
                return BadRequest(new { mensaje = $"El SKU {dto.Sku} ya existe." });
            }

            var nuevoProducto = new Productos
            {
                id_producto = Guid.NewGuid().ToString(),
                sku = dto.Sku,
                nombre = dto.Nombre,
                descripcion = dto.Descripcion,
                categoria = dto.Categoria,
                imagen = dto.Imagen,
                pais = dto.Pais,
                precio_venta = dto.PrecioVenta, // Asumiendo que en BD es decimal
                marca = dto.Marca,
                precio_compra = dto.PrecioCompra,
                umbral_alerta_stock_bajo = (int)dto.UmbralAlerta, // Convertimos a int si la BD lo requiere, o quita el (int) si es decimal
                estado = dto.Estado,
                id_proveedor = dto.ProveedorId
            };

            _context.productos.Add(nuevoProducto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductoBySku), new { sku = nuevoProducto.sku }, nuevoProducto);
        }

        // 5. PUT: api/productos/{sku} (Actualizar)
        [HttpPut("{sku}")]
        public async Task<IActionResult> UpdateProducto(string sku, CrearProductoDTO dto)
        {
            var productoExistente = await _context.productos.FirstOrDefaultAsync(p => p.sku == sku);

            if (productoExistente == null)
            {
                return NotFound(new { mensaje = $"No se encontró producto con SKU: {sku}" });
            }

            // Actualizamos campos
            productoExistente.nombre = dto.Nombre;
            productoExistente.descripcion = dto.Descripcion;
            productoExistente.categoria = dto.Categoria;
            productoExistente.imagen = dto.Imagen;
            productoExistente.pais = dto.Pais;
            productoExistente.precio_venta = dto.PrecioVenta;
            productoExistente.marca = dto.Marca;
            productoExistente.precio_compra = dto.PrecioCompra;
            productoExistente.umbral_alerta_stock_bajo = (int)dto.UmbralAlerta;
            productoExistente.estado = dto.Estado;
            // id_proveedor también se podría actualizar si es necesario:
            productoExistente.id_proveedor = dto.ProveedorId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw; 
            }

            return Ok(productoExistente);
        }
    }
}