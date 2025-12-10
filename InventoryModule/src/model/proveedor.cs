using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryModule.src.model
{
    public class Proveedor
    {
        [Key]
        public required string id_proveedor { get; set; }
        
        public required string nombre { get; set; }
        
        public required string numero_telefono { get; set; }
        
        public required string correo { get; set; }
        
        public required string producto_que_vende { get; set; }
        
        // Relación: Un proveedor vende muchos productos
        public ICollection<Productos> Productos { get; set; } = new List<Productos>();
        }
}