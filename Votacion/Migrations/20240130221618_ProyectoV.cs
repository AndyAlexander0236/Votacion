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
                name: "Elecciones",
                columns: table => new
                {
                    IdEleccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cargo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activo = table.Column<decimal>(type: "decimal (18,2)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elecciones", x => x.IdEleccion);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "Candidatos",
                columns: table => new
                {
                    IdCandidato = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImgCandidato = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreCandidato = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApellidoCandidato = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EleccionIdEleccion = table.Column<int>(type: "int", nullable: true),
                    IdEleccion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidatos", x => x.IdCandidato);
                    table.ForeignKey(
                        name: "FK_Candidatos_Elecciones_EleccionIdEleccion",
                        column: x => x.EleccionIdEleccion,
                        principalTable: "Elecciones",
                        principalColumn: "IdEleccion");
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
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EleccionIdEleccion = table.Column<int>(type: "int", nullable: true),
                    IdEleccion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votantes", x => x.IdVotante);
                    table.ForeignKey(
                        name: "FK_Votantes_Elecciones_EleccionIdEleccion",
                        column: x => x.EleccionIdEleccion,
                        principalTable: "Elecciones",
                        principalColumn: "IdEleccion");
                });

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
                    RolesIdRol = table.Column<int>(type: "int", nullable: true),
                    IdRol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_RolesIdRol",
                        column: x => x.RolesIdRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol");
                });

            migrationBuilder.CreateTable(
                name: "Votaciones",
                columns: table => new
                {
                    IdVotacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CandidatoIdCandidato = table.Column<int>(type: "int", nullable: false),
                    IdCandidato = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votaciones", x => x.IdVotacion);
                    table.ForeignKey(
                        name: "FK_Votaciones_Candidatos_CandidatoIdCandidato",
                        column: x => x.CandidatoIdCandidato,
                        principalTable: "Candidatos",
                        principalColumn: "IdCandidato",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidatos_EleccionIdEleccion",
                table: "Candidatos",
                column: "EleccionIdEleccion");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolesIdRol",
                table: "Usuarios",
                column: "RolesIdRol");

            migrationBuilder.CreateIndex(
                name: "IX_Votaciones_CandidatoIdCandidato",
                table: "Votaciones",
                column: "CandidatoIdCandidato");

            migrationBuilder.CreateIndex(
                name: "IX_Votantes_EleccionIdEleccion",
                table: "Votantes",
                column: "EleccionIdEleccion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Votaciones");

            migrationBuilder.DropTable(
                name: "Votantes");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Candidatos");

            migrationBuilder.DropTable(
                name: "Elecciones");
        }
    }
}
