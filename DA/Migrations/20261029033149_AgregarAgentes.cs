using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DA.Migrations
{
    /// <inheritdoc />
    public partial class AgregarAgentes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agentes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    CapacidadMaximaTickets = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agentes", x => x.Id);
                    table.CheckConstraint("CK_Agentes_Capacidad", "[CapacidadMaximaTickets] BETWEEN 1 AND 50");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AgenteId",
                table: "Tickets",
                column: "AgenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Agentes_Correo",
                table: "Agentes",
                column: "Correo",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Agentes_AgenteId",
                table: "Tickets",
                column: "AgenteId",
                principalTable: "Agentes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Agentes_AgenteId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "Agentes");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_AgenteId",
                table: "Tickets");
        }
    }
}
