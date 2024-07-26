using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestAgape.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DossierPersonnel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Documents",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "Settings",
                        principalTable: "Documents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DocsADepose",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClasseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocsADepose", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocsADepose_Classes_ClasseId",
                        column: x => x.ClasseId,
                        principalSchema: "Settings",
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocsADepose_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "Settings",
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocsDepose",
                schema: "Admission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Depose = table.Column<bool>(type: "bit", nullable: false),
                    Docpath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateImp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CandidatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocsDepose", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocsDepose_Candidats_CandidatId",
                        column: x => x.CandidatId,
                        principalSchema: "Admission",
                        principalTable: "Candidats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocsDepose_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "Settings",
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocsADepose_ClasseId",
                schema: "Settings",
                table: "DocsADepose",
                column: "ClasseId");

            migrationBuilder.CreateIndex(
                name: "IX_DocsADepose_DocumentId",
                schema: "Settings",
                table: "DocsADepose",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocsDepose_CandidatId",
                schema: "Admission",
                table: "DocsDepose",
                column: "CandidatId");

            migrationBuilder.CreateIndex(
                name: "IX_DocsDepose_DocumentId",
                schema: "Admission",
                table: "DocsDepose",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentId",
                schema: "Settings",
                table: "Documents",
                column: "DocumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocsADepose",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "DocsDepose",
                schema: "Admission");

            migrationBuilder.DropTable(
                name: "Documents",
                schema: "Settings");
        }
    }
}
