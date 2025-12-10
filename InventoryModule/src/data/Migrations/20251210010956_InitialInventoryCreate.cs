using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryModule.src.data.Migrations
{
    /// <inheritdoc />
    public partial class InitialInventoryCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "proveedores",
                columns: table => new
                {
                    id_proveedor = table.Column<string>(type: "text", nullable: false),
                    nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    numero_telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    correo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    producto_que_vende = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_proveedores", x => x.id_proveedor);
                });

            migrationBuilder.CreateTable(
                name: "productos",
                columns: table => new
                {
                    id_producto = table.Column<string>(type: "text", nullable: false),
                    sku = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    categoria = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    imagen = table.Column<string>(type: "text", nullable: false),
                    pais = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    precio_venta = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    marca = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    precio_compra = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    umbral_alerta_stock_bajo = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    estado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    id_proveedor = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productos", x => x.id_producto);
                    table.ForeignKey(
                        name: "FK_productos_proveedores_id_proveedor",
                        column: x => x.id_proveedor,
                        principalTable: "proveedores",
                        principalColumn: "id_proveedor",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "inventarios",
                columns: table => new
                {
                    id_inventario = table.Column<string>(type: "text", nullable: false),
                    stock_actual = table.Column<int>(type: "integer", nullable: false),
                    stock_reservado = table.Column<int>(type: "integer", nullable: false),
                    stock_disponible = table.Column<int>(type: "integer", nullable: true),
                    id_producto = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventarios", x => x.id_inventario);
                    table.ForeignKey(
                        name: "FK_inventarios_productos_id_producto",
                        column: x => x.id_producto,
                        principalTable: "productos",
                        principalColumn: "id_producto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_inventarios_id_producto",
                table: "inventarios",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_productos_id_proveedor",
                table: "productos",
                column: "id_proveedor");

            migrationBuilder.CreateIndex(
                name: "IX_productos_sku",
                table: "productos",
                column: "sku",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inventarios");

            migrationBuilder.DropTable(
                name: "productos");

            migrationBuilder.DropTable(
                name: "proveedores");
        }
    }
}
