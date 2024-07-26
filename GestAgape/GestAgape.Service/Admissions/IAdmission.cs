using GestAgape.Core.Entities.Admission;
using GestAgape.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Service.Admissions
{
    public interface IAdmission
    {
        #region AnneeAcademique
        public List<AnneeAcademique> GetAllAnneeAcademique { get; }
        public bool CreateAnneeAcademique(AnneeAcademique model);
        public bool UpdateAnneeAcademique(AnneeAcademique model);
        public bool DeleteAnneeAcademique(AnneeAcademique model);
        public AnneeAcademique GetAnneeAcademique(Guid Id);
        public bool VerifExistAnneeAcademique(string Anneedebut);
        public string[] LastAA();



        #endregion

        #region FraisConcours
        public List<FraisConcours> GetAllFraisConcours { get; }
        public bool CreateFraisConcours(FraisConcours model);
        public bool UpdateFraisConcours(FraisConcours model);
        public bool DeleteFraisConcours(FraisConcours model);
        public FraisConcours GetFraisConcours(Guid Id);
        public bool VerifFraisConcours(FraisConcours model);

        #endregion

        #region FraisEtudeDossier
        public List<FraisEtudeDossier> GetAllFraisEtudeDossier { get; }
        public bool CreateFraisEtudeDossier(FraisEtudeDossier model);
        public bool UpdateFraisEtudeDossier(FraisEtudeDossier model);
        public bool DeleteFraisEtudeDossier(FraisEtudeDossier model);
        public FraisEtudeDossier GetFraisEtudeDossier(Guid Id);
        public bool VerifFraisEtudeDossier(FraisEtudeDossier model);

        #endregion

        #region Concours
        public List<Concours> GetAllConcours { get; }
        public bool CreateConcours(ConcoursVM model);
        public bool UpdateConcours(ConcoursVM model);
        public bool DeleteConcours(Concours model);
        public Concours GetConcours(Guid Id);
        public ConcoursVM GetConcoursVM(Guid Id);
        public bool VerifConcours(ConcoursVM model);
        public int NombreJourNextConcours();
        public int TotalAdmisLastConcours();
        #endregion

        #region Candidat
        public List<Candidat> GetAllCandidat { get; }
        public bool CreateCandidat(CandidatVM model);
        public bool UpdateCandidat(CandidatVM model);
        public bool DeleteCandidat(Candidat model);
        public CandidatVM GetCandidatVM(Guid Id);
        public Candidat GetCandidat(Guid Id);
        public bool VerifExistCandidat(string Nom, string Prenom, string Telephone, string etab);



        #endregion

        #region DemandeAdmission
        public List<DemandeAdmission> GetAllDA { get; }
        public List<DemandeAdmission> GetDAByFilter(Guid? cycle, Guid? specialite, Guid? niveau, TypeAdmission? type, Decision? decision);

        public bool CreateDA(DemandeAdmission model);
        public bool UpdateDA(DemandeAdmission model);
        public bool DeleteDA(DemandeAdmission model);
        public DemandeAdmission GetDA(Guid Id);
        public bool VerifExistDAConcours(Guid CandidatId, Guid? ConcoursId);
        public bool ExistDAEtudeDossier(Guid classeId, Guid candidatId, Guid anneeAcaId);
        public bool UpdateStatutAdmission(DemandeAdmission model);
        public List<DemandeAdmission> GetDAParCycle(Guid cycleId);
        public List<DemandeAdmission> GetDAParClasse(Guid classeId);
        public bool VerifPaiementAllFraisAdmission(DemandeAdmission model);
        public int TotalDAParJour();

        #endregion

        #region DossierPersonnel
        public List<DossierPersonnel> GetAllDossierPersonnel { get; }
        public bool CreateDossierPersonnel(DossierPersonnelVM model);
        public bool UpdateDossierPersonnel(DossierPersonnelVM model);
        public bool DeleteDossierPersonnel(DossierPersonnel model);
        public DossierPersonnel GetDossierPersonnel(Guid Id);

        #endregion

        #region Paiement&Recu
        public List<Paiement> GetAllpaiement { get; }
        public bool CreatePaiement(Paiement model);
        public Paiement GetPaiement(Guid Id);
        public double? ResteFraisconcours(Guid da, DateTime dateimp);
        public double? ResteFraisEtdudeDossier(Guid da, DateTime dateimp);
        public string NumberToWords(int number);
        public List<Paiement> PaiementsParPeriode(DateTime dateDebut, DateTime dateFin);
        public int NbrePaiementParJour();

        #endregion

    }
}
