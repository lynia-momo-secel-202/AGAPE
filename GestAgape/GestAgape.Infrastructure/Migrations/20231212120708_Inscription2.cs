using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestAgape.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Inscription2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CampusId",
                schema: "Scolarite",
                table: "Inscriptions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Bourses",
                schema: "Scolarite",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Montant = table.Column<double>(type: "float", nullable: true),
                    MotifReduction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    InscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bourses_Inscriptions_InscriptionId",
                        column: x => x.InscriptionId,
                        principalSchema: "Scolarite",
                        principalTable: "Inscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Changements",
                schema: "Scolarite",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PastSpecialite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NextSpecialite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PastCampus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NextCampus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotifChangement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    InscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Changements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Changements_Inscriptions_InscriptionId",
                        column: x => x.InscriptionId,
                        principalSchema: "Scolarite",
                        principalTable: "Inscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FraisConcours",
                schema: "Admission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Montant = table.Column<double>(type: "float", nullable: true),
                    CycleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AnneeAcademiqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FraisConcours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FraisConcours_AnneeAcademiques_AnneeAcademiqueId",
                        column: x => x.AnneeAcademiqueId,
                        principalSchema: "Admission",
                        principalTable: "AnneeAcademiques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FraisConcours_Cycles_CycleId",
                        column: x => x.CycleId,
                        principalSchema: "Settings",
                        principalTable: "Cycles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FraisEtudeDossier",
                schema: "Admission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Montant = table.Column<double>(type: "float", nullable: true),
                    ClasseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnneeAcademiqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FraisEtudeDossier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FraisEtudeDossier_AnneeAcademiques_AnneeAcademiqueId",
                        column: x => x.AnneeAcademiqueId,
                        principalSchema: "Admission",
                        principalTable: "AnneeAcademiques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FraisEtudeDossier_Classes_ClasseId",
                        column: x => x.ClasseId,
                        principalSchema: "Settings",
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_CampusId",
                schema: "Scolarite",
                table: "Inscriptions",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Bourses_InscriptionId",
                schema: "Scolarite",
                table: "Bourses",
                column: "InscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Changements_InscriptionId",
                schema: "Scolarite",
                table: "Changements",
                column: "InscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_FraisConcours_AnneeAcademiqueId",
                schema: "Admission",
                table: "FraisConcours",
                column: "AnneeAcademiqueId");

            migrationBuilder.CreateIndex(
                name: "IX_FraisConcours_CycleId",
                schema: "Admission",
                table: "FraisConcours",
                column: "CycleId");

            migrationBuilder.CreateIndex(
                name: "IX_FraisEtudeDossier_AnneeAcademiqueId",
                schema: "Admission",
                table: "FraisEtudeDossier",
                column: "AnneeAcademiqueId");

            migrationBuilder.CreateIndex(
                name: "IX_FraisEtudeDossier_ClasseId",
                schema: "Admission",
                table: "FraisEtudeDossier",
                column: "ClasseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inscriptions_Campus_CampusId",
                schema: "Scolarite",
                table: "Inscriptions",
                column: "CampusId",
                principalSchema: "Settings",
                principalTable: "Campus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inscriptions_Campus_CampusId",
                schema: "Scolarite",
                table: "Inscriptions");

            migrationBuilder.DropTable(
                name: "Bourses",
                schema: "Scolarite");

            migrationBuilder.DropTable(
                name: "Changements",
                schema: "Scolarite");

            migrationBuilder.DropTable(
                name: "FraisConcours",
                schema: "Admission");

            migrationBuilder.DropTable(
                name: "FraisEtudeDossier",
                schema: "Admission");

            migrationBuilder.DropIndex(
                name: "IX_Inscriptions_CampusId",
                schema: "Scolarite",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "CampusId",
                schema: "Scolarite",
                table: "Inscriptions");
        }
    }
}
