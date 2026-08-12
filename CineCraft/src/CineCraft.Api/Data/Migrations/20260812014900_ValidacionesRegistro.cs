using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineCraft.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class ValidacionesRegistro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_resenia_calificacion",
                table: "resenia");

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_solicitud",
                table: "solicitud",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "motivo_rechazo",
                table: "solicitud",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuario_correo",
                table: "usuario",
                column: "correo",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_resenia_calificacion",
                table: "resenia",
                sql: "calificacion >= 1 AND calificacion <= 5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_usuario_correo",
                table: "usuario");

            migrationBuilder.DropCheckConstraint(
                name: "CK_resenia_calificacion",
                table: "resenia");

            migrationBuilder.DropColumn(
                name: "fecha_solicitud",
                table: "solicitud");

            migrationBuilder.DropColumn(
                name: "motivo_rechazo",
                table: "solicitud");

            migrationBuilder.AddCheckConstraint(
                name: "CK_resenia_calificacion",
                table: "resenia",
                sql: "calificacion >= 0 AND calificacion <= 10");
        }
    }
}
