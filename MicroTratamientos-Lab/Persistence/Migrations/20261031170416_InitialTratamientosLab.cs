using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialTratamientosLab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CatalogoEstudios",
                columns: table => new
                {
                    IdCatalogoEstudio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreEstudio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogoEstudios", x => x.IdCatalogoEstudio);
                });

            migrationBuilder.CreateTable(
                name: "Medicamentos",
                columns: table => new
                {
                    IdMedicamento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreComercial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreGenerico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Presentacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Concentracion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ViaAdministracion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicamentos", x => x.IdMedicamento);
                });

            migrationBuilder.CreateTable(
                name: "Prescripciones",
                columns: table => new
                {
                    IdPrescripcion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPaciente = table.Column<int>(type: "int", nullable: false),
                    IdMedico = table.Column<int>(type: "int", nullable: false),
                    IdCita = table.Column<int>(type: "int", nullable: true),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescripciones", x => x.IdPrescripcion);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudEstudios",
                columns: table => new
                {
                    IdSolicitudEstudio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPaciente = table.Column<int>(type: "int", nullable: false),
                    IdMedicoSolicitante = table.Column<int>(type: "int", nullable: false),
                    IdCita = table.Column<int>(type: "int", nullable: true),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Prioridad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiagnosticoPresuntivo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudEstudios", x => x.IdSolicitudEstudio);
                });

            migrationBuilder.CreateTable(
                name: "PrescripcionDetalles",
                columns: table => new
                {
                    IdPrescripcionDetalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPrescripcion = table.Column<int>(type: "int", nullable: false),
                    IdMedicamento = table.Column<int>(type: "int", nullable: false),
                    Dosis = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UnidadMedida = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Frecuencia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DuracionDias = table.Column<int>(type: "int", nullable: false),
                    IndicacionesEspeciales = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescripcionDetalles", x => x.IdPrescripcionDetalle);
                    table.ForeignKey(
                        name: "FK_PrescripcionDetalles_Medicamentos_IdMedicamento",
                        column: x => x.IdMedicamento,
                        principalTable: "Medicamentos",
                        principalColumn: "IdMedicamento",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrescripcionDetalles_Prescripciones_IdPrescripcion",
                        column: x => x.IdPrescripcion,
                        principalTable: "Prescripciones",
                        principalColumn: "IdPrescripcion",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudEstudioDetalles",
                columns: table => new
                {
                    IdSolicitudEstudioDetalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSolicitudEstudio = table.Column<int>(type: "int", nullable: false),
                    IdCatalogoEstudio = table.Column<int>(type: "int", nullable: false),
                    Indicaciones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudEstudioDetalles", x => x.IdSolicitudEstudioDetalle);
                    table.ForeignKey(
                        name: "FK_SolicitudEstudioDetalles_CatalogoEstudios_IdCatalogoEstudio",
                        column: x => x.IdCatalogoEstudio,
                        principalTable: "CatalogoEstudios",
                        principalColumn: "IdCatalogoEstudio",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudEstudioDetalles_SolicitudEstudios_IdSolicitudEstudio",
                        column: x => x.IdSolicitudEstudio,
                        principalTable: "SolicitudEstudios",
                        principalColumn: "IdSolicitudEstudio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResultadoEstudios",
                columns: table => new
                {
                    IdResultadoEstudio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSolicitudEstudioDetalle = table.Column<int>(type: "int", nullable: false),
                    IdTecnicoOMedico = table.Column<int>(type: "int", nullable: false),
                    FechaResultado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hallazgos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Conclusion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValoresReferencia = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadoEstudios", x => x.IdResultadoEstudio);
                    table.ForeignKey(
                        name: "FK_ResultadoEstudios_SolicitudEstudioDetalles_IdSolicitudEstudioDetalle",
                        column: x => x.IdSolicitudEstudioDetalle,
                        principalTable: "SolicitudEstudioDetalles",
                        principalColumn: "IdSolicitudEstudioDetalle",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArchivoClinicos",
                columns: table => new
                {
                    IdArchivoClinico = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdResultadoEstudio = table.Column<int>(type: "int", nullable: true),
                    IdPaciente = table.Column<int>(type: "int", nullable: true),
                    NombreArchivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoMime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RutaAlmacenamiento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TamanoBytes = table.Column<long>(type: "bigint", nullable: false),
                    FechaSubida = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchivoClinicos", x => x.IdArchivoClinico);
                    table.ForeignKey(
                        name: "FK_ArchivoClinicos_ResultadoEstudios_IdResultadoEstudio",
                        column: x => x.IdResultadoEstudio,
                        principalTable: "ResultadoEstudios",
                        principalColumn: "IdResultadoEstudio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArchivoClinicos_IdResultadoEstudio",
                table: "ArchivoClinicos",
                column: "IdResultadoEstudio");

            migrationBuilder.CreateIndex(
                name: "IX_PrescripcionDetalles_IdMedicamento",
                table: "PrescripcionDetalles",
                column: "IdMedicamento");

            migrationBuilder.CreateIndex(
                name: "IX_PrescripcionDetalles_IdPrescripcion",
                table: "PrescripcionDetalles",
                column: "IdPrescripcion");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoEstudios_IdSolicitudEstudioDetalle",
                table: "ResultadoEstudios",
                column: "IdSolicitudEstudioDetalle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudEstudioDetalles_IdCatalogoEstudio",
                table: "SolicitudEstudioDetalles",
                column: "IdCatalogoEstudio");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudEstudioDetalles_IdSolicitudEstudio",
                table: "SolicitudEstudioDetalles",
                column: "IdSolicitudEstudio");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArchivoClinicos");

            migrationBuilder.DropTable(
                name: "PrescripcionDetalles");

            migrationBuilder.DropTable(
                name: "ResultadoEstudios");

            migrationBuilder.DropTable(
                name: "Medicamentos");

            migrationBuilder.DropTable(
                name: "Prescripciones");

            migrationBuilder.DropTable(
                name: "SolicitudEstudioDetalles");

            migrationBuilder.DropTable(
                name: "CatalogoEstudios");

            migrationBuilder.DropTable(
                name: "SolicitudEstudios");
        }
    }
}
