using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryModule.src.model
{
    public class Inventario
    {
        [Key]
        public required string id_inventario { get; set; }
        
        public required int stock_actual { get; set; }
        
        public required int stock_reservado { get; set; }
        
        public int? stock_disponible { get; set; }
        
        // Foreign Key
        [ForeignKey("Productos")]
        public required string id_producto { get; set; }
        
        // Navegación
        public Productos? Producto { get; set; }
    }
}