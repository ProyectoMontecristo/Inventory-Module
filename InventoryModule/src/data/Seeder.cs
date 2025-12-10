using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryModule.src.model;

namespace InventoryModule.src.data
{
    public class Seeder
    {
        /// <summary>
        /// Ensures the db is created and seeds inventory data
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDBContext>();
                
                context.Database.EnsureCreated();

                // Verificar si ya hay datos
                if (!context.proveedores.Any())
                {
                    // Crear Proveedores
                    var proveedores = new List<Proveedor>
                    {
                        new Proveedor
                        {
                            id_proveedor = Guid.NewGuid().ToString(),
                            nombre = "Ferretería Industrial S.A.",
                            numero_telefono = "+56 2 2345 6789",
                            correo = "ventas@ferreteriaind.cl",
                            producto_que_vende = "Herramientas, Tornillos, Tuercas, Materiales de Construcción"
                        },
                        new Proveedor
                        {
                            id_proveedor = Guid.NewGuid().ToString(),
                            nombre = "Eléctricos del Norte",
                            numero_telefono = "+56 2 2456 7890",
                            correo = "contacto@electricosnorte.cl",
                            producto_que_vende = "Cables, Lámparas, Material Eléctrico"
                        },
                        new Proveedor
                        {
                            id_proveedor = Guid.NewGuid().ToString(),
                            nombre = "Pinturas y Recubrimientos Ltda.",
                            numero_telefono = "+56 2 2567 8901",
                            correo = "pedidos@pinturas.cl",
                            producto_que_vende = "Pinturas, Barnices, Adhesivos"
                        },
                        new Proveedor
                        {
                            id_proveedor = Guid.NewGuid().ToString(),
                            nombre = "Suministros Industriales Chile",
                            numero_telefono = "+56 2 2678 9012",
                            correo = "ventas@suministroschile.cl",
                            producto_que_vende = "Válvulas, Filtros, Aceites, Lubricantes"
                        },
                        new Proveedor
                        {
                            id_proveedor = Guid.NewGuid().ToString(),
                            nombre = "Cementos y Agregados S.A.",
                            numero_telefono = "+56 2 2789 0123",
                            correo = "info@cementosyagregados.cl",
                            producto_que_vende = "Cemento, Áridos, Materiales de Construcción"
                        }
                    };

                    context.proveedores.AddRange(proveedores);
                    context.SaveChanges();

                    Random random = new Random();

                    // Crear Productos
                    var productos = new List<Productos>
                    {
                        new Productos
                        {
                            id_producto = Guid.NewGuid().ToString(),
                            sku = "TOR-M6-001",
                            nombre = "Tornillos Hexagonales M6",
                            descripcion = "Tornillos hexagonales de acero galvanizado, medida M6x20mm, ideal para construcción y montaje industrial",
                            categoria = "Ferretería",
                            imagen = "https://example.com/images/tornillos-m6.jpg",
                            pais = "Chile",
                            precio_venta = 150.00m,
                            marca = "Aceros del Pacífico",
                            precio_compra = 90.00m,
                            umbral_alerta_stock_bajo = 100,
                            estado = "Activo",
                            id_proveedor = proveedores[0].id_proveedor
                        },
                        new Productos
                        {
                            id_producto = Guid.NewGuid().ToString(),
                            sku = "CAB-UTP6-002",
                            nombre = "Cable UTP Categoría 6",
                            descripcion = "Cable de red UTP Cat6, 305 metros, certificado para redes Gigabit Ethernet",
                            categoria = "Eléctrico",
                            imagen = "https://example.com/images/cable-utp6.jpg",
                            pais = "China",
                            precio_venta = 85000.00m,
                            marca = "NetConnect",
                            precio_compra = 65000.00m,
                            umbral_alerta_stock_bajo = 10,
                            estado = "Activo",
                            id_proveedor = proveedores[1].id_proveedor
                        },
                        new Productos
                        {
                            id_producto = Guid.NewGuid().ToString(),
                            sku = "PIN-BLA-003",
                            nombre = "Pintura Látex Blanca 5 Litros",
                            descripcion = "Pintura látex lavable para interiores, acabado mate, alto cubrimiento",
                            categoria = "Pinturas",
                            imagen = "https://example.com/images/pintura-blanca.jpg",
                            pais = "Chile",
                            precio_venta = 15990.00m,
                            marca = "ColorPlus",
                            precio_compra = 11500.00m,
                            umbral_alerta_stock_bajo = 50,
                            estado = "Activo",
                            id_proveedor = proveedores[2].id_proveedor
                        },
                        new Productos
                        {
                            id_producto = Guid.NewGuid().ToString(),
                            sku = "CEM-POR-004",
                            nombre = "Cemento Portland 42.5kg",
                            descripcion = "Cemento Portland tipo I, ideal para construcción general y obras civiles",
                            categoria = "Construcción",
                            imagen = "https://example.com/images/cemento.jpg",
                            pais = "Chile",
                            precio_venta = 7990.00m,
                            marca = "Cementos Biobío",
                            precio_compra = 6200.00m,
                            umbral_alerta_stock_bajo = 200,
                            estado = "Activo",
                            id_proveedor = proveedores[4].id_proveedor
                        },
                        new Productos
                        {
                            id_producto = Guid.NewGuid().ToString(),
                            sku = "TUE-HEX-005",
                            nombre = "Tuercas Hexagonales M8",
                            descripcion = "Tuercas hexagonales de acero inoxidable A2, medida M8",
                            categoria = "Ferretería",
                            imagen = "https://example.com/images/tuercas-m8.jpg",
                            pais = "Brasil",
                            precio_venta = 180.00m,
                            marca = "FastenBolt",
                            precio_compra = 120.00m,
                            umbral_alerta_stock_bajo = 150,
                            estado = "Activo",
                            id_proveedor = proveedores[0].id_proveedor
                        },
                        new Productos
                        {
                            id_producto = Guid.NewGuid().ToString(),
                            sku = "VAL-BRO-006",
                            nombre = "Válvula de Bronce 1/2 pulgada",
                            descripcion = "Válvula esférica de bronce, rosca NPT 1/2', presión máxima 600 PSI",
                            categoria = "Plomería",
                            imagen = "https://example.com/images/valvula-bronce.jpg",
                            pais = "Italia",
                            precio_venta = 12500.00m,
                            marca = "HydroFlow",
                            precio_compra = 9800.00m,
                            umbral_alerta_stock_bajo = 30,
                            estado = "Activo",
                            id_proveedor = proveedores[3].id_proveedor
                        },
                        new Productos
                        {
                            id_producto = Guid.NewGuid().ToString(),
                            sku = "LAM-LED-007",
                            nombre = "Lámpara LED 12W E27",
                            descripcion = "Ampolleta LED 12W equivalente a 100W incandescente, luz blanca fría 6500K",
                            categoria = "Iluminación",
                            imagen = "https://example.com/images/lampara-led.jpg",
                            pais = "China",
                            precio_venta = 3990.00m,
                            marca = "BrightLight",
                            precio_compra = 2500.00m,
                            umbral_alerta_stock_bajo = 80,
                            estado = "Activo",
                            id_proveedor = proveedores[1].id_proveedor
                        },
                        new Productos
                        {
                            id_producto = Guid.NewGuid().ToString(),
                            sku = "ADH-IND-008",
                            nombre = "Adhesivo Industrial Multiuso 1L",
                            descripcion = "Adhesivo de contacto profesional, alta resistencia, secado rápido",
                            categoria = "Químicos",
                            imagen = "https://example.com/images/adhesivo.jpg",
                            pais = "Chile",
                            precio_venta = 8500.00m,
                            marca = "MegaBond",
                            precio_compra = 6200.00m,
                            umbral_alerta_stock_bajo = 40,
                            estado = "Activo",
                            id_proveedor = proveedores[2].id_proveedor
                        },
                        new Productos
                        {
                            id_producto = Guid.NewGuid().ToString(),
                            sku = "ACE-LUB-009",
                            nombre = "Aceite Lubricante 20W-50 5L",
                            descripcion = "Aceite mineral multigrado para motores a gasolina y diesel",
                            categoria = "Lubricantes",
                            imagen = "https://example.com/images/aceite-lubricante.jpg",
                            pais = "Argentina",
                            precio_venta = 18990.00m,
                            marca = "MotorTech",
                            precio_compra = 14500.00m,
                            umbral_alerta_stock_bajo = 25,
                            estado = "Activo",
                            id_proveedor = proveedores[3].id_proveedor
                        },
                        new Productos
                        {
                            id_producto = Guid.NewGuid().ToString(),
                            sku = "FIL-AIR-010",
                            nombre = "Filtro de Aire Industrial",
                            descripcion = "Filtro de aire HEPA para sistemas de ventilación industrial",
                            categoria = "Filtros",
                            imagen = "https://example.com/images/filtro-aire.jpg",
                            pais = "Estados Unidos",
                            precio_venta = 35000.00m,
                            marca = "AirPure",
                            precio_compra = 27000.00m,
                            umbral_alerta_stock_bajo = 15,
                            estado = "Activo",
                            id_proveedor = proveedores[3].id_proveedor
                        }
                    };

                    context.productos.AddRange(productos);
                    context.SaveChanges();

                    // Crear Inventarios con diferentes escenarios
                    var inventarios = new List<Inventario>();
                    
                    for (int i = 0; i < productos.Count; i++)
                    {
                        int stockActual;
                        int stockReservado;
                        
                        // Crear diferentes escenarios de stock
                        if (i % 3 == 0)
                        {
                            // Stock bajo - crítico
                            stockActual = random.Next(5, 20);
                            stockReservado = random.Next(0, stockActual / 2);
                        }
                        else if (i % 3 == 1)
                        {
                            // Stock normal
                            stockActual = random.Next(100, 300);
                            stockReservado = random.Next(10, 50);
                        }
                        else
                        {
                            // Stock alto
                            stockActual = random.Next(500, 1000);
                            stockReservado = random.Next(50, 150);
                        }
                        
                        var inventario = new Inventario
                        {
                            id_inventario = Guid.NewGuid().ToString(),
                            stock_actual = stockActual,
                            stock_reservado = stockReservado,
                            stock_disponible = stockActual - stockReservado,
                            id_producto = productos[i].id_producto
                        };
                        
                        inventarios.Add(inventario);
                    }

                    context.inventarios.AddRange(inventarios);
                    context.SaveChanges();

                    Console.WriteLine($"Seeder completado: {proveedores.Count} proveedores, {productos.Count} productos, {inventarios.Count} inventarios creados.");
                }
            }
        }
    }
}