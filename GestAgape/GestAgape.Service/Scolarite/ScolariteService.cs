using GestAgape.Core.Entities.Admission;
using GestAgape.Core.Entities.Parametrage;
using GestAgape.Core.Entities.Scolarite;
using GestAgape.Models;
using GestAgape.Service.Admissions;
using GestAgape.Service.Parametrages;
using GestAgape.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Service.Scolarite
{
    public class ScolariteService : IScolarite
    {
        #region Membres prives

        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        protected readonly ILogger _logger;
        private readonly IAdmission _admission;
        protected readonly IParametrage _param;
        private readonly IWebHostEnvironment _hostingEnv;
        private IdentityContext _context;

        #endregion

        #region Constructeur
        public ScolariteService(
            IAdmission admission,
            IEmailSender emailSender,
            IUnitOfWork unitOfWork,
            IParametrage param,
            ILoggerFactory loggerFactory,
            IWebHostEnvironment hostingEnv,
            IdentityContext context

        )
        {
            _param = param;
            _admission = admission;
            _emailSender = emailSender; ;
            _unitOfWork = unitOfWork;
            _logger = loggerFactory.CreateLogger("logs");
            _hostingEnv = hostingEnv;
            _context = context;
        }
        #endregion

        #region Matricule
        public List<Matricule> GetAllMatricule => _unitOfWork.Matricule.Values
                                                   .Include(e => e.Inscription)
                                                   .ToList();
        //Get Candidat 
        public Matricule GetMatricule(Guid Id)
        {
            try
            {
                var Matricule = GetAllMatricule.FirstOrDefault(e => e.Id == Id);
                if (Matricule != null)
                {
                    return Matricule;
                }
                _logger.LogError($"l'Matricule d' " + $" d'{Id} n'existe pas", typeof(ScolariteService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la recuperation du matricule : {Id}", typeof(ScolariteService));
                return null;
            }
        }

        public string GenerateMatricule(Guid demandeId)
        {
            int num = 0001;
            string? mat = "23IME" + num;
            var demande = _admission.GetDA(demandeId);
            foreach (DemandeAdmission? dmd in _admission.GetAllDA)
            {
                if (dmd.InscriptionId != null)
                {
                    if (dmd.CandidatId == demande.CandidatId)
                    {
                        var insc = GetInscription(dmd.InscriptionId);
                        var matric = GetMatricule(insc.MatriculeId);
                        mat = matric.LibelleMatricule;
                        break;
                    }
                    else
                    {
                        foreach (AnneeAcademique? a in _unitOfWork.AnneeAcademique.GetAll())
                        {
                            if (a.Id == dmd.AnneeAcademiqueId)
                            {
                                num = num + 1;
                            }
                        }
                        mat = DateTime.Now.ToString("yy") + "IME" + num.ToString();
                    }
                }
            }
            return mat;
        }

        #endregion

        #region Inscription

        // récupération liste des insciptions 

        public List<Inscription> GetAllInscription => _unitOfWork.Inscription.Values
                                                   .Include(i => i.Matricule)
                                                   .Include(i => i.DemandeAdmission)
                                                        .ThenInclude(da => da.Candidat)
                                                            .ThenInclude(c => c.Personne)
                                                    .Include(i => i.DemandeAdmission)
                                                        .ThenInclude(da => da.AnneeAcademique)
                                                    .Include(i => i.DemandeAdmission)
                                                        .ThenInclude(da => da.Classe)
                                                            .ThenInclude(cl => cl.Cycle)
                                                    .Include(i => i.DemandeAdmission)
                                                        .ThenInclude(da => da.Classe)
                                                            .ThenInclude(cl => cl.Filiere)
                                                    .Include(i => i.DemandeAdmission)
                                                        .ThenInclude(da => da.Classe)
                                                            .ThenInclude(cl => cl.Niveau)
                                                    .Include(i => i.Campus)
                                                    .ToList();


        // Enregistrement d'une inscription

        public bool CreateInscription(Matricule matricule, Inscription inscription, Paiement paiement, string campusId)
        {
            try
            {
                Matricule m = new Matricule()
                {
                    Id = new Guid(),
                    AddedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                };

                _unitOfWork.Matricule.Insert(m);

                Paiement P = new Paiement()
                {
                    Id = new Guid(),
                    AddedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Motif = Motif.FraisInscription,
                    DemandeAdmissionId = paiement.DemandeAdmissionId,
                    Modepaiement = paiement.Modepaiement,
                    Montant = paiement.Montant
                };

                _unitOfWork.Paiement.Insert(P);


                Inscription i = new Inscription()

                {
                    Id = new Guid(),
                    CampusId = Guid.Parse(campusId),
                    AddedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    MatriculeId = m.Id,
                };


                _unitOfWork.Inscription.Insert(i);

                m.LibelleMatricule = GenerateMatricule(paiement.DemandeAdmissionId);

                _unitOfWork.Matricule.Insert(m);
                matricule.Id = m.Id;
                paiement.Id = P.Id;
                inscription.Id = i.Id;

                var demande = _unitOfWork.DemandeAdmission.Get(paiement.DemandeAdmissionId);
                {
                    demande.InscriptionId = i.Id;
                    _unitOfWork.DemandeAdmission.Update(demande);
                }
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de l'enregistrement de l'inscription ");
                return false;
            }

        }

        public Inscription GetInscription(Guid? inscription)
        {
            try
            {
                var incript = GetAllInscription.FirstOrDefault(i => i.Id == inscription);
                if (incript != null)
                {
                    return incript;
                }
                _logger.LogError($"l'inscription d'id {inscription} n'existe pas", typeof(ScolariteService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la visualisation de l'inscription de l'Matricule : {inscription}", typeof(ScolariteService));
                return null;
            }
        }


        // détermination du reste à payer pour les frais d'inscription
        public double? ResteFraisInscription(Guid Id, DateTime dateimp)
        {
            double? fraisInscription = 0, somme = 0;
            var demande = _unitOfWork.DemandeAdmission.Get(Id);

            foreach (Paiement? paiement in _unitOfWork.Paiement.GetAll())
            {

                if (paiement.DemandeAdmissionId == demande.Id && paiement.Motif == Motif.FraisInscription && paiement.AddedDate < dateimp)
                {
                    somme += paiement.Montant;
                }
            }

            foreach (FraisInscription? i in GetAllFraisInscription)
            {

                if (i.ClasseId == demande.ClasseId && i.AnneeAcademiqueId == demande.AnneeAcademiqueId)
                    fraisInscription = i.Montant;
                break;
            }
            return fraisInscription - somme;

        }

        // Verification du paiement

        public bool VerifExistInscription(Guid Id)
        {
            var dem = _unitOfWork.DemandeAdmission.Get(Id);
            if (dem.InscriptionId == null)
            {
                return false;

            }

            return true;
        }

        // Verification du statut d'admission d'un candidat
        public bool StatutAdmission(DemandeAdmission model)
        {
            if (model.Decision == Decision.Admis)
            {
                return false;

            }
            else
            {
                return true;
            }
        }
        public List<Inscription> GetInscriptByFilter(Guid? cycle, Guid? specialite, Guid? niveau, DateTime? datedebut, DateTime? datefin)
        {
            List<Inscription> insciptdispo = GetAllInscription;
            List<Inscription> inscriptfiltrer = new List<Inscription>();

            foreach (var inscript in insciptdispo)
            {
                if
                    (
                       ((inscript.AddedDate.Date >= datedebut) && (inscript.AddedDate.Date <= datefin)) ||

                       ((inscript.AddedDate.Date <= datefin) && (datedebut == null || datedebut == DateTime.MinValue)) ||
                       ((inscript.AddedDate.Date >= datedebut) && (datefin == null || datefin == DateTime.MinValue)) ||

                        ((datedebut == null || datedebut == DateTime.MinValue) && (datefin == null || datefin == DateTime.MinValue))

                     )
                {
                    if (
                    ((inscript.DemandeAdmission.Classe.CycleId == cycle) && (inscript.DemandeAdmission.Classe.FiliereId == specialite) && (inscript.DemandeAdmission.Classe.NiveauId == niveau)) ||

                    ((cycle == null || cycle == Guid.Empty) && (specialite == null || specialite == Guid.Empty) && (niveau == null || niveau == Guid.Empty)) ||

                    ((cycle == null || cycle == Guid.Empty) && (specialite == null || specialite == Guid.Empty) && (inscript.DemandeAdmission.Classe.NiveauId == niveau)) ||
                    ((inscript.DemandeAdmission.Classe.CycleId == cycle) && (specialite == null || specialite == Guid.Empty) && (niveau == null || niveau == Guid.Empty)) ||
                    ((cycle == null || cycle == Guid.Empty) && (inscript.DemandeAdmission.Classe.FiliereId == specialite) && (niveau == null || niveau == Guid.Empty)) ||

                    ((cycle == null || cycle == Guid.Empty) && (inscript.DemandeAdmission.Classe.FiliereId == specialite) && (inscript.DemandeAdmission.Classe.NiveauId == niveau)) ||
                    ((inscript.DemandeAdmission.Classe.CycleId == cycle) && (inscript.DemandeAdmission.Classe.FiliereId == specialite) && (niveau == null || niveau == Guid.Empty)) ||
                    ((inscript.DemandeAdmission.Classe.CycleId == cycle) && (specialite == null || specialite == Guid.Empty) && (inscript.DemandeAdmission.Classe.NiveauId == niveau))

                    )
                    {

                        inscriptfiltrer.Add(inscript);

                    }
                }
            }
            return inscriptfiltrer;
        }

        public int TotalInscritsParJour()
        {
            var TotalInscritList = GetAllInscription;
            int TotalInscrit = 0;
            if (TotalInscritList.Count != 0)
            {
                var date = DateTime.Now.Date;
                foreach (var ins in TotalInscritList)
                {

                    if (ins.AddedDate.Date == date)
                    {
                        TotalInscrit++;
                    }
                }
            }
            return TotalInscrit;
        }

        public int[] NbreInscritsAnnuel()
        {

            IEnumerable<AnneeAcademique> AA = _admission.GetAllAnneeAcademique.OrderByDescending(aa => aa.AnneeDebut).Take(10);
            var nbreInscrit = GetAllInscription;
            int[] tabNombreInscrits = new int[10];
            int i = 0;
            //int j = 0;
            if (AA.Count() != 0)
            {
                foreach (AnneeAcademique anneeAcademique in AA)
                {
                    int CompteurInscrit = 0;

                    foreach (Inscription Ins in nbreInscrit)
                    {
                        if (Ins.DemandeAdmission.AnneeAcademiqueId == anneeAcademique.Id)
                        {
                            CompteurInscrit++;
                        }
                    }
                    tabNombreInscrits.SetValue(CompteurInscrit, i);
                    i++;
                    CompteurInscrit = 0;
                }
            }

            return tabNombreInscrits;
        }

        #endregion

        #region FraisInscription

        //Get all FraisInscription
        public List<FraisInscription> GetAllFraisInscription => _unitOfWork.FraisInscription.Values.Include(fi => fi.Classe)
                                                                                                   .Include(fi => fi.AnneeAcademique).ToList();
        //create FraisInscription
        public bool CreateFraisInscription(FraisInscription model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                model.Id = new Guid();
                model.AddedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.FraisInscription.Insert(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la création des frais d'inscription : {model.Montant}", typeof(ScolariteService));
                return false;
            }
        }

        //Update FraisInscription
        public bool UpdateFraisInscription(FraisInscription model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.FraisInscription.Update(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la modificaion des frais d'inscription : {model.Montant}", typeof(ScolariteService));
                return false;
            }
        }

        //delete FraisInscription
        public bool DeleteFraisInscription(FraisInscription model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                _unitOfWork.FraisInscription.Delete(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la supression des fais d'inscription : {model.Montant}", typeof(ScolariteService));
                return false;
            }
        }

        //Get FraisInscription
        public FraisInscription GetFraisInscription(Guid Id)
        {
            try
            {
                var fraisInscription = _unitOfWork.FraisInscription.Get(Id);
                if (fraisInscription != null)
                {
                    return fraisInscription;
                }
                _logger.LogError($"les frais d'inscription d'{Id} n'existe pas", typeof(ScolariteService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la visualisation des frais d'inscription : {Id}", typeof(ScolariteService));
                return null;
            }
        }

        //verification de l'existence des frais d'inscription
        public bool VerifFraisInscription(FraisInscription model)
        {
            FraisInscription? fraisInscription = new FraisInscription();

            fraisInscription = _unitOfWork.FraisInscription.Values.FirstOrDefault(c => c.AnneeAcademiqueId == model.AnneeAcademiqueId && c.ClasseId == model.ClasseId);
            if (fraisInscription == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region FraisMedicaux
        //Get all FraisMedicaux
        public List<FraisMedicaux> GetAllFraisMedicaux => _unitOfWork.FraisMedicaux.Values.Include(fm => fm.Campus)
                                                                                          .Include(fm => fm.AnneeAcademique).ToList();

        //create DeleteFraisMedicaux
        public bool CreateFraisMedicaux(FraisMedicaux model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                model.Id = new Guid();
                model.AddedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.FraisMedicaux.Insert(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la création des frais medicaux : {model.Montant}", typeof(ScolariteService));
                return false;
            }
        }

        //Update FraisMedicaux
        public bool UpdateFraisMedicaux(FraisMedicaux model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.FraisMedicaux.Update(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la modificaion des frais medicaux : {model.Montant}", typeof(ScolariteService));
                return false;
            }
        }

        //delete FraisMedicaux
        public bool DeleteFraisMedicaux(FraisMedicaux model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                _unitOfWork.FraisMedicaux.Delete(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la supression des fais medicaux : {model.Montant}", typeof(ScolariteService));
                return false;
            }
        }

        //Get FraisMedicaux
        public FraisMedicaux GetFraisMedicaux(Guid Id)
        {
            try
            {
                var fraisMedicaux = _unitOfWork.FraisMedicaux.Get(Id);
                if (fraisMedicaux != null)
                {
                    return fraisMedicaux;
                }
                _logger.LogError($"les frais medicaux d'{Id} n'existe pas", typeof(ScolariteService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la visualisation des frais medicaux : {Id}", typeof(ScolariteService));
                return null;
            }
        }

        public double? ResteFraisMedicaux(Guid Id, DateTime dateimp)
        {
            double? fraismedi = 0, somme = 0;
            var demande = _admission.GetDA(Id);

            foreach (Paiement paiement in _admission.GetAllpaiement)
            {

                if (paiement.DemandeAdmissionId == demande.Id && paiement.Motif == Motif.FraisMedicaux && paiement.AddedDate < dateimp)
                {
                    somme += paiement.Montant;
                }
            }

            foreach (FraisMedicaux medi in GetAllFraisMedicaux)
            {

                if (medi.CampusId == demande.Inscription.CampusId && medi.AnneeAcademiqueId == demande.AnneeAcademiqueId)
                    fraismedi = medi.Montant;
                break;
            }
            return fraismedi - somme;
        }
        //verification de l'existence des frais medicaux
        public bool VerifFraisMedicaux(FraisMedicaux model)
        {
            FraisMedicaux? fraisMedicaux = new FraisMedicaux();

            fraisMedicaux = _unitOfWork.FraisMedicaux.Values.FirstOrDefault(c => c.AnneeAcademiqueId == model.AnneeAcademiqueId && c.CampusId == model.CampusId);
            if (fraisMedicaux == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool CreatePaiementfraisMedicaux(Paiement model)
        {
            try
            {

                Paiement P = new Paiement()
                {
                    Id = new Guid(),
                    AddedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    DemandeAdmissionId = model.DemandeAdmissionId,
                    Montant = model.Montant,
                    Modepaiement = model.Modepaiement,
                    Motif = Motif.FraisMedicaux

                };

                _unitOfWork.Paiement.Insert(P);
                model.Id = P.Id;
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors du paiement pour : {model.Motif} du {DateTime.Now}", typeof(ScolariteService));
                return false;
            }

        }
        public bool VerifPaiementFraisMedicaux(Paiement model, string campus)
        {
            double? somme = 0;
            foreach (FraisMedicaux paiement in GetAllFraisMedicaux)
            {

                if (paiement.AnneeAcademiqueId == model.DemandeAdmission.AnneeAcademiqueId && paiement.CampusId.ToString() == campus)
                {
                    do
                    {
                        somme += paiement.Montant;
                    }
                    while (somme < paiement.Montant);
                    if (somme > paiement.Montant)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region FraisDossierExamen
        public List<FraisDossierExamen> GetAllFraisDossierExamen => _unitOfWork.FraisDossierExamen.Values
                                                                                                           .Include(fde => fde.Classe)
                                                                                                              .ThenInclude(fde => fde.Niveau)
                                                                                                           .Include(fde => fde.Classe)
                                                                                                              .ThenInclude(fde => fde.Filiere)
                                                                                                           .Include(fde => fde.Classe)
                                                                                                              .ThenInclude(fde => fde.Cycle)
                                                                                                           .Include(aa => aa.AnneeAcademique)
                                                                                                           .ToList();

        //create frais FraisDossierExamen
        public bool CreateFraisDossierExamen(FraisDossierExamen model)
        {
            try
            {
                model.Id = new Guid();
                model.AddedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.FraisDossierExamen.Insert(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la création des frais de dossiers d'examen : ", typeof(ScolariteService));
                return false;
            }
        }

        //Update FraisDossierExamen
        public bool UpdateFraisDossierExamen(FraisDossierExamen model)
        {
            try
            {
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.FraisDossierExamen.Update(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la modification des frais de dossiers d'examen", typeof(ScolariteService));
                return false;
            }
        }

        //Get FraisDossierExamen
        public FraisDossierExamen GetFraisDossierExamen(Guid Id)
        {
            try
            {
                var FraisDossierExamen = _unitOfWork.FraisDossierExamen.Get(Id);
                if (FraisDossierExamen != null)
                {
                    return FraisDossierExamen;
                }
                _logger.LogError($"les Frais de dossier d'examen d'{Id} n'existe pas", typeof(ScolariteService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la visualisation des Frais de dossier d'examen : {Id}", typeof(ScolariteService));
                return null;
            }
        }

        //delete FraisDossierExamen
        public bool DeleteFraisDossierExamen(FraisDossierExamen model)
        {
            try
            {
                _unitOfWork.FraisDossierExamen.Delete(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la supression des Frais de Dossier Examen", typeof(ScolariteService));
                return false;
            }
        }

        //verification de l'existence du campus
        public bool VerifFraisDossierExamen(FraisDossierExamen model)
        {
            FraisDossierExamen? FraisDossierExamen = new FraisDossierExamen();

            FraisDossierExamen = _unitOfWork.FraisDossierExamen.Values.FirstOrDefault(c => c.ClasseId == model.ClasseId && c.AnneeAcademiqueId == model.AnneeAcademiqueId);
            if (FraisDossierExamen == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CreatePaiementFraisDossierExamen(Paiement model)
        {
            try
            {

                Paiement P = new Paiement()
                {
                    Id = new Guid(),
                    AddedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    DemandeAdmissionId = model.DemandeAdmissionId,
                    Montant = model.Montant,
                    Modepaiement = model.Modepaiement,
                    Motif = Motif.FraisDossierExamen

                };

                _unitOfWork.Paiement.Insert(P);
                model.Id = P.Id;
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors du paiement pour : {model.Motif} du {DateTime.Now}", typeof(ScolariteService));
                return false;
            }

        }

        public bool VerifPaiement(FraisDossierExamen model)
        {
            double? somme = 0;
            foreach (FraisDossierExamen paiement in GetAllFraisDossierExamen)
            {

                if (paiement.ClasseId == model.ClasseId)
                {
                    do
                    {
                        somme += paiement.Montant;
                    }
                    while (somme < paiement.Montant);
                    if (somme > paiement.Montant)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // détermination du reste à payer pour les frais d'examen
        public double? ResteFraisExamen(Guid Id, DateTime dateimp)
        {
            double? fraisExam = 0, somme = 0;
            var demande = _admission.GetDA(Id);

            foreach (Paiement paiement in _admission.GetAllpaiement)
            {

                if (paiement.DemandeAdmissionId == demande.Id && paiement.Motif == Motif.FraisDossierExamen && paiement.AddedDate < dateimp)
                {
                    somme += paiement.Montant;
                }
            }

            foreach (FraisDossierExamen exam in GetAllFraisDossierExamen)
            {

                if (exam.ClasseId == demande.ClasseId && exam.AnneeAcademiqueId == demande.AnneeAcademiqueId)
                    fraisExam = exam.Montant;
                break;
            }
            return fraisExam - somme;

        }

        #endregion

        #region FraisSoutenance
        public List<FraisSoutenance> GetAllFraisSoutenance => _unitOfWork.FraisSoutenance.Values
                                                                                                         .Include(fde => fde.Classe)
                                                                                                            .ThenInclude(fde => fde.Niveau)
                                                                                                         .Include(fde => fde.Classe)
                                                                                                            .ThenInclude(fde => fde.Filiere)
                                                                                                         .Include(fde => fde.Classe)
                                                                                                            .ThenInclude(fde => fde.Cycle)
                                                                                                         .Include(aa => aa.AnneeAcademique)
                                                                                                         .ToList();

        //create FraisSoutenance
        public bool CreateFraisSoutenance(FraisSoutenance model)
        {
            try
            {
                model.Id = new Guid();
                model.AddedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.FraisSoutenance.Insert(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la création des frais de soutenance : ", typeof(ScolariteService));
                return false;
            }
        }

        //Update FraisSoutenance
        public bool UpdateFraisSoutenance(FraisSoutenance model)
        {
            try
            {
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.FraisSoutenance.Update(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la modification des frais soutenance", typeof(ScolariteService));
                return false;
            }
        }

        //delete FraisSoutenance
        public bool DeleteFraisSoutenance(FraisSoutenance model)
        {
            try
            {
                _unitOfWork.FraisSoutenance.Delete(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la supression des frais de soutenance", typeof(ScolariteService));
                return false;
            }
        }

        //Get FraisSoutenance
        public FraisSoutenance GetFraisSoutenance(Guid Id)
        {
            try
            {
                var fraisSoutenance = _unitOfWork.FraisSoutenance.Get(Id);
                if (fraisSoutenance != null)
                {
                    return fraisSoutenance;
                }
                _logger.LogError($"les frais de soutenance d'{Id} n'existe pas", typeof(ScolariteService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la visualisation des  frais de soutenance : {Id}", typeof(ScolariteService));
                return null;
            }
        }

        //verification de l'existence FraisSoutenance
        public bool VerifFraisSoutenance(FraisSoutenance model)
        {
            FraisSoutenance? fraisSoutenance = new FraisSoutenance();

            fraisSoutenance = _unitOfWork.FraisSoutenance.Values.FirstOrDefault(c => c.ClasseId == model.ClasseId && c.AnneeAcademiqueId == model.AnneeAcademiqueId);

            if (fraisSoutenance == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public List<FraisSoutenance> GetAllFraisSout => _unitOfWork.FraisSoutenance.Values
                                                     .Include(t => t.AnneeAcademique)
                                                     .Include(t => t.Classe)
                                                         .ThenInclude(cl => cl.Cycle)
                                                     .Include(t => t.Classe)
                                                         .ThenInclude(cl => cl.Filiere)
                                                     .Include(t => t.Classe)
                                                         .ThenInclude(cl => cl.Niveau)
                                                     .ToList();

        // verification du montant et retour du reste a payer pour la soutenance

        public double? ResteFraisSout(Guid inscription, DateTime dateimp)
        {
            double? fraissout = 0, somme = 0;
            var inscript = GetInscription(inscription);

            foreach (Paiement paiement in _admission.GetAllpaiement)
            {

                if (paiement.DemandeAdmission.InscriptionId == inscript.Id && paiement.Motif == Motif.FraisSoutenance && paiement.AddedDate < dateimp)
                {
                    somme += paiement.Montant;
                }
            }

            foreach (FraisSoutenance sout in GetAllFraisSout)
            {

                if (sout.ClasseId == inscript.DemandeAdmission.ClasseId && sout.AnneeAcademiqueId == inscript.DemandeAdmission.AnneeAcademiqueId)
                {
                    fraissout = sout.Montant;
                    break;
                }
            }
            return fraissout - somme;
        }

        #endregion

        #region FraisScolarite
        public List<TrancheScolarite> GetAllTrancheScolarite => _unitOfWork.TrancheScolarite.Values
                                                                                           .Include(t => t.Classe)
                                                                                                .ThenInclude(cl => cl.Niveau)
                                                                                           .Include(t => t.Classe)
                                                                                                .ThenInclude(sp => sp.Filiere)
                                                                                           .Include(t => t.Classe)
                                                                                                .ThenInclude(sp => sp.Cycle)
                                                                                           .Include(e => e.Campus)
                                                                                                .ThenInclude(e => e.Ipes)
                                                                                           .Include(e => e.AnneeAcademique)
                                                                                           .ToList();

        //create TrancheScolarite
        public bool CreateTrancheScolarite(TrancheScolarite model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                model.Id = new Guid();
                model.AddedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.TrancheScolarite.Insert(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la création des tranches de scolarite: ", typeof(ScolariteService));
                return false;
            }
        }

        //Update TrancheScolarite
        public bool UpdateTrancheScolarite(TrancheScolarite model)
        {
            try
            {
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.TrancheScolarite.Update(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la Modification des tranches de scolarite : ", typeof(ScolariteService));
                return false;
            }
        }

        //delete TrancheScolarite
        public bool DeleteTrancheScolarite(TrancheScolarite model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                _unitOfWork.TrancheScolarite.Delete(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} chec lors de la Modification des tranches de scolarite", typeof(ScolariteService));
                return false;
            }
        }

        //Get TrancheScolarite
        public TrancheScolarite GetTrancheScolarite(Guid Id)
        {
            try
            {
                var TrancheScolarite = _unitOfWork.TrancheScolarite.Get(Id);
                if (TrancheScolarite != null)
                {
                    return TrancheScolarite;
                }
                _logger.LogError($"les tranches d'inscription d'{Id} n'existe pas", typeof(ScolariteService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} echec lors de la Visualisation des tranches d'inscription : {Id}", typeof(ScolariteService));
                return null;
            }
        }

        //verification de l'existence TrancheScolarite
        public bool VerifTrancheScolarite(TrancheScolarite model)
        {
            TrancheScolarite? TrancheScolarite = new TrancheScolarite();

            TrancheScolarite = _unitOfWork.TrancheScolarite.Values.FirstOrDefault(c => c.ClasseId == model.ClasseId
                                                                                    && c.LibelleTranche == model.LibelleTranche
                                                                                    && c.AnneeAcademiqueId == model.AnneeAcademiqueId);
            if (TrancheScolarite == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //Verification de l'existence d'une inscription pour un Matricule

        public double? VerifExistInscript(Guid inscription)
        {
            double? rapi = 0, som = 0; //rapi = reste a payer pour l'inscription
                                       //var inscript = _unitOfWork.Inscription.Values.FirstOrDefault(i => i.MatriculeId == Matricule);
            var inscript = GetInscription(inscription);

            if (inscript != null)
            {
                foreach (Paiement paiement in _admission.GetAllpaiement)
                {
                    if (paiement.DemandeAdmission.InscriptionId == inscript.Id && paiement.Motif == Motif.FraisInscription)
                    {
                        som += paiement.Montant;
                    }
                }
                var fraisinscript = _unitOfWork.FraisInscription.Values.FirstOrDefault(i => i.ClasseId == inscript.DemandeAdmission.ClasseId && i.AnneeAcademiqueId == inscript.DemandeAdmission.AnneeAcademiqueId);
                rapi = fraisinscript.Montant - som;
            }
            return rapi;
        }

        // Determination du montant restant pour combler la scolarite
        public double? ResteScolarite(Guid inscription, string campus, DateTime dateimp)
        {
            double? ms = 0, somme = 0, bourse = 0;
            var inscript = GetInscription(inscription);
            foreach (Paiement paiement in _admission.GetAllpaiement)
            {

                if (paiement.DemandeAdmission.InscriptionId == inscript.Id && paiement.Motif == Motif.FraisScolarite && paiement.AddedDate < dateimp)
                {
                    somme += paiement.Montant;
                }
            }
            foreach (TrancheScolarite tranche in GetAllTrancheScolarite)
            {

                if (tranche.ClasseId == inscript.DemandeAdmission.ClasseId && tranche.AnneeAcademiqueId == inscript.DemandeAdmission.AnneeAcademiqueId && tranche.CampusId.ToString() == campus)
                {
                    ms += tranche.Montant;
                }
            }
            foreach (Bourse b in GetAllBourses)
            {
                if (b.InscriptionId == inscription && b.Statut == Statut.accepte && b.ModifiedDate <= dateimp.Date)
                {
                    bourse = bourse + b.Montant;
                }
            }

            return ms - somme - bourse;
        }

        #endregion

        #region Bourse
        public bool CreateBourse(Bourse model)
        {
            try
            {
                model.Id = new Guid();
                model.Statut = Statut.EnAttente;
                model.AddedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.Bourse.Insert(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de l'initiation de la réduction de la scolarité: ", typeof(ScolariteService));
                return false;
            }
        }
        public List<Bourse> GetAllBourses => _unitOfWork.Bourse.Values.Include(i => i.Inscription)
                                                                            .ThenInclude(m => m.Matricule)
                                                                       .Include(i => i.Inscription)
                                                                            .ThenInclude(da => da.DemandeAdmission)
                                                                                .ThenInclude(c => c.Candidat)
                                                                                    .ThenInclude(p => p.Personne)

                                                                        .ToList();

        //Get Bourse
        public Bourse GetBourse(Guid Id)
        {
            try
            {
                var bourse = _unitOfWork.Bourse.Get(Id);
                if (bourse != null)
                {
                    return bourse;
                }
                _logger.LogError($"la bourse d'{Id} n'existe pas", typeof(ScolariteService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la visualisation de la bourse : {Id}", typeof(ScolariteService));
                return null;
            }
        }

        //Update bourse
        public bool UpdateBourse(Bourse model)
        {
            try
            {
                model.ModifiedDate = DateTime.Now;
                model.Statut = Statut.EnAttente;
                _unitOfWork.Bourse.Update(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de l'édition de la bourse");
                return false;
            }
        }
        // Changer le statut de la bourse
        public bool ValidateBourse(Bourse model)
        {
            try
            {
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.Bourse.Update(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la validation de la bourse");
                return false;
            }
        }

        #endregion

        #region Changements
        public List<ChangementFiliereOrCampus> GetAllchangementCampus => _unitOfWork.ChangementFiliereOrCampus.Values
                                                                                                   .Include(Cc => Cc.Inscription)
                                                                                                      .ThenInclude(ins => ins.Campus)
                                                                                                   .Include(cc => cc.Inscription)
                                                                                                      .ThenInclude(da => da.DemandeAdmission)
                                                                                                          .ThenInclude(ca => ca.Candidat)
                                                                                                                .ThenInclude(p => p.Personne)
                                                                                                    .Include(i => i.Inscription)
                                                                                                      .ThenInclude(m => m.Matricule)
                                                                                                   .ToList();
        public ChangementFiliereOrCampus Getchangcampus(Guid Id)
        {
            try
            {
                var changcampus = _unitOfWork.ChangementFiliereOrCampus.Get(Id);
                if (changcampus != null)
                {
                    return changcampus;
                }
                _logger.LogError($"le changement de campus {Id} n'existe pas", typeof(ScolariteService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la visualisation du changement de campus : {Id}", typeof(ScolariteService));
                return null;
            }
        }
        public bool CreateChangementCampus(ChangementFiliereOrCampus model)
        {
            try
            {
                Inscription ins = GetInscription(model.InscriptionId);
                ChangementFiliereOrCampus chang = new ChangementFiliereOrCampus()
                {
                    Id = new Guid(),
                    AddedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    NextCampus = model.NextCampus,
                    InscriptionId = ins.Id,
                    PastCampus = ins.CampusId.ToString(),
                    Statut = Statut.EnAttente,
                    MotifChangement = model.MotifChangement,
                };
                _unitOfWork.ChangementFiliereOrCampus.Insert(chang);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors du changement du campus", typeof(ScolariteService));
                return false;
            }
        }
        public bool UpdateChangementCampus(ChangementFiliereOrCampus model)
        {
            try
            {
                model.ModifiedDate = DateTime.Now;
                model.Statut = Statut.EnAttente;
                _unitOfWork.ChangementFiliereOrCampus.Update(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de l'édition du changement");
                return false;
            }
        }
        public bool ValidateChangementCampus(ChangementFiliereOrCampus model)
        {
            try
            {
                model.ModifiedDate = DateTime.Now;

                if (model.Statut == Statut.accepte)
                {
                    Inscription ins = GetInscription(model.InscriptionId);

                    Inscription inscription = new Inscription()
                    {
                        Id = model.InscriptionId,
                        CampusId = Guid.Parse(model.NextCampus),
                        ModifiedDate = model.AddedDate,
                        MatriculeId = ins.MatriculeId,
                        AddedDate = ins.AddedDate

                    };
                    _unitOfWork.Inscription.Update(inscription);
                }

                _unitOfWork.ChangementFiliereOrCampus.Update(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la mise à jour de la décision :", typeof(ScolariteService));
                return false;
            }
        }
        public bool VerifChangementCampus(ChangementFiliereOrCampus model)
        {
            ChangementFiliereOrCampus? chang = new ChangementFiliereOrCampus();

            chang = _unitOfWork.ChangementFiliereOrCampus.Values.FirstOrDefault(c => c.InscriptionId == model.InscriptionId && c.PastCampus == model.PastCampus && c.NextCampus == model.NextCampus);
            if (chang == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public List<ChangementFiliereOrCampus> GetAllChangementFiliere => _unitOfWork.ChangementFiliereOrCampus.Values
                                       .Include(i => i.Inscription)
                                            .ThenInclude(i => i.DemandeAdmission)
                                                .ThenInclude(i => i.Candidat)
                                                    .ThenInclude(i => i.Personne)
                                       .Include(cc => cc.Inscription)
                                            .ThenInclude(da => da.DemandeAdmission)
                                                   .ThenInclude(ca => ca.Candidat)
                                                         .ThenInclude(p => p.Personne)
                                       .Include(i => i.Inscription)
                                              .ThenInclude(m => m.Matricule)
                                        .ToList();
        public bool CreateChangementFiliere(ChangementFiliereOrCampus model)
        {
            try
            {
                Inscription inscription = GetInscription(model.InscriptionId);
                DemandeAdmission demande = _admission.GetDA(model.Inscription.DemandeAdmission.Id);
                Classe classe = _param.GetAllClasse.FirstOrDefault(cl => cl.FiliereId == Guid.Parse(model.NextFiliere) && cl.NiveauId == demande.Classe.NiveauId);
                //model.Inscription = inscription;
                if (classe != null)
                {

                    ChangementFiliereOrCampus ChangeFiliere = new ChangementFiliereOrCampus()
                    {
                        Id = new Guid(),
                        AddedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        Statut = Statut.accepte,
                        MotifChangement = model.MotifChangement,
                        InscriptionId = inscription.Id,
                        NextFiliere = model.NextFiliere,
                        PastFiliere = demande.Classe.FiliereId.ToString()

                    };
                    _unitOfWork.ChangementFiliereOrCampus.Insert(ChangeFiliere);

                    DemandeAdmission DA = new DemandeAdmission()
                    {
                        ModifiedDate = DateTime.Now,
                        CandidatId = demande.CandidatId,
                        AnneeAcademiqueId = demande.AnneeAcademiqueId,
                        ClasseId = classe.Id,
                        Decision = demande.Decision,
                        Id = demande.Id,
                        InscriptionId = model.InscriptionId,
                        TypeAdmission = demande.TypeAdmission

                    };
                    _unitOfWork.DemandeAdmission.Update(DA);
                    model.Id = ChangeFiliere.Id;
                    _unitOfWork.Save();
                    _unitOfWork.Commit();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors du changement de filiere ", typeof(AdmissionService));
                return false;
            }
        }

        #endregion

        #region recherche
        public IEnumerable<Inscription> Recherche(string term, string campus = null)
        {
            try
            {
                term = string.IsNullOrEmpty(term) ? "@" : term.ToLower();
                var etudiant = (from emp in GetAllInscription
                                where term == "" || emp.DemandeAdmission.Candidat.Personne.Nom.ToLower().StartsWith(term) || emp.Matricule.LibelleMatricule.ToLower().StartsWith(term)
                                where emp.CampusId.ToString() == campus
                                select new Inscription
                                {
                                    Id = emp.Id,
                                    Matricule = emp.Matricule,
                                    AddedDate = emp.AddedDate,
                                    ModifiedDate = emp.ModifiedDate,
                                    DemandeAdmission = emp.DemandeAdmission

                                }
                            );
                var empdata = etudiant;
                return (empdata);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la visualisation de l'etudiant", typeof(AdmissionService));
                return null;
            }
        }

        #endregion
    }
}