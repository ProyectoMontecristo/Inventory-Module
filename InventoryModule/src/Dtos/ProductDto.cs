namespace InventoryModule.dtos;

public record CrearProductoDTO(
    string Sku,
    string Nombre,
    string Descripcion,
    string Categoria,
    string Imagen,    
    string Pais,
    decimal PrecioVenta,
    string Marca,
    decimal PrecioCompra,
    decimal UmbralAlerta,
    string Estado,
    string? ProveedorId 
);

public record ProductoResponseDTO(
    string IdProducto, 
    string Nombre, 
    string Sku, 
    decimal PrecioVenta, 
    int StockTotal 
);

public record UpdateStockDTO(
    int StockActual,
    int? UmbralAlerta
);