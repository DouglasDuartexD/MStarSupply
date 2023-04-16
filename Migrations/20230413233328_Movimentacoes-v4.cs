using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MStarSupply.Migrations
{
    public partial class Movimentacoesv4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Local",
                table: "Movimentacoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Não Definido");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Local",
                table: "Movimentacoes");
        }
    }
}
