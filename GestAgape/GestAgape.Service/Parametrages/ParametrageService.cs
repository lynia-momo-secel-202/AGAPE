using GestAgape.Core.Entities.Identity;
using GestAgape.Core.Entities.Parametrage;
using GestAgape.Core.ViewModels;
using GestAgape.Models;
using GestAgape.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;
using System;
using GestAgape.Core.Entities;
using iText.Layout.Element;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using System.Runtime.Loader;

namespace GestAgape.Service.Parametrages
{
    public class ParametrageService : IParametrage
    {
        #region membres prives
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        protected readonly ILogger _logger;
        private readonly IWebHostEnvironment _hostingEnv;
        private IdentityContext _context;



        #endregion

        #region constructeur
        public ParametrageService(
            IEmailSender emailSender,
            IUnitOfWork unitOfWork,
            ILoggerFactory loggerFactory,
            IWebHostEnvironment hostingEnv,
            IdentityContext context

        )
        {
            _emailSender = emailSender; ;
            _unitOfWork = unitOfWork;
            _logger = loggerFactory.CreateLogger("logs");
            _hostingEnv = hostingEnv;
            _context = context;
        }
        #endregion

        #region campus
        //Get all campus
        public List<Campus> GetAllCampus => _unitOfWork.Campus.Values.Include(e => e.Ipes).ToList();

        //create campus
        public bool CreateCampus(Campus model)
        {
            try
            {
                model.Id = new Guid();
                model.AddedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.Campus.Insert(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la création du campus : {model.Nom}", typeof(ParametrageService));
                return false;
            }
        }

        //Update campus
        public bool UpdateCampus(Campus model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.Campus.Update(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de l'édition du campus : {model.Nom}", typeof(ParametrageService));
                return false;
            }
        }

        //delete campus
        public bool DeleteCampus(Campus model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                _unitOfWork.Campus.Delete(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la supression du campus : {model.Nom}", typeof(ParametrageService));
                return false;
            }
        }

        //Get campus
        public Campus GetCampus(Guid Id)
        {
            try
            {
                var campus = _unitOfWork.Campus.Get(Id);
                if (campus != null)
                {
                    return campus;
                }
                _logger.LogError($"le campus d'{Id} n'existe pas", typeof(ParametrageService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la visualisation du campus : {Id}", typeof(ParametrageService));
                return null;
            }
        }

        //verification de l'existence du campus
        public bool VerifCampus(Campus model)
        {
            Campus? campus = new Campus();

            campus = _unitOfWork.Campus.Values.FirstOrDefault(c => c.Nom.ToLower() == model.Nom.ToLower() && c.IPESId == model.IPESId && c.Adresse == model.Adresse);
            if (campus == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region chef departement

        // Afficher les chefs de département
        public List<ChefDepartement> GetAllChefDepartement => _unitOfWork.ChefDepartement.Values
            .Include(c => c.Departement)
            .Include(c => c.Personne).ToList();

        //nouveau chef de département

        public bool CreateChefDepartement(ChefDepartementVM model)
        {

            try
            {


                Personne personne = new Personne()
                {
                    Id = new Guid(),
                    ModifiedDate = DateTime.UtcNow,
                    AddedDate = DateTime.UtcNow,
                    Email = model.Email,
                    Nom = model.Nom,
                    Prenom = model.Prenom,
                    Telephone = model.Telephone,
                    Region = model.Region,
                    Langue = model.Langue,
                    Handicape = model.Handicape,
                    Sexe = model.Sexe,
                    Nationalite = model.Nationalite

                };

                _unitOfWork.Personne.Insert(personne);


                if (model.DateNomination == DateTime.MinValue)
                {
                    model.DateNomination = DateTime.UtcNow;
                }
                ChefDepartement chefdepartement = new ChefDepartement()
                {
                    Id = new Guid(),
                    ModifiedDate = DateTime.UtcNow,
                    AddedDate = DateTime.UtcNow,
                    DepartementId = model.DepartementId,
                    DateNomination = model.DateNomination,
                    DateFin = model.DateFin,
                    Statut = true,
                    PersonneId = personne.Id
                };


                foreach (ChefDepartement chefDepart in GetAllChefDepartement)
                {
                    if (chefDepart.Statut == true && chefDepart.DepartementId == chefdepartement.DepartementId)
                    {
                        chefDepart.Statut = false;
                        chefDepart.DateFin = chefdepartement.DateNomination;
                        _unitOfWork.ChefDepartement.Update(chefDepart);
                    }
                }
                _unitOfWork.ChefDepartement.Insert(chefdepartement);
                model.ChefDepartementId = chefdepartement.Id;
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }

            catch (Exception ex)
            {
                _unitOfWork?.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la création du chef de département : {model.Nom}", typeof(ParametrageService));
                return false;
            }
        }

        // UpdateChefDepartement
        public bool UpdateChefDepartementVM(ChefDepartementVM model)
        {
            try
            {
                Personne personne = new Personne()
                {
                    Id = model.PersonneId,
                    ModifiedDate = DateTime.UtcNow,
                    AddedDate = DateTime.UtcNow,
                    Email = model.Email,
                    Nom = model.Nom,
                    Prenom = model.Prenom,
                    Telephone = model.Telephone,
                    Region = model.Region,
                    Langue = model.Langue,
                    Handicape = model.Handicape,
                    Sexe = model.Sexe,
                    Nationalite = model.Nationalite

                };

                _unitOfWork.Personne.Update(personne);

                ChefDepartement chefdepartement = new ChefDepartement()
                {
                    Id = model.ChefDepartementId,
                    ModifiedDate = DateTime.UtcNow,
                    AddedDate = DateTime.UtcNow,
                    DepartementId = model.DepartementId,
                    DateNomination = model.DateNomination,
                    DateFin = model.DateFin,
                    Statut = model.Statut,
                    PersonneId = model.PersonneId
                };

                _unitOfWork.ChefDepartement.Update(chefdepartement);
                _unitOfWork.Save();

                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de l'édition du chef de département", typeof(ParametrageService));
                return false;
            }
        }

        public ChefDepartement GetChefDepartement(Guid Id)
        {
            try
            {
                var chefDepart = GetAllChefDepartement.FirstOrDefault(c => c.Id == Id);
                if (chefDepart != null)
                {
                    return chefDepart;
                }
                _logger.LogError($"le chef de département n'existe pas dans la base de donnee", typeof(ParametrageService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la Recupération du chef de département", typeof(ParametrageService));
                return null;
            }
        }
        public ChefDepartementVM GetChefDepartementVM(Guid Id)
        {
            try
            {
                ChefDepartement chefDepart = _unitOfWork.ChefDepartement.Get(Id);
                if (chefDepart != null)
                {
                    ChefDepartementVM cdvm = new ChefDepartementVM();
                    cdvm.ChefDepartementId = chefDepart.Id;
                    cdvm.DateFin = chefDepart.DateFin;
                    cdvm.DateNomination = chefDepart.DateNomination;
                    cdvm.Statut = chefDepart.Statut;
                    cdvm.PersonneId = chefDepart.PersonneId;



                    Personne personne = _unitOfWork.Personne.Get(chefDepart.PersonneId);
                    cdvm.Nom = personne.Nom;
                    cdvm.Prenom = personne.Prenom;
                    cdvm.Email = personne.Email;
                    cdvm.Telephone = personne.Handicape;
                    cdvm.Langue = personne.Langue;
                    cdvm.Region = personne.Region;
                    cdvm.Sexe = personne.Sexe;
                    cdvm.StatutMatrimonial = personne.StatutMatrimonial;

                    return cdvm;
                }
                _logger.LogError($"ce chef de département n'existe pas dans la base de donnee", typeof(ParametrageService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la Recuperation du chef de département : {Id}", typeof(ParametrageService));
                return null;
            }
        }

        //verification de l'existence du chef de depart
        public bool ExistChefDepartement(string Nom, string Prenom, string Telephone, Guid Id)
        {
            var chefDepart = _unitOfWork.ChefDepartement.Values.FirstOrDefault(c => c.Personne.Nom == Nom && c.Personne.Prenom == Prenom && c.Personne.Telephone == Telephone && c.DepartementId == Id);
            if (chefDepart == null)
            {
                return false;
            }
            return true;
        }


        #endregion

        #region Classe
        //Get all Classe
        public List<Classe> GetAllClasse => _unitOfWork.Classe.Values.Include(e => e.Cycle).Include(e => e.Filiere).Include(e => e.Niveau).ToList();

        //Get create Classe
        public bool CreateClasse(Classe model)
        {
            var niv = GetNiveau(model.NiveauId);
            var spc = GetFiliere(model.FiliereId);
            try
            {
                model.Id = new Guid();
                model.AddedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Libelle = $"{spc.Libelle}  " + $"{niv.Libelle}  ";
                model.Code = $"{spc.Code}  " + $"{niv.Libelle}  ";
                _unitOfWork.Classe.Insert(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} echec lors de la création du Classe : {model.Code}", typeof(ParametrageService));
                return false;
            }
        }

        //update Classe
        public bool UpdateClasse(Classe model)
        {
            var niv = GetNiveau(model.NiveauId);
            var spc = GetFiliere(model.FiliereId);
            try
            {
                model.ModifiedDate = DateTime.Now;
                model.Libelle = $"{spc.Libelle}  " + $"{niv.Libelle}  ";
                model.Code = $"{spc.Code}  " + $"{niv.Libelle}  ";
                _unitOfWork.Classe.Update(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} echec lors de l'Updateion du Classe : {model.Code}", typeof(ParametrageService));
                return false;
            }
        }

        //delete Classe
        public bool DeleteClasse(Classe model)
        {
            try
            {
                _unitOfWork.Classe.Delete(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} echec lors de la supression du Classe : {model.Code}", typeof(ParametrageService));
                return false;
            }
        }

        //Get Classe
        public Classe GetClasse(Guid id)
        {
            try
            {
                var Classe = _unitOfWork.Classe.Get(id);
                if (Classe != null)
                {
                    return Classe;
                }
                _logger.LogError($" la Classe d'id {id} n'existe pas dans la base de donnee", typeof(ParametrageService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} echec lors de la visualisation du Classe : {id}", typeof(ParametrageService));
                return null;
            }
        }

        //Verification de l'existance d'une classe

        public bool VerifExistClasse(Guid specialiteId, Guid niveauId)
        {
            var classe = _unitOfWork.Classe.Values.FirstOrDefault(c => c.FiliereId == specialiteId && c.NiveauId == niveauId);
            if (classe == null)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region cycle
        public List<Cycle> GetAllCycle => _unitOfWork.Cycle.Values.Include(e => e.Classes).ToList();

        public bool CreateCycle(Cycle model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                model.Id = new Guid();
                model.AddedDate = DateTime.UtcNow;
                model.ModifiedDate = DateTime.UtcNow;
                _unitOfWork.Cycle.Insert(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la création du Cycle : {model.Libelle}", typeof(ParametrageService));
                return false;
            }
        }

        public bool UpdateCycle(Cycle model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                model.ModifiedDate = DateTime.UtcNow;
                _unitOfWork.Cycle.Update(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de l'édition du Cycle : {model.Libelle}", typeof(ParametrageService));
                return false;
            }
        }

        public Cycle GetCycle(Guid Id)
        {
            try
            {
                var cycle = _unitOfWork.Cycle.Get(Id);
                if (cycle != null)
                {
                    return cycle;
                }
                _logger.LogError($"le cycle d'Id {Id} n'existe pas dans la base de donnee", typeof(ParametrageService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la Recuperation du Cycle : {Id}", typeof(ParametrageService));
                return null;
            }
        }

        //Verification de l'existance d'un cycle
        public bool VerifCycle(Cycle model, ExistType existType)
        {
            Cycle? cycle = new Cycle();
            switch (existType)
            {
                case ExistType.Create:
                    cycle = _unitOfWork.Cycle.Values.FirstOrDefault(f => f.Libelle.ToLower() == model.Libelle.ToLower() || f.Code.ToLower() == model.Code.ToLower());
                    break;
                case ExistType.Update:
                    cycle = _unitOfWork.Cycle.Values.FirstOrDefault(f => f.Libelle.ToLower() == model.Libelle.ToLower() && f.Code.ToLower() == model.Code.ToLower());
                    break;
                default: break;
            }
            if (cycle == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        #endregion

        #region departement

        // Afficher les chefs de département

        public List<Departement> GetAllDepartement => _unitOfWork.Departement.Values.ToList();

        //create Departement
        public bool CreateDepartement(Departement model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                model.Id = new Guid();
                model.AddedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.Departement.Insert(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la création du Departement : {model.Libelle}", typeof(ParametrageService));
                return false;
            }
        }

        //update Departement
        public bool UpdateDepartement(Departement model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.Departement.Update(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de l'edition du Departement : {model.Libelle}", typeof(ParametrageService));
                return false;
            }
        }

        //Get Departement
        public Departement GetDepartement(Guid Id)
        {
            try
            {
                var departement = _unitOfWork.Departement.Get(Id);
                if (departement != null)
                {
                    return (departement);
                }
                _logger.LogError($"le Departement d'{Id} n'existe pas", typeof(ParametrageService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la visualisation du Departement : {Id}", typeof(ParametrageService));
                return null;
            }
        }


        //delete Departement
        public bool DeleteDepartement(Departement model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                _unitOfWork.Departement.Delete(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la supression du Departement : {model.Libelle}", typeof(ParametrageService));
                return false;
            }
        }

        //verification de l'existence du departement
        public bool ExistDepartement(string Libelle, string Code, ExistType existType)
        {
            Departement? dep = new Departement();
            switch (existType)
            {
                case ExistType.Create:
                    dep = _unitOfWork.Departement.Values.FirstOrDefault(f => f.Libelle.ToLower() == Libelle.ToLower() || f.Code.ToLower() == Code.ToLower());
                    break;
                case ExistType.Update:
                    dep = _unitOfWork.Departement.Values.FirstOrDefault(f => f.Libelle.ToLower() == Libelle.ToLower() && f.Code.ToLower() == Code.ToLower());
                    break;
                default: break;
            }
            if (dep == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region filiere

        //Get all Filiere
        public List<Filiere> GetAllFiliere => _unitOfWork.Filiere.Values.Include(e => e.Departement).ToList();

        public IdentityContext GetContext()
        {
            return this._context;
        }

        //create Filiere
        public bool CreateFiliere(Filiere model)
        {
            try
            {
                //var depart = _context.Departements.FindAsync(model.DepartementID);

                //model.Departement = depart;

                //_unitOfWork.CreateTransaction();
                model.Id = new Guid();
                model.AddedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.Filiere.Insert(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la création de la Filiere : {model.Libelle}", typeof(ParametrageService));
                return false;
            }
        }

        //update Filiere
        public bool UpdateFiliere(Filiere model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.Filiere.Update(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de l'edition du Filiere : {model.Libelle}", typeof(ParametrageService));
                return false;
            }
        }

        //Get Filiere
        public Filiere GetFiliere(Guid Id)
        {
            try
            {
                var filiere = _unitOfWork.Filiere.Get(Id);

                if (filiere != null)
                {
                    return filiere;
                }
                _logger.LogError($"le filiere d'{Id} n'existe pas", typeof(ParametrageService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la visualisation du filiere : {Id}", typeof(ParametrageService));
                return null;
            }
        }

        //delete Filiere
        public bool DeleteFiliere(Filiere model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                _unitOfWork.Filiere.Delete(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la supression du campus : {model.Libelle}", typeof(ParametrageService));
                return false;
            }
        }

        //verification de l'existence de la filiere
        public bool ExistFiliere(string Libelle, string Code, ExistType existType)
        {
            Filiere? fil = new Filiere();
            switch (existType)
            {
                case ExistType.Create:
                    fil = _unitOfWork.Filiere.Values.FirstOrDefault(f => f.Libelle.ToLower() == Libelle.ToLower() || f.Code.ToLower() == Code.ToLower());
                    break;
                case ExistType.Update:
                    fil = _unitOfWork.Filiere.Values.FirstOrDefault(f => f.Libelle.ToLower() == Libelle.ToLower() && f.Code.ToLower() == Code.ToLower());
                    break;
                default: break;
            }
            if (fil == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region ipes

        //Get all Ipes
        public List<Ipes> GetAllIpes => _unitOfWork.Ipes.Values.ToList();

        //create Ipes
        public bool CreateIpes(Ipes model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                model.Id = new Guid();
                model.AddedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.Ipes.Insert(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la création du campus : {model.Nom}", typeof(ParametrageService));
                return false;
            }
        }

        //update Ipes
        public bool UpdateIpes(Ipes model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                model.ModifiedDate = DateTime.Now;
                _unitOfWork.Ipes.Update(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de l'edition du campus : {model.Nom}", typeof(ParametrageService));
                return false;
            }
        }

        //delete Ipes
        public bool DeleteIpes(Ipes model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                _unitOfWork.Ipes.Delete(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la supression de l' Ipes : {model.Nom}", typeof(ParametrageService));
                return false;
            }
        }

        //Get Ipes
        public Ipes GetIpes(Guid Id)
        {
            try
            {
                var ipes = _unitOfWork.Ipes.Get(Id);
                if (ipes != null)
                {
                    return ipes;
                }
                _logger.LogError($"l'ipes d'{Id} n'existe pas", typeof(ParametrageService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la visualisation de l'Ipes : {Id}", typeof(ParametrageService));
                return null;
            }
        }

        public bool VerifIpes(Ipes model, ExistType existType)
        {
            Ipes? ipes = new Ipes();

            switch (existType)
            {
                case ExistType.Create:
                    ipes = _unitOfWork.Ipes.Values.FirstOrDefault(f => f.Nom.ToLower() == model.Nom.ToLower());
                    break;
                case ExistType.Update:
                    ipes = _unitOfWork.Ipes.Values.FirstOrDefault(f => f.Nom.ToLower() == model.Nom.ToLower() && f.AdresseCampusPrincipal.ToLower() == model.AdresseCampusPrincipal.ToLower());
                    break;
                default: break;
            }
            if (ipes == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region Niveau
        public List<Niveau> GetAllNiveau => _unitOfWork.Niveau.Values.Include(e => e.Classes).ToList();

        public bool CreateNiveau(Niveau model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                model.Id = new Guid();
                model.AddedDate = DateTime.UtcNow;
                model.ModifiedDate = DateTime.UtcNow;
                _unitOfWork.Niveau.Insert(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la création du FiliereNiveau : {model.Id}", typeof(ParametrageService));
                return false;
            }
        }
        public bool UpdateNiveau(Niveau model)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                model.ModifiedDate = DateTime.UtcNow;
                _unitOfWork.Niveau.Update(model);
                _unitOfWork.Save();
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de l'édition du Niveau : {model.Id}", typeof(ParametrageService));
                return false;
            }
        }

        public Niveau GetNiveau(Guid Id)
        {
            try
            {
                var niv = _unitOfWork.Niveau.Get(Id);
                if (niv != null)
                {
                    return niv;
                }
                _logger.LogError($" le niveau d'id {Id} n'existe pas dans la base de donnée", typeof(ParametrageService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la visualisation du Niveau : {Id}", typeof(ParametrageService));
                return null;
            }
        }
        //verification de l'existence du niveau
        public bool VerifNiveau(Niveau model)
        {
            foreach (Niveau niveau in GetAllNiveau)
            {

                if (niveau.Libelle == model.Libelle)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

    }
}
