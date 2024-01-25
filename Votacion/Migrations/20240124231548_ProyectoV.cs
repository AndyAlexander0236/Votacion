using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Votacion.Migrations
{
    /// <inheritdoc />
    public partial class ProyectoV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentoIdentidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApellidoUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorreoUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClaveUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    URLFotoPerfil = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Elecciones",
                columns: table => new
                {
                    IdEleccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cargo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activo = table.Column<decimal>(type: "decimal (18,2)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elecciones", x => x.IdEleccion);
                    table.ForeignKey(
                        name: "FK_Elecciones_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Candidatos",
                columns: table => new
                {
                    IdCandidato = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCandidato = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activo = table.Column<decimal>(type: "decimal (18,2)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdEleccion = table.Column<int>(type: "int", nullable: true),
                    IdUsuario = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidatos", x => x.IdCandidato);
                    table.ForeignKey(
                        name: "FK_Candidatos_Elecciones_IdEleccion",
                        column: x => x.IdEleccion,
                        principalTable: "Elecciones",
                        principalColumn: "IdEleccion");
                    table.ForeignKey(
                        name: "FK_Candidatos_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Votantes",
                columns: table => new
                {
                    IdVotante = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentoIdentidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreVotante = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApellidoVotante = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdEleccion = table.Column<int>(type: "int", nullable: true),
                    IdUsuario = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votantes", x => x.IdVotante);
                    table.ForeignKey(
                        name: "FK_Votantes_Elecciones_IdEleccion",
                        column: x => x.IdEleccion,
                        principalTable: "Elecciones",
                        principalColumn: "IdEleccion");
                    table.ForeignKey(
                        name: "FK_Votantes_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateTable(
                name: "Votaciones",
                columns: table => new
                {
                    IdVotacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdEleccion = table.Column<int>(type: "int", nullable: true),
                    IdVotante = table.Column<int>(type: "int", nullable: true),
                    IdCandidato = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votaciones", x => x.IdVotacion);
                    table.ForeignKey(
                        name: "FK_Votaciones_Candidatos_IdCandidato",
                        column: x => x.IdCandidato,
                        principalTable: "Candidatos",
                        principalColumn: "IdCandidato");
                    table.ForeignKey(
                        name: "FK_Votaciones_Elecciones_IdEleccion",
                        column: x => x.IdEleccion,
                        principalTable: "Elecciones",
                        principalColumn: "IdEleccion");
                    table.ForeignKey(
                        name: "FK_Votaciones_Votantes_IdVotante",
                        column: x => x.IdVotante,
                        principalTable: "Votantes",
                        principalColumn: "IdVotante");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidatos_IdEleccion",
                table: "Candidatos",
                column: "IdEleccion");

            migrationBuilder.CreateIndex(
                name: "IX_Candidatos_IdUsuario",
                table: "Candidatos",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Elecciones_IdUsuario",
                table: "Elecciones",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Votaciones_IdCandidato",
                table: "Votaciones",
                column: "IdCandidato");

            migrationBuilder.CreateIndex(
                name: "IX_Votaciones_IdEleccion",
                table: "Votaciones",
                column: "IdEleccion");

            migrationBuilder.CreateIndex(
                name: "IX_Votaciones_IdVotante",
                table: "Votaciones",
                column: "IdVotante");

            migrationBuilder.CreateIndex(
                name: "IX_Votantes_IdEleccion",
                table: "Votantes",
                column: "IdEleccion");

            migrationBuilder.CreateIndex(
                name: "IX_Votantes_IdUsuario",
                table: "Votantes",
                column: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Votaciones");

            migrationBuilder.DropTable(
                name: "Candidatos");

            migrationBuilder.DropTable(
                name: "Votantes");

            migrationBuilder.DropTable(
                name: "Elecciones");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
