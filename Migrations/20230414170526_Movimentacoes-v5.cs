using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MStarSupply.Migrations
{
    public partial class Movimentacoesv5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movimentacoes2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Produto_id = table.Column<int>(type: "int", nullable: false),
                    DataHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    TipoMovimentacao = table.Column<bool>(type: "bit", nullable: false),
                    Local = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimentacoes2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movimentacoes2_Produto_Produto_id",
                        column: x => x.Produto_id,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacoes_Produto_id",
                table: "Movimentacoes",
                column: "Produto_id");

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacoes2_Produto_id",
                table: "Movimentacoes2",
                column: "Produto_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movimentacoes_Produto_Produto_id",
                table: "Movimentacoes",
                column: "Produto_id",
                principalTable: "Produto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movimentacoes_Produto_Produto_id",
                table: "Movimentacoes");

            migrationBuilder.DropTable(
                name: "Movimentacoes2");

            migrationBuilder.DropIndex(
                name: "IX_Movimentacoes_Produto_id",
                table: "Movimentacoes");
        }
    }
}
