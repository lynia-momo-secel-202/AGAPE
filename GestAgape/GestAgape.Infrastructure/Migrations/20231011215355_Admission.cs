using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestAgape.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Admission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Personne",
                schema: "Management",
                table: "Personne");

            migrationBuilder.EnsureSchema(
                name: "Admission");

            migrationBuilder.EnsureSchema(
                name: "Settings");

            migrationBuilder.RenameTable(
                name: "Personne",
                schema: "Management",
                newName: "Personnes",
                newSchema: "Management");

            migrationBuilder.AddColumn<Guid>(
                name: "CandidatId",
                schema: "Management",
                table: "Personnes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Personnes",
                schema: "Management",
                table: "Personnes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Candidats",
                schema: "Admission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomPere = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelephonePere = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelephoneMere = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomMere = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessionMere = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessionPere = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quartier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Etablissement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnneeAcademiques",
                schema: "Admission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnneeDebut = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnneeFin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnneeAcademiques", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Concours",
                schema: "Admission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeureDebut = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeureFin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Flyers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Resultats = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concours", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cycles",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FraisConcours = table.Column<double>(type: "float", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cycles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departements",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IPES",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdresseCampusPrincipal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteWeb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoitePostale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cachet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroCompteBancaire = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IPES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Niveaux",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Niveaux", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DossiersPersonnel",
                schema: "Admission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActeNaissance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleveBac = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CNI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Photos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleveNiveau1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleveNiveau2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleveMaster1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleveBTS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleveLicence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CandidatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DossiersPersonnel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DossiersPersonnel_Candidats_CandidatId",
                        column: x => x.CandidatId,
                        principalSchema: "Admission",
                        principalTable: "Candidats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChefsDepartement",
                schema: "Management",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateNomination = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Statut = table.Column<bool>(type: "bit", nullable: false),
                    PersonneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChefsDepartement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChefsDepartement_Departements_DepartementId",
                        column: x => x.DepartementId,
                        principalSchema: "Settings",
                        principalTable: "Departements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChefsDepartement_Personnes_PersonneId",
                        column: x => x.PersonneId,
                        principalSchema: "Management",
                        principalTable: "Personnes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Filieres",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartementID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filieres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Filieres_Departements_DepartementID",
                        column: x => x.DepartementID,
                        principalSchema: "Settings",
                        principalTable: "Departements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Campus",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Responsable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IPESId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campus_IPES_IPESId",
                        column: x => x.IPESId,
                        principalSchema: "Settings",
                        principalTable: "IPES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Specialites",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FiliereID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Specialites_Filieres_FiliereID",
                        column: x => x.FiliereID,
                        principalSchema: "Settings",
                        principalTable: "Filieres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Affectations",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateAffectation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CampusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Affectations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Affectations_Campus_CampusId",
                        column: x => x.CampusId,
                        principalSchema: "Settings",
                        principalTable: "Campus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Affectations_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProgrammeAcademique = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FraisEtudeDossier = table.Column<double>(type: "float", nullable: true),
                    CycleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpecialiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NiveauId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_Cycles_CycleId",
                        column: x => x.CycleId,
                        principalSchema: "Settings",
                        principalTable: "Cycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Classes_Niveaux_NiveauId",
                        column: x => x.NiveauId,
                        principalSchema: "Settings",
                        principalTable: "Niveaux",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Classes_Specialites_SpecialiteId",
                        column: x => x.SpecialiteId,
                        principalSchema: "Settings",
                        principalTable: "Specialites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DemandesAdmission",
                schema: "Admission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Decision = table.Column<int>(type: "int", nullable: false),
                    Mention = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeAdmission = table.Column<int>(type: "int", nullable: false),
                    CandidatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConcoursId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AnneeAcademiqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClasseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandesAdmission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemandesAdmission_Candidats_CandidatId",
                        column: x => x.CandidatId,
                        principalSchema: "Admission",
                        principalTable: "Candidats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DemandesAdmission_AnneeAcademiques_AnneeAcademiqueId",
                        column: x => x.AnneeAcademiqueId,
                        principalSchema: "Admission",
                        principalTable: "AnneeAcademiques",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DemandesAdmission_Classes_ClasseId",
                        column: x => x.ClasseId,
                        principalSchema: "Settings",
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DemandesAdmission_Concours_ConcoursId",
                        column: x => x.ConcoursId,
                        principalSchema: "Admission",
                        principalTable: "Concours",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Paiements",
                schema: "Admission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Montant = table.Column<double>(type: "float", nullable: true),
                    Motif = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DemandeAdmissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paiements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paiements_DemandesAdmission_DemandeAdmissionId",
                        column: x => x.DemandeAdmissionId,
                        principalSchema: "Admission",
                        principalTable: "DemandesAdmission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personnes_CandidatId",
                schema: "Management",
                table: "Personnes",
                column: "CandidatId");

            migrationBuilder.CreateIndex(
                name: "IX_Affectations_CampusId",
                schema: "Settings",
                table: "Affectations",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Affectations_UserId",
                schema: "Settings",
                table: "Affectations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Campus_IPESId",
                schema: "Settings",
                table: "Campus",
                column: "IPESId");

            migrationBuilder.CreateIndex(
                name: "IX_ChefsDepartement_DepartementId",
                schema: "Management",
                table: "ChefsDepartement",
                column: "DepartementId");

            migrationBuilder.CreateIndex(
                name: "IX_ChefsDepartement_PersonneId",
                schema: "Management",
                table: "ChefsDepartement",
                column: "PersonneId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CycleId",
                schema: "Settings",
                table: "Classes",
                column: "CycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_NiveauId",
                schema: "Settings",
                table: "Classes",
                column: "NiveauId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_SpecialiteId",
                schema: "Settings",
                table: "Classes",
                column: "SpecialiteId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandesAdmission_AnneeAcademiqueId",
                schema: "Admission",
                table: "DemandesAdmission",
                column: "AnneeAcademiqueId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandesAdmission_CandidatId",
                schema: "Admission",
                table: "DemandesAdmission",
                column: "CandidatId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandesAdmission_ClasseId",
                schema: "Admission",
                table: "DemandesAdmission",
                column: "ClasseId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandesAdmission_ConcoursId",
                schema: "Admission",
                table: "DemandesAdmission",
                column: "ConcoursId");

            migrationBuilder.CreateIndex(
                name: "IX_DossiersPersonnel_CandidatId",
                schema: "Admission",
                table: "DossiersPersonnel",
                column: "CandidatId");

            migrationBuilder.CreateIndex(
                name: "IX_Filieres_DepartementID",
                schema: "Settings",
                table: "Filieres",
                column: "DepartementID");

            migrationBuilder.CreateIndex(
                name: "IX_Paiements_DemandeAdmissionId",
                schema: "Admission",
                table: "Paiements",
                column: "DemandeAdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialites_FiliereID",
                schema: "Settings",
                table: "Specialites",
                column: "FiliereID");

            migrationBuilder.AddForeignKey(
                name: "FK_Personnes_Candidats_CandidatId",
                schema: "Management",
                table: "Personnes",
                column: "CandidatId",
                principalSchema: "Admission",
                principalTable: "Candidats",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personnes_Candidats_CandidatId",
                schema: "Management",
                table: "Personnes");

            migrationBuilder.DropTable(
                name: "Affectations",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "ChefsDepartement",
                schema: "Management");

            migrationBuilder.DropTable(
                name: "DossiersPersonnel",
                schema: "Admission");

            migrationBuilder.DropTable(
                name: "Paiements",
                schema: "Admission");

            migrationBuilder.DropTable(
                name: "Campus",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "DemandesAdmission",
                schema: "Admission");

            migrationBuilder.DropTable(
                name: "IPES",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "Candidats",
                schema: "Admission");

            migrationBuilder.DropTable(
                name: "AnneeAcademiques",
                schema: "Admission");

            migrationBuilder.DropTable(
                name: "Classes",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "Concours",
                schema: "Admission");

            migrationBuilder.DropTable(
                name: "Cycles",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "Niveaux",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "Specialites",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "Filieres",
                schema: "Settings");

            migrationBuilder.DropTable(
                name: "Departements",
                schema: "Settings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Personnes",
                schema: "Management",
                table: "Personnes");

            migrationBuilder.DropIndex(
                name: "IX_Personnes_CandidatId",
                schema: "Management",
                table: "Personnes");

            migrationBuilder.DropColumn(
                name: "CandidatId",
                schema: "Management",
                table: "Personnes");

            migrationBuilder.RenameTable(
                name: "Personnes",
                schema: "Management",
                newName: "Personne",
                newSchema: "Management");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Personne",
                schema: "Management",
                table: "Personne",
                column: "Id");
        }
    }
}
