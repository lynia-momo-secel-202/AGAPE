using GestAgape.Service.Admissions;
using GestAgape.Service.Parametrages;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using GestAgape.Core.Entities.Admission;
using GestAgape.Core.ViewModels;
using GestAgape.Core.ViewModels.FluentValidators;
using GestAgape.Infrastructure.Utilities;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Org.BouncyCastle.Bcpg;
using GestAgape.Service.Scolarite;
using GestAgape.Core.Entities.Scolarite;
using Azure;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using FluentValidation;
using System.Reflection.Metadata;
using GestAgape.Core.Entities.Parametrage;
using System.Linq;
using iText.Barcodes.Dmcode;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.X509;

namespace GestAgape.Controllers
{
    public class ScolariteController : Controller
    {
        #region Membres prives

        private readonly IScolarite _scolarite;
        private readonly IAdmission _admission;
        private readonly IEmailSender _emailSender;
        private readonly IParametrage _param;
        private readonly IWebHostEnvironment _webHostEnvironment;

        #endregion

        #region Constructeur
        public ScolariteController(IScolarite scolarite, IAdmission admission, IEmailSender emailSender, IParametrage param, IWebHostEnvironment webHostEnvironment)
        {
            _scolarite = scolarite;
            _admission = admission;
            _emailSender = emailSender;
            _param = param;
            _webHostEnvironment = webHostEnvironment;
        }

        #endregion

        #region Etudiant

        //GET : Scolarite/Etudiant
        [HttpGet]
        public IActionResult Etudiant(Guid cycle, Guid specialite, Guid niveau, DateTime? datedebut, DateTime? datefin)
        {
            var inscription = _scolarite.GetInscriptByFilter(cycle, specialite, niveau, datedebut, datefin);
            ViewBag.choixcycle = _param.GetCycle(cycle);
            ViewBag.choixspecialite = _param.GetFiliere(specialite);
            ViewBag.choixniveau = _param.GetNiveau(niveau);
            ViewBag.choixdatedebut = datedebut;
            ViewBag.choixdatefin = datefin;
            ViewBag.Classe = _param.GetAllClasse;
            ViewBag.Campus = _param.GetAllCampus;
            ViewBag.Cycle = _param.GetAllCycle;
            ViewBag.Niveau = _param.GetAllNiveau;
            ViewBag.Filiere = _param.GetAllFiliere;
            if (datedebut > datefin)
            {


            }
            return View(inscription);
        }

        //[Authorize(Roles = "Support Informatique , Administrateur, Service Caisse & Opérations ")]

        [HttpGet]
        public IActionResult UpdateEtudiant(Guid Id)
        {
            ViewBag.InscriptionList = _scolarite.GetAllInscription;
            CandidatVM candidat = _admission.GetCandidatVM(Id);
            if (candidat != null)
            {
                return View(candidat);

            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult UpdateEtudiant(CandidatVM model)
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
                        response.Message = "Etudiant mis à jour avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de l'édition de l'étudiant";
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

        [HttpGet]
        public IActionResult EtatScolarite(Guid cycle, Guid specialite, Guid niveau)
        {
            var inscription = _scolarite.GetInscriptByFilter(cycle, specialite, niveau, null, null);

            ViewBag.choixcycle = _param.GetCycle(cycle);
            ViewBag.choixspecialite = _param.GetFiliere(specialite);
            ViewBag.choixniveau = _param.GetNiveau(niveau);
            //ViewBag.Classe = _param.GetAllClasse;
            ViewBag.FraisScolarite = _scolarite.GetAllTrancheScolarite;
            ViewBag.Cycle = _param.GetAllCycle;
            ViewBag.Paiement = _admission.GetAllpaiement;
            ViewBag.Filiere = _param.GetAllFiliere;
            ViewBag.Niveau = _param.GetAllNiveau;
            ViewBag.Bourse = _scolarite.GetAllBourses;
            return View(inscription);
        }

        public IActionResult DetailEtudiant(Guid Id)
        {
            try
            {
                ViewBag.DAList = _admission.GetAllDA;
                ViewBag.Inscription = _scolarite.GetAllInscription;
                var inscription = _scolarite.GetInscription(Id);
                if (inscription != null)
                {
                    return View(inscription);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        #endregion

        #region Inscription

        //[Authorize(Roles = "Administrateur, Service Caisse & Opérations ")]

        [HttpGet]
        public IActionResult CreateInscription(string Id)
        {
            string campus = HttpContext.Session.GetString(SessionInfo.CurrentCampusId);
            //ViewBag.DAList = _admission.GetAllDA;
            ViewBag.FraisInscriptionList = _scolarite.GetAllFraisInscription;
            var demande = _admission.GetDA(new Guid(Id));
            if (demande != null)
            {

                Matricule matricule = new Matricule()
                {


                };

                Paiement paiement = new Paiement()
                {
                    DemandeAdmissionId = demande.Id,
                    DemandeAdmission = demande,
                };

                Inscription inscription = new Inscription()
                {
                    Matricule = matricule,
                    MatriculeId = matricule.Id,
                    DemandeAdmission = demande

                };

                return View(paiement);

            };
            return NotFound();

        }

        [HttpPost]
        public IActionResult CreateInscription(Matricule matricule, Paiement paiement, Inscription inscription)
        {
            String campus = HttpContext.Session.GetString(SessionInfo.CurrentCampusId);
            //ViewBag.DAList = _admission.GetAllDA;
            ViewBag.FraisInscriptionList = _scolarite.GetAllFraisInscription;
            ResponseVM response = new ResponseVM();
            PaiementValidator validator = new PaiementValidator();
            ValidationResult validationResult = validator.Validate(paiement);
            response = FluentUtilities.GetValidationError(validationResult);
            if (response.IsValid)
            {
                DemandeAdmission d = _admission.GetDA(paiement.DemandeAdmissionId);
                try
                {
                    if (_scolarite.VerifExistInscription(paiement.DemandeAdmissionId))
                    {
                        response.IsValid = false;
                        response.Message = "Cet étudiant est déjà inscrit." +
                            " Pour compléter les frais, allez sur le menu paiement des frais de scolarité.";
                        return Ok(response);

                    }
                    if (_scolarite.StatutAdmission(d))
                    {
                        response.IsValid = false;
                        response.Message = "Le candidat que vous souhaitez inscrire n'est pas admis pour cette demande d'admission";
                        return Ok(response);
                    }
                    var resteFraisInscript = _scolarite.ResteFraisInscription(paiement.DemandeAdmissionId, DateTime.UtcNow);
                    double? montant = 0;

                    if (resteFraisInscript < paiement.Montant)
                    {
                        montant = resteFraisInscript;
                        paiement.Montant = montant;
                        response.Message = $"<strong> Vous devez rembourser {paiement.Montant - resteFraisInscript} <strong><br/>";
                    }
                    if (paiement.Montant < 0)
                    {
                        response.IsValid = false;
                        response.Message = "Impossible d'enregistrer un montant négatif";
                        return Ok(response);
                    }
                    if (_scolarite.CreateInscription(matricule, inscription, paiement, campus))
                    {
                        response.Data = paiement.Id.ToString();
                        response.Message = "Inscription effectuée avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de l'inscription de l'étudiant";
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

        //[Authorize(Roles = "Support Informatique , Administrateur, Service Caisse & Opérations ")]
        public IActionResult PrintRecuPaiementInscription(Guid Id)
        {
            Paiement paiement = _admission.GetPaiement(Id);
            if (paiement != null)
            {
                double? montant = 0, reste = 0, fraisInsc = 0;

                reste = _scolarite.ResteFraisInscription(paiement.DemandeAdmissionId, paiement.AddedDate);

                foreach (FraisInscription f in _scolarite.GetAllFraisInscription)
                {
                    if (f.ClasseId == paiement.DemandeAdmission.ClasseId && f.AnneeAcademiqueId == paiement.DemandeAdmission.AnneeAcademiqueId)
                    {
                        fraisInsc = f.Montant;

                    }
                }
                montant = fraisInsc - reste;
                var recu = PrintRecuPaiement(paiement, montant, reste);

                if (recu != null)
                {
                    return recu;
                }
                return NotFound();
            }
            else
            {
                return NotFound();
            }
        }

        #endregion

        #region FraisInscription
        public IActionResult FraisInscription()
        {
            var fraisInscription = _scolarite.GetAllFraisInscription;
            return View(fraisInscription);
        }

        //[Authorize(Roles = "Support Informatique , Administrateur")]

        //GET : Parametrage/CreateFraisInscription
        [HttpGet]
        public IActionResult CreateFraisInscription()
        {
            ViewBag.IPESList = _scolarite.GetAllFraisInscription;
            ViewBag.ClasseList = _param.GetAllClasse;
            ViewBag.Anneeacademique = _admission.GetAllAnneeAcademique;
            return View();
        }

        //POST : Parametrage/CreateFraisInscription/{FraisInscription model}
        [HttpPost]
        public IActionResult CreateFraisInscription(FraisInscription model)
        {
            ViewBag.IPESList = _scolarite.GetAllFraisInscription;
            ViewBag.ClasseList = _param.GetAllClasse;
            ViewBag.Anneeacademique = _admission.GetAllAnneeAcademique;
            FraisInscriptionValidator validator = new FraisInscriptionValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (_scolarite.VerifFraisInscription(model))
                    {
                        response.IsValid = false;
                        response.Message = "Les frais d'inscription ont déjà été enregistrés pour cette classe";
                        return Ok(response);
                    }
                    if (_scolarite.CreateFraisInscription(model))
                    {
                        response.Message = "Frais d'inscription enregistrés avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de la création des frais d'inscription";
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

        //GET : Parametrage/UpdateFraisInscription/{Guid Id}
        [HttpGet]
        public IActionResult UpdateFraisInscription(Guid Id)
        {

            ViewBag.IPESList = _scolarite.GetAllFraisInscription;
            ViewBag.ClasseList = _param.GetAllClasse;
            ViewBag.Anneeacademique = _admission.GetAllAnneeAcademique;
            var fraisInscription = _scolarite.GetFraisInscription(Id);
            if (fraisInscription != null)
            {
                //ViewBag.IPESList = _scolarite.GetAllFraisInscription;
                return View(fraisInscription);
            }
            return NotFound();
        }

        //POST : Parametrage/UpdateFraisInscription/{FraisInscription model}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateFraisInscription(FraisInscription model)
        {
            //model.IPAddress = HttpContext.Session.GetString(SessionInfo.IPAddress) ?? "";
            FraisInscriptionValidator validator = new FraisInscriptionValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = new ResponseVM();
            if (response.IsValid)
            {
                try
                {
                    if (_scolarite.VerifFraisInscription(model))
                    {
                        response.IsValid = false;
                        response.Message = "Les frais d'inscription ont déjà été enregistrés pour cette classe";
                        return Ok(response);
                    }
                    if (_scolarite.UpdateFraisInscription(model))
                    {
                        response.Message = "les frais ont été modifié avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        //ViewBag.IPESList = _scolarite.GetAllFraisInscription;
                        response.IsValid = false;
                        response.Message = "Echec lors de la création des frais d'inscription";
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

        //GET : Parametrage/DeleteFraisInscription/{Guid Id}
        public IActionResult DeleteFraisInscription(Guid Id)
        {
            var fraisInscription = _scolarite.GetFraisInscription(Id);
            if (fraisInscription != null)
            {
                return View(fraisInscription);
            }
            return NotFound();
        }

        //POST : Parametrage/DeleteFraisInscription/{FraisInscription model}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFraisInscription(FraisInscription model)
        {
            //model.IPAddress = HttpContext.Session.GetString(SessionInfo.IPAddress) ?? "";
            ResponseVM response = new ResponseVM();
            try
            {
                if (_scolarite.DeleteFraisInscription(model))
                {
                    response.Message = "les frais d'inscription ont été supprimé avec succès";
                    return Ok(response);
                }
                else
                {
                    response.IsValid = false;
                    response.Message = "Echec lors de la suppression des frais d'inscription";
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(response);
            }

        }

        //GET : Parametrage/DetailsFraisInscription/{Guid Id}
        public IActionResult DetailsFraisInscription(Guid Id)
        {
            try
            {
                var fraisInscription = _scolarite.GetFraisInscription(Id);
                if (fraisInscription != null)
                {
                    return View(fraisInscription);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        #endregion

        #region FraisMedicaux

        public IActionResult FraisMedicaux()
        {
            var fraisMedicaux = _scolarite.GetAllFraisMedicaux;
            return View(fraisMedicaux);
        }

        //[Authorize(Roles = "Support Informatique , Administrateur")]

        //GET : Parametrage/CreateFraisMedicaaux
        [HttpGet]
        public IActionResult CreateFraisMedicaux()
        {
            ViewBag.Campus = _param.GetAllCampus;
            ViewBag.AnneeAcademique = _admission.GetAllAnneeAcademique;
            return View();
        }

        //POST : Parametrage/CreateFraisMedicaux/{FraisMedicaux model}
        [HttpPost]
        public IActionResult CreateFraisMedicaux(FraisMedicaux model)
        {
            ViewBag.Campus = _param.GetAllCampus;
            FraisMedicauxValidator validator = new FraisMedicauxValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (_scolarite.VerifFraisMedicaux(model))
                    {
                        response.IsValid = false;
                        response.Message = "Les frais médicaux ont déjà été enregistrés pour ce campus";
                        return Ok(response);
                    }
                    if (_scolarite.CreateFraisMedicaux(model))
                    {
                        response.Message = "Frais médicaux créés avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de la création des frais médicaux";
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

        //GET : Parametrage/UpdateFraisMedicaux/{Guid Id}
        public IActionResult UpdateFraisMedicaux(Guid Id)
        {
            ViewBag.IPESList = _scolarite.GetAllFraisMedicaux;
            ViewBag.Campus = _param.GetAllCampus;
            ViewBag.AnneeAcademique = _admission.GetAllAnneeAcademique;
            var fraisMedicaux = _scolarite.GetFraisMedicaux(Id);
            if (fraisMedicaux != null)
            {
                //ViewBag.IPESList = _scolarite.GetAllFraisMedicaux;
                return View(fraisMedicaux);
            }
            return NotFound();
        }

        //POST : Parametrage/UpdateFraisMedicaux/{FraisMedicaux model}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateFraisMedicaux(FraisMedicaux model)
        {
            //model.IPAddress = HttpContext.Session.GetString(SessionInfo.IPAddress) ?? "";
            FraisMedicauxValidator validator = new FraisMedicauxValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = new ResponseVM();
            if (response.IsValid)
            {
                try
                {
                    if (_scolarite.UpdateFraisMedicaux(model))
                    {
                        response.Message = "les frais ont été modifié avec succès";
                        return Ok(response);
                    }
                    if (_scolarite.UpdateFraisMedicaux(model))
                    {
                        response.Message = "les frais ont été modifié avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        //ViewBag.IPESList = _scolarite.GetAllFraisMedicaux;
                        response.IsValid = false;
                        response.Message = "Echec lors de la création des frais médicaux";
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

        //GET : Parametrage/DeleteFraisMedicaux/{Guid Id}
        public IActionResult DeleteFraisMedicaux(Guid Id)
        {
            var fraisMedicaux = _scolarite.GetFraisMedicaux(Id);
            if (fraisMedicaux != null)
            {
                return View(fraisMedicaux);
            }
            return NotFound();
        }

        //POST : Parametrage/DeleteFraisMedicaux/{FraisMedicaux model}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFraisMedicaux(FraisMedicaux model)
        {
            //model.IPAddress = HttpContext.Session.GetString(SessionInfo.IPAddress) ?? "";
            ResponseVM response = new ResponseVM();
            try
            {
                if (_scolarite.DeleteFraisMedicaux(model))
                {
                    response.Message = "les frais médicaux ont été supprimé avec succès";
                    return Ok(response);
                }
                else
                {
                    response.IsValid = false;
                    response.Message = "Echec lors de la suppression des frais médicaux";
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(response);
            }

        }

        //GET : Parametrage/DetailsFraisMedicaux/{Guid Id}
        public IActionResult DetailsFraisMedicaux(Guid Id)
        {
            try
            {
                var fraisMedicaux = _scolarite.GetFraisMedicaux(Id);
                if (fraisMedicaux != null)
                {
                    return View(fraisMedicaux);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        //[Authorize(Roles = "Administrateur, Service Caisse & Opérations")]

        ////[Authorize(Roles = "Service Caisse & Opérations , Administrateur")]
        [HttpGet]
        public IActionResult CreatePaiementFraisMedicaux(String id)
        {
            Inscription inscription = _scolarite.GetInscription(new Guid(id));
            ViewBag.frais = _scolarite.GetAllFraisMedicaux;
            if (inscription != null)
            {
                var paiement = new Paiement();
                {
                    paiement.DemandeAdmissionId = inscription.DemandeAdmission.Id;
                    paiement.DemandeAdmission = inscription.DemandeAdmission;

                }
                return View(paiement);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePaiementFraisMedicaux(Paiement model)
        {
            PaiementValidator validator = new PaiementValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);
            string campus = HttpContext.Session.GetString(SessionInfo.CurrentCampusId);

            if (response.IsValid)
            {
                try
                {

                    if (model.Montant <= 0)
                    {
                        response.IsValid = false;
                        response.Message = "Impossible d'enregistrer un montant négatif";
                        return Ok(response);
                    }

                    var resteFraisMedi = _scolarite.ResteFraisMedicaux(model.DemandeAdmissionId, DateTime.UtcNow);
                    if (resteFraisMedi == 0)
                    {
                        response.IsValid = false;
                        response.Message = "Vous avez réglé déjà la totalité des frais médicaux";
                        return Ok(response);
                    }
                    double? montant = 0;
                    if (resteFraisMedi < model.Montant)
                    {
                        montant = resteFraisMedi;
                        model.Montant = montant;
                        response.IsValid = false;
                        response.Message = $"Le montant versé est supérieur au montant total des frais médicaux <br> <strong> Vous devez lui rembourser {model.Montant - resteFraisMedi} <strong><br/>";
                    }
                    if (_scolarite.CreatePaiementfraisMedicaux(model))
                    {
                        response.Data = model.Id.ToString();
                        response.Message = "Paiement effectué avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors du paiement";
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

        ////[Authorize(Roles = "Service Caisse & Opérations , Administrateur")]

        //[Authorize(Roles = "Administrateur, Service Caisse & Opérations")]
        public IActionResult PrintRecuPaiementFraisMedicaux(Guid Id)
        {
            string campus = HttpContext.Session.GetString(SessionInfo.CurrentCampusId);
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
                        foreach (var fraisM in _scolarite.GetAllFraisMedicaux)
                        {
                            foreach (var paie in _admission.GetAllpaiement)
                            {
                                if (paie.DemandeAdmission.InscriptionId == paiement.DemandeAdmission.InscriptionId
                                    && paie.Motif == paiement.Motif && fraisM.CampusId.ToString() == campus
                                    && paie.DemandeAdmission.AnneeAcademiqueId == paiement.DemandeAdmission.AnneeAcademiqueId
                                    && paie.AddedDate < paiement.AddedDate)
                                    
                                {
                                    somme += paie.Montant;
                                }
                                reste = fraisM.Montant - somme;
                                if (fraisM.AnneeAcademique.AnneeDebut != null)
                                {
                                    fields.TryGetValue("Année", out toset);
                                    toset.SetValue(fraisM.AnneeAcademique.AnneeDebut + "/" + fraisM.AnneeAcademique.AnneeFin).SetReadOnly(true);
                                    fields.TryGetValue("Année2", out toset);
                                    toset.SetValue(fraisM.AnneeAcademique.AnneeDebut + "/" + fraisM.AnneeAcademique.AnneeFin).SetReadOnly(true);
                                }
                            }
                        }

                        if (paiement.Montant != null)
                        {
                            fields.TryGetValue("Montant", out toset);
                            toset.SetValue(paiement.Montant.ToString()).SetReadOnly(true);
                            fields.TryGetValue("Montant2", out toset);
                            toset.SetValue(paiement.Montant.ToString()).SetReadOnly(true);
                        }

                        if (paiement.DemandeAdmission.Inscription.Matricule.LibelleMatricule != null)
                        {
                            fields.TryGetValue("Matricule", out toset);
                            toset.SetValue(paiement.DemandeAdmission.Inscription.Matricule.LibelleMatricule).SetReadOnly(true);
                            fields.TryGetValue("Matricule2", out toset);
                            toset.SetValue(paiement.DemandeAdmission.Inscription.Matricule.LibelleMatricule).SetReadOnly(true);
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
                            fields.TryGetValue("somme", out toset);
                            toset.SetValue(paiement.Montant.ToString()).SetReadOnly(true);
                            fields.TryGetValue("Somme2", out toset);
                            toset.SetValue(paiement.Montant.ToString()).SetReadOnly(true);
                        }

                        if (paiement.Motif != null)
                        {
                            string val = "Paiement des frais médicaux";
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

        #endregion

        #region FraisDossierExamen

        [HttpGet]
        public IActionResult FraisDossierExamen()
        {
            var FraisDossierExamen = _scolarite.GetAllFraisDossierExamen;
            return View(FraisDossierExamen);
        }

        //GET : Parametrage/CreateCampus
        //[Authorize(Roles = "Support Informatique , Administrateur")]

        [HttpGet]
        public IActionResult CreateFraisDossierExamen()
        {
            ViewBag.ClasseList = _param.GetAllClasse;
            ViewBag.AnneeAcademiqueList = _admission.GetAllAnneeAcademique;
            return View();
        }

        //POST : Parametrage/CreateCampus/{Campus model}
        [HttpPost]
        public IActionResult CreateFraisDossierExamen(FraisDossierExamen model)
        {
            ViewBag.CLASSEList = _param.GetAllClasse;
            FraisDossierExamenValidator validator = new FraisDossierExamenValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (model.Montant <= 0)
                    {
                        response.IsValid = false;
                        response.Message = "le montant ne peut pas être nul ou inférieur à 0";
                        return Ok(response);
                    }
                    if (_scolarite.VerifFraisDossierExamen(model))
                    {
                        response.IsValid = false;
                        response.Message = "Les frais de dossier d'examen ont déjà été définis pour cette classe";
                        return Ok(response);
                    }
                    if (_scolarite.CreateFraisDossierExamen(model))
                    {
                        response.Message = "Nouveau Frais de dossier d'examen enregistré avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de la création des Frais de dossier d'examen";
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

        //GET : Parametrage/UpdateFraisExam/{Guid Id}
        //[Authorize(Roles = "Support Informatique , Administrateur")]
        public IActionResult UpdateFraisDossierExamen(Guid Id)
        {
            ViewBag.CLASSEList = _param.GetAllClasse;
            ViewBag.ANNEEACADEMIQUEList = _admission.GetAllAnneeAcademique;
            FraisDossierExamen CreateFraisDossierExamen = _scolarite.GetFraisDossierExamen(Id);
            if (CreateFraisDossierExamen != null)
            {
                //ViewBag.IPESList = _param.GetAllIpes;
                return View(CreateFraisDossierExamen);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateFraisDossierExamen(FraisDossierExamen model)
        {
            //model.IPAddress = HttpContext.Session.GetString(SessionInfo.IPAddress) ?? "";
            FraisDossierExamenValidator validator = new FraisDossierExamenValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = new ResponseVM();
            if (response.IsValid)
            {
                try
                {
                    if (model.Montant <= 0)
                    {
                        response.IsValid = false;
                        response.Message = "le montant ne peut pas être nul ou inférieur à 0";
                        return Ok(response);
                    }
                    //if (_scolarite.VerifFraisDossierExamen(model))
                    //{
                    //    response.IsValid = false;
                    //    response.Message = "Les frais de dossier d'examen ont déjà été définis pour cette classe";
                    //    return Ok(response);
                    //}
                    if (_scolarite.UpdateFraisDossierExamen(model))
                    {
                        response.Message = "les Frais de dossier d'examen ont étés modifié avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        //ViewBag.IPESList = _param.GetAllIpes;
                        response.IsValid = false;
                        response.Message = "Echec lors de la création des Frais de dossier d'examen";
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

        public IActionResult DeleteFraisDossierExamen(Guid Id)
        {
            var FraisDossierExamen = _scolarite.GetFraisDossierExamen(Id);
            if (FraisDossierExamen != null)
            {
                return View(FraisDossierExamen);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFraisDossierExamen(FraisDossierExamen model)
        {
            //model.IPAddress = HttpContext.Session.GetString(SessionInfo.IPAddress) ?? "";
            ResponseVM response = new ResponseVM();
            try
            {
                if (_scolarite.DeleteFraisDossierExamen(model))
                {
                    response.Message = "les Frais de dossier d'examen ont étés supprimés avec succès";
                    return Ok(response);
                }
                else
                {
                    response.IsValid = false;
                    response.Message = "Echec lors de la suppression des Frais de dossier d'examen";
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(response);
            }

        }

        public IActionResult DetailsFraisDossierExamen(Guid Id)
        {
            try
            {
                var fraisDossierExamen = _scolarite.GetFraisDossierExamen(Id);
                if (fraisDossierExamen != null)
                {
                    return View(FraisDossierExamen);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        //[Authorize(Roles = "Administrateur, Service Caisse & Opérations ")]

        [HttpGet]
        public IActionResult CreatePaiementFraisDossierExamen(String id)
        {
            Inscription inscription = _scolarite.GetInscription(new Guid(id));
            ViewBag.frais = _scolarite.GetAllFraisDossierExamen;
            if (inscription != null)
            {
                var paiement = new Paiement();
                {
                    paiement.DemandeAdmissionId = inscription.DemandeAdmission.Id;
                    paiement.DemandeAdmission = inscription.DemandeAdmission;

                }
                return View(paiement);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePaiementFraisDossierExamen(Paiement model)
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
                        response.IsValid = false;
                        response.Message = "Impossible d'enregistrer un montant négatif ou nul";
                        return Ok(response);
                    }
                    var resteFraisExam = _scolarite.ResteFraisExamen(model.DemandeAdmissionId, DateTime.UtcNow);
                    if (resteFraisExam == 0)
                    {
                        response.IsValid = false;
                        response.Message = "Cet étudiant a déjà réglé la totalité des frais d'examen";
                        return Ok(response);
                    }
                    double? montant = 0;
                    if (resteFraisExam < model.Montant)
                    {
                        montant = resteFraisExam;
                        model.Montant = montant;
                        response.Message = $"<strong> Vous devez rembourser {model.Montant - resteFraisExam} <strong><br/>";
                    }
                    if (_scolarite.CreatePaiementFraisDossierExamen(model))
                    {
                        response.Data = model.Id.ToString();
                        response.Message = "Paiement effectué avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors du paiement";
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

        //[Authorize(Roles = "Service Caisse & Opérations , Administrateur")]
        public IActionResult PrintRecuPaiementFraisDossierExamen(Guid Id)
        {
            Paiement paiement = _admission.GetPaiement(Id);
            if (paiement != null)
            {
                double? montant = 0, reste = 0, fraisExam = 0;

                reste = _scolarite.ResteFraisExamen(paiement.DemandeAdmissionId, paiement.AddedDate);

                foreach (FraisDossierExamen f in _scolarite.GetAllFraisDossierExamen)
                {
                    if (f.ClasseId == paiement.DemandeAdmission.ClasseId && f.AnneeAcademiqueId == paiement.DemandeAdmission.AnneeAcademiqueId)
                    {
                        fraisExam = f.Montant;

                    }
                }
                montant = fraisExam - reste;
                var recu = PrintRecuPaiement(paiement, montant, reste);

                if (recu != null)
                {
                    return recu;
                }
                return NotFound();
            }
            else
            {
                return NotFound();
            }
        }

        #endregion

        #region FraisSoutenance

        [HttpGet]
        public IActionResult FraisSoutenance()
        {
            var fraisSoutenance = _scolarite.GetAllFraisSoutenance;
            return View(fraisSoutenance);
        }


        //GET : Parametrage/CreateFraisSoutenance
        //[Authorize(Roles = "Support Informatique , Administrateur")]

        [HttpGet]
        public IActionResult CreateFraisSoutenance()
        {
            ViewBag.ClasseList = _param.GetAllClasse;
            ViewBag.AnneeAcademiqueList = _admission.GetAllAnneeAcademique;
            return View();
        }

        //POST : Scolarite/CreateFraisSoutenance/{Campus model}
        [HttpPost]
        public IActionResult CreateFraisSoutenance(FraisSoutenance model)
        {
            ViewBag.ClasseList = _param.GetAllClasse;
            ViewBag.AnneeAcademiqueList = _admission.GetAllAnneeAcademique; FraisSoutenanceValidator validator = new FraisSoutenanceValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (model.Montant <= 0)
                    {
                        response.IsValid = false;
                        response.Message = "le montant ne peut pas être nul ou négatif";
                        return Ok(response);
                    }
                    if (_scolarite.VerifFraisSoutenance(model))
                    {
                        response.IsValid = false;
                        response.Message = "Les frais de soutenance ont déjà été définis pour cette classe";
                        return Ok(response);
                    }
                    if (_scolarite.CreateFraisSoutenance(model))
                    {
                        response.Message = "Nouveau frais de soutenance enregistré avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de la création des Frais de soutenance";
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
        public IActionResult UpdateFraisSoutenance(Guid Id)
        {
            ViewBag.CLASSEList = _param.GetAllClasse;
            ViewBag.ANNEEACADEMIQUEList = _admission.GetAllAnneeAcademique;
            FraisSoutenance CreateFraisSoutenance = _scolarite.GetFraisSoutenance(Id);
            if (CreateFraisSoutenance != null)
            {
                return View(CreateFraisSoutenance);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateFraisSoutenance(FraisSoutenance model)
        {
            FraisSoutenanceValidator validator = new FraisSoutenanceValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = new ResponseVM();
            if (response.IsValid)
            {
                try
                {
                    if (model.Montant <= 0)
                    {
                        response.IsValid = false;
                        response.Message = "Le montant ne peut pas être nul ou inférieur à 0";
                        return Ok(response);
                    }
                    if (_scolarite.VerifFraisSoutenance(model))
                    {
                        response.IsValid = false;
                        response.Message = "Les frais de soutenance ont déjà été définis pour cette classe";
                        return Ok(response);
                    }
                    if (_scolarite.UpdateFraisSoutenance(model))
                    {
                        response.Message = "Les frais de soutenance ont étés modifié avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de la création des Frais de soutenance";
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

        public IActionResult DeleteFraisSoutenance(Guid Id)
        {
            var fraisSoutenance = _scolarite.GetFraisSoutenance(Id);
            if (fraisSoutenance != null)
            {
                return View(fraisSoutenance);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFraisSoutenance(FraisSoutenance model)
        {
            ResponseVM response = new ResponseVM();
            try
            {
                if (_scolarite.DeleteFraisSoutenance(model))
                {
                    response.Message = "les Frais de soutenance ont étés supprimés avec succès";
                    return Ok(response);
                }
                else
                {
                    response.IsValid = false;
                    response.Message = "Echec lors de la suppression des Frais de soutenance";
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(response);
            }

        }

        public IActionResult DetailsFraisSoutenance(Guid Id)
        {
            try
            {
                var fraisSoutenance = _scolarite.GetFraisSoutenance(Id);
                if (fraisSoutenance != null)
                {
                    return View(FraisDossierExamen);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        //GET : Scolarite/CreatePaiementFraisSoutenance/{Guid id}
        //[Authorize(Roles = "Administrateur, Service Caisse & Opérations")]

        [HttpGet]
        public IActionResult CreatePaiementFraisSoutenance(Guid id)
        {
            var inscription = _scolarite.GetInscription(id);
            if (inscription != null)
            {
                var paiement = new Paiement();
                paiement.DemandeAdmission = inscription.DemandeAdmission;
                ViewBag.FraisSout = _scolarite.GetAllFraisSoutenance;
                return View(paiement);
            }
            return NotFound();
        }

        //POST : Scolarite/CreatePaiementFraisSoutenance/{Paiement model}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePaiementFraisSoutenance(Paiement model)
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
                    var resteFraisSout = _scolarite.ResteFraisSout(model.DemandeAdmission.Inscription.Id, DateTime.UtcNow);
                    if (resteFraisSout == 0)
                    {
                        response.IsValid = false;
                        response.Message = "Cet étudiant a déjà réglé la totalité des frais de Soutenance";
                    }
                    double? montant = model.Montant;
                    if (resteFraisSout < model.Montant)
                    {
                        montant = resteFraisSout;
                        response.Message = $"<strong> Vous devez rembourser {model.Montant - resteFraisSout} <strong><br/>";
                    }
                    if (montant != 0)
                    {
                        Paiement paiementFraisSout = new Paiement()
                        {
                            DemandeAdmissionId = model.DemandeAdmission.Id,
                            Montant = montant,
                            Motif = Motif.FraisSoutenance
                        };

                        if (_admission.CreatePaiement(paiementFraisSout))
                        {
                            response.Data = paiementFraisSout.Id.ToString();
                            response.Message = response.Message + "Paiement des frais de soutenance effectué avec succès !!!!";
                            return Ok(response);
                        }
                        else
                        {
                            response.IsValid = false;
                            response.Message = "Echec lors du paiement des frais de soutenance ";
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

        //[Authorize(Roles = "Administrateur, Service Caisse & Opérations ")]

        public IActionResult PrintRecuPaiementFraisSoutenance(Guid Id)
        {
            Paiement paiement = _admission.GetPaiement(Id);
            if (paiement != null)
            {
                double? somme = 0, reste = 0, fraissout = 0;
                reste = _scolarite.ResteFraisSout(paiement.DemandeAdmission.Inscription.Id, paiement.AddedDate);

                foreach (FraisSoutenance sout in _scolarite.GetAllFraisSoutenance)
                {
                    if (sout.ClasseId == paiement.DemandeAdmission.ClasseId && sout.AnneeAcademiqueId == paiement.DemandeAdmission.AnneeAcademiqueId)
                    {
                        fraissout = sout.Montant;
                        break;
                    }
                }
                somme = fraissout - reste;
                var recu = PrintRecuPaiement(paiement, somme, reste);

                if (recu != null)
                {
                    return recu;
                }
                return NotFound();
            }
            else
            {
                return NotFound();
            }
        }

        #endregion

        #region FraisScolarite

        [HttpGet]
        public IActionResult TrancheScolarite()
        {
            var trancheScolarite = _scolarite.GetAllTrancheScolarite;
            return View(trancheScolarite);
        }

        //GET : Scolarite/Tranche
        //[Authorize(Roles = "Support Informatique , Administrateur")]

        [HttpGet]
        public IActionResult CreateTrancheScolarite()
        {
            ViewBag.ClasseList = _param.GetAllClasse;
            ViewBag.AnneeAcademiqueList = _admission.GetAllAnneeAcademique;
            ViewBag.CampusList = _param.GetAllCampus;
            return View();
        }

        //POST : Scolarite/CreateCampus/{Campus model}
        [HttpPost]
        public IActionResult CreateTrancheScolarite(TrancheScolarite model)
        {
            ViewBag.ClasseList = _param.GetAllClasse;
            TrancheScolariteValidator validator = new TrancheScolariteValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (model.Montant <= 0)
                    {
                        response.IsValid = false;
                        response.Message = "le montant ne peut pas être nul ou inférieur à 0";
                        return Ok(response);
                    }
                    if (_scolarite.VerifTrancheScolarite(model))
                    {
                        response.IsValid = false;
                        response.Message = "Cette tranche de scolarité a déjà été définie pour cette classe";
                        return Ok(response);
                    }
                    if (_scolarite.CreateTrancheScolarite(model))
                    {
                        response.Message = "Nouvelle tranche de scolarité enregistrée avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de la création des tranches de scolarité";
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

        //GET : Scolarite/UpdateTrancheScolarite/{Guid Id}
        //[Authorize(Roles = "Support Informatique , Administrateur")]
        [HttpGet]
        public IActionResult UpdateTrancheScolarite(Guid Id)
        {
            ViewBag.CLASSEList = _param.GetAllClasse;
            ViewBag.CAMPUSList = _param.GetAllCampus;
            ViewBag.CLASSEList = _param.GetAllClasse;
            ViewBag.ANNEEACADEMIQUEList = _admission.GetAllAnneeAcademique;
            var fraisDossierExamen = _scolarite.GetTrancheScolarite(Id);
            if (fraisDossierExamen != null)
            {
                return View(fraisDossierExamen);
            }
            return NotFound();
        }

        //POST : Scolarite/UpdateTrancheScolarite/{TrancheScolarite model}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateTrancheScolarite(TrancheScolarite model)
        {
            TrancheScolariteValidator validator = new TrancheScolariteValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = new ResponseVM();
            if (response.IsValid)
            {
                try
                {
                    if (model.Montant <= 0)
                    {
                        response.IsValid = false;
                        response.Message = "le montant ne peut pas être nul ou inférieur à 0";
                        return Ok(response);
                    }
                    if (_scolarite.UpdateTrancheScolarite(model))
                    {
                        response.Message = "les Frais de dossier d'examen ont étés modifié avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de la création destranche de scolarite";
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

        //GET : Scolarite/DeleteTrancheScolarite/{Guid Id}
        public IActionResult DeleteTrancheScolarite(Guid Id)
        {
            var trancheScolarite = _scolarite.GetTrancheScolarite(Id);
            if (TrancheScolarite != null)
            {
                return View(trancheScolarite);
            }
            return NotFound();
        }

        //POST : Scolarite/DeleteTrancheScolarite/{TrancheScolarite model}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTrancheScolarite(TrancheScolarite model)
        {
            ResponseVM response = new ResponseVM();
            try
            {
                if (_scolarite.DeleteTrancheScolarite(model))
                {
                    response.Message = "les tranches de scolarite ont étés supprimés avec succès";
                    return Ok(response);
                }
                else
                {
                    response.IsValid = false;
                    response.Message = "Echec lors de la suppression des tranche de scolarite";
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(response);
            }

        }

        //GET : Scolarite/DetailsTrancheScolarite/{Guid Id}
        public IActionResult DetailsTrancheScolarite(Guid Id)
        {
            try
            {
                var trancheScolarite = _scolarite.GetTrancheScolarite(Id);
                if (trancheScolarite != null)
                {
                    return View(TrancheScolarite);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        //GET : Scolarite/CreatePaiementScolarite/{Guid id}
        [HttpGet]
        public IActionResult CreatePaiementScolarite(Guid id)
        {
            var inscription = _scolarite.GetInscription(id);
            if (inscription != null)
            {
                var paiement = new Paiement();
                paiement.DemandeAdmission = inscription.DemandeAdmission;
                ViewBag.TrancheScolarite = _scolarite.GetAllTrancheScolarite;
                return View(paiement);
            }
            return NotFound();
        }

        //POST : Scolarite/CreatePaiementScolarite/{Paiement model}

        //[Authorize(Roles = "Administrateur, Service Caisse & Opérations")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePaiementScolarite(Paiement model)
        {
            PaiementValidator validator = new PaiementValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);
            var campus = HttpContext.Session.GetString(SessionInfo.CurrentCampusId);

            if (response.IsValid)
            {
                try
                {
                    if (model.Montant <= 0)
                    {
                        //si le montant entrer est negatif
                        response.IsValid = false;
                        response.Message = "Impossible d'enregistrer un montant négatif";
                        return Ok(response);
                    }

                    var rapi = _scolarite.VerifExistInscript(model.DemandeAdmission.Inscription.Id).ToString();
                    if (_scolarite.VerifExistInscription(model.DemandeAdmission.Id) == false)
                    {
                        //s'il n'est pas inscrit
                        response.IsValid = false;
                        response.Message = "Cet etudiant n'est pas inscrit";
                        return BadRequest(response);
                    }
                    else
                    {
                        //s'il est inscrit
                        var inscription = _scolarite.GetInscription(model.DemandeAdmission.Inscription.Id);
                        double? montantinscrip = 0, montantscolarite = 0;
                        if (Int32.Parse(rapi) != 0)
                        {
                            //s'il n'a pas finit de payer ses frais d'inscription
                            var reste = model.Montant - double.Parse(rapi);
                            if (reste > 0)
                            {
                                //si le montant quil donne est superieur au reste a payer pour l'inscription
                                montantinscrip = double.Parse(rapi);
                                montantscolarite = reste;
                            }
                            if (reste <= 0)
                            {
                                //si le montant quil donne est inferieur au reste a payer pour l'inscription
                                montantinscrip = model.Montant;
                                montantscolarite = 0;
                            }
                            Paiement paiementInscript = new Paiement()
                            {
                                DemandeAdmissionId = model.DemandeAdmission.Id,
                                Montant = montantinscrip,
                                Motif = Motif.FraisInscription
                            };
                            response.Message = "Cet etudiant n'avait pas soldé la totalité des frais d'inscription.";

                            if (_admission.CreatePaiement(paiementInscript))
                            {

                                response.Message = response.Message +
                               "<br/> Une partie du montant à étè versée comme frais d'inscription " +
                               "<br> Veuillez imprimer le reçu ultérieurement";

                            }
                            else
                            {
                                response.IsValid = false;
                                response.Message = response.Message + "Echec lors du paiement des frais d'inscription ";
                                return Ok(response);
                            }

                        }
                        else
                        {
                            //s'il a deja finit son inscription
                            montantscolarite = model.Montant;
                        }

                        if (montantscolarite != 0)
                        {
                            var resteScolarite = _scolarite.ResteScolarite(inscription.Id, campus, DateTime.UtcNow);

                            if (resteScolarite == 0)
                            {
                                response.IsValid = false;
                                response.Message = "Cet étudiant a déjà réglé la totalité des frais de scolarité";
                            }
                            if (resteScolarite < montantscolarite)
                            {
                                response.Message = $"<strong> vous devez lui rembourser {montantscolarite - resteScolarite} <strong>";
                                montantscolarite = resteScolarite;
                            }

                            Paiement paiementScolarite = new Paiement()
                            {
                                DemandeAdmissionId = model.DemandeAdmission.Id,
                                Montant = montantscolarite,
                                Motif = Motif.FraisScolarite
                            };
                            if (_admission.CreatePaiement(paiementScolarite))
                            {
                                response.Data = paiementScolarite.Id.ToString();
                                response.Message = response.Message + "Paiement des frais de scolarité effectué avec succès";
                                return Ok(response);
                            }
                            else
                            {
                                response.IsValid = false;
                                response.Message = "Echec lors du paiement des frais de scolarité ";
                                return Ok(response);
                            }
                        }
                        else
                        {
                            return Ok(response);
                        }
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(response);
                }
            }
            return BadRequest(response);
        }

        //[Authorize(Roles = "Support Informatique , Administrateur, Service Caisse & Opérations ")]
        public IActionResult PrintRecuPaiementScolarite(Guid Id)
        {
            ResponseVM response = new ResponseVM();
            Paiement paiement = _admission.GetPaiement(Id);
            if (paiement != null)
            {
                var campus = HttpContext.Session.GetString(SessionInfo.CurrentCampusId);
                double? somme = 0, reste = 0, fraisScolarite = 0;
                reste = _scolarite.ResteScolarite(paiement.DemandeAdmission.Inscription.Id, campus, paiement.AddedDate);

                foreach (TrancheScolarite tranche in _scolarite.GetAllTrancheScolarite)
                {
                    if (tranche.ClasseId == paiement.DemandeAdmission.ClasseId && tranche.AnneeAcademiqueId == paiement.DemandeAdmission.AnneeAcademiqueId && tranche.CampusId.ToString() == campus)
                    {

                        fraisScolarite += tranche.Montant;
                    }
                }
                somme = fraisScolarite - reste;
                var recu = PrintRecuPaiement(paiement, somme, reste);

                if (recu != null)
                {
                    response.Data = null;
                    return recu;
                }
                return NotFound();
            }
            else
            {
                return NotFound();
            }
        }

        #endregion

        #region Impression du recu
        private FileContentResult PrintRecuPaiement(Paiement paiement, double? somme, double? reste)
        {
            var insc = _scolarite.GetInscription(paiement.DemandeAdmission.InscriptionId);
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

                    if (paiement.Montant != null)
                    {
                        fields.TryGetValue("Montant", out toset);
                        toset.SetValue(paiement.Montant.ToString()).SetReadOnly(true);
                        fields.TryGetValue("Montant2", out toset);
                        toset.SetValue(paiement.Montant.ToString()).SetReadOnly(true);
                    }
                    string montant;
                    if (insc.Matricule.LibelleMatricule != null)
                    {
                        fields.TryGetValue("Matricule", out toset);
                        toset.SetValue(insc.Matricule.LibelleMatricule).SetReadOnly(true);
                        fields.TryGetValue("Matricule2", out toset);
                        toset.SetValue(insc.Matricule.LibelleMatricule).SetReadOnly(true);
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

                        if (paiement.Motif == Motif.FraisInscription)
                        {
                            val = "Paiement des frais d'inscription";
                        }
                        else if (paiement.Motif == Motif.FraisMedicaux)
                        {
                            val = "Paiement des frais médicaux";
                        }
                        else if (paiement.Motif == Motif.FraisSoutenance)
                        {
                            val = "Paiement des frais de soutenance";
                        }
                        else if (paiement.Motif == Motif.FraisScolarite)
                        {
                            val = "Paiement des frais de scolarité";
                        }
                        else
                        {
                            val = "Paiement des frais de dossiers d'examen";
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
                        toset.SetValue(campusName.ToString()).SetReadOnly(true);
                        fields.TryGetValue("Lieu2", out toset);
                        toset.SetValue(campusName.ToString()).SetReadOnly(true);
                    }

                    fields.TryGetValue("Date", out toset);
                    toset.SetValue(paiement.AddedDate.ToString()).SetReadOnly(true);
                    fields.TryGetValue("Date2", out toset);
                    toset.SetValue(paiement.AddedDate.ToString()).SetReadOnly(true);

                    pdf.Close();
                    byte[] bytes = outputStream.ToArray();
                    return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, paiement.DemandeAdmission.Candidat.Personne.Nom.ToString() + "_" + paiement.Motif.ToString() + "_Recu.pdf");
                }
            }
            return null;
        }
        #endregion

        #region Bourse
        public IActionResult Bourse()
        {
            var bourse = _scolarite.GetAllBourses;

            return View(bourse);

        }
        //[Authorize(Roles = "Support Informatique , Administrateur, Service Caisse & Opérations ")]

        [HttpGet]
        public IActionResult CreateBourse()
        {
            ViewBag.InscriptionList = _scolarite.GetAllInscription;
            ViewBag.DAList = _admission.GetAllDA;
            return View();

        }

        [HttpPost]
        public IActionResult CreateBourse(Bourse model)
        {
            BourseValidator validator = new BourseValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (model.Montant < 0)
                    {
                        response.IsValid = false;
                        response.Message = "le montant à réduire ne peut pas ètre négatif";
                        return Ok(response);

                    }
                    if (_scolarite.CreateBourse(model))
                    {
                        response.IsValid = true;
                        response.Message = "Nouvelle réduction de scolarité enregistrée avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de l'initiation de la réduction de la scolarité";
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

        //GET : UpdateBourse
        //[Authorize(Roles = "Support Informatique , Administrateur, Service Caisse & Opérations ")]
        [HttpGet]
        public IActionResult UpdateBourse(Guid Id)
        {
            ViewBag.InscriptionList = _scolarite.GetAllInscription;
            ViewBag.DAList = _admission.GetAllDA;
            var bourse = _scolarite.GetBourse(Id);
            if (bourse != null)
            {
                return View(bourse);
            }
            return NotFound();
        }

        //GET : UpdateBourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateBourse(Bourse model)
        {
            ViewBag.InscriptionList = _scolarite.GetAllInscription;
            ViewBag.DAList = _admission.GetAllDA;
            BourseValidator validator = new BourseValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = new ResponseVM();
            if (response.IsValid)
            {
                try
                {
                    if (_scolarite.UpdateBourse(model))
                    {
                        response.Message = "la bourse a été modifiée avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        //ViewBag.IPESList = _param.GetAllIpes;
                        response.IsValid = false;
                        response.Message = "Echec lors de l'édition de la bourse";
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

        //[Authorize(Roles = "Administrateur")]
        [HttpGet]
        public IActionResult ValidateBourse(Guid Id)
        {
            ViewBag.InscriptionList = _scolarite.GetAllInscription;
            ViewBag.DAList = _admission.GetAllDA;
            var bourse = _scolarite.GetBourse(Id);
            if (bourse != null)
            {
                return View(bourse);
            }
            return NotFound();
        }

        //POST : UpdateBourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ValidateBourse(Bourse model)
        {
            ViewBag.InscriptionList = _scolarite.GetAllInscription;
            ViewBag.DAList = _admission.GetAllDA;
            BourseValidator validator = new BourseValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = new ResponseVM();
            if (response.IsValid)
            {
                try
                {
                    if (_scolarite.ValidateBourse(model))
                    {
                        response.Message = "la bourse a été validée avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        //ViewBag.IPESList = _param.GetAllIpes;
                        response.IsValid = false;
                        response.Message = "Echec lors de la validation de la bourse";
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

        #region Changement
        public IActionResult ChangementCampus()
        {
            var changement = _scolarite.GetAllchangementCampus;
            ViewBag.Campus = _param.GetAllCampus;
            return View(changement);
        }

        //[Authorize(Roles = "Support Informatique , Administrateur, Service Caisse & Opérations ")]

        [HttpGet]
        public IActionResult CreateChangementCampus(String id)
        {
            Inscription inscription = _scolarite.GetInscription(new Guid(id));
            if (inscription != null)
            {
                var changcampus = new ChangementFiliereOrCampus();
                {
                    changcampus.InscriptionId = inscription.Id;
                    changcampus.Inscription = inscription;
                }
                ViewBag.Campus = _param.GetAllCampus;
                ViewBag.inscription = inscription;

                return View(changcampus);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult CreateChangementCampus(ChangementFiliereOrCampus model)
        {
            ViewBag.Campus = _param.GetAllCampus;
            ChangementFiliereOrCampusValidator2 validator = new ChangementFiliereOrCampusValidator2();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (model.PastCampus == model.NextCampus)
                    {
                        response.IsValid = false;
                        response.Message = "Le campus de destination doit différer du campus actuel";
                        return Ok(response);
                    }
                    if (_scolarite.CreateChangementCampus(model))
                    {
                        response.Message = "Changement de campus initialisé avec succès<br> Veuillez attendre sa validation";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors du changement du Campus";
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

        public IActionResult UpdateChangementCampus(Guid Id)
        {
            var chang = _scolarite.Getchangcampus(Id);
            if (chang != null)
            {
                ViewBag.Campus = _param.GetAllCampus;
                return View(chang);
            }
            return NotFound();
        }

        //GET : UpdateChanges
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateChangementCampus(ChangementFiliereOrCampus model)
        {
            ViewBag.InscriptionList = _scolarite.GetAllInscription;
            ViewBag.DAList = _admission.GetAllDA;
            ChangementFiliereOrCampusValidator2 validator = new ChangementFiliereOrCampusValidator2();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = new ResponseVM();
            if (response.IsValid)
            {
                try
                {
                    if (_scolarite.UpdateChangementCampus(model))
                    {
                        response.Message = "Changement modifié avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        //ViewBag.IPESList = _param.GetAllIpes;
                        response.IsValid = false;
                        response.Message = "Echec lors de l'édition du changement";
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


        //[Authorize(Roles = "Administrateur")]
        [HttpGet]
        public IActionResult ValidateChangementCampus(Guid Id)
        {
            ChangementFiliereOrCampus changcampus = _scolarite.Getchangcampus(Id);
            if (changcampus != null)
            {
                return View(changcampus);
            }
            return NotFound();
        }

        //POST : ValidateChangementCampus
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult ValidateChangementCampus(ChangementFiliereOrCampus model)
        {
            ChangementFiliereOrCampusValidator2 validator = new ChangementFiliereOrCampusValidator2();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (_scolarite.ValidateChangementCampus(model))
                    {
                        response.Message = "Validation effectuée avec succès";
                        return Ok(response);
                    }
                    else
                    {

                        response.IsValid = false;
                        response.Message = "Echec lors de la modification de la décision";
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

        public IActionResult ChangementFiliere()
        {
            ViewBag.specialiteList = _param.GetAllFiliere;
            var changementFiliere = _scolarite.GetAllChangementFiliere;
            return View(changementFiliere);
        }

        //[Authorize(Roles = "Administrateur, Chef de departement")]

        [HttpGet]
        public IActionResult CreateChangementFiliere(Guid Id)
        {

            Inscription inscription = _scolarite.GetInscription(Id);
            if (inscription != null)
            {

                ChangementFiliereOrCampus changeFiliere = new ChangementFiliereOrCampus()
                {
                    InscriptionId = inscription.Id,
                    Inscription = inscription
                };
                ViewBag.specialiteList = _param.GetAllFiliere;
                return View(changeFiliere);
            }
            return NotFound();
        }


        //POST : CreateChangementFiliere
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateChangementFiliere(ChangementFiliereOrCampus model)
        {

            ResponseVM response = new ResponseVM();
            UpdateDAFiliereValidator validator = new UpdateDAFiliereValidator();
            ValidationResult validationResult = validator.Validate(model);
            response = FluentUtilities.GetValidationError(validationResult);
            if (response.IsValid)
            {
                try
                {
                    if (model.PastFiliere == model.NextFiliere)
                    {
                        response.IsValid = false;
                        response.Message = "La spécialité de destination doit différer de la spécialité actuelle";
                        return Ok(response);
                    }
                    if (model.PastFiliere == model.NextFiliere)
                    {
                        response.IsValid = false;
                        response.Message = "La spécialité destination doit différer de la spécialité actuelle";
                        return Ok(response);
                    }
                    if (_scolarite.CreateChangementFiliere(model))
                    {

                        ViewBag.ConcoursList = _admission.GetAllConcours;
                        ViewBag.AnneList = _admission.GetAllAnneeAcademique;
                        ViewBag.CandidatList = _admission.GetAllCandidat;
                        ViewBag.ClasseList = _param.GetAllClasse;
                        ViewBag.CycleList = _param.GetAllCycle;
                        response.Message = "la filière de l'étudiant à été modifiée avec succès";
                        return Ok(response);
                    }
                    response.IsValid = false;
                    response.Message = "Echec lors de la modification de la filière de l'étudiant";
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    return BadRequest(response);
                }
            }
            return BadRequest(response);


        }

        #endregion

        #region recherche
        public IActionResult Recherche(string term = " ")
        {

            string campus = HttpContext.Session.GetString(SessionInfo.CurrentCampusId);
            var empdata = _scolarite.Recherche(term, campus);
            if (empdata.IsNullOrEmpty())
            {
                string b = "nf";
                ViewBag.term = b;
            }
            else
            {
                ViewBag.term = term;
            }
            return View(empdata);
        }
        #endregion

    }
}



