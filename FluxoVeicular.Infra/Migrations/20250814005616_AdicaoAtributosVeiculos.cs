using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FluxoVeicular.ServiceDefaults.Migrations
{
    /// <inheritdoc />
    public partial class AdicaoAtributosVeiculos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cor",
                table: "Veiculos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Placa",
                table: "Veiculos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cor",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Placa",
                table: "Veiculos");
        }
    }
}
