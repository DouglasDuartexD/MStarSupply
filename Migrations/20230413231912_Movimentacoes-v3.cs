using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MStarSupply.Migrations
{
    public partial class Movimentacoesv3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TipoMovimentacao",
                table: "Movimentacoes",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoMovimentacao",
                table: "Movimentacoes");
        }
    }
}
