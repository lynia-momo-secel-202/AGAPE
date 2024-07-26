using GestAgape.Core.Entities;
using GestAgape.Core.Entities.Identity;
using GestAgape.Core.Entities.Admission;
using GestAgape.Core.Entities.Parametrage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using GestAgape.Core.Entities.Scolarite;

namespace GestAgape.Models
{
    public class IdentityContext : IdentityDbContext
    {
        #region constructeurs
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }
        public class DataContextFactory : IDesignTimeDbContextFactory<IdentityContext>
        {
            public IdentityContext CreateDbContext(string[] args)
            {
                var configuration = new ConfigurationBuilder()
                 .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                 .AddJsonFile("appsettings.json")
                 .Build();

                var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

                return new IdentityContext(optionsBuilder.Options);
            }
        }
        #endregion

        #region FluentAPI
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Management");
            
            //Identity
            builder.Entity<Connexion>().ToTable("Connexions", "Identity");
            builder.Entity<PasswordHistory>().ToTable("PasswordHistories", "Identity");

            //Management
            builder.Entity<Affectation>().ToTable("Affectations", "Settings");
            builder.Entity<ChefDepartement>().ToTable("ChefsDepartement", "Management");
            builder.Entity<Personne>().ToTable("Personnes", "Management");

            //Settings
            builder.Entity<Campus>().ToTable("Campus", "Settings");
            builder.Entity<Classe>().ToTable("Classes", "Settings");
            builder.Entity<Cycle>().ToTable("Cycles", "Settings");
            builder.Entity<Departement>().ToTable("Departements", "Settings");
            builder.Entity<Ipes>().ToTable("IPES", "Settings");
            builder.Entity<Niveau>().ToTable("Niveaux", "Settings");
            builder.Entity<DocADepose>().ToTable("DocsADepose", "Settings");
            builder.Entity<Document>().ToTable("Documents", "Settings");
            builder.Entity<Filiere>().ToTable("Serie", "Settings");

            //Admission
            builder.Entity<AnneeAcademique>().ToTable("AnneeAcademiques", "Admission");
            builder.Entity<Candidat>().ToTable("Candidats", "Admission");
            builder.Entity<Concours>().ToTable("Concours", "Admission");
            builder.Entity<DemandeAdmission>().ToTable("DemandesAdmission", "Admission");
            builder.Entity<DossierPersonnel>().ToTable("DossiersPersonnel", "Admission");
            builder.Entity<Paiement>().ToTable("Paiements", "Admission");
            builder.Entity<FraisConcours>().ToTable("FraisConcours", "Admission");
            builder.Entity<FraisEtudeDossier>().ToTable("FraisEtudeDossier", "Admission");
            builder.Entity<DocDepose>().ToTable("DocsDepose", "Admission");

            //Inscription
            builder.Entity<Inscription>().ToTable("Inscriptions", "Scolarite");
            builder.Entity<Matricule>().ToTable("Matricule", "Scolarite");
            builder.Entity<FraisSoutenance>().ToTable("FraisSoutenances", "Scolarite");
            builder.Entity<FraisDossierExamen>().ToTable("FraisDossierExamen", "Scolarite");
            builder.Entity<FraisInscription>().ToTable("FraisInscription", "Scolarite");
            builder.Entity<FraisMedicaux>().ToTable("FraisMedicaux", "Scolarite");
            builder.Entity<TrancheScolarite>().ToTable("TranchesScolarites", "Scolarite");
            builder.Entity<Bourse>().ToTable("Bourses", "Scolarite");
            builder.Entity<ChangementFiliereOrCampus>().ToTable("Changements", "Scolarite");

            builder.Entity<IdentityUser>(entity =>
            {
                entity.ToTable(name: "User", schema: "Identity");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role", schema: "Identity");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles", schema: "Identity");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims", schema: "Identity");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins", schema: "Identity");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims", schema: "Identity");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens", schema: "Identity");
            });
        }
        #endregion
         
        #region identitydbset
        public DbSet<ApplicationUsers> ApplicationUser { get; set; }
        public DbSet<Connexion> Connexions { get; set; }
        public DbSet<PasswordHistory> PasswordHistories { get; set; }
        public DbSet<Personne> Personnes { get; set; }
        #endregion

        #region ParametreDbset
        public DbSet<Affectation> Affectations { get; set; }
        public DbSet<Campus> Campus { get; set; }
        public DbSet<ChefDepartement> ChefDepartements { get; set; }
        public DbSet<Classe> Classes { get; set; }
        public DbSet<Cycle> Cycles { get; set; }
        public DbSet<Departement> Departements { get; set; }
        public DbSet<Filiere> Filieres { get; set; }
        public DbSet<Niveau> Niveaux { get; set; }
        public DbSet<DocADepose> DocsADepose { get; set; }
        public DbSet<Document> Documents { get; set; }
        #endregion

        #region AdmissionDbset
        public DbSet<AnneeAcademique> AnneeAcademiques { get; set; }
        public DbSet<Candidat> Candidats { get; set; }
        public DbSet<Concours> Concours { get; set; }
        public DbSet<DemandeAdmission> DemandeAdmissions { get; set; }
        public DbSet<DossierPersonnel> DossierPersonnels { get; set; }
        public DbSet<FraisConcours> FraisConcours { get; set; }
        public DbSet<FraisEtudeDossier> FraisEtudeDossier { get; set; }
        public DbSet<Paiement> Paiements { get; set; }
        public DbSet<DocDepose> DocsDepose { get; set; }

        #endregion      

        #region InscriptionDbSet
        public DbSet<Matricule> Matricules { get; set; }
        public DbSet<Inscription> Inscriptions { get; set; }
        public DbSet<FraisDossierExamen> FraisDossierExamens { get; set; }
        public DbSet<FraisInscription> FraisInscriptions { get; set; }
        public DbSet<FraisMedicaux> FraisMedicaux { get; set; }
        public DbSet<FraisSoutenance> FraisSoutenances { get; set; }
        public DbSet<TrancheScolarite> TranchesScolarites { get; set; }
        public DbSet<Bourse> Bourses { get; set; }
        public DbSet<ChangementFiliereOrCampus> Changements { get; set; }

        #endregion

    }
}
