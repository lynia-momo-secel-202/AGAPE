using GestAgape.Core.Entities.Admission;
using GestAgape.Core.Entities.Scolarite;
using GestAgape.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Service.Scolarite
{
    public interface IScolarite
    {
        #region Etudiant
        #endregion

        #region Inscription
        public bool CreateInscription(Matricule matricule, Inscription inscription, Paiement paiement, string campusId);
        public bool StatutAdmission(DemandeAdmission model);
        public List<Inscription> GetAllInscription { get; }
        public Inscription GetInscription(Guid? Id);

        //public Inscription GetInscriptByEtud(Guid Id);
        public List<Inscription> GetInscriptByFilter(Guid? cycle, Guid? specialite, Guid? niveau, DateTime? datedebut, DateTime? datefin);
        public bool VerifExistInscription(Guid Id);
        public int TotalInscritsParJour();
        public int[] NbreInscritsAnnuel();
        public double? ResteFraisInscription(Guid Id, DateTime dateimp);


        #endregion

        #region FraisInscription
        public List<FraisInscription> GetAllFraisInscription { get; }
        public bool CreateFraisInscription(FraisInscription model);
        public bool UpdateFraisInscription(FraisInscription model);
        public bool DeleteFraisInscription(FraisInscription model);
        public FraisInscription GetFraisInscription(Guid Id);
        public bool VerifFraisInscription(FraisInscription model);

        #endregion

        #region FraisMedicaux
        public List<FraisMedicaux> GetAllFraisMedicaux { get; }
        public bool CreateFraisMedicaux(FraisMedicaux model);
        public bool UpdateFraisMedicaux(FraisMedicaux model);
        public bool DeleteFraisMedicaux(FraisMedicaux model);
        public FraisMedicaux GetFraisMedicaux(Guid Id);
        public bool VerifFraisMedicaux(FraisMedicaux model);
        public bool CreatePaiementfraisMedicaux(Paiement model);
        public bool VerifPaiementFraisMedicaux(Paiement model, string campus);
        public double? ResteFraisMedicaux(Guid Id, DateTime dateimp);

        #endregion

        #region FraisDossierExamen
        public List<FraisDossierExamen> GetAllFraisDossierExamen { get; }
        public bool CreateFraisDossierExamen(FraisDossierExamen model);
        public FraisDossierExamen GetFraisDossierExamen(Guid Id);
        public bool UpdateFraisDossierExamen(FraisDossierExamen model);
        public bool DeleteFraisDossierExamen(FraisDossierExamen model);
        public bool VerifFraisDossierExamen(FraisDossierExamen model);
        public bool CreatePaiementFraisDossierExamen(Paiement model);
        public double? ResteFraisExamen(Guid inscription, DateTime dateimp);
        #endregion

        #region FraisSoutenance
        public List<FraisSoutenance> GetAllFraisSoutenance { get; }
        public bool CreateFraisSoutenance(FraisSoutenance model);
        public FraisSoutenance GetFraisSoutenance(Guid Id);
        public bool UpdateFraisSoutenance(FraisSoutenance model);
        public bool DeleteFraisSoutenance(FraisSoutenance model);
        public bool VerifFraisSoutenance(FraisSoutenance model);
        public double? ResteFraisSout(Guid inscription, DateTime dateimp);
        #endregion

        #region FraisScolarite
        public List<TrancheScolarite> GetAllTrancheScolarite { get; }
        public bool CreateTrancheScolarite(TrancheScolarite model);
        public TrancheScolarite GetTrancheScolarite(Guid Id);
        public bool UpdateTrancheScolarite(TrancheScolarite model);
        public bool DeleteTrancheScolarite(TrancheScolarite model);
        public bool VerifTrancheScolarite(TrancheScolarite model);
        public double? VerifExistInscript(Guid inscription);
        public double? ResteScolarite(Guid inscription, string campus, DateTime dateimp);


        #endregion

        #region Bourse
        public List<Bourse> GetAllBourses { get; }
        public bool CreateBourse(Bourse model);
        public bool UpdateBourse(Bourse model);
        public Bourse GetBourse(Guid Id);
        public bool ValidateBourse(Bourse model);

        #endregion

        #region Changement
        public List<ChangementFiliereOrCampus> GetAllchangementCampus { get; }
        public ChangementFiliereOrCampus Getchangcampus(Guid Id);
        public bool CreateChangementCampus(ChangementFiliereOrCampus model);
        public bool ValidateChangementCampus(ChangementFiliereOrCampus model);
        public bool VerifChangementCampus(ChangementFiliereOrCampus model);
        public bool CreateChangementFiliere(ChangementFiliereOrCampus model);
        public List<ChangementFiliereOrCampus> GetAllChangementFiliere { get; }
        public bool UpdateChangementCampus(ChangementFiliereOrCampus model);


        #endregion

        #region Recherche
        public IEnumerable<Inscription> Recherche(string term, string campus = null);

        #endregion

    }
}
