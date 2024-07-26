using FluentValidation.Results;
using GestAgape.Core.Entities.Admission;
using GestAgape.Core.ViewModels;
using GestAgape.Core.ViewModels.FluentValidators;
using GestAgape.Infrastructure.Utilities;
using GestAgape.Service.Admissions;
using GestAgape.Service.Parametrages;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Org.BouncyCastle.Bcpg;

namespace GestAgape.Controllers
{
    public class AdmissionController : Controller
    {
        #region Membres prives

        private readonly IAdmission _admission;
        private readonly IEmailSender _emailSender;
        private readonly IParametrage _param;
        private readonly IWebHostEnvironment _webHostEnvironment;


        #endregion

        #region Constructeur

        public AdmissionController(IAdmission admission, IEmailSender emailSender, IParametrage param, IWebHostEnvironment webHostEnvironment)
        {
            _admission = admission;
            _emailSender = emailSender;
            _param = param;
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion

        #region Annee Academique
        public IActionResult AnneeAcademique()
        {
            var anneeacademique = _admission.GetAllAnneeAcademique;
            return View(anneeacademique);
        }

        //[Authorize(Roles = "Administrateur , Support Informatique")]

        [HttpGet]
        public IActionResult CreateAnneeAcademique()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAnneeAcademique(AnneeAcademique model)
        {
            AnneeAcademiqueValidator validator = new AnneeAcademiqueValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (_admission.VerifExistAnneeAcademique(model.AnneeDebut))
                    {
                        response.IsValid = false;
                        response.Message = "Cette année académique existe déjà";
                        return Ok(response);
                    }
                    if (_admission.CreateAnneeAcademique(model))
                    {
                        response.Message = "Année académique " + model.AnneeDebut + " - " + model.AnneeFin + " creer avec succès";
                        return Ok(response);
                    }

                    else
                    {
                        response.IsValid = false;
                        response.Message = "Erreur lors de la creation de l'année académique" + model.AnneeDebut;
                        return Ok(response);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(response);
                }
            }
            return BadRequest(response);
        }

        //[Authorize(Roles = "Support Informatique , Administrateur")]

        [HttpGet]

        public IActionResult UpdateAnneeAcademique(Guid id)
        {
            AnneeAcademique anneeacademique = _admission.GetAnneeAcademique(id);
            if (anneeacademique != null)
            {
                return View(anneeacademique);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult UpdateAnneeAcademique(AnneeAcademique model)
        {
            AnneeAcademiqueValidator validator = new AnneeAcademiqueValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (_admission.UpdateAnneeAcademique(model))
                    {
                        response.Message = "Année academique " + model.AnneeDebut + " modifié avec succès";
                        return Ok(response);
                    }

                    else
                    {
                        response.IsValid = false;
                        response.Message = "Erreur lors de la modification de l'Année academique" + model.AnneeDebut;
                        return Ok(response);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(response);
                }
            }
            return BadRequest(response);
        }


        #endregion

        #region FraisConcours

        public IActionResult FraisConcours()
        {
            var fraisConcours = _admission.GetAllFraisConcours;
            return View(fraisConcours);
        }

        //[Authorize(Roles = "Support Informatique , Administrateur")]

        //GET : Admission/CreateFraisConcours
        [HttpGet]
        public IActionResult CreateFraisConcours()
        {
            ViewBag.Cycle = _param.GetAllCycle;
            ViewBag.AnneeAcademique = _admission.GetAllAnneeAcademique;
            return View();
        }

        //POST : Admission/CreateFraisConcours/{FraisConcours model}
        [HttpPost]
        public IActionResult CreateFraisConcours(FraisConcours model)
        {
            ViewBag.Cycle = _param.GetAllCycle;
            FraisConcoursValidator validator = new FraisConcoursValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (_admission.VerifFraisConcours(model))
                    {
                        response.IsValid = false;
                        response.Message = "Les frais de concours ont déjà été enregistrés pour ce cycle";
                        return Ok(response);
                    }
                    if (_admission.CreateFraisConcours(model))
                    {
                        response.Message = "Frais de concours créés avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de la création des frais de concours";
                        return Ok(response);
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(response);
                }

            }
            return BadRequest(response);
        }

        //GET : Parametrage/UpdateFraisConcours/{Guid Id}
        public IActionResult UpdateFraisConcours(Guid Id)
        {
            ViewBag.Cycle = _param.GetAllCycle;
            ViewBag.ConcoursList = _admission.GetAllFraisConcours;
            ViewBag.AnneeAcademique = _admission.GetAllAnneeAcademique;
            var fraisConcours = _admission.GetFraisConcours(Id);
            if (fraisConcours != null)
            {
                //ViewBag.IPESList = _admission.GetAllFraisConcours;
                return View(fraisConcours);
            }
            return NotFound();
        }

        //POST : Parametrage/UpdateFraisConcours/{FraisConcours model}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateFraisConcours(FraisConcours model)
        {
            //model.IPAddress = HttpContext.Session.GetString(SessionInfo.IPAddress) ?? "";
            FraisConcoursValidator validator = new FraisConcoursValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = new ResponseVM();
            if (response.IsValid)
            {
                try
                {
                    if (_admission.UpdateFraisConcours(model))
                    {
                        response.Message = "les frais ont été modifié avec succès";
                        return Ok(response);
                    }
                    if (_admission.UpdateFraisConcours(model))
                    {
                        response.Message = "les frais ont été modifié avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        //ViewBag.IPESList = _scolarite.GetAllFraisConcours;
                        response.IsValid = false;
                        response.Message = "Echec lors de la création des frais de concours";
                        return Ok(response);
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(response);
                }
            }
            return BadRequest(response);
        }

        //GET : Admission/DeleteFraisConcours/{Guid Id}
        public IActionResult DeleteFraisConcours(Guid Id)
        {
            var fraisConcours = _admission.GetFraisConcours(Id);
            if (fraisConcours != null)
            {
                return View(fraisConcours);
            }
            return NotFound();
        }

        //POST : Admission/DeleteFraisConcours/{FraisConcours model}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFraisConcours(FraisConcours model)
        {
            //model.IPAddress = HttpContext.Session.GetString(SessionInfo.IPAddress) ?? "";
            ResponseVM response = new ResponseVM();
            try
            {
                if (_admission.DeleteFraisConcours(model))
                {
                    response.Message = "les frais de concours ont été supprimé avec succès";
                    return Ok(response);
                }
                else
                {
                    response.IsValid = false;
                    response.Message = "Echec lors de la suppression des frais de concours";
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(response);
            }

        }

        //GET : Admission/DetailsFraisConcours/{Guid Id}
        public IActionResult DetailsFraisConcours(Guid Id)
        {
            try
            {
                var fraisConcours = _admission.GetFraisConcours(Id);
                if (fraisConcours != null)
                {
                    return View(fraisConcours);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        #endregion

        #region FraisEtudeDossier

        public IActionResult FraisEtudeDossier()
        {
            var fraisEtudeDossier = _admission.GetAllFraisEtudeDossier;
            return View(fraisEtudeDossier);
        }

        //[Authorize(Roles = "Support Informatique , Administrateur")]

        //GET : Admission/CreateFraisConcours
        [HttpGet]
        public IActionResult CreateFraisEtudeDossier()
        {
            ViewBag.Classe = _param.GetAllClasse;
            ViewBag.AnneeAcademique = _admission.GetAllAnneeAcademique;
            return View();
        }

        //POST : Admission/CreateFraisEtudeDossier/{FraisEtudeDossier model}
        [HttpPost]
        public IActionResult CreateFraisEtudeDossier(FraisEtudeDossier model)
        {
            ViewBag.Classe = _param.GetAllClasse;
            FraisEtudeDossierValidator validator = new FraisEtudeDossierValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (_admission.VerifFraisEtudeDossier(model))
                    {
                        response.IsValid = false;
                        response.Message = "Les frais d'étude de dossier ont déjà été enregistrés pour cette classe";
                        return Ok(response);
                    }
                    if (_admission.CreateFraisEtudeDossier(model))
                    {
                        response.Message = "Frais d'étude de dossier créés avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de la création des frais d'étude de dossier";
                        return Ok(response);
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(response);
                }

            }
            return BadRequest(response);
        }

        //[Authorize(Roles = "Support Informatique , Administrateur")]

        //GET : Parametrage/UpdateFraisEtudeDossier/{Guid Id}
        public IActionResult UpdateFraisEtudeDossier(Guid Id)
        {
            ViewBag.Classe = _param.GetAllClasse;
            ViewBag.EtudeDossierList = _admission.GetAllFraisEtudeDossier;
            ViewBag.AnneeAcademique = _admission.GetAllAnneeAcademique;
            var fraisEtudeDossier = _admission.GetFraisEtudeDossier(Id);
            if (fraisEtudeDossier != null)
            {
                //ViewBag.IPESList = _admission.GetAllFraisEtudeDossier;
                return View(fraisEtudeDossier);
            }
            return NotFound();
        }

        //POST : Parametrage/UpdateFraisEtudeDossier/{FraisEtudeDossier model}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateFraisEtudeDossier(FraisEtudeDossier model)
        {
            //model.IPAddress = HttpContext.Session.GetString(SessionInfo.IPAddress) ?? "";
            FraisEtudeDossierValidator validator = new FraisEtudeDossierValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = new ResponseVM();
            if (response.IsValid)
            {
                try
                {
                    if (_admission.UpdateFraisEtudeDossier(model))
                    {
                        response.Message = "les frais ont été modifié avec succès";
                        return Ok(response);
                    }
                    if (_admission.UpdateFraisEtudeDossier(model))
                    {
                        response.Message = "les frais ont été modifié avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        //ViewBag.IPESList = _scolarite.GetAllFraisConcours;
                        response.IsValid = false;
                        response.Message = "Echec lors de la création des frais de concours";
                        return Ok(response);
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(response);
                }
            }
            return BadRequest(response);
        }

        //GET : Admission/DeleteFraisConcours/{Guid Id}
        public IActionResult DeleteFraisEtudeDossier(Guid Id)
        {
            var fraisEtudeDossier = _admission.GetFraisEtudeDossier(Id);
            if (fraisEtudeDossier != null)
            {
                return View(fraisEtudeDossier);
            }
            return NotFound();
        }

        //POST : Admission/DeleteFraisEtudeDossier/{FraisConcours model}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFraisEtudeDossier(FraisEtudeDossier model)
        {
            //model.IPAddress = HttpContext.Session.GetString(SessionInfo.IPAddress) ?? "";
            ResponseVM response = new ResponseVM();
            try
            {
                if (_admission.DeleteFraisEtudeDossier(model))
                {
                    response.Message = "les frais d'étude de dossier ont été supprimé avec succès";
                    return Ok(response);
                }
                else
                {
                    response.IsValid = false;
                    response.Message = "Echec lors de la suppression des frais d'étude de dossier";
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(response);
            }

        }

        //GET : Admission/DetailsFraisEtudeDossier/{Guid Id}
        public IActionResult DetailsFraisEtudeDossier(Guid Id)
        {
            try
            {
                var fraisEtudeDossier = _admission.GetFraisEtudeDossier(Id);
                if (fraisEtudeDossier != null)
                {
                    return View(fraisEtudeDossier);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        #endregion

        #region Concours
        public IActionResult Concours()
        {
            var concours = _admission.GetAllConcours;
            return View(concours);
        }

        [HttpGet]

        public IActionResult CreateConcours()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CreateConcours(ConcoursVM model)
        {
            ConcoursVMValidator validator = new ConcoursVMValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (_admission.VerifConcours(model))
                    {
                        response.IsValid = false;
                        response.Message = "Ce concours existe déjà";
                        return Ok(response);
                    }
                    if (DateTime.Parse(model.HeureFin) <= DateTime.Parse(model.HeureDebut))
                    {
                        response.IsValid = false;
                        response.Message = "l'heure de début ne doit pas être supérieure ou égale à l'heure de fin";
                        return Ok(response);

                    }
                    if (_admission.CreateConcours(model))
                    {
                        response.Message = "Nouveau concours " + model.Libelle + " créé avec succès";
                        return Ok(response);
                    }

                    else
                    {
                        response.IsValid = false;
                        response.Message = "Erreur lors de la creation du concours" + model.Libelle;
                        return Ok(response);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(response);
                }
            }
            return BadRequest(response);
        }
        [HttpGet]
        public IActionResult UpdateConcours(Guid id)
        {
            ConcoursVM concours = _admission.GetConcoursVM(id);
            if (concours != null)
            {
                return View(concours);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult UpdateConcours(ConcoursVM model)
        {
            ConcoursVMValidator validator = new ConcoursVMValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (DateTime.Parse(model.HeureFin) <= DateTime.Parse(model.HeureDebut))
                    {
                        response.IsValid = false;
                        response.Message = "l'heure de début ne doit pas être supérieure ou égale à l'heure de fin";
                        return Ok(response);

                    }
                    if (_admission.UpdateConcours(model))
                    {
                        response.Message = "Concours " + model.Libelle + " modifié avec succès";
                        return Ok(response);
                    }

                    else
                    {
                        response.IsValid = false;
                        response.Message = "Erreur lors de la modification du concours" + model.Libelle;
                        return Ok(response);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(response);
                }
            }
            return BadRequest(response);
        }

        //GET : Parametrage/DetailsConcours/{Guid Id}
        public IActionResult DetailsConcours(Guid Id)
        {
            try
            {
                ViewBag.DAList = _admission.GetAllDA;
                var concours = _admission.GetConcours(Id);
                if (concours != null)
                {
                    return View(concours);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        #endregion

        #region Candidat
        public IActionResult Candidat()
        {
            var candidat = _admission.GetAllCandidat;
            return View(candidat);
        }

        //[Authorize(Roles = "Administrateur, Service Caisse & Opérations ")]

        [HttpGet]
        public IActionResult CreateCandidat()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCandidat(CandidatVM model)
        {
            CandidatVMValidator validator = new CandidatVMValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (_admission.VerifExistCandidat(model.Nom, model.Prenom, model.Telephone, model.Etablissement))
                    {
                        response.IsValid = false;
                        response.Message = "Ce candidat existe déjà";
                        return Ok(response);
                    }
                    if (_admission.CreateCandidat(model))
                    {
                        response.Data = model.CandidatId.ToString();
                        response.Message = "Candidat enregistré avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de la création du candidat";
                        return Ok(response);
                    }

                }
                catch (Exception ex)
                {

                    return BadRequest(response);
                }
            }

            return BadRequest(response);
        }

        //[Authorize(Roles = "Administrateur, Service Caisse & Opérations ")]

        [HttpGet]
        public IActionResult UpdateCandidat(Guid Id)
        {
            CandidatVM candidat = _admission.GetCandidatVM(Id);
            if (candidat != null)
            {
                return View(candidat);

            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult UpdateCandidat(CandidatVM model)
        {

            CandidatVMValidator validator = new CandidatVMValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (_admission.UpdateCandidat(model))
                    {
                        response.Message = "candidat mis à jour avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de l'édition du candidat";
                        return Ok(response);
                    }

                }
                catch (Exception ex)
                {

                    return BadRequest(response);
                }
            }

            return BadRequest(response);
        }

        [HttpGet]
        public IActionResult DeleteCandidat(Guid Id)
        {
            CandidatVM candidat = _admission.GetCandidatVM(Id);
            if (candidat != null)
            {
                return View(candidat);

            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult DeleteCandidat(CandidatVM model)
        {
            Candidat candidat = _admission.GetCandidat(model.CandidatId);
            CandidatVMValidator validator = new CandidatVMValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);
            if (_admission.DeleteCandidat(candidat))
            {
                response.Message = "candidat supprimé avec succès";
                return Ok(response);
            }
            else
            {
                response.IsValid = false;
                response.Message = "Echec lors de la suppression du candidat";
                return Ok(response);
            }
        }

        //GET : Parametrage/DetailsClasse/{Guid Id}
        public IActionResult DetailsCandidat(Guid Id)
        {
            try
            {
                ViewBag.DAList = _admission.GetAllDA;
                var candidat = _admission.GetCandidat(Id);
                if (candidat != null)
                {
                    return View(candidat);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        #endregion

        #region DossierPersonnel
        public ActionResult DossierPersonnel()
        {
            var dossiers = _admission.GetAllDossierPersonnel;
            return View(dossiers);
        }

        //[Authorize(Roles = "Administrateur, Service Caisse & Opérations ")]

        [HttpGet]
        public IActionResult CreateDossierPersonnel()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateDossierPersonnel(DossierPersonnelVM model)
        {
            DossierPersonnelVMValidator validator = new DossierPersonnelVMValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (_admission.CreateDossierPersonnel(model))
                    {
                        response.Message = "DossierPersonnel enregistré avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de la création du DossierPersonnel";
                        return Ok(response);
                    }

                }
                catch (Exception ex)
                {

                    return BadRequest(response);
                }
            }

            return BadRequest(response);
        }

        //[Authorize(Roles = "Administrateur, Service Caisse & Opérations ")]

        [HttpGet]
        public IActionResult UpdateDossierPersonnel(Guid Id)
        {
            DossierPersonnel dossier = _admission.GetDossierPersonnel(Id);
            if (dossier != null)
            {
                DossierPersonnelVM DossierpersonnelVM = new DossierPersonnelVM()
                {
                    DossierPersonnelId = Id,
                    ActeNaissance = dossier.ActeNaissance,
                    ReleveBac = dossier.ReleveBac,
                    ReleveMaster1 = dossier.ReleveMaster1,
                    ReleveBTS = dossier.ReleveBTS,
                    ReleveLicence = dossier.ReleveLicence,
                    CNI = dossier.CNI,
                    Candidat = dossier.Candidat,
                    Photos = dossier.Photos,
                    ReleveNiveau1 = dossier.ReleveNiveau1,
                    ReleveNiveau2 = dossier.ReleveNiveau2,
                };
                return View(DossierpersonnelVM);

            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult UpdateDossierPersonnel(DossierPersonnelVM model)
        {

            DossierPersonnelVMValidator validator = new DossierPersonnelVMValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (_admission.UpdateDossierPersonnel(model))
                    {
                        response.Message = "DossierPersonnel mis à jour avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de l'édition du DossierPersonnel";
                        return Ok(response);
                    }

                }
                catch (Exception ex)
                {

                    return BadRequest(response);
                }
            }

            return BadRequest(response);
        }


        [HttpGet]
        public IActionResult DeleteDossierPersonnel(Guid Id)
        {
            DossierPersonnel dossier = _admission.GetDossierPersonnel(Id);
            if (dossier != null)
            {
                return View(dossier);

            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult DeleteDossierPersonnel(DossierPersonnelVM model)
        {
            DossierPersonnel dossier = _admission.GetDossierPersonnel(model.DossierPersonnelId);
            DossierPersonnelVMValidator validator = new DossierPersonnelVMValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);
            if (_admission.DeleteDossierPersonnel(dossier))
            {
                response.Message = "DossierPersonnel supprimé avec succès";
                return Ok(response);
            }
            else
            {
                response.IsValid = false;
                response.Message = "Echec lors de la suppression du DossierPersonnel";
                return Ok(response);
            }
        }

        //GET : Parametrage/DetailsDossierPersonnel/{Guid Id}
        public IActionResult DetailsDossierPersonnel(Guid Id)
        {
            try
            {
                var DossierPersonnel = _admission.GetDossierPersonnel(Id);
                if (DossierPersonnel != null)
                {
                    return View(DossierPersonnel);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        #endregion

        #region Demande Admission

        //GET : Admission/DemandeAdmission
        [HttpGet]
        public IActionResult DemandeAdmission(Guid cycle, Guid specialite, Guid niveau, TypeAdmission? type, Decision? decision)
        {
            var demandeadmission = _admission.GetDAByFilter(cycle, specialite, niveau, type, decision);

            ViewBag.choixcycle = _param.GetCycle(cycle);
            ViewBag.choixspecialite = _param.GetFiliere(specialite);
            ViewBag.choixniveau = _param.GetNiveau(niveau);
            ViewBag.choixtype = type;
            ViewBag.choixdecision = decision;
            ViewBag.Classe = _param.GetAllClasse;
            //ViewBag.Campus = _param.GetAllCampus;
            ViewBag.Cycle = _param.GetAllCycle;
            ViewBag.Niveau = _param.GetAllNiveau;
            ViewBag.Filiere = _param.GetAllFiliere;
            return View(demandeadmission);
        }

        //GET : Admission/CreateDemandeAdmission

        //[Authorize(Roles = "Service Caisse & Opérations, Support Informatique, Administrateur")]
        [HttpGet]
        public IActionResult CreateDemandeAdmission(string Id)
        {
            var candidat = _admission.GetCandidat(new Guid(Id));
            if (candidat != null)
            {
                ViewBag.AnneeList = _admission.GetAllAnneeAcademique;
                ViewBag.ConcoursList = _admission.GetAllConcours;
                ViewBag.ClasseList = _param.GetAllClasse;
                DemandeAdmission demande = new DemandeAdmission()
                {
                    CandidatId = candidat.Id,
                    Candidat = candidat,
                };
                return View(demande);
            }
            return NotFound();
        }

        //POST : Scolarite/CreateInscription
        public IActionResult CreateDemandeAdmission(DemandeAdmission model)
        {
            ViewBag.ConcoursList = _admission.GetAllConcours;
            ViewBag.AnneeList = _admission.GetAllAnneeAcademique;
            ViewBag.CandidatList = _admission.GetAllCandidat;
            ViewBag.ClasseList = _param.GetAllClasse;
            ViewBag.CycleList = _param.GetAllCycle;
            ResponseVM response = new ResponseVM();
            if (model.TypeAdmission == TypeAdmission.EtudeDossier)
            {
                DemandeAdmissionValidator validator = new DemandeAdmissionValidator();
                ValidationResult validationResult = validator.Validate(model);
                response = FluentUtilities.GetValidationError(validationResult);
            }
            else
            {
                DemandeAdmissionConcoursValidator validator = new DemandeAdmissionConcoursValidator();
                ValidationResult validationResult = validator.Validate(model);
                response = FluentUtilities.GetValidationError(validationResult);
            }
            if (response.IsValid)
            {

                try
                {
                    if (model.TypeAdmission == TypeAdmission.Concours)
                    {
                        if (_admission.VerifExistDAConcours(model.CandidatId, model.ConcoursId))
                        {
                            response.IsValid = false;
                            response.Message = "Un candidat ne peut pas postuler deux fois à un mème concours";
                            return Ok(response);
                        }
                    }
                    else if (model.TypeAdmission == TypeAdmission.EtudeDossier)

                    {
                        if (_admission.ExistDAEtudeDossier(model.ClasseId, model.CandidatId, model.AnneeAcademiqueId))
                        {
                            response.IsValid = false;
                            response.Message = "Un candidat ne peut pas postuler deux fois par étude de dossier pour la mème classe en une année";
                            return Ok(response);
                        }

                    }
                    if (_admission.CreateDA(model))
                    {
                        response.Data = model.Id.ToString();
                        response.Message = "Nouvelle demande d'admission enregistrée avec succès";
                        return Ok(response);
                    }
                    else
                    {

                        response.IsValid = false;
                        response.Message = "Echec lors de la création de la demande d'admission";
                        return Ok(response);
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(response);
                }

            }
            return BadRequest(response);
        }

        //GET : Admission/UpdateDemandeAdmission/{Guid Id}

        //[Authorize(Roles = "Service Caisse & Opérations , Support Informatique , Administrateur")]
        public IActionResult UpdateDemandeAdmission(Guid Id)
        {
            var DemandeAdmission = _admission.GetDA(Id);
            if (DemandeAdmission != null)
            {
                ViewBag.ConcoursList = _admission.GetAllConcours;
                ViewBag.AnneeList = _admission.GetAllAnneeAcademique;
                ViewBag.CandidatList = _admission.GetAllCandidat;
                ViewBag.ClasseList = _param.GetAllClasse;
                ViewBag.CycleList = _param.GetAllCycle;
                return View(DemandeAdmission);
            }
            return NotFound();
        }

        //POST : Admission/UpdateDemandeAdmission/{DemandeAdmission model}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateDemandeAdmission(DemandeAdmission model)
        {
            ViewBag.ConcoursList = _admission.GetAllConcours;
            ViewBag.AnneeList = _admission.GetAllAnneeAcademique;
            ViewBag.CandidatList = _admission.GetAllCandidat;
            ViewBag.ClasseList = _param.GetAllClasse;
            ViewBag.CycleList = _param.GetAllCycle;

            ResponseVM response = new ResponseVM();
            if (model.TypeAdmission == TypeAdmission.EtudeDossier)
            {
                DemandeAdmissionValidator validator = new DemandeAdmissionValidator();
                ValidationResult validationResult = validator.Validate(model);
                response = FluentUtilities.GetValidationError(validationResult);
            }
            else
            {
                DemandeAdmissionConcoursValidator validator = new DemandeAdmissionConcoursValidator();
                ValidationResult validationResult = validator.Validate(model);
                response = FluentUtilities.GetValidationError(validationResult);
            }
            if (response.IsValid)
            {
                try
                {
                    if (model.TypeAdmission == TypeAdmission.Concours)
                    {
                        if (_admission.VerifExistDAConcours(model.CandidatId, model.ConcoursId))
                        {
                            response.IsValid = false;
                            response.Message = "Un candidat ne peut pas postuler deux fois à un mème concours";
                            return Ok(response);
                        }
                    }
                    else if (model.TypeAdmission == TypeAdmission.EtudeDossier)

                    {
                        if (_admission.ExistDAEtudeDossier(model.ClasseId, model.CandidatId, model.AnneeAcademiqueId))
                        {
                            response.IsValid = false;
                            response.Message = "Un candidat ne peut pas postuler deux fois par étude de dossier pour la mème classe";
                            return Ok(response);
                        }

                    }
                    if (_admission.UpdateDA(model))
                    {
                        response.Message = "la Demande d'Admission à été modifiée avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de la modification de la DemandeAdmission";
                        return Ok(response);
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(response);
                }

            }
            return BadRequest(response);

        }

        //GET : Admission/UpdateStatutAdmission/{Guid Id}

        //[Authorize(Roles = "Service Caisse & Opérations , Support Informatique , Administrateur")]

        public IActionResult UpdateStatutAdmission(Guid Id)
        {

            DemandeAdmission demandeAdmission = _admission.GetDA(Id);
            if (demandeAdmission != null)
            {
                ViewBag.ConcoursList = _admission.GetAllConcours;
                ViewBag.AnneList = _admission.GetAllAnneeAcademique;
                ViewBag.CandidatList = _admission.GetAllCandidat;
                ViewBag.ClasseList = _param.GetAllClasse;
                ViewBag.CycleList = _param.GetAllCycle;
                return View(demandeAdmission);
            }
            return NotFound();
        }

        //POST : Admission/UpdateStatutAdmission/{DemandeAdmission model}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatutAdmission(DemandeAdmission model)
        {
            ResponseVM response = new ResponseVM();
            if (model.TypeAdmission == TypeAdmission.EtudeDossier)
            {
                DemandeAdmissionValidator validator = new DemandeAdmissionValidator();
                ValidationResult validationResult = validator.Validate(model);
                response = FluentUtilities.GetValidationError(validationResult);
            }
            else
            {
                DemandeAdmissionConcoursValidator validator = new DemandeAdmissionConcoursValidator();
                ValidationResult validationResult = validator.Validate(model);
                response = FluentUtilities.GetValidationError(validationResult);
            }
            if (response.IsValid)
            {
                try
                {
                    if (_admission.VerifPaiementAllFraisAdmission(model))
                    {
                        response.IsValid = false;
                        response.Message = "Ce candidat n'a pas encore payé la totalité des frais d'admission. Son statut ne peut donc pas être modifié.";
                        return Ok(response);
                    }
                    if (_admission.UpdateStatutAdmission(model))
                    {
                        response.IsValid = true;
                        response.Message = "le statut d'admission à été modifié avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        ViewBag.ConcoursList = _admission.GetAllConcours;
                        ViewBag.AnneList = _admission.GetAllAnneeAcademique;
                        ViewBag.CandidatList = _admission.GetAllCandidat;
                        ViewBag.ClasseList = _param.GetAllClasse;
                        ViewBag.CycleList = _param.GetAllCycle; response.IsValid = false;
                        response.Message = "Echec lors de la modification du statut d'admission";
                        return Ok(response);
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(response);
                }
            }
            return BadRequest(response);

        }

        //GET : Admission/DeleteDemandeAdmission/{Guid Id}
        public IActionResult DeleteDemandeAdmission(Guid Id)
        {
            var DemandeAdmission = _admission.GetDA(Id);
            if (DemandeAdmission != null)
            {
                return View(DemandeAdmission);
            }
            return NotFound();
        }

        //POST : Admission/DeleteDemandeAdmission/{DemandeAdmission model}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteDemandeAdmission(DemandeAdmission model)
        {
            ResponseVM response = new ResponseVM();
            try
            {
                if (_admission.DeleteDA(model))
                {
                    response.Message = "la Demande d'Admission à été supprimée avec succès";
                    return Ok(response);
                }
                else
                {
                    response.IsValid = false;
                    response.Message = "Echec lors de la suppression de la Demande d'Admission";
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(response);
            }

        }

        //GET : Admission/DetailsDemandeAdmission/{Guid Id}
        public IActionResult DetailsDemandeAdmission(Guid Id)
        {
            try
            {
                var DemandeAdmission = _admission.GetDA(Id);
                if (DemandeAdmission != null)
                {
                    return View(DemandeAdmission);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        //[HttpGet("Admission/DemandesAdmissionParCycle/{CycleId}")]

        //public IActionResult DemandesAdmissionParCycle(Guid CycleId)
        //{
        //    ViewBag.ChoixDuCycle = _param.GetCycle(CycleId);
        //    var demandesParCycle = _admission.GetDAParCycle(CycleId);
        //    return View(demandesParCycle);
        //}


        //[HttpGet("Admission/DemandesAdmissionParClasse/{ClasseId}")]

        //public IActionResult DemandesAdmissionParClasse(Guid ClasseId)
        //{
        //    ViewBag.ChoixDelaclasse = _param.GetClasse(ClasseId);
        //    var demandesParClasse = _admission.GetDAParClasse(ClasseId);
        //    return View(demandesParClasse);
        //}

        #endregion

        #region Paiement Et reçu
        public IActionResult Paiement(DateTime dateDebut, DateTime dateFin)
        {
            if (dateDebut == DateTime.MinValue)
            {
                dateDebut = DateTime.Now.Date;
            }
            if (dateFin == DateTime.MinValue)
            {
                dateFin = DateTime.Now.Date;
            }

            var paiements = _admission.PaiementsParPeriode(dateDebut, dateFin);
            ViewBag.Paiements = _admission.PaiementsParPeriode(dateDebut, dateFin);
            ViewBag.dateDebut = dateDebut;
            ViewBag.dateFin = dateFin;
            return View(paiements);
        }

        [Authorize(Roles = "Service Caisse & Opérations , Administrateur")]

        [HttpGet]
        public IActionResult CreatePaiement(String id)
        {
            DemandeAdmission demandeAdmission = _admission.GetDA(new Guid(id));
            if (demandeAdmission != null)
            {
                var paiement = new Paiement();
                {
                    paiement.DemandeAdmissionId = demandeAdmission.Id;
                    paiement.DemandeAdmission = demandeAdmission;
                }
                ViewBag.FraisEtudeDossier = _admission.GetAllFraisEtudeDossier;
                ViewBag.FraisConcours = _admission.GetAllFraisConcours;
                return View(paiement);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePaiement(Paiement model)
        {
            PaiementValidator validator = new PaiementValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {

                    if (model.Montant <= 0)
                    {
                        //si le montant entrer est negatif
                        response.IsValid = false;
                        response.Message = "Impossible d'enregistrer un montant négatif ou null";
                        return Ok(response);
                    }
                    double? resteFraisAdmission;
                    if (model.DemandeAdmission.TypeAdmission == TypeAdmission.EtudeDossier)
                    {
                        resteFraisAdmission = _admission.ResteFraisEtdudeDossier(model.DemandeAdmissionId, DateTime.UtcNow);
                    }
                    else
                    {
                        resteFraisAdmission = _admission.ResteFraisconcours(model.DemandeAdmissionId, DateTime.UtcNow);
                    }
                    if (resteFraisAdmission == 0)
                    {
                        response.IsValid = false;
                        response.Message = "Cet étudiant a déjà réglé la totalité des frais d'admission ";
                        return Ok(response);
                    }
                    double? montant = model.Montant;
                    if (resteFraisAdmission < model.Montant)
                    {
                        montant = resteFraisAdmission;
                        response.Message = $"<strong> Vous devez rembourser {model.Montant - resteFraisAdmission} <strong><br/>";
                    }
                    if (montant != 0)
                    {
                        Paiement paiementFraisAdmission = new Paiement()
                        {
                            DemandeAdmissionId = model.DemandeAdmissionId,
                            Montant = montant,
                            Motif = model.Motif,
                        };

                        if (_admission.CreatePaiement(paiementFraisAdmission))
                        {
                            response.Data = paiementFraisAdmission.Id.ToString();
                            response.Message = response.Message + "Paiement des frais de demande d'admission effectué avec succès !!";
                            return Ok(response);
                        }
                        else
                        {
                            response.IsValid = false;
                            response.Message = "Echec lors du paiement des frais de demande d'admission ";
                            return Ok(response);
                        }
                    }
                    else
                    {
                        return Ok(response);
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(response);
                }
            }
            return BadRequest(response);
        }

        //GET : Admission/PrintRecuPaiement/{Guid Id}

        ////[Authorize(Roles = "Service Caisse & Opérations , Administrateur")]

        public IActionResult PrintRecuPaiement(Guid Id)
        {
            Paiement paiement = _admission.GetPaiement(Id);
            if (paiement != null)
            {
                string webroot = _webHostEnvironment.WebRootPath;
                string path = "";
                if (paiement.AddedDate.Date == DateTime.Now.Date && paiement.AddedDate.Hour == DateTime.Now.Hour && paiement.AddedDate.Minute == DateTime.Now.Hour)
                {
                    path = Path.Combine(webroot, "docs/Pdf/recu.pdf");
                }
                else
                {
                    path = Path.Combine(webroot, "docs/Pdf/Duplicata.pdf");
                }
                if (System.IO.File.Exists(path))
                {
                    var SourceFileStream = System.IO.File.OpenRead(path);
                    var outputStream = new MemoryStream();

                    var pdf = new PdfDocument(new PdfReader(SourceFileStream), new PdfWriter(outputStream));
                    PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, false);
                    if (form != null)
                    {
                        IDictionary<String, PdfFormField> fields = form.GetFormFields();
                        PdfFormField? toset = null;
                        double? somme = 0, reste = 0;
                        string montant;
                        foreach (var paie in _admission.GetAllpaiement)
                        {
                            if (paiement.DemandeAdmission.Id == paie.DemandeAdmission.Id && paie.Motif == paiement.Motif)
                            {
                                somme += paie.Montant;
                            }
                        }
                        if (paiement.DemandeAdmission.TypeAdmission == TypeAdmission.Concours)
                        {
                            reste = _admission.ResteFraisconcours(paiement.DemandeAdmission.Id, paiement.AddedDate);
                        }
                        else
                        {
                            reste = _admission.ResteFraisEtdudeDossier(paiement.DemandeAdmission.Id, paiement.AddedDate); ;
                        }
                        if (paiement.Montant != null)
                        {
                            fields.TryGetValue("Montant", out toset);
                            toset.SetValue(paiement.Montant.ToString()).SetReadOnly(true);
                            fields.TryGetValue("Montant2", out toset);
                            toset.SetValue(paiement.Montant.ToString()).SetReadOnly(true);
                        }

                        if (paiement.DemandeAdmission.Candidat.Code != null)
                        {
                            fields.TryGetValue("Matricule", out toset);
                            toset.SetValue(paiement.DemandeAdmission.Candidat.Code).SetReadOnly(true);
                            fields.TryGetValue("Matricule2", out toset);
                            toset.SetValue(paiement.DemandeAdmission.Candidat.Code).SetReadOnly(true);
                        }

                        if (paiement.DemandeAdmission.AnneeAcademique.AnneeDebut != null)
                        {
                            fields.TryGetValue("Année", out toset);
                            toset.SetValue(paiement.DemandeAdmission.AnneeAcademique.AnneeDebut + "/" + paiement.DemandeAdmission.AnneeAcademique.AnneeFin).SetReadOnly(true);
                            fields.TryGetValue("Année2", out toset);
                            toset.SetValue(paiement.DemandeAdmission.AnneeAcademique.AnneeDebut + "/" + paiement.DemandeAdmission.AnneeAcademique.AnneeFin).SetReadOnly(true);
                        }

                        if (paiement.DemandeAdmission.Candidat.Personne.Nom != null)
                        {
                            fields.TryGetValue("Nom", out toset);
                            toset.SetValue(paiement.DemandeAdmission.Candidat.Personne.Nom + " " + paiement.DemandeAdmission.Candidat.Personne.Prenom).SetReadOnly(true);
                            fields.TryGetValue("Nom2", out toset);
                            toset.SetValue(paiement.DemandeAdmission.Candidat.Personne.Nom + " " + paiement.DemandeAdmission.Candidat.Personne.Prenom).SetReadOnly(true);
                        }

                        if (paiement.DemandeAdmission.Classe.Filiere.Libelle != null)
                        {
                            fields.TryGetValue("Filiere", out toset);
                            toset.SetValue(paiement.DemandeAdmission.Classe.Filiere.Libelle).SetReadOnly(true);
                            fields.TryGetValue("Filiere2", out toset);
                            toset.SetValue(paiement.DemandeAdmission.Classe.Filiere.Libelle).SetReadOnly(true);
                        }

                        if (paiement.DemandeAdmission.Classe.Niveau.Libelle != null)
                        {
                            fields.TryGetValue("Niveau", out toset);
                            toset.SetValue(paiement.DemandeAdmission.Classe.Niveau.Libelle).SetReadOnly(true);
                            fields.TryGetValue("Niveau2", out toset);
                            toset.SetValue(paiement.DemandeAdmission.Classe.Niveau.Libelle).SetReadOnly(true);
                        }

                        if (paiement.Montant != null)
                        {
                            montant = _admission.NumberToWords(int.Parse(paiement.Montant.ToString()));
                            fields.TryGetValue("somme", out toset);
                            toset.SetValue(montant).SetReadOnly(true);
                            fields.TryGetValue("Somme2", out toset);
                            toset.SetValue(montant).SetReadOnly(true);
                        }

                        if (paiement.Motif != null)
                        {
                            string val = "";
                            if (paiement.Motif == Motif.FraisConcours)
                            {
                                val = "Paiement des frais de concours";
                            }
                            else
                            {
                                val = "Paiement des frais d'étude de dossier";
                            }
                            fields.TryGetValue("Motif", out toset);
                            toset.SetValue(val).SetReadOnly(true);
                            fields.TryGetValue("Motif2", out toset);
                            toset.SetValue(val).SetReadOnly(true);
                        }

                        fields.TryGetValue("Avance", out toset);
                        toset.SetValue(somme.ToString()).SetReadOnly(true);
                        fields.TryGetValue("Avance2", out toset);
                        toset.SetValue(somme.ToString()).SetReadOnly(true);

                        fields.TryGetValue("Reste", out toset);
                        toset.SetValue(reste.ToString()).SetReadOnly(true);
                        fields.TryGetValue("Reste2", out toset);
                        toset.SetValue(reste.ToString()).SetReadOnly(true);

                        var campusName = HttpContext.Session.GetString(SessionInfo.CurrentCampusName);
                        if (campusName != null)
                        {
                            fields.TryGetValue("Lieu", out toset);
                            toset.SetValue(campusName.ToString()).SetReadOnly(true);//"Campus "+
                            fields.TryGetValue("Lieu2", out toset);
                            toset.SetValue(campusName.ToString()).SetReadOnly(true);
                        }

                        fields.TryGetValue("Date", out toset);
                        toset.SetValue(paiement.AddedDate.ToString()).SetReadOnly(true);
                        fields.TryGetValue("Date2", out toset);
                        toset.SetValue(paiement.AddedDate.ToString()).SetReadOnly(true);

                        pdf.Close();
                        byte[] bytes = outputStream.ToArray();
                        return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, paiement.DemandeAdmission.Candidat.Personne.Nom.ToString() + "_" + paiement.DemandeAdmission.TypeAdmission.ToString() + "_Recu.pdf");
                    }
                }
                return NotFound();

            }
            else
            {
                return NotFound();
            }
        }


        //GET : Admission/GetPaiement/{Guid Id}
        public IActionResult DetailsPaiement(Guid Id)
        {
            try
            {
                Paiement paiement = _admission.GetPaiement(Id);
                if (paiement != null)
                {
                    return View(paiement);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        #endregion
    }
}



