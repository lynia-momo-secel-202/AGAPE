using GestAgape.Core.Entities.Admission;
using GestAgape.Core.Entities.Identity;
using GestAgape.Core.Entities.Parametrage;
using GestAgape.Core.Entities.Scolarite;
using GestAgape.Core.ViewModels;
using GestAgape.Infrastructure.Utilities;
using GestAgape.Models;
using GestAgape.Service.Parametrages;
using GestAgape.Service.Scolarite;
using GestAgape.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Service.Admissions
{
    public class AdmissionService : IAdmission
    {

        #region membres prives
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        protected readonly ILogger _logger;
        protected readonly IParametrage _param;
        private readonly IWebHostEnvironment _hostingEnv;
        private IdentityContext _context;


        #endregion

        #region constructeur
        public AdmissionService(
            IEmailSender emailSender,
            IUnitOfWork unitOfWork,
            ILoggerFactory loggerFactory,
            IParametrage param,
            IWebHostEnvironment hostingEnv,
            IdentityContext context

        )
        {
            _emailSender = emailSender; ;
            _unitOfWork = unitOfWork;
            _logger = loggerFactory.CreateLogger("logs");
            _param = param;
            _hostingEnv = hostingEnv;
            _context = context;
        }
        #endregion

        #region Annee Academique

        public List<AnneeAcademique> GetAllAnneeAcademique => _unitOfWork.AnneeAcademique.Values.ToList();

        //create Annee academique
        public bool CreateAnneeAcademique(AnneeAcademique model)
        {
            try
            {

                model.Id = new Guid();
                model.AddedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                int anneefin = Int32.Parse(model.AnneeDebut) + 1;
                model.AnneeFin = anneefin.ToString();
                _unitOfWork.AnneeAcademique.Insert(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la création de l'année académique : {model.AnneeDebut}", typeof(AdmissionService));
                return false;
            }
        }

        //update Année académique
        public bool UpdateAnneeAcademique(AnneeAcademique model)
        {
            try
            {
                int anneefin = Int32.Parse(model.AnneeDebut) + 1;
                model.AnneeFin = anneefin.ToString();
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.AnneeAcademique.Update(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de l'edition de l'année académique : {model.AnneeDebut}", typeof(AdmissionService));
                return false;
            }
        }

        //Get Année académique
        public AnneeAcademique GetAnneeAcademique(Guid Id)
        {
            try
            {
                var departement = _unitOfWork.AnneeAcademique.Get(Id);
                if (departement != null)
                {
                    return (departement);
                }
                _logger.LogError($"l'année académique d'{Id} n'existe pas", typeof(AdmissionService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la visualisation de l'année académique : {Id}", typeof(AdmissionService));
                return null;
            }
        }

        //delete Année académique
        public bool DeleteAnneeAcademique(AnneeAcademique model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                _unitOfWork.AnneeAcademique.Delete(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la supression de l'année académique : {model.AnneeDebut}", typeof(AdmissionService));
                return false;
            }
        }

        //Verification de l'existance del' AnneeAcademique
        public bool VerifExistAnneeAcademique(string Anneedebut)
        {
            var annee = _unitOfWork.AnneeAcademique.Values.FirstOrDefault(a => a.AnneeDebut == Anneedebut);
            if (annee == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string[] LastAA()
        {
            var lastYears = GetAllAnneeAcademique.OrderByDescending(aa => aa.AnneeDebut).Take(10);
            AnneeAcademique? val;
            string[] tab = new string[10];
            int n = lastYears.Count();
            for (int i = 0; i < n; i++)
            {
                val = lastYears.ElementAt(i);
                tab[i] = val.AnneeDebut + "-" + val.AnneeFin;
            }
            return tab;
        }
        #endregion

        #region FraisConcours

        //Get all FraisConcoursm
        public List<FraisConcours> GetAllFraisConcours => _unitOfWork.FraisConcours.Values.Include(fc => fc.Cycle)
                                                                                          .Include(fc => fc.AnneeAcademique).ToList();

        //create FraisConcours
        public bool CreateFraisConcours(FraisConcours model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                model.Id = new Guid();
                model.AddedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.FraisConcours.Insert(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la création des frais de concours : {model.Montant}", typeof(AdmissionService));
                return false;
            }
        }

        //Update FraisConcours
        public bool UpdateFraisConcours(FraisConcours model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.FraisConcours.Update(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la modificaion des frais de concours : {model.Montant}", typeof(AdmissionService));
                return false;
            }
        }

        //delete FraisConcours
        public bool DeleteFraisConcours(FraisConcours model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                _unitOfWork.FraisConcours.Delete(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la supression des fais de concours : {model.Montant}", typeof(AdmissionService));
                return false;
            }
        }

        //Get concours
        public FraisConcours GetFraisConcours(Guid Id)
        {
            try
            {
                var fraisConcours = _unitOfWork.FraisConcours.Get(Id);
                if (fraisConcours != null)
                {
                    return fraisConcours;
                }
                _logger.LogError($"les frais de concours d'{Id} n'existe pas", typeof(AdmissionService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la visualisation des frais de concours : {Id}", typeof(AdmissionService));
                return null;
            }
        }

        //verification de l'existence des frais de concours
        public bool VerifFraisConcours(FraisConcours model)
        {
            FraisConcours? fraisConcours = new FraisConcours();

            fraisConcours = _unitOfWork.FraisConcours.Values.FirstOrDefault(c => c.AnneeAcademiqueId == model.AnneeAcademiqueId && c.CycleId == model.CycleId);
            if (fraisConcours == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region FraisEtudeDossier

        //Get all FraisEtudeDossier
        public List<FraisEtudeDossier> GetAllFraisEtudeDossier => _unitOfWork.FraisEtudeDossier.Values.Include(fed => fed.Classe)
                                                                                          .Include(fed => fed.AnneeAcademique).ToList();



        //create FraisConcours
        public bool CreateFraisEtudeDossier(FraisEtudeDossier model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                model.Id = new Guid();
                model.AddedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.FraisEtudeDossier.Insert(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la création des frais d'étude de dossier : {model.Montant}", typeof(AdmissionService));
                return false;
            }
        }

        //Update FraisEtudeDossier
        public bool UpdateFraisEtudeDossier(FraisEtudeDossier model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.FraisEtudeDossier.Update(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la modificaion des frais d'étude de dossier : {model.Montant}", typeof(AdmissionService));
                return false;
            }
        }

        //delete FraisEtudeDossier
        public bool DeleteFraisEtudeDossier(FraisEtudeDossier model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                _unitOfWork.FraisEtudeDossier.Delete(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la supression des fais d'étude de dossier : {model.Montant}", typeof(AdmissionService));
                return false;
            }
        }

        //Get FraisEtudeDossier
        public FraisEtudeDossier GetFraisEtudeDossier(Guid Id)
        {
            try
            {
                var fraisEtudeDossier = _unitOfWork.FraisEtudeDossier.Get(Id);
                if (fraisEtudeDossier != null)
                {
                    return fraisEtudeDossier;
                }
                _logger.LogError($"les frais d'étude de dossier d'{Id} n'existe pas", typeof(AdmissionService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la visualisation des frais d'étude de dossier : {Id}", typeof(AdmissionService));
                return null;
            }
        }

        //verification de l'existence des frais d'étude de dossier
        public bool VerifFraisEtudeDossier(FraisEtudeDossier model)
        {
            FraisEtudeDossier? fraisEtudeDossier = new FraisEtudeDossier();

            fraisEtudeDossier = _unitOfWork.FraisEtudeDossier.Values.FirstOrDefault(fde => fde.AnneeAcademiqueId == model.AnneeAcademiqueId && fde.ClasseId == model.ClasseId);
            if (fraisEtudeDossier == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region Concours
        public List<Concours> GetAllConcours => _unitOfWork.Concours.Values.ToList();

        //create Concours
        public bool CreateConcours(ConcoursVM model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                Concours concours = new Concours()
                {
                    Id = new Guid(),
                    Libelle = model.Libelle,
                    Date = model.Date,
                    HeureDebut = model.HeureDebut,
                    HeureFin = model.HeureFin,
                    Description = model.Description,
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,

                };

                string[] nom = (model.Libelle).Split(" ");
                string nomcorect = string.Join("_", nom);

                if (model.Flyers != null)
                {
                    concours.Flyers = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.Flyers, FileType.Image, $"Flyers_{nomcorect}", GestAgapeUtilitiesFunctions.LogoImageFolder);
                }
                if (model.Resultats != null)
                {
                    concours.Resultats = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.Resultats, FileType.Document, $"Résultats_{nomcorect}", GestAgapeUtilitiesFunctions.LogoImageFolder);

                }
                _unitOfWork.Concours.Insert(concours);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la création du concours : {model.Libelle}", typeof(AdmissionService));
                return false;
            }
        }

        //update Concours
        public bool UpdateConcours(ConcoursVM model)
        {
            try
            {

                Concours concours = new Concours()
                {
                    HeureDebut = model.HeureDebut,
                    HeureFin = model.HeureFin,
                    Description = model.Description,
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Date = model.Date,
                    Libelle = model.Libelle,
                    Id = model.ConcoursId

                };


                string[] nom = (model.Libelle).Split(" ");
                string nomcorect = string.Join("_", nom);

                if (model.Flyers != null)
                {
                    concours.Flyers = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.Flyers, FileType.Image, $"Flyers_{nomcorect}", GestAgapeUtilitiesFunctions.LogoImageFolder);
                }
                else
                {
                    concours.Flyers = model.FlyersPath;
                }

                if (model.Resultats != null)
                {
                    concours.Resultats = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.Resultats, FileType.Document, $"Résultats_{nomcorect}", GestAgapeUtilitiesFunctions.LogoImageFolder);

                }
                else
                {
                    concours.Resultats = model.ResultatsPath;
                }

                _unitOfWork.Concours.Update(concours);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de l'edition du concours : {model.Libelle}", typeof(AdmissionService));
                return false;
            }
        }

        //Get Concours
        public Concours GetConcours(Guid Id)
        {
            try
            {
                var concours = _unitOfWork.Concours.Get(Id);
                if (concours != null)
                {
                    return (concours);
                }
                _logger.LogError($"le concours d'{Id} n'existe pas", typeof(AdmissionService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la visualisation du concours : {Id}", typeof(AdmissionService));
                return null;
            }
        }

        public ConcoursVM GetConcoursVM(Guid Id)
        {
            try
            {
                var model = _unitOfWork.Concours.Get(Id);
                if (model != null)
                {
                    ConcoursVM Cvm = new ConcoursVM()
                    {
                        HeureDebut = model.HeureDebut,
                        HeureFin = model.HeureFin,
                        Description = model.Description,
                        Date = model.Date,
                        FlyersPath = model.Flyers,
                        ResultatsPath = model.Resultats,
                        ConcoursId = model.Id,
                        Libelle = model.Libelle,
                    };



                    return (Cvm);
                }
                _logger.LogError($"le concours d'{Id} n'existe pas", typeof(AdmissionService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la visualisation du concours : {Id}", typeof(AdmissionService));
                return null;
            }
        }

        //delete Concours
        public bool DeleteConcours(Concours model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                _unitOfWork.Concours.Delete(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la supression du concours : {model.Libelle}", typeof(AdmissionService));
                return false;
            }
        }

        // verification de l'existance du concours
        public bool VerifConcours(ConcoursVM model)
        {
            foreach (Concours concour in GetAllConcours)
            {

                if (concour.Libelle == model.Libelle && concour.Date == model.Date)
                {
                    return true;
                }
            }
            return false;
        }

        // Nombre de jours restant avant le prochain concours

        public int NombreJourNextConcours()
        {
            var dernierConcours = GetAllConcours.LastOrDefault();
            var dateDuJourActuel = DateTime.Now.DayOfYear;
            int resultat = 0;

            if (dernierConcours != null)
            {
                var dateDernierConcours = DateTime.Parse(dernierConcours.Date);

                resultat = dateDernierConcours.DayOfYear - dateDuJourActuel;

            }

            return resultat;
        }

        // Nombre d'étudiants admis au dernier concours
        public int TotalAdmisLastConcours()
        {
            var dernierConcours = GetAllConcours.LastOrDefault();
            var dateDuJourActuel = DateTime.Now.DayOfYear;
            int resultat = 0;

            if (dernierConcours != null)
            {
                var dateDernierConcours = DateTime.Parse(dernierConcours.Date);

                if (dateDernierConcours < DateTime.Now)
                {
                    foreach (DemandeAdmission d in GetAllDA)
                    {
                        if ((d.TypeAdmission == TypeAdmission.Concours) && (d.ConcoursId == dernierConcours.Id) && (d.Decision == Decision.Admis))
                        {
                            resultat++;
                        }
                    }
                }

            }

            return resultat;
        }
        #endregion

        #region candidat
        public string GenerateCodeCandidat()
        {
            int num = 0001;
            string code = "CA23IME" + num;

            foreach (Candidat? cnd in _unitOfWork.Candidat.GetAll())
            {
                {
                    num = num + 1;
                }
                code = "CA" + DateTime.Now.ToString("yy") + "IME" + num.ToString();
            }
            return code;
        }
        public List<Candidat> GetAllCandidat => _unitOfWork.Candidat.Values
                                                              .Include(c => c.Personne)
                                                              .Include(c => c.DossierPersonnel)
                                                              .ToList();

        //create Candidat
        public bool CreateCandidat(CandidatVM model)
        {
            try
            {

                Personne personne = new Personne()
                {
                    Id = new Guid(),
                    AddedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    StatutMatrimonial = model.StatutMatrimonial,
                    Nationalite = model.Nationalite,
                    Region = model.Region,
                    Langue = model.Langue,
                    Handicape = model.Handicape,
                    Sexe = model.Sexe,
                    Email = model.Email,
                    Nom = model.Nom,
                    Prenom = model.Prenom,
                    Telephone = model.Telephone,
                    DateNaissance = model.DateNaissance,
                    LieuNaissance = model.LieuNaissance,
                };
                if (model.PhotoFile != null)
                {
                    personne.Photo = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.PhotoFile, FileType.Image, $"{model.Email}_{model.Nom}", GestAgapeUtilitiesFunctions.dossier);
                }
                if (model.CurriculumVitaeFile != null)
                {
                    personne.CurriculumVitae = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.CurriculumVitaeFile, FileType.Document, $"CV_{model.Email}_{model.Nom}", GestAgapeUtilitiesFunctions.dossier);
                }
                _unitOfWork.Personne.Insert(personne);

                DossierPersonnel dossier = new DossierPersonnel()
                {
                    Id = new Guid(),
                    Photos = personne.Photo,
                    AddedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                };
                _unitOfWork.DossierPersonnel.Insert(dossier);
                Candidat candidat = new Candidat()
                {
                    Id = new Guid(),
                    AddedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Code = GenerateCodeCandidat(),
                    NomPere = model.NomPere,
                    TelephonePere = model.TelephonePere,
                    TelephoneMere = model.TelephoneMere,
                    NomMere = model.NomMere,
                    ProfessionMere = model.ProfessionMere,
                    ProfessionPere = model.ProfessionPere,
                    Vision = model.Vision,
                    Quartier = model.Quartier,
                    Etablissement = model.Etablissement,
                    PersonneId = personne.Id,
                    DossierPersonnelId = dossier.Id,
                };
                _unitOfWork.Candidat.Insert(candidat);
                model.CandidatId = candidat.Id;
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la création du Candidat : {model.Nom}", typeof(AdmissionService));
                return false;
            }
        }

        //update Candidat
        public bool UpdateCandidat(CandidatVM model)
        {
            try
            {

                Personne personne = new Personne()
                {
                    Id = model.PersonneId,
                    ModifiedDate = DateTime.UtcNow,
                    StatutMatrimonial = model.StatutMatrimonial,
                    Nationalite = model.Nationalite,
                    Region = model.Region,
                    Langue = model.Langue,
                    Handicape = model.StatutMatrimonial,
                    Sexe = model.Sexe,
                    Photo = model.Photo,
                    Email = model.Email,
                    Nom = model.Nom,
                    Prenom = model.Prenom,
                    Telephone = model.Telephone,
                    DateNaissance = model.DateNaissance,
                    LieuNaissance = model.LieuNaissance,
                    CurriculumVitae = model.CurriculumVitae,

                };
                if (model.PhotoFile != null)
                {
                    personne.Photo = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.PhotoFile, FileType.Image, $"{model.Email}_{model.Nom}", GestAgapeUtilitiesFunctions.dossier);
                }
                if (model.CurriculumVitaeFile != null)
                {
                    personne.CurriculumVitae = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.CurriculumVitaeFile, FileType.Document, $"CV_{model.Email}_{model.Nom}", GestAgapeUtilitiesFunctions.dossier);
                }
                _unitOfWork.Personne.Update(personne);
                _unitOfWork.Save();

                Candidat candidat = new Candidat()
                {
                    Id = model.CandidatId,
                    ModifiedDate = DateTime.UtcNow,
                    Code = model.Code,
                    NomPere = model.NomPere,
                    TelephonePere = model.TelephonePere,
                    TelephoneMere = model.TelephoneMere,
                    NomMere = model.NomMere,
                    ProfessionMere = model.ProfessionMere,
                    ProfessionPere = model.ProfessionPere,
                    Vision = model.Vision,
                    Quartier = model.Quartier,
                    Etablissement = model.Etablissement,
                    PersonneId = personne.Id,
                    DossierPersonnelId = model.DossierPersonnelId


                };
                _unitOfWork.Candidat.Update(candidat);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de l'édition du candidat : {model.Nom}", typeof(AdmissionService));
                return false;
            }
        }

        //delete Candidat
        public bool DeleteCandidat(Candidat model)
        {
            try
            {
                _unitOfWork.Candidat.Delete(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la supression du Candidat : {model.Personne.Nom}", typeof(AdmissionService));
                return false;
            }
        }

        //Get CandidatVM 
        public CandidatVM GetCandidatVM(Guid Id)
        {
            try
            {
                var candidat = GetAllCandidat.FirstOrDefault(e => e.Id == Id);
                if (candidat != null)
                {
                    CandidatVM candidatVM = new CandidatVM
                    {
                        Code = candidat.Code,
                        NomPere = candidat.NomPere,
                        TelephonePere = candidat.TelephonePere,
                        TelephoneMere = candidat.TelephoneMere,
                        NomMere = candidat.NomMere,
                        ProfessionMere = candidat.ProfessionMere,
                        ProfessionPere = candidat.ProfessionPere,
                        Vision = candidat.Vision,
                        Quartier = candidat.Quartier,
                        Etablissement = candidat.Etablissement,
                        StatutMatrimonial = candidat.Personne.StatutMatrimonial,
                        Nationalite = candidat.Personne.Nationalite,
                        Region = candidat.Personne.Region,
                        Langue = candidat.Personne.Langue,
                        Handicape = candidat.Personne.Handicape,
                        Sexe = candidat.Personne.Sexe,
                        Photo = candidat.Personne.Photo,
                        Email = candidat.Personne.Email,
                        Nom = candidat.Personne.Nom,
                        Prenom = candidat.Personne.Prenom,
                        Telephone = candidat.Personne.Telephone,
                        DateNaissance = candidat.Personne.DateNaissance,
                        LieuNaissance = candidat.Personne.LieuNaissance,
                        CurriculumVitae = candidat.Personne.CurriculumVitae,
                        CandidatId = candidat.Id,
                        PersonneId = candidat.PersonneId,
                        DossierPersonnelId = candidat.DossierPersonnelId
                    };
                    return candidatVM;
                }
                _logger.LogError($"le candidat d' " + $" d'{Id} n'existe pas", typeof(AdmissionService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la visualisation du candidat : {Id}", typeof(ParametrageService));
                return null;
            }
        }

        //Get Candidat 
        public Candidat GetCandidat(Guid Id)
        {
            try
            {
                var candidat = GetAllCandidat.FirstOrDefault(e => e.Id == Id);
                if (candidat != null)
                {
                    return candidat;
                }
                _logger.LogError($"le candidat d' " + $" d'{Id} n'existe pas", typeof(AdmissionService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la recuperation du candidat : {Id}", typeof(ParametrageService));
                return null;
            }
        }

        //Verification de l'existance du Candidat 
        public bool VerifExistCandidat(string Nom, string Prenom, string Telephone, string etab)
        {
            var candidat = _unitOfWork.Candidat.Values.FirstOrDefault(c => c.Personne.Nom == Nom && c.Personne.Prenom == Prenom && c.Etablissement == etab && c.Personne.Telephone == Telephone);
            if (candidat == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region Demande Admission
        public List<DemandeAdmission> GetAllDA => _unitOfWork.DemandeAdmission.Values
                                                            .Include(da => da.Candidat)
                                                                .ThenInclude(c => c.Personne)
                                                            .Include(da => da.Candidat)
                                                                .ThenInclude(c => c.DossierPersonnel)
                                                            .Include(da => da.Concours)
                                                            .Include(da => da.AnneeAcademique)
                                                            .Include(da => da.Classe)
                                                                .ThenInclude(cl => cl.Cycle)
                                                            .Include(da => da.Classe)
                                                                .ThenInclude(cl => cl.Filiere)
                                                            //.ThenInclude(sp => sp.Filiere)
                                                            .Include(da => da.Classe)
                                                                .ThenInclude(cl => cl.Niveau)
                                                            .ToList();

        public List<DemandeAdmission> GetDAByFilter(Guid? cycle, Guid? specialite, Guid? niveau, TypeAdmission? type, Decision? decision)
        {
            List<DemandeAdmission> DAdispo = GetAllDA;
            List<DemandeAdmission> DAfiltrer = new List<DemandeAdmission>();

            foreach (var da in DAdispo)
            {
                if
                    (
                       ((da.TypeAdmission == type) && (da.Decision == decision)) ||

                       ((da.TypeAdmission == type) && (decision == null)) ||
                       ((da.Decision == decision) && (type == null)) ||

                        ((type == null) && (decision == null))
                     )
                {
                    if (

                        ((da.Classe.CycleId == cycle) && (da.Classe.FiliereId == specialite) && (da.Classe.NiveauId == niveau)) ||

                        ((cycle == null || cycle == Guid.Empty) && (specialite == null || specialite == Guid.Empty) && (niveau == null || niveau == Guid.Empty)) ||

                        ((cycle == null || cycle == Guid.Empty) && (specialite == null || specialite == Guid.Empty) && (da.Classe.NiveauId == niveau)) ||
                        ((da.Classe.CycleId == cycle) && (specialite == null || specialite == Guid.Empty) && (niveau == null || niveau == Guid.Empty)) ||
                        ((cycle == null || cycle == Guid.Empty) && (da.Classe.FiliereId == specialite) && (niveau == null || niveau == Guid.Empty)) ||

                        ((cycle == null || cycle == Guid.Empty) && (da.Classe.FiliereId == specialite) && (da.Classe.NiveauId == niveau)) ||
                        ((da.Classe.CycleId == cycle) && (da.Classe.FiliereId == specialite) && (niveau == null || niveau == Guid.Empty)) ||
                        ((da.Classe.CycleId == cycle) && (specialite == null || specialite == Guid.Empty) && (da.Classe.NiveauId == niveau))

                     )
                    {

                        DAfiltrer.Add(da);

                    }
                }
            }
            return DAfiltrer;
        }

        public bool CreateDA(DemandeAdmission model)
        {
            try
            {
                DemandeAdmission DA = new DemandeAdmission()
                {
                    Id = new Guid(),
                    AddedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.Now,
                    Decision = Decision.EnCours,
                    CandidatId = model.CandidatId,
                    AnneeAcademiqueId = model.AnneeAcademiqueId,
                    ConcoursId = model.ConcoursId,
                    ClasseId = model.ClasseId,
                    TypeAdmission = model.TypeAdmission
                };

                _unitOfWork.DemandeAdmission.Insert(DA);
                model.Id = DA.Id;
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la création de la DemandeAdmission ", typeof(AdmissionService));
                return false;
            }
        }

        public bool UpdateDA(DemandeAdmission model)
        {
            try
            {
                DemandeAdmission DA = new DemandeAdmission()
                {
                    ModifiedDate = DateTime.Now,
                    CandidatId = model.CandidatId,
                    AnneeAcademiqueId = model.AnneeAcademiqueId,
                    ClasseId = model.ClasseId,
                    Decision = Decision.EnCours,
                    Id = model.Id,
                    TypeAdmission = model.TypeAdmission

                };
                if (model.TypeAdmission == TypeAdmission.Concours)
                {
                    DA.ConcoursId = model.ConcoursId;
                }
                _unitOfWork.DemandeAdmission.Update(DA);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la mise à jour de la demande d'admission :", typeof(AdmissionService));
                return false;
            }
        }
        public bool UpdateStatutAdmission(DemandeAdmission model)
        {
            try
            {
                DemandeAdmission demande = new DemandeAdmission()
                {
                    Id = model.Id,
                    AddedDate = model.AddedDate,
                    ModifiedDate = DateTime.Now,
                    Decision = model.Decision,
                    CandidatId = model.CandidatId,
                    AnneeAcademiqueId = model.AnneeAcademiqueId,
                    ConcoursId = model.ConcoursId,
                    ClasseId = model.ClasseId,
                    TypeAdmission = model.TypeAdmission

                };
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.DemandeAdmission.Update(demande);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la mise à jour du statut de ce candidat :", typeof(AdmissionService));
                return false;
            }
        }
        public bool DeleteDA(DemandeAdmission model)
        {
            try
            {
                _unitOfWork.DemandeAdmission.Delete(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la supression  de la DemandeAdmission : ", typeof(AdmissionService));
                return false;
            }
        }

        public DemandeAdmission GetDA(Guid Id)
        {
            try
            {
                var demande = GetAllDA.FirstOrDefault(e => e.Id == Id);
                if (demande != null)
                {
                    return demande;
                }
                _logger.LogError($"la DemandeAdmission  d'{Id} n'existe pas", typeof(AdmissionService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la visualisation  de la DemandeAdmission  : {Id}", typeof(AdmissionService));
                return null;
            }
        }

        //Verification de l'existance d'une demande d'admission pour un candidat postulant par ED pour une meme classe la meme annee

        public bool ExistDAEtudeDossier(Guid classeId, Guid candidatId, Guid anneeAcaId)
        {
            var daEt = _unitOfWork.DemandeAdmission.Values.FirstOrDefault(d => d.ClasseId == classeId && d.CandidatId == candidatId && d.AnneeAcademiqueId == anneeAcaId);

            if (daEt == null)
            {
                return false;

            }
            return true;

        }

        //Verification de l'existance d'une demande d'admission pour un cand a un concours
        public bool VerifExistDAConcours(Guid CandidatId, Guid? ConcoursId)
        {
            var da = _unitOfWork.DemandeAdmission.Values.FirstOrDefault(d => d.CandidatId == CandidatId && d.ConcoursId == ConcoursId);
            if (da == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // verifier s'il a payé la totalité des frais d'admission
        public bool VerifPaiementAllFraisAdmission(DemandeAdmission model)
        {
            double? tot = 0; double? montant = 0;

            Classe classe = _param.GetClasse(model.Classe.Id);

            if (model.TypeAdmission == TypeAdmission.Concours)
            {
                foreach (var fraisCancours in GetAllFraisConcours)
                {
                    if (fraisCancours.CycleId == classe.CycleId && fraisCancours.AnneeAcademiqueId == model.AnneeAcademiqueId)
                    {
                        montant = fraisCancours.Montant;
                    }
                }

            }
            else
            {
                foreach (var fraisEtudeDossier in GetAllFraisEtudeDossier)
                {
                    if (fraisEtudeDossier.ClasseId == model.ClasseId && fraisEtudeDossier.AnneeAcademiqueId == model.AnneeAcademiqueId)
                    {
                        montant = fraisEtudeDossier.Montant;
                        break;
                    }
                }

            }
            foreach (Paiement p in GetAllpaiement)
            {
                if (p.DemandeAdmission.InscriptionId == null && p.DemandeAdmissionId == model.Id)
                {
                    tot = tot + p.Montant;
                }
            }
            if (tot == montant)
            {
                return false;
            }
            return true;
        }

        // total des demandes d'admission enregistrées au cours d'une journée
        public int TotalDAParJour()
        {
            var totalDA = GetAllDA;
            int CompteurDA = 0;
            if (totalDA.Count() != 0)
            {
                var date = DateTime.Now.Date;
                foreach (var DA in totalDA)
                {

                    if (DA.AddedDate.Date == date)
                    {
                        CompteurDA++;
                    }
                }
            }
            return CompteurDA;
        }


        #endregion

        #region Dossier Personnel
        public List<DossierPersonnel> GetAllDossierPersonnel => _unitOfWork.DossierPersonnel.Values
                                                                                            .Include(da => da.Candidat)
                                                                                                .ThenInclude(c => c.Personne)
                                                                                            .ToList();
        public bool CreateDossierPersonnel(DossierPersonnelVM model)
        {
            try
            {
                DossierPersonnel dossier = new DossierPersonnel()
                {
                    Id = model.DossierPersonnelId,
                    ModifiedDate = DateTime.Now,
                };
                if (model.ActeNaissanceFile != null)
                {
                    dossier.ActeNaissance = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.ActeNaissanceFile, FileType.Document, $"Acte_{model.Candidat.Personne.Email}_{model.Candidat.Personne.Nom}", GestAgapeUtilitiesFunctions.dossier);//, model.Candidat.Personne.Nom + "_" + model.Candidat.Personne.Prenom
                }
                if (model.ReleveBacFile != null)
                {
                    dossier.ReleveBac = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.ReleveBacFile, FileType.Document, $"ReleveBac_{model.Candidat.Personne.Email}_{model.Candidat.Personne.Nom}", GestAgapeUtilitiesFunctions.dossier);
                }
                if (model.CNIFile != null)
                {
                    dossier.CNI = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.CNIFile, FileType.Document, $"CNI_{model.Candidat.Personne.Email}_{model.Candidat.Personne.Nom}", GestAgapeUtilitiesFunctions.dossier);
                }
                if (model.PhotosFile != null)
                {
                    dossier.Photos = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.PhotosFile, FileType.Image, $"Photos_{model.Candidat.Personne.Email}_{model.Candidat.Personne.Nom}", GestAgapeUtilitiesFunctions.dossier);
                }
                if (model.ReleveNiveau1File != null)
                {
                    dossier.ReleveNiveau1 = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.ReleveNiveau1File, FileType.Document, $"ReleveNiveau1_{model.Candidat.Personne.Email}_{model.Candidat.Personne.Nom}", GestAgapeUtilitiesFunctions.dossier);
                }
                if (model.ReleveNiveau2File != null)
                {
                    dossier.ReleveNiveau2 = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.ReleveNiveau2File, FileType.Document, $"ReleveNiveau2_{model.Candidat.Personne.Email}_{model.Candidat.Personne.Nom}", GestAgapeUtilitiesFunctions.dossier);
                }
                if (model.ReleveMaster1File != null)
                {
                    dossier.ReleveMaster1 = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.ReleveMaster1File, FileType.Document, $"ReleveMaster1_{model.Candidat.Personne.Email}_{model.Candidat.Personne.Nom}", GestAgapeUtilitiesFunctions.dossier);
                }
                if (model.ReleveBTSFile != null)
                {
                    dossier.ReleveBTS = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.ReleveBTSFile, FileType.Document, $"ReleveBTS_{model.Candidat.Personne.Email}_{model.Candidat.Personne.Nom}", GestAgapeUtilitiesFunctions.dossier);
                }
                if (model.ReleveLicenceFile != null)
                {
                    dossier.ReleveLicence = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.ReleveLicenceFile, FileType.Document, $"ReleveLicence_{model.Candidat.Personne.Email}_{model.Candidat.Personne.Nom}", GestAgapeUtilitiesFunctions.dossier);
                }
                _unitOfWork.DossierPersonnel.Insert(dossier);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la création de la DossierPersonnel ", typeof(AdmissionService));
                return false;
            }
        }

        public bool UpdateDossierPersonnel(DossierPersonnelVM model)
        {
            try
            {
                var dp = GetDossierPersonnel(model.DossierPersonnelId);
                if (dp == null)
                {
                    return false;
                }
                model.Candidat = dp.Candidat;

                DossierPersonnel dossier = new DossierPersonnel()
                {
                    Id = model.DossierPersonnelId,
                    ModifiedDate = DateTime.Now,
                    ActeNaissance = dp.ActeNaissance,
                    ReleveBac = dp.ReleveBac,
                    CNI = dp.CNI,
                    Photos = dp.Photos,
                    ReleveNiveau1 = dp.ReleveNiveau1,
                    ReleveNiveau2 = dp.ReleveNiveau2,
                    ReleveMaster1 = dp.ReleveMaster1,
                    ReleveBTS = dp.ReleveBTS,
                    ReleveLicence = dp.ReleveLicence,

                };
                if (model.ActeNaissanceFile != null)
                {
                    dossier.ActeNaissance = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.ActeNaissanceFile, FileType.Document, $"Acte_{model.Candidat.Personne.Email}_{model.Candidat.Personne.Nom}", GestAgapeUtilitiesFunctions.dossier);//, model.Candidat.Personne.Nom + "_" + model.Candidat.Personne.Prenom
                }
                if (model.ReleveBacFile != null)
                {
                    dossier.ReleveBac = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.ReleveBacFile, FileType.Document, $"ReleveBac_{model.Candidat.Personne.Email}_{model.Candidat.Personne.Nom}", GestAgapeUtilitiesFunctions.dossier);
                }
                if (model.CNIFile != null)
                {
                    dossier.CNI = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.CNIFile, FileType.Document, $"CNI_{model.Candidat.Personne.Email}_{model.Candidat.Personne.Nom}", GestAgapeUtilitiesFunctions.dossier);
                }
                if (model.PhotosFile != null)
                {
                    dossier.Photos = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.PhotosFile, FileType.Image, $"Photos_{model.Candidat.Personne.Email}_{model.Candidat.Personne.Nom}", GestAgapeUtilitiesFunctions.dossier);
                }
                if (model.ReleveNiveau1File != null)
                {
                    dossier.ReleveNiveau1 = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.ReleveNiveau1File, FileType.Document, $"ReleveNiveau1_{model.Candidat.Personne.Email}_{model.Candidat.Personne.Nom}", GestAgapeUtilitiesFunctions.dossier);
                }
                if (model.ReleveNiveau2File != null)
                {
                    dossier.ReleveNiveau2 = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.ReleveNiveau2File, FileType.Document, $"ReleveNiveau2_{model.Candidat.Personne.Email}_{model.Candidat.Personne.Nom}", GestAgapeUtilitiesFunctions.dossier);
                }
                if (model.ReleveMaster1File != null)
                {
                    dossier.ReleveMaster1 = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.ReleveMaster1File, FileType.Document, $"ReleveMaster1_{model.Candidat.Personne.Email}_{model.Candidat.Personne.Nom}", GestAgapeUtilitiesFunctions.dossier);
                }
                if (model.ReleveBTSFile != null)
                {
                    dossier.ReleveBTS = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.ReleveBTSFile, FileType.Document, $"ReleveBTS_{model.Candidat.Personne.Email}_{model.Candidat.Personne.Nom}", GestAgapeUtilitiesFunctions.dossier);
                }
                if (model.ReleveLicenceFile != null)
                {
                    dossier.ReleveLicence = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.ReleveLicenceFile, FileType.Document, $"ReleveLicence_{model.Candidat.Personne.Email}_{model.Candidat.Personne.Nom}", GestAgapeUtilitiesFunctions.dossier);
                }
                _unitOfWork.DossierPersonnel.Update(dossier);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la mise a jour  du DossierPersonnel :", typeof(AdmissionService));
                return false;
            }
        }

        public bool DeleteDossierPersonnel(DossierPersonnel model)
        {
            try
            {
                _unitOfWork.DossierPersonnel.Delete(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la supression  du DossierPersonnel : ", typeof(AdmissionService));
                return false;
            }
        }

        public DossierPersonnel GetDossierPersonnel(Guid Id)
        {
            try
            {
                var dossier = GetAllDossierPersonnel.FirstOrDefault(e => e.Id == Id);
                if (dossier != null)
                {
                    return dossier;
                }
                _logger.LogError($"le DossierPersonnel  d'{Id} n'existe pas", typeof(AdmissionService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la visualisation  du DossierPersonnel  : {Id}", typeof(AdmissionService));
                return null;
            }
        }
        #endregion

        #region Paiement Et reçu

        public List<Paiement> GetAllpaiement => _unitOfWork.Paiement.Values
                                                      .Include(p => p.DemandeAdmission)
                                                          .ThenInclude(da => da.Candidat)
                                                              .ThenInclude(c => c.Personne)
                                                      .Include(p => p.DemandeAdmission)
                                                          .ThenInclude(da => da.Concours)
                                                      .Include(p => p.DemandeAdmission)
                                                          .ThenInclude(da => da.AnneeAcademique)
                                                      .Include(p => p.DemandeAdmission)
                                                          .ThenInclude(da => da.Classe)
                                                              .ThenInclude(cl => cl.Cycle)
                                                      .Include(p => p.DemandeAdmission)
                                                          .ThenInclude(da => da.Classe)
                                                              .ThenInclude(cl => cl.Filiere)
                                                      .Include(p => p.DemandeAdmission)
                                                              .ThenInclude(da => da.Classe)
                                                                  .ThenInclude(cl => cl.Niveau)
                                                      .Include(p => p.DemandeAdmission)
                                                          .ThenInclude(da => da.Inscription)
                                                              .ThenInclude(i => i.Matricule)
                                                      .ToList();
        public bool CreatePaiement(Paiement model)
        {
            try
            {

                Paiement P = new Paiement()
                {
                    Id = new Guid(),
                    AddedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    Montant = model.Montant,
                    Motif = model.Motif,
                    Modepaiement = model.Modepaiement,
                    DemandeAdmissionId = model.DemandeAdmissionId
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
                _logger.LogError(ex, $"{ex} Echec lors du paiement pour : {model.Motif} du {DateTime.Now}", typeof(AdmissionService));
                return false;
            }

        }
        public Paiement GetPaiement(Guid Id)
        {
            try
            {
                var paiement = GetAllpaiement.FirstOrDefault(e => e.Id == Id);
                if (paiement != null)
                {
                    return paiement;
                }
                _logger.LogError($"le paiement  d'{Id} n'existe pas", typeof(AdmissionService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la visualisation du paiement  : {Id}", typeof(AdmissionService));
                return null;
            }
        }
        public double? ResteFraisconcours(Guid demandeAdmission, DateTime dateimp)
        {
            double? mfc = 0, somme = 0;
            var da = GetDA(demandeAdmission);

            foreach (Paiement paiement in GetAllpaiement)
            {

                if (paiement.DemandeAdmission.Id == da.Id && paiement.Motif == Motif.FraisConcours && paiement.AddedDate < dateimp)
                {
                    somme += paiement.Montant;
                }
            }

            foreach (FraisConcours fc in GetAllFraisConcours)
            {
                if (fc.CycleId == da.Classe.CycleId && fc.AnneeAcademiqueId == da.AnneeAcademiqueId)
                {
                    mfc = fc.Montant;
                    break;
                }
            }
            return mfc - somme;
        }
        public double? ResteFraisEtdudeDossier(Guid demandeAdmission, DateTime dateimp)
        {
            double? mfde = 0, somme = 0;
            var da = GetDA(demandeAdmission);

            foreach (Paiement paiement in GetAllpaiement)
            {

                if (paiement.DemandeAdmission.Id == da.Id && paiement.Motif == Motif.FraisEtudeDossier && paiement.AddedDate < dateimp)
                {
                    somme += paiement.Montant;
                }
            }

            foreach (FraisEtudeDossier fed in GetAllFraisEtudeDossier)
            {
                if (fed.ClasseId == da.ClasseId && fed.AnneeAcademiqueId == da.AnneeAcademiqueId)
                {
                    mfde = fed.Montant;
                    break;
                }
            }
            return mfde - somme;
        }

        // récupération des demandes d'admission par cycle
        public List<DemandeAdmission> GetDAParCycle(Guid cycleId)
        {
            List<DemandeAdmission> DemandesParCycle = new List<DemandeAdmission>();
            foreach (var da in GetAllDA)
                if (da.Classe.CycleId == cycleId)
                {
                    DemandesParCycle.Add(da);
                }
            return DemandesParCycle;
        }


        // récupération des demandes d'admission par classe
        public List<DemandeAdmission> GetDAParClasse(Guid classeId)
        {
            List<DemandeAdmission> DemandesParClasse = new List<DemandeAdmission>();
            foreach (var da in GetAllDA)
                if (da.ClasseId == classeId)
                {
                    DemandesParClasse.Add(da);
                }
            return DemandesParClasse;
        }


        // Convertion en lettre
        public string NumberToWords(int number)
        {
            if (number == 0)
                return "Zéro";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Mille ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Cent ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += " ";
                string[] unitsMap = { "Zéro", "un", "Deux", "Trois", "Quatre", "Cinq", "Six", "Sept", "Huit", "Neuf", "Dix", "Onze", "Douze", "Treize", "Quatorze", "Quinze", "Seize", "Dix-sept", "Dix-huit", "Dix-neuf" };
                string[] tensMap = { "Zéro", "Dix", "Vingt", "Trente", "Quarante", "Cinquante", "Soixante", "Soixante-dix", "Quatre-vingts", "Quatre-vingt-dix" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        // affichage des paiements d'une periode

        public List<Paiement> PaiementsParPeriode(DateTime dateDebut, DateTime dateFin)
        {
            List<Paiement> paiements = GetAllpaiement;
            List<Paiement> paiementsTries = new List<Paiement>();
            foreach (var p in paiements)
            {
                if (p.AddedDate.Date > dateDebut.Date && p.AddedDate.Date < dateFin.Date
                    || p.AddedDate.Date == dateDebut.Date
                    || p.AddedDate.Date == dateFin.Date)
                {
                    paiementsTries.Add(p);

                }
            }
            return paiementsTries;
        }

        // Nombre de versements encaissés aujourd'hui

        public int NbrePaiementParJour()
        {
            var nbrePaiement = GetAllpaiement;
            int CompteurPaiement = 0;
            if (nbrePaiement.Count() != 0)
            {
                var date = DateTime.Now.Date;
                foreach (var Paiement in nbrePaiement)
                {
                    if (Paiement.AddedDate.Date == date)
                    {
                        CompteurPaiement++;
                    }
                }
            }
            return CompteurPaiement;
        }
        #endregion

    }
}
