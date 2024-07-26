using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestAgape.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Inscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Scolarite");

            migrationBuilder.AlterColumn<int>(
                name: "Motif",
                schema: "Admission",
                table: "Paiements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InscriptionId",
                schema: "Admission",
                table: "DemandesAdmission",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FraisInscription",
                schema: "Scolarite",
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
                    table.PrimaryKey("PK_FraisInscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FraisInscription_AnneeAcademiques_AnneeAcademiqueId",
                        column: x => x.AnneeAcademiqueId,
                        principalSchema: "Admission",
                        principalTable: "AnneeAcademiques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FraisInscription_Classes_ClasseId",
                        column: x => x.ClasseId,
                        principalSchema: "Settings",
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FraisMedicaux",
                schema: "Scolarite",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Montant = table.Column<double>(type: "float", nullable: true),
                    CampusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnneeAcademiqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FraisMedicaux", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FraisMedicaux_AnneeAcademiques_AnneeAcademiqueId",
                        column: x => x.AnneeAcademiqueId,
                        principalSchema: "Admission",
                        principalTable: "AnneeAcademiques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FraisMedicaux_Campus_CampusId",
                        column: x => x.CampusId,
                        principalSchema: "Settings",
                        principalTable: "Campus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FraisSoutenances",
                schema: "Scolarite",
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
                    table.PrimaryKey("PK_FraisSoutenances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FraisSoutenances_AnneeAcademiques_AnneeAcademiqueId",
                        column: x => x.AnneeAcademiqueId,
                        principalSchema: "Admission",
                        principalTable: "AnneeAcademiques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FraisSoutenances_Classes_ClasseId",
                        column: x => x.ClasseId,
                        principalSchema: "Settings",
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FrasisDossierExamen",
                schema: "Scolarite",
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
                    table.PrimaryKey("PK_FrasisDossierExamen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FrasisDossierExamen_AnneeAcademiques_AnneeAcademiqueId",
                        column: x => x.AnneeAcademiqueId,
                        principalSchema: "Admission",
                        principalTable: "AnneeAcademiques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FrasisDossierExamen_Classes_ClasseId",
                        column: x => x.ClasseId,
                        principalSchema: "Settings",
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matricule",
                schema: "Scolarite",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LibelleMatricule = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matricule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TranchesScolarites",
                schema: "Scolarite",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LibelleTranche = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Montant = table.Column<double>(type: "float", nullable: true),
                    DateLimitePaiement = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClasseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CampusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnneeAcademiqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranchesScolarites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TranchesScolarites_AnneeAcademiques_AnneeAcademiqueId",
                        column: x => x.AnneeAcademiqueId,
                        principalSchema: "Admission",
                        principalTable: "AnneeAcademiques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TranchesScolarites_Campus_CampusId",
                        column: x => x.CampusId,
                        principalSchema: "Settings",
                        principalTable: "Campus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TranchesScolarites_Classes_ClasseId",
                        column: x => x.ClasseId,
                        principalSchema: "Settings",
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inscriptions",
                schema: "Scolarite",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatriculeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inscriptions_Matricule_MatriculeId",
                        column: x => x.MatriculeId,
                        principalSchema: "Scolarite",
                        principalTable: "Matricule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemandesAdmission_InscriptionId",
                schema: "Admission",
                table: "DemandesAdmission",
                column: "InscriptionId",
                unique: true,
                filter: "[InscriptionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FraisInscription_AnneeAcademiqueId",
                schema: "Scolarite",
                table: "FraisInscription",
                column: "AnneeAcademiqueId");

            migrationBuilder.CreateIndex(
                name: "IX_FraisInscription_ClasseId",
                schema: "Scolarite",
                table: "FraisInscription",
                column: "ClasseId");

            migrationBuilder.CreateIndex(
                name: "IX_FraisMedicaux_AnneeAcademiqueId",
                schema: "Scolarite",
                table: "FraisMedicaux",
                column: "AnneeAcademiqueId");

            migrationBuilder.CreateIndex(
                name: "IX_FraisMedicaux_CampusId",
                schema: "Scolarite",
                table: "FraisMedicaux",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_FraisSoutenances_AnneeAcademiqueId",
                schema: "Scolarite",
                table: "FraisSoutenances",
                column: "AnneeAcademiqueId");

            migrationBuilder.CreateIndex(
                name: "IX_FraisSoutenances_ClasseId",
                schema: "Scolarite",
                table: "FraisSoutenances",
                column: "ClasseId");

            migrationBuilder.CreateIndex(
                name: "IX_FrasisDossierExamen_AnneeAcademiqueId",
                schema: "Scolarite",
                table: "FrasisDossierExamen",
                column: "AnneeAcademiqueId");

            migrationBuilder.CreateIndex(
                name: "IX_FrasisDossierExamen_ClasseId",
                schema: "Scolarite",
                table: "FrasisDossierExamen",
                column: "ClasseId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_MatriculeId",
                schema: "Scolarite",
                table: "Inscriptions",
                column: "MatriculeId");

            migrationBuilder.CreateIndex(
                name: "IX_TranchesScolarites_AnneeAcademiqueId",
                schema: "Scolarite",
                table: "TranchesScolarites",
                column: "AnneeAcademiqueId");

            migrationBuilder.CreateIndex(
                name: "IX_TranchesScolarites_CampusId",
                schema: "Scolarite",
                table: "TranchesScolarites",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_TranchesScolarites_ClasseId",
                schema: "Scolarite",
                table: "TranchesScolarites",
                column: "ClasseId");

            migrationBuilder.AddForeignKey(
                name: "FK_DemandesAdmission_Inscriptions_InscriptionId",
                schema: "Admission",
                table: "DemandesAdmission",
                column: "InscriptionId",
                principalSchema: "Scolarite",
                principalTable: "Inscriptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DemandesAdmission_Inscriptions_InscriptionId",
                schema: "Admission",
                table: "DemandesAdmission");

            migrationBuilder.DropTable(
                name: "FraisInscription",
                schema: "Scolarite");

            migrationBuilder.DropTable(
                name: "FraisMedicaux",
                schema: "Scolarite");

            migrationBuilder.DropTable(
                name: "FraisSoutenances",
                schema: "Scolarite");

            migrationBuilder.DropTable(
                name: "FrasisDossierExamen",
                schema: "Scolarite");

            migrationBuilder.DropTable(
                name: "Inscriptions",
                schema: "Scolarite");

            migrationBuilder.DropTable(
                name: "TranchesScolarites",
                schema: "Scolarite");

            migrationBuilder.DropTable(
                name: "Matricule",
                schema: "Scolarite");

            migrationBuilder.DropIndex(
                name: "IX_DemandesAdmission_InscriptionId",
                schema: "Admission",
                table: "DemandesAdmission");

            migrationBuilder.DropColumn(
                name: "InscriptionId",
                schema: "Admission",
                table: "DemandesAdmission");

            migrationBuilder.AlterColumn<string>(
                name: "Motif",
                schema: "Admission",
                table: "Paiements",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
