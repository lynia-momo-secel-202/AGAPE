using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestAgape.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Admission2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DossiersPersonnel_Candidats_CandidatId",
                schema: "Admission",
                table: "DossiersPersonnel");

            migrationBuilder.DropForeignKey(
                name: "FK_Personnes_Candidats_CandidatId",
                schema: "Management",
                table: "Personnes");

            migrationBuilder.DropIndex(
                name: "IX_Personnes_CandidatId",
                schema: "Management",
                table: "Personnes");

            migrationBuilder.DropIndex(
                name: "IX_DossiersPersonnel_CandidatId",
                schema: "Admission",
                table: "DossiersPersonnel");

            migrationBuilder.DropColumn(
                name: "CandidatId",
                schema: "Management",
                table: "Personnes");

            migrationBuilder.DropColumn(
                name: "DateEnregistrement",
                schema: "Management",
                table: "Personnes");

            migrationBuilder.DropColumn(
                name: "CandidatId",
                schema: "Admission",
                table: "DossiersPersonnel");

            migrationBuilder.AlterColumn<bool>(
                name: "Statut",
                schema: "Management",
                table: "ChefsDepartement",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<Guid>(
                name: "DossierPersonnelId",
                schema: "Admission",
                table: "Candidats",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PersonneId",
                schema: "Admission",
                table: "Candidats",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Candidats_DossierPersonnelId",
                schema: "Admission",
                table: "Candidats",
                column: "DossierPersonnelId",
                unique: true,
                filter: "[DossierPersonnelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Candidats_PersonneId",
                schema: "Admission",
                table: "Candidats",
                column: "PersonneId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Candidats_DossiersPersonnel_DossierPersonnelId",
                schema: "Admission",
                table: "Candidats",
                column: "DossierPersonnelId",
                principalSchema: "Admission",
                principalTable: "DossiersPersonnel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidats_Personnes_PersonneId",
                schema: "Admission",
                table: "Candidats",
                column: "PersonneId",
                principalSchema: "Management",
                principalTable: "Personnes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidats_DossiersPersonnel_DossierPersonnelId",
                schema: "Admission",
                table: "Candidats");

            migrationBuilder.DropForeignKey(
                name: "FK_Candidats_Personnes_PersonneId",
                schema: "Admission",
                table: "Candidats");

            migrationBuilder.DropIndex(
                name: "IX_Candidats_DossierPersonnelId",
                schema: "Admission",
                table: "Candidats");

            migrationBuilder.DropIndex(
                name: "IX_Candidats_PersonneId",
                schema: "Admission",
                table: "Candidats");

            migrationBuilder.DropColumn(
                name: "DossierPersonnelId",
                schema: "Admission",
                table: "Candidats");

            migrationBuilder.DropColumn(
                name: "PersonneId",
                schema: "Admission",
                table: "Candidats");

            migrationBuilder.AddColumn<Guid>(
                name: "CandidatId",
                schema: "Management",
                table: "Personnes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEnregistrement",
                schema: "Management",
                table: "Personnes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CandidatId",
                schema: "Admission",
                table: "DossiersPersonnel",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<bool>(
                name: "Statut",
                schema: "Management",
                table: "ChefsDepartement",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Personnes_CandidatId",
                schema: "Management",
                table: "Personnes",
                column: "CandidatId");

            migrationBuilder.CreateIndex(
                name: "IX_DossiersPersonnel_CandidatId",
                schema: "Admission",
                table: "DossiersPersonnel",
                column: "CandidatId");

            migrationBuilder.AddForeignKey(
                name: "FK_DossiersPersonnel_Candidats_CandidatId",
                schema: "Admission",
                table: "DossiersPersonnel",
                column: "CandidatId",
                principalSchema: "Admission",
                principalTable: "Candidats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Personnes_Candidats_CandidatId",
                schema: "Management",
                table: "Personnes",
                column: "CandidatId",
                principalSchema: "Admission",
                principalTable: "Candidats",
                principalColumn: "Id");
        }
    }
}
