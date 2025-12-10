using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryModule.src.model;
using Microsoft.EntityFrameworkCore;

namespace InventoryModule.src.data
{
    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) 
            : base(options)
        {
        }
        
        public DbSet<Proveedor> proveedores { get; set; }
        public DbSet<Productos> productos { get; set; }
        public DbSet<Inventario> inventarios { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configuración de Proveedor
            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.ToTable("proveedores");
                entity.HasKey(e => e.id_proveedor);
                
                entity.Property(e => e.nombre).IsRequired().HasMaxLength(200);
                entity.Property(e => e.numero_telefono).IsRequired().HasMaxLength(20);
                entity.Property(e => e.correo).IsRequired().HasMaxLength(200);
                entity.Property(e => e.producto_que_vende).IsRequired().HasMaxLength(500);
                
                // Relación: Un proveedor tiene muchos productos
                entity.HasMany(p => p.Productos)
                    .WithOne(pr => pr.Proveedor)
                    .HasForeignKey(pr => pr.id_proveedor)
                    .OnDelete(DeleteBehavior.SetNull);
            });
            
            // Configuración de Productos
            modelBuilder.Entity<Productos>(entity =>
            {
                entity.ToTable("productos");
                entity.HasKey(e => e.id_producto);
                
                entity.Property(e => e.sku).IsRequired().HasMaxLength(100);
                entity.Property(e => e.nombre).IsRequired().HasMaxLength(200);
                entity.Property(e => e.descripcion).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.categoria).IsRequired().HasMaxLength(100);
                entity.Property(e => e.imagen).IsRequired();
                entity.Property(e => e.pais).IsRequired().HasMaxLength(100);
                entity.Property(e => e.precio_venta).IsRequired().HasPrecision(18, 2);
                entity.Property(e => e.marca).IsRequired().HasMaxLength(100);
                entity.Property(e => e.precio_compra).IsRequired().HasPrecision(18, 2);
                entity.Property(e => e.umbral_alerta_stock_bajo).IsRequired().HasPrecision(18, 2);
                entity.Property(e => e.estado).IsRequired().HasMaxLength(50);
                
                // Índice único para SKU
                entity.HasIndex(e => e.sku).IsUnique();
                
                // Relación: Un producto gestiona muchos inventarios
                entity.HasMany(p => p.Inventarios)
                    .WithOne(i => i.Producto)
                    .HasForeignKey(i => i.id_producto)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            // Configuración de Inventario
            modelBuilder.Entity<Inventario>(entity =>
            {
                entity.ToTable("inventarios");
                entity.HasKey(e => e.id_inventario);
                
                entity.Property(e => e.stock_actual).IsRequired();
                entity.Property(e => e.stock_reservado).IsRequired();
                entity.Property(e => e.stock_disponible);
                
                // Índice para búsquedas por producto
                entity.HasIndex(e => e.id_producto);
            });
        }
    }
}