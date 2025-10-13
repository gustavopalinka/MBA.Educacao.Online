using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MBA.Educacao.Online.API.Migrations.Pagamento
{
    /// <inheritdoc />
    public partial class InitialPagamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MatriculaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AlunoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataConfirmacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NumeroCartao = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    NomeTitular = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Validade = table.Column<string>(type: "TEXT", maxLength: 7, nullable: false),
                    CVV = table.Column<string>(type: "TEXT", maxLength: 4, nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    MotivoRejeicao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamentos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_AlunoId",
                table: "Pagamentos",
                column: "AlunoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_DataPagamento",
                table: "Pagamentos",
                column: "DataPagamento");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_MatriculaId",
                table: "Pagamentos",
                column: "MatriculaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pagamentos");
        }
    }
}
