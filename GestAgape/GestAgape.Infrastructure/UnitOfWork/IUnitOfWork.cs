using GestAgape.Infrastructure.GenericRepository;

namespace GestAgape.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        #region Identity
        IPasswordHistoryRep PasswordHistory { get; }
        IConnexionRep Connexion { get; }
        IAffectationRep Affectation { get; }
        IPersonneRep Personne { get; }
        IChefDepartementRep ChefDepartement { get; }
        #endregion

        #region Parametrage
        IAnneeAcademiqueRep AnneeAcademique { get; }
        ICampusRep Campus { get; }
        ICycleRep Cycle { get; }
        IFiliereRep Filiere { get; }
        IClasseRep Classe { get; }
        IIpesRep Ipes { get; }
        INiveauRep Niveau { get; }
        IDepartementRep Departement { get; }
        IDocumentRep Document { get; }
        IDocADeposeRep DocADepose { get; }
        #endregion

        #region Admission
        ICandidatRep Candidat { get; }
        IConcoursRep Concours { get; }
        IDemandeAdmissionRep DemandeAdmission { get; }
        IDossierPersonnelRep DossierPersonnel { get; }
        IFraisConcoursRep FraisConcours { get; }
        IFraisEtudeDossierRep FraisEtudeDossier { get; }
        IPaiementRep Paiement { get; }
        IDocDeposeRep DocDepose { get; }

        #endregion

        #region Scolarite
        IMatriculeRep Matricule { get; }
        IFraisInscriptionRep FraisInscription { get; }
        IFraisDossierExamenRep FraisDossierExamen { get; }
        IFraisMedicauxRep FraisMedicaux { get; }
        IFraisSoutenanceRep FraisSoutenance { get; }
        IInscriptionRep Inscription { get; }
        ITrancheScolariteRep TrancheScolarite { get; }
        IBourseRep Bourse { get; }
        IChangementFiliereOrCampusRep ChangementFiliereOrCampus { get; }
        #endregion

        #region
        #endregion

        void CreateTransaction();
        void Commit();
        void Rollback();
        void Save();
        //Task Commit();
        //Task Rollback();
    }
}
