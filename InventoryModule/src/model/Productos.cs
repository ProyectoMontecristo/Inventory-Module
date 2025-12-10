using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryModule.src.model
{
    public class Productos
    {
        [Key]
        public required string id_producto { get; set; }
        
        public required string sku { get; set; }
        
        public required string nombre { get; set; }
        
        public required string descripcion { get; set; }
        
        public required string categoria { get; set; }
        
        public required string imagen { get; set; }
        
        public required string pais { get; set; }
        
        public required decimal precio_venta { get; set; }
        
        public required string marca { get; set; }
        
        public required decimal precio_compra { get; set; }
        
        public required decimal umbral_alerta_stock_bajo { get; set; }
        
        public required string estado { get; set; }
        
        // Foreign Key
        [ForeignKey("Proveedor")]
        public string? id_proveedor { get; set; }
        
        // Navegación
        public Proveedor? Proveedor { get; set; }
        
        // Relación: Un producto gestiona muchos inventarios
        public ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();
    }
}