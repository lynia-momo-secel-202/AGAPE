using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestAgape.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateToAgape : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Specialites_SpecialiteId",
                schema: "Settings",
                table: "Classes");

            migrationBuilder.DropTable(
                name: "Specialites",
                schema: "Settings");

            migrationBuilder.DropForeignKey(
                name: "FK_Filieres_Departements_DepartementID",
                schema: "Settings",
                table: "Filieres");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Filieres_Filieres_FiliereID",
            //    schema: "Settings",
            //    table: "Filieres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Filieres",
                schema: "Settings",
                table: "Filieres");

            //migrationBuilder.DropIndex(
            //    name: "IX_Filieres_FiliereID",
            //    schema: "Settings",
            //    table: "Filieres");

            migrationBuilder.RenameColumn(
                name: "SpecialiteId",
                schema: "settings",
                table: "Classes",
                newName: "FiliereId");

            migrationBuilder.RenameTable(
                name: "Filieres",
                schema: "Settings",
                newName: "Serie",
                newSchema: "Settings");

            migrationBuilder.RenameIndex(
                name: "IX_Filieres_DepartementID",
                schema: "Settings",
                table: "Serie",
                newName: "IX_Serie_DepartementID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Serie",
                schema: "Settings",
                table: "Serie",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Serie_FiliereId",
                schema: "Settings",
                table: "Classes",
                column: "FiliereId",
                principalSchema: "Settings",
                principalTable: "Serie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Serie_Departements_DepartementID",
                schema: "Settings",
                table: "Serie",
                column: "DepartementID",
                principalSchema: "Settings",
                principalTable: "Departements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Serie_FiliereId",
                schema: "Settings",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Serie_Departements_DepartementID",
                schema: "Settings",
                table: "Serie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Serie",
                schema: "Settings",
                table: "Serie");

            migrationBuilder.RenameTable(
                name: "Serie",
                schema: "Settings",
                newName: "Filieres",
                newSchema: "Settings");

            migrationBuilder.RenameIndex(
                name: "IX_Serie_DepartementID",
                schema: "Settings",
                table: "Filieres",
                newName: "IX_Filieres_DepartementID");

            migrationBuilder.AddColumn<Guid>(
                name: "FiliereID",
                schema: "Settings",
                table: "Filieres",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Filieres",
                schema: "Settings",
                table: "Filieres",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Filieres_FiliereID",
                schema: "Settings",
                table: "Filieres",
                column: "FiliereID");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Filieres_FiliereId",
                schema: "Settings",
                table: "Classes",
                column: "FiliereId",
                principalSchema: "Settings",
                principalTable: "Filieres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Filieres_Departements_DepartementID",
                schema: "Settings",
                table: "Filieres",
                column: "DepartementID",
                principalSchema: "Settings",
                principalTable: "Departements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Filieres_Filieres_FiliereID",
                schema: "Settings",
                table: "Filieres",
                column: "FiliereID",
                principalSchema: "Settings",
                principalTable: "Filieres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
