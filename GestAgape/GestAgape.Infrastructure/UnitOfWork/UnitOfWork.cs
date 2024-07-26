using GestAgape.Core.Entities.Admission;
using GestAgape.Core.Entities.Scolarite;
using GestAgape.Infrastructure.GenericRepository;
using GestAgape.Infrastructure.Repositories;
using GestAgape.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace GestAgape.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly IdentityContext _context;
        private bool _disposed;
        private IDbContextTransaction? _transaction;
        private string _errorMessage = string.Empty;
        private readonly ILogger _logger;

        #region Identity
        public IPasswordHistoryRep PasswordHistory { get; private set; }
        public IConnexionRep Connexion { get; private set; }
        public IAffectationRep Affectation { get; private set; }
        public IPersonneRep Personne { get; private set; }
        public IChefDepartementRep ChefDepartement { get; private set; }
        #endregion

        #region Parametrage
        public IAnneeAcademiqueRep AnneeAcademique { get; private set; }
        public ICampusRep Campus { get; private set; }
        public ICycleRep Cycle { get; private set; }
        public IFiliereRep Filiere { get; private set; }
        public IClasseRep Classe { get; private set; }
        public IIpesRep Ipes { get; private set; }
        public INiveauRep Niveau { get; private set; }
        public IDepartementRep Departement { get; private set; }
        public IDocumentRep Document { get; private set; }
        public IDocADeposeRep DocADepose { get; private set; }
        #endregion

        #region Admission
        public IConcoursRep Concours { get; private set; }
        public IDemandeAdmissionRep DemandeAdmission { get; private set; }
        public IDossierPersonnelRep DossierPersonnel { get; private set; }
        public IFraisConcoursRep FraisConcours { get; private set; }
        public IFraisEtudeDossierRep FraisEtudeDossier { get; private set; }
        public IPaiementRep Paiement { get; private set; }
        public ICandidatRep Candidat { get; private set; }
        public IDocDeposeRep DocDepose { get; private set;  }
        #endregion

        #region Scolarite
        public IMatriculeRep Matricule { get; private set; }
        public IFraisInscriptionRep FraisInscription { get; private set; }
        public IFraisDossierExamenRep FraisDossierExamen { get; private set; }
        public IFraisMedicauxRep FraisMedicaux { get; private set; }
        public IFraisSoutenanceRep FraisSoutenance { get; private set; }
        public IInscriptionRep Inscription { get; private set; }
        public ITrancheScolariteRep TrancheScolarite { get; private set; }
        public IBourseRep Bourse { get; private set; }
        public IChangementFiliereOrCampusRep ChangementFiliereOrCampus { get; private set; }
        #endregion

        #region
        #endregion

        public UnitOfWork(IdentityContext context, ILoggerFactory loggerFactory)
        {
            this._context = context;
            _logger = loggerFactory.CreateLogger("logs");
            Connexion = new ConnexionRepository(context, _logger);
            PasswordHistory = new PasswordHistoryRepository(context, _logger);
            AnneeAcademique = new AnneeAcademiqueRepository(context, _logger);
            Campus = new CampusRepository(context, _logger);
            Affectation = new AffectationRepository(context, _logger);
            Cycle = new CycleRepository(context, _logger);
            Filiere = new FiliereRepository(context, _logger);
            Classe = new ClasseRepository(context, _logger);
            Ipes = new IpesRepository(context, _logger);
            Niveau = new NiveauRepository(context, _logger);
            Departement = new DepartementRepository(context, _logger);
            Personne = new PersonneRepository(context, _logger);
            Candidat = new CandidatRepository(context, _logger);
            ChefDepartement = new ChefDepartementRepository(context, _logger);
            Concours = new ConcoursRepository(context, _logger);
            DemandeAdmission = new DemandeAdmissionRepository(context, _logger);
            DossierPersonnel = new DossierPersonnelRepository(context, _logger);
            Paiement = new PaiementRepository(context, _logger);
            Inscription = new InscriptionRepository(context, _logger);
            Matricule = new MatriculeRepository(context, _logger);
            FraisInscription = new FraisInscriptionRepository(context, _logger);
            FraisMedicaux = new FraisMedicauxRepository(context, _logger);
            FraisSoutenance = new FraisSoutenanceRepository(context, _logger);
            FraisDossierExamen = new FraisDossierExamenRepository(context, _logger);
            TrancheScolarite = new TrancheScolariteRepository(context, _logger);
            FraisEtudeDossier = new FraisEtudeDossierRepository(context, _logger);
            FraisConcours = new FraisConcoursRepository(context, _logger);
            ChangementFiliereOrCampus = new ChangementFiliereOrCampusRepository(context, _logger);
            Bourse = new BourseRepository(context, _logger);
            Document = new DocumentRepository(context, _logger);
            DocADepose = new DocADeposeRepository(context, _logger);
            DocDepose = new DocDeposeRepository(context, _logger);
        }

        public void Commit()
        {
            //_transaction.Commit();
            _context.SaveChanges();

        }

        public void CreateTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Rollback()
        {
            //_transaction.Rollback();
            //_transaction.Dispose();
            _context.Dispose();

        }
        //public async Task CommitAsync()
        //    => await _context.SaveChangesAsync();
        //public async Task RollbackA()
        //    => await _context.DisposeAsync();
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();
            _disposed = true;
        }
        public void Save()
        {
            //Calling DbContext Class SaveChanges method 
            //_context.SaveChanges();
        }
    }
}
