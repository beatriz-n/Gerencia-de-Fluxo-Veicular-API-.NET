using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FluxoVeicular.ServiceDefaults.Migrations
{
    /// <inheritdoc />
    public partial class RetiradaAcesso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Acesso",
                table: "Veiculos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Acesso",
                table: "Veiculos",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
