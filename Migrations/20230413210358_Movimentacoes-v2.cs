using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MStarSupply.Migrations
{
    public partial class Movimentacoesv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "quantidade",
                table: "Movimentacoes",
                newName: "Quantidade");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantidade",
                table: "Movimentacoes",
                newName: "quantidade");
        }
    }
}
