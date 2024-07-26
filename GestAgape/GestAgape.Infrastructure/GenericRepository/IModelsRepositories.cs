using GestAgape.Core.Entities;
using GestAgape.Core.Entities.Admission;
using GestAgape.Core.Entities.Identity;
using GestAgape.Core.Entities.Parametrage;
using GestAgape.Core.Entities.Scolarite;
using GestAgape.GenericRepository;

namespace GestAgape.Infrastructure.GenericRepository
{
    #region Identite
    public interface IPasswordHistoryRep : IGenericRepository<PasswordHistory> { }
    public interface IConnexionRep : IGenericRepository<Connexion> { }
    public interface IPersonneRep : IGenericRepository<Personne> { }
    public interface IAffectationRep : IGenericRepository<Affectation> { }
    public interface IChefDepartementRep : IGenericRepository<ChefDepartement> { }
    #endregion

    #region Parametrage
    public interface ICampusRep : IGenericRepository<Campus> { }
    public interface IClasseRep : IGenericRepository<Classe> { }
    public interface ICycleRep : IGenericRepository<Cycle> { }
    public interface IDepartementRep : IGenericRepository<Departement> { }
    public interface IFiliereRep : IGenericRepository<Filiere> { }
    public interface IIpesRep : IGenericRepository<Ipes> { }
    public interface INiveauRep : IGenericRepository<Niveau> { }
    public interface IAnneeAcademiqueRep : IGenericRepository<AnneeAcademique> { }
    public interface IDocumentRep : IGenericRepository<Document> { }
    public interface IDocADeposeRep : IGenericRepository<DocADepose> { }
    #endregion

    #region Admission
    public interface ICandidatRep : IGenericRepository<Candidat> { }
    public interface IConcoursRep : IGenericRepository<Concours> { }
    public interface IDemandeAdmissionRep : IGenericRepository<DemandeAdmission> { }
    public interface IDossierPersonnelRep : IGenericRepository<DossierPersonnel> { }
    public interface IDocDeposeRep : IGenericRepository<DocDepose> { }
    public interface IPaiementRep : IGenericRepository<Paiement> { }
    public interface IFraisConcoursRep : IGenericRepository<FraisConcours> { }
    public interface IFraisEtudeDossierRep : IGenericRepository<FraisEtudeDossier> { }
    #endregion

    #region Scolarite
    public interface IMatriculeRep : IGenericRepository<Matricule> { }
    public interface IInscriptionRep : IGenericRepository<Inscription> { }
    public interface IFraisInscriptionRep : IGenericRepository<FraisInscription> { }
    public interface IFraisDossierExamenRep : IGenericRepository<FraisDossierExamen> { }
    public interface IFraisMedicauxRep : IGenericRepository<FraisMedicaux> { }
    public interface IFraisSoutenanceRep : IGenericRepository<FraisSoutenance> { }
    public interface ITrancheScolariteRep : IGenericRepository<TrancheScolarite> { }
    public interface IBourseRep : IGenericRepository<Bourse> { }
    public interface IChangementFiliereOrCampusRep : IGenericRepository<ChangementFiliereOrCampus> { }
    #endregion

    #region
    #endregion


}
