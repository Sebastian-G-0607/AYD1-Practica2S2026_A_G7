using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CineCraft.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rol",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rol", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "solicitud_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_solicitud_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "solicitud",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    correo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    contrasenia = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_solicitud", x => x.id);
                    table.ForeignKey(
                        name: "FK_solicitud_solicitud_status_status_id",
                        column: x => x.status_id,
                        principalTable: "solicitud_status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    rol_id = table.Column<int>(type: "integer", nullable: false),
                    contrasenia = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    correo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    solicitud_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.id);
                    table.ForeignKey(
                        name: "FK_usuario_rol_rol_id",
                        column: x => x.rol_id,
                        principalTable: "rol",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_usuario_solicitud_solicitud_id",
                        column: x => x.solicitud_id,
                        principalTable: "solicitud",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "etiqueta",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    usuario_id = table.Column<int>(type: "integer", nullable: false),
                    descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_etiqueta", x => x.id);
                    table.ForeignKey(
                        name: "FK_etiqueta_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "resenia",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    usuario_autor_id = table.Column<int>(type: "integer", nullable: false),
                    titulo_pelicula = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    calificacion = table.Column<int>(type: "integer", nullable: false),
                    comentario = table.Column<string>(type: "text", nullable: true),
                    etiqueta_id = table.Column<int>(type: "integer", nullable: false),
                    destacada = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    archivada = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resenia", x => x.id);
                    table.CheckConstraint("CK_resenia_calificacion", "calificacion >= 0 AND calificacion <= 10");
                    table.ForeignKey(
                        name: "FK_resenia_etiqueta_etiqueta_id",
                        column: x => x.etiqueta_id,
                        principalTable: "etiqueta",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_resenia_usuario_usuario_autor_id",
                        column: x => x.usuario_autor_id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "compartir_resenia_usuario",
                columns: table => new
                {
                    resenia_id = table.Column<int>(type: "integer", nullable: false),
                    usuario_remitente_id = table.Column<int>(type: "integer", nullable: false),
                    usuario_destinatario_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_compartir_resenia_usuario", x => new { x.resenia_id, x.usuario_remitente_id, x.usuario_destinatario_id });
                    table.ForeignKey(
                        name: "FK_compartir_resenia_usuario_resenia_resenia_id",
                        column: x => x.resenia_id,
                        principalTable: "resenia",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_compartir_resenia_usuario_usuario_usuario_destinatario_id",
                        column: x => x.usuario_destinatario_id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_compartir_resenia_usuario_usuario_usuario_remitente_id",
                        column: x => x.usuario_remitente_id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_compartir_resenia_usuario_usuario_destinatario_id",
                table: "compartir_resenia_usuario",
                column: "usuario_destinatario_id");

            migrationBuilder.CreateIndex(
                name: "IX_compartir_resenia_usuario_usuario_remitente_id",
                table: "compartir_resenia_usuario",
                column: "usuario_remitente_id");

            migrationBuilder.CreateIndex(
                name: "IX_etiqueta_usuario_id",
                table: "etiqueta",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_resenia_etiqueta_id",
                table: "resenia",
                column: "etiqueta_id");

            migrationBuilder.CreateIndex(
                name: "IX_resenia_usuario_autor_id",
                table: "resenia",
                column: "usuario_autor_id");

            migrationBuilder.CreateIndex(
                name: "IX_solicitud_status_id",
                table: "solicitud",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_rol_id",
                table: "usuario",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_solicitud_id",
                table: "usuario",
                column: "solicitud_id",
                unique: true,
                filter: "solicitud_id IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "compartir_resenia_usuario");

            migrationBuilder.DropTable(
                name: "resenia");

            migrationBuilder.DropTable(
                name: "etiqueta");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "rol");

            migrationBuilder.DropTable(
                name: "solicitud");

            migrationBuilder.DropTable(
                name: "solicitud_status");
        }
    }
}
