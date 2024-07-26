using GestAgape.Core.Entities.Parametrage;
using GestAgape.Core.ViewModels.FluentValidators;
using GestAgape.Core.ViewModels;
using GestAgape.Infrastructure.Utilities;
using GestAgape.Service.Parametrages;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using FluentValidation;
using Azure;
using Microsoft.AspNetCore.Authorization;
using GestAgape.Core.Entities;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace GestAgape.Controllers
{
    //[Authorize(Roles = "Administrateur , Support Informatique")]
    public class ParametrageController : Controller
    {
        #region Membres prives
        private readonly IParametrage _param;
        private readonly IEmailSender _emailSender;
        #endregion

        #region Constructeur
        public ParametrageController(IParametrage param, IEmailSender emailSender)
        {
            _param = param;
            _emailSender = emailSender;
        }
        #endregion

        #region Campus

        //GET : Parametrage/Campus
        [HttpGet]
        public IActionResult Campus()
        {
            var campus = _param.GetAllCampus;
            return View(campus);
        }


        //GET : Parametrage/CreateCampus
        [HttpGet]
        public IActionResult CreateCampus()
        {
            ViewBag.IPESList = _param.GetAllIpes;
            return View();
        }

        //POST : Parametrage/CreateCampus/{Campus model}
        [HttpPost]
        public IActionResult CreateCampus(Campus model)
        {
            ViewBag.IPESList = _param.GetAllIpes;
            CampusValidator validator = new CampusValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (_param.VerifCampus(model))
                    {
                        response.IsValid = false;
                        response.Message = "Ce campus existe déjà";
                        return Ok(response);
                    }
                    if (_param.CreateCampus(model))
                    {
                        response.Message = "Nouveau campus enregistré avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de la création du campus";
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

        //GET : Parametrage/UpdateCampus/{Guid Id}
        public IActionResult UpdateCampus(Guid Id)
        {
            ViewBag.IPESList = _param.GetAllIpes;
            var campus = _param.GetCampus(Id);
            if (campus != null)
            {
                //ViewBag.IPESList = _param.GetAllIpes;
                return View(campus);
            }
            return NotFound();
        }

        //POST : Parametrage/UpdateCampus/{Campus model}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCampus(Campus model)
        {
            CampusValidator validator = new CampusValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = new ResponseVM();
            if (response.IsValid)
            {
                try
                {
                    if (_param.UpdateCampus(model))
                    {
                        response.Message = "la Campus à été modifié avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        //ViewBag.IPESList = _param.GetAllIpes;
                        response.IsValid = false;
                        response.Message = "Echec lors de l'édition du Campus";
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

        //GET : Parametrage/DeleteCampus/{Guid Id}
        public IActionResult DeleteCampus(Guid Id)
        {
            var campus = _param.GetCampus(Id);
            if (campus != null)
            {
                return View(campus);
            }
            return NotFound();
        }

        //POST : Parametrage/DeleteCampus/{Campus model}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCampus(Campus model)
        {
            //model.IPAddress = HttpContext.Session.GetString(SessionInfo.IPAddress) ?? "";
            ResponseVM response = new ResponseVM();
            try
            {
                if (_param.DeleteCampus(model))
                {
                    response.Message = "la Campus à été supprimé avec succès";
                    return Ok(response);
                }
                else
                {
                    response.IsValid = false;
                    response.Message = "Echec lors de la suppression de la Campus";
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(response);
            }

        }

        //GET : Parametrage/DetailsCampus/{Guid Id}
        public IActionResult DetailsCampus(Guid Id)
        {
            try
            {
                var campus = _param.GetCampus(Id);
                if (campus != null)
                {
                    return View(campus);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        #endregion

        #region Chef Departement

        public IActionResult ChefDepartement()
        {
            ViewBag.DepartementList = _param.GetAllDepartement;
            var chefDepart = _param.GetAllChefDepartement;
            return View(chefDepart);
        }

        [HttpGet]
        public IActionResult CreateChefDepartement()
        {
            ViewBag.DepartementList = _param.GetAllDepartement;
            return View();
        }
        [HttpPost]
        public IActionResult CreateChefDepartement(ChefDepartementVM model)
        {
            ViewBag.DepartementList = _param.GetAllDepartement;
            ChefDepartementVMValidator validator = new ChefDepartementVMValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (_param.ExistChefDepartement(model.Nom, model.Prenom, model.Telephone, model.DepartementId))
                    {
                        response.IsValid = false;
                        response.Message = "Cette personne est responsable ou a déjà été responsable de ce département";
                        return Ok(response);
                    }
                    if (_param.CreateChefDepartement(model))
                    {

                        response.Message = "Chef de département enregistré avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de la création du chef de département";
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

        //GET : Parametrage/DetailsFiliere/{Guid Id}
        public IActionResult DetailsChefDepartement(Guid Id)
        {
            ViewBag.ChefDepartementList = _param.GetAllChefDepartement;
            try
            {
                var chefDepartement = _param.GetChefDepartement(Id);
                if (chefDepartement != null)
                {
                    return View(chefDepartement);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult UpdateChefDepartement(Guid id)
        {
            ViewBag.DepartementList = _param.GetAllDepartement;
            ChefDepartementVM chefDepart = _param.GetChefDepartementVM(id);
            if (chefDepart != null)
            {
                return View(chefDepart);

            }
            return NotFound();
        }
        public IActionResult UpdateChefDepartement(ChefDepartementVM model)
        {
            ViewBag.DepartementList = _param.GetAllDepartement;
            ChefDepartementVMValidator validator = new ChefDepartementVMValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (_param.UpdateChefDepartementVM(model))
                    {
                        response.Message = "Chef de département mis à jour avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de l'édition du chef de département";
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

        #region Classe
        //GET : Parametrage/Classe
        [HttpGet]
        public IActionResult Classe()
        {
            var classe = _param.GetAllClasse;
            return View(classe);
        }

        //GET : Parametrage/CreateClasse
        public IActionResult CreateClasse()
        {

            ViewBag.NiveauList = _param.GetAllNiveau;
            ViewBag.FiliereList = _param.GetAllFiliere;
            ViewBag.Cyclelist = _param.GetAllCycle;
            return View();
        }

        //POST : Parametrage/CreateClasse/{Classe model}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateClasse(Classe model)
        {
            ClasseValidator validator = new ClasseValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = new ResponseVM();

            if (response.IsValid)
            {
                try
                {
                    if (_param.VerifExistClasse(model.FiliereId, model.NiveauId))
                    {
                        response.IsValid = false;
                        response.Message = "Cette classe existe déjà";
                        return Ok(response);
                    }
                    if (_param.CreateClasse(model))
                    {
                        response.Message = "Nouvelle classe enregistrée avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de la création de la classe";
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

        //GET : Parametrage/UpdateClasse/{Guid Id}
        public IActionResult UpdateClasse(Guid Id)
        {
            var classe = _param.GetClasse(Id);
            if (classe != null)
            {
                ViewBag.NiveauList = _param.GetAllNiveau;
                ViewBag.FiliereList = _param.GetAllFiliere;
                ViewBag.Cyclelist = _param.GetAllCycle;
                return View(classe);
            }
            return NotFound();
        }

        //POST : Parametrage/UpdateClasse/{Classe model}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateClasse(Classe model)
        {
            ClasseValidator validator = new ClasseValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = new ResponseVM();

            if (response.IsValid)
            {
                try
                {
                    if (_param.VerifExistClasse(model.FiliereId, model.NiveauId))
                    {
                        response.IsValid = false;
                        response.Message = "Cette classe existe déjà";
                        return Ok(response);
                    }
                    if (_param.UpdateClasse(model))
                    {
                        response.Message = "la classe à été modifié avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        ViewBag.NiveauList = _param.GetAllNiveau;
                        ViewBag.FiliereList = _param.GetAllFiliere;
                        ViewBag.Cyclelist = _param.GetAllCycle;
                        response.IsValid = false;
                        response.Message = "Echec lors de la création de la classe";
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

        //GET : Parametrage/DeleteClasse/{Guid Id}
        public IActionResult DeleteClasse(Guid Id)
        {
            var classe = _param.GetClasse(Id);
            if (classe != null)
            {
                return View(classe);
            }
            return NotFound();
        }

        //POST : Parametrage/DeleteClasse/{Classe model}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteClasse(Classe model)
        {
            ResponseVM response = new ResponseVM();
            try
            {
                if (_param.DeleteClasse(model))
                {
                    response.Message = "la classe à été supprimé avec succès";
                    return Ok(response);
                }
                else
                {
                    response.IsValid = false;
                    response.Message = "Echec lors de la suppression de la classe";
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(response);
            }

        }

        //GET : Parametrage/DetailsClasse/{Guid Id}
        public IActionResult DetailsClasse(Guid Id)
        {
            try
            {
                var classe = _param.GetClasse(Id);
                if (classe != null)
                {
                    return View(classe);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        #endregion

        #region Cycle

        [HttpGet]
        public IActionResult Cycle()
        {
            var cycle = _param.GetAllCycle;
            return View(cycle);
        }

        [HttpGet]
        public IActionResult CreateCycle()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCycle(Cycle model)
        {
            CycleValidator validator = new CycleValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (_param.VerifCycle(model, ExistType.Create))
                    {
                        response.IsValid = false;
                        response.Message = "Le libellé ou le code de ce cycle existe déjà";
                        return Ok(response);
                    }
                    if (_param.CreateCycle(model))
                    {
                        response.Message = "Nouveau cycle enregistré avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de la création du cycle";
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
        public IActionResult UpdateCycle(Guid id)
        {
            Cycle cycle = _param.GetCycle(id);
            if (cycle != null)
            {
                return View(cycle);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult UpdateCycle(Cycle model)
        {
            CycleValidator validator = new CycleValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {

                    if (_param.VerifCycle(model, ExistType.Update))
                    {
                        response.IsValid = false;
                        response.Message = "Le libellé et le code de ce cycle existent déjà";
                        return Ok(response);
                    }
                    if (_param.UpdateCycle(model))
                    {
                        response.Message = "Cycle modifié avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de la modification du cycle";
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

        #region Departement

        public IActionResult Departement()
        {
            var departement = _param.GetAllDepartement;
            return View(departement);
        }

        [HttpGet]
        public IActionResult CreateDepartement()
        {

            return View();
        }

        [HttpPost]
        public IActionResult CreateDepartement(Departement model)
        {
            DepartementValidator validator = new DepartementValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                if (_param.ExistDepartement(model.Libelle, model.Code, ExistType.Create))
                {
                    response.IsValid = false;
                    response.Message = "Le libellé ou le code de ce département existe déjà";
                    return Ok(response);
                }
                try
                {
                    if (_param.CreateDepartement(model))
                    {
                        response.Message = "Nouveau Departement " + model.Libelle + " créé avec succès";
                        return Ok(response);
                    }

                    else
                    {
                        response.IsValid = false;
                        response.Message = "Erreur lors de la création du département" + model.Libelle;
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
        public IActionResult UpdateDepartement(Guid id)
        {
            Departement dpt = _param.GetDepartement(id);
            if (dpt != null)
            {
                return View(dpt);
            }
            return NotFound();

        }

        [HttpPost]
        public IActionResult UpdateDepartement(Departement model)
        {
            DepartementValidator validator = new DepartementValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (_param.ExistDepartement(model.Libelle, model.Code, ExistType.Update))
                    {
                        response.IsValid = false;
                        response.Message = "Ce département existe déjà";
                        return Ok(response);
                    }
                    if (_param.UpdateDepartement(model))
                    {
                        response.Message = "Departement " + model.Libelle + " modifié avec succès";
                        return Ok(response);
                    }

                    else
                    {
                        response.IsValid = false;
                        response.Message = "Erreur lors de la modification du département" + model.Libelle;
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

        #region Filiere

        [HttpGet]
        public IActionResult Filiere()
        {
            ViewBag.DepartementsList = _param.GetAllDepartement;
            var filiere = _param.GetAllFiliere;
            return View(filiere);
        }
        [HttpGet]
        public IActionResult CreateFiliere()
        {

            ViewBag.DepartementsList = _param.GetAllDepartement;
            return View();
        }
        [HttpPost]
        public IActionResult CreateFiliere(Filiere model)
        {
            FiliereValidator validator = new FiliereValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {

                    if (_param.ExistFiliere(model.Libelle, model.Code, ExistType.Create))
                    {
                        response.IsValid = false;
                        response.Message = "Le nom ou le code de cette filière existe déjà";
                        return Ok(response);
                    }

                    if (_param.CreateFiliere(model))
                    {
                        response.Message = "Nouvelle Filiere enregistrée avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de la création de la Filière";
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

        public IActionResult UpdateFiliere(Guid Id)
        {
            Filiere filiere = _param.GetFiliere(Id);
            if (filiere != null)
            {
                ViewBag.DepartementsList = _param.GetAllDepartement;
                return View(filiere);
            }
            return NotFound();
        }

        //POST : Parametrage/UpdateFiliere/{filiere model}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateFiliere(Filiere model)
        {
            FiliereValidator validator = new FiliereValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);
            if (response.IsValid)
            {
                try
                {
                    if (_param.ExistFiliere(model.Libelle, model.Code, ExistType.Update))
                    {
                        response.IsValid = false;
                        response.Message = "Le nom ou le code de cette filière existe déjà";
                        return Ok(response);
                    }
                    if (_param.UpdateFiliere(model))
                    {
                        response.Message = "la Filiere a été modifiée avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de la création de la Filiere";
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

        //GET : Parametrage/DeleteFiliere/{Guid Id}
        public IActionResult DeleteFiliere(Guid Id)
        {
            var Filiere = _param.GetFiliere(Id);
            if (Filiere != null)
            {
                return View(Filiere);
            }
            return NotFound();
        }

        //POST : Parametrage/DeleteFiliere/{Filiere model}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFiliere(Filiere model)
        {
            //model.IPAddress = HttpContext.Session.GetString(SessionInfo.IPAddress) ?? "";
            ResponseVM response = new ResponseVM();
            try
            {
                if (_param.DeleteFiliere(model))
                {
                    response.Message = "la Filiere à été supprimé avec succès";
                    return Ok(response);
                }
                else
                {
                    response.IsValid = false;
                    response.Message = "Echec lors de la suppression de la Filiere";
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(response);
            }

        }

        //GET : Parametrage/DetailsFiliere/{Guid Id}
        public IActionResult DetailsFiliere(Guid Id)
        {
            try
            {
                var Filiere = _param.GetFiliere(Id);
                if (Filiere != null)
                {
                    return View(Filiere);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        #endregion

        #region Ipes

        // affichage des Ipes

        public IActionResult Ipes(Guid Id)
        {
            var ipes = _param.GetAllIpes;
            return View(ipes);

        }
        public IActionResult CreateIpes()
        {
            return View();
        }

        //Partie de l'ajout d'un Ipes

        [HttpPost]
        public IActionResult CreateIpes(Ipes model)
        {

            IPESValidator validator = new IPESValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (_param.VerifIpes(model, ExistType.Create))
                    {
                        response.IsValid = false;
                        response.Message = "Cet IPES existe déjà";
                        return Ok(response);
                    }
                    if (_param.CreateIpes(model))
                    {
                        response.Message = "Nouveau Ipes enregistré avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de la création du Ipes";
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

        //partie de la modification de l'IPES

        [HttpGet]
        public IActionResult UpdateIpes(Guid Id)
        {
            var ipes = _param.GetIpes(Id);

            if (ipes != null)
            {
                return View(ipes);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult UpdateIpes(Ipes ipes)
        {


            IPESValidator validator = new IPESValidator();
            ValidationResult validationResult = validator.Validate(ipes);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (_param.VerifIpes(ipes, ExistType.Update))
                    {
                        response.IsValid = false;
                        response.Message = "Cet IPES existe déjà";
                        return Ok(response);
                    }
                    if (_param.UpdateIpes(ipes))
                    {
                        response.Message = $" l'IPES a été modifié avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de la modification de l'IPES";
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

        #region Niveau

        //IndexNiveau
        [HttpGet]
        public IActionResult Niveau()
        {
            var niveau = _param.GetAllNiveau;

            return View(niveau);
        }

        // Get Niveau
        public IActionResult CreateNiveau()
        {
            return View();
        }

        // Post Niveau

        [HttpPost]
        public IActionResult CreateNiveau(Niveau model)
        {
            NiveauValidator validator = new NiveauValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (_param.VerifNiveau(model))
                    {
                        response.IsValid = false;
                        response.Message = "Ce niveau existe déjà";
                        return Ok(response);
                    }
                    if (_param.CreateNiveau(model))
                    {
                        response.Message = "Niveau enregistré avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de la création du niveau";
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

        // Get NiveauById
        public IActionResult UpdateNiveau(Guid id)
        {
            Niveau niveau = _param.GetNiveau(id);
            if (niveau != null)
            {
                return View(niveau);
            }
            return NotFound();
        }

        // Post NiveauUpdate

        [HttpPost]
        public IActionResult UpdateNiveau(Niveau model)
        {
            NiveauValidator validator = new NiveauValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (_param.VerifNiveau(model))
                    {
                        response.IsValid = false;
                        response.Message = "Echec!! ce niveau existe déjà";
                        return Ok(response);
                    }
                    if (_param.UpdateNiveau(model))
                    {
                        response.Message = "Niveau édité avec succès";
                        return Ok(response);
                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "Echec lors de l'édition du niveau";
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

    }

}
