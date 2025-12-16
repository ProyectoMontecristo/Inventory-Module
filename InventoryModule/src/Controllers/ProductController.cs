using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryModule.src.data;
using InventoryModule.src.model;
using InventoryModule.dtos;

namespace InventoryModule.src.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ProductosController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetProductos()
        {
            // AQUÍ ESTÁ LA MAGIA: Usamos .Select para moldear los datos
            // y traer el stock desde la tabla relacionada.
            var productos = await _context.productos
                .Include(p => p.Inventarios) // Aseguramos que cargue el inventario
                .Select(p => new
                {
                    p.id_producto,
                    p.sku,
                    p.nombre,
                    p.descripcion,
                    p.categoria,
                    p.imagen,
                    p.pais,
                    p.precio_venta,
                    p.marca,
                    p.precio_compra,
                    p.umbral_alerta_stock_bajo,
                    p.estado,
                    p.id_proveedor,
                    // Sumamos el stock si hay varios registros, o tomamos 0 si es nulo
                    stock_actual = p.Inventarios.Sum(i => i.stock_actual) 
                })
                .ToListAsync();

            return Ok(productos);
        }

        // GET: api/Productos/{sku}
        [HttpGet("{sku}")]
        public async Task<ActionResult<object>> GetProductoBySku(string sku)
        {
            var producto = await _context.productos
                .Include(p => p.Inventarios)
                .Where(p => p.sku == sku)
                .Select(p => new // Proyectamos igual que arriba para mantener consistencia
                {
                    p.id_producto,
                    p.sku,
                    p.nombre,
                    p.descripcion,
                    p.categoria,
                    p.imagen,
                    p.pais,
                    p.precio_venta,
                    p.marca,
                    p.precio_compra,
                    p.umbral_alerta_stock_bajo,
                    p.estado,
                    p.id_proveedor,
                    stock_actual = p.Inventarios.Sum(i => i.stock_actual)
                })
                .FirstOrDefaultAsync();

            if (producto == null)
            {
                return NotFound(new { mensaje = $"No se encontró el producto con SKU: {sku}" });
            }

            return Ok(producto);
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<object>> GetProductoById(string id)
        {
            var producto = await _context.productos
                .Where(p => p.id_producto == id)
                .Select(p => new
                {
                    p.id_producto,
                    p.sku,
                    p.nombre,
                    p.descripcion,
                    p.categoria,
                    p.imagen,
                    p.pais,
                    p.precio_venta,
                    p.marca,
                    p.precio_compra,
                    p.umbral_alerta_stock_bajo,
                    p.estado,
                    p.id_proveedor,
                    stock_actual = p.Inventarios.Sum(i => i.stock_actual)
                })
                .FirstOrDefaultAsync();

            if (producto == null)
            {
                return NotFound(new { mensaje = $"No se encontró el producto con el ID interno: {id}" });
            }

            return Ok(producto);
        }

        [HttpPost("Agregar")]
        public async Task<ActionResult<Productos>> CreateProducto(CrearProductoDTO dto)
        {
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
                precio_venta = dto.PrecioVenta,
                marca = dto.Marca,
                precio_compra = dto.PrecioCompra,
                umbral_alerta_stock_bajo = (int)dto.UmbralAlerta,
                estado = dto.Estado,
                id_proveedor = dto.ProveedorId
            };

            // Creamos también un inventario inicial en 0 para evitar problemas futuros
            var inventarioInicial = new Inventario
            {
                id_inventario = Guid.NewGuid().ToString(),
                id_producto = nuevoProducto.id_producto,
                stock_actual = 0,
                stock_disponible = 0,
                stock_reservado = 0
            };

            _context.productos.Add(nuevoProducto);
            _context.inventarios.Add(inventarioInicial);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductoBySku), new { sku = nuevoProducto.sku }, 
                new { nuevoProducto.sku, nuevoProducto.nombre, mensaje = "Creado exitosamente" });
        }

        [HttpPut("{sku}/stock")]
        public async Task<IActionResult> UpdateStock(string sku, [FromBody] UpdateStockDTO dto)
        {
            // 1. Buscamos el producto
            var producto = await _context.productos.FirstOrDefaultAsync(p => p.sku == sku);

            if (producto == null)
            {
                return NotFound(new { mensaje = $"No se encontró el producto con SKU: {sku}" });
            }

            // 2. Buscamos su inventario
            var inventario = await _context.inventarios
                .FirstOrDefaultAsync(i => i.id_producto == producto.id_producto);

            if (inventario == null)
            {
                inventario = new Inventario
                {
                    id_inventario = Guid.NewGuid().ToString(),
                    id_producto = producto.id_producto,
                    stock_actual = dto.StockActual,
                    stock_reservado = 0,
                    stock_disponible = dto.StockActual
                };
                _context.inventarios.Add(inventario);
            }
            else
            {
                inventario.stock_actual = dto.StockActual;
                inventario.stock_disponible = dto.StockActual - inventario.stock_reservado;
            }

            if (dto.UmbralAlerta.HasValue)
            {
                producto.umbral_alerta_stock_bajo = dto.UmbralAlerta.Value;
            }

            await _context.SaveChangesAsync();

            // 3. RETORNO LIMPIO (Soluciona el error de "Cycle detected")
            // Devolvemos un objeto anónimo plano sin referencias circulares
            return Ok(new
            {
                mensaje = "Stock actualizado correctamente",
                sku = producto.sku,
                nombre = producto.nombre,
                stock_actual = inventario.stock_actual,
                umbral_alerta = producto.umbral_alerta_stock_bajo,
                stock_disponible = inventario.stock_disponible
            });
        }

        [HttpPut("{sku}/deshabilitar")]
        public async Task<IActionResult> DeshabilitarProducto(string sku)
        {
            var producto = await _context.productos.FirstOrDefaultAsync(p => p.sku == sku);

            if (producto == null)
            {
                return NotFound(new { mensaje = $"No se encontró el producto con SKU: {sku}" });
            }

            producto.estado = "Deshabilitado";
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = $"El producto {sku} ha sido deshabilitado correctamente.",
                estado_nuevo = producto.estado
            });
        }

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
            productoExistente.id_proveedor = dto.ProveedorId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            // Retornamos el objeto simple para evitar ciclos si es que el serializador está sensible
            return Ok(new { 
                productoExistente.sku, 
                productoExistente.nombre, 
                mensaje = "Actualizado correctamente" 
            });
        }
    }
}