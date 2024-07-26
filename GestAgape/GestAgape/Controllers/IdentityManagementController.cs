using FluentValidation.Results;
using GestAgape.Core.Entities;
using GestAgape.Core.Entities.Identity;
using GestAgape.Core.Entities.Parametrage;
using GestAgape.Core.ViewModels;
using GestAgape.Core.ViewModels.FluentValidators;
using GestAgape.Infrastructure.Utilities;
using GestAgape.Service.Identity;
using GestAgape.Service.Parametrages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace GestAgape.Controllers
{

    public class IdentityManagementController : Controller
    {
        private readonly IIdentityManagement _identityService;
        private readonly IEmailSender _emailSender;
        private readonly IParametrage _param;

        public IdentityManagementController(
            IIdentityManagement identityService,
            IParametrage param,
            IEmailSender emailSender
            )
        {
            _identityService = identityService;
            _emailSender = emailSender;
            _param = param;

        }

        #region Connexion
        public IActionResult Login()
        {
            ViewBag.Campus = _param.GetAllCampus;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            model.IPAdress = HttpContext.Session.GetString(SessionInfo.IPAddress) ?? "";
            LoginValidator validator = new LoginValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);

            if (response.IsValid)
            {
                try
                {
                    if (await _identityService.Login(model))
                    {
                        // récupération des infos sur l'user
                        Affectation userAffect = _identityService.ConnectAffectUser(model);
                        if (userAffect != null)
                        {
                            HttpContext.Session.SetString(SessionInfo.AffectationId, userAffect.Id.ToString());
                            HttpContext.Session.SetString(SessionInfo.CurrentCampusId, userAffect.CampusId.ToString());
                            HttpContext.Session.SetString(SessionInfo.CurrentCampusName, userAffect.Campus.Nom.ToString());
                            HttpContext.Session.SetString(SessionInfo.UserId, model.UserId);
                            response.Message = "connexion réussie";
                            return Ok(response);
                        }
                        else
                        {
                            response.IsValid = false;
                            response.Message = "Vous ne pouvez pas accéder à ce campus (veuillez contacter votre administrateur)";
                            await _identityService.LogOut();
                            return Ok(response);
                        }

                    }
                    else
                    {
                        response.IsValid = false;
                        response.Message = "paramètres de connexion incorrects";
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

        //[Authorize(Roles = "Administrateur, Support Informatique")]

        #region Affectation
        [HttpGet]
        public IActionResult AffectList()
        {
            var UsersAff = _identityService.GetAllAffectation;
            return View(UsersAff);
        }

        //[Authorize(Roles = "Support Informatique")]

        [HttpGet]
        public IActionResult Affectation()
        {
            ViewBag.CampusList = _param.GetAllCampus;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Affectation(AffectationVM model)
        {
            AffectationVMValidator validator = new AffectationVMValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);
            if (response.IsValid)
            {

                try
                {
                    if (await _identityService.AssignToCamp(model.UserId, model.Campus))
                    {
                        response.Message = "Utilisateur affecté  avec succès";
                        return Ok(response);
                    }

                    else
                    {

                        response.IsValid = false;
                        response.Message = " échec lors de l'affectation de l'utilisateur";
                        return Ok(response);
                    }
                }
                catch (Exception? ex)
                {
                    return BadRequest(response);
                }
            }
            return BadRequest(response);
        }
        #endregion

        #region Inscription

        //[Authorize(Roles = "Administrateur, Support Informatique")]

        public async Task<IActionResult> Register()
        {
            ViewBag.Campus = _param.GetAllCampus;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {

            ViewBag.Campus = _param.GetAllCampus;

            RegisterValidator validator = new RegisterValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);
            if (response.IsValid)
            {
                try
                {

                    string? code = await _identityService.Register(model);

                    if (code != null)
                    {
                        var callbackurl = Url.Action("ConfirmEmail", "IdentityManagement", new { userId = model.UserId, code = code }, protocol: HttpContext.Request.Scheme);
                        //await _emailSender.SendEmailAsync(model.Email, "Confirm your account", "<p>Please Confirm your account by clicking here : <a href=\"" + callbackurl + "\">Link</a></p>");
                        if (model.Roles != null && (model.Roles.Count() > 0))
                        {
                            foreach (_enumAppRoles role in model.Roles)
                            {
                                await _identityService.AddToRole(model.UserId, role);
                            }
                        }
                        if (model.Campus != null && (model.Campus.Count() > 0))
                        {
                            await _identityService.AssignToCamp(model.UserId, model.Campus);
                        }
                        response.Message = "Création de compte réussie";
                        return CreatedAtAction(nameof(Register), response);
                    }
                    response.IsValid = false;
                    response.Message = "Echec lors de la création du compte";
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    response.Message = "Echec lors de la création du compte";
                    return BadRequest(response);
                }

            }

            return BadRequest(response);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (!(userId == null || code == null))
            {
                try
                {
                    if (await _identityService.ConfirmEmail(userId, code))
                    {
                        TempData["sucessMessage"] = "Le mot de passe a été réinitialisé";
                        return RedirectToAction("Login");
                    }

                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = ex;
                }
            }
            TempData["WarningMessage"] = "Verifier vos champs" + Response;
            return View();

        }
        #endregion

        #region Gestion des mots de passe
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)

        {
            model.IPAdress = GetIPAdress();

            ForgotPasswordValidator validator = new ForgotPasswordValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);
            if (response.IsValid)
            {
                try
                {
                    string? code = await _identityService.ForgotPassword(model) ?? "";

                    var callbackurl = Url.Action("ResetPassword", "IdentityManagement", new { userId = model.UserId, code = code }, protocol: HttpContext.Request.Scheme);

                    await _emailSender.SendEmailAsync((model.Email ?? ""), "Confirm Email Address", "<p>Please Confirm your Account by clicking here : <a href=\"" + callbackurl + "\">Link</a></p>");
                    TempData["sucessMessage"] = "L'email de récupération a été envoyé";
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = ex;
                }

            }

            TempData["WarningMessage"] = "Verifier vos champs";
            return View(model);
        }
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            model.IPAdress = GetIPAdress();

            ResetPasswordValidator validator = new ResetPasswordValidator();
            ValidationResult validationResult = validator.Validate(model);
            ResponseVM response = FluentUtilities.GetValidationError(validationResult);
            if (response.IsValid)
            {
                try
                {
                    if (await _identityService.ResetPassword(model))
                        TempData["sucessMessage"] = "Le mot de passe a été réinitialisé";
                }
                catch (Exception ex)
                {
                    TempData["errorMessage"] = ex;
                }

            }
            TempData["WarningMessage"] = "";

            return View(model);
        }
        #endregion

        #region  Adresse Ip
        private string? GetIPAdress()
        {
            string ip = Response.HttpContext.Connection.RemoteIpAddress.ToString();
            if (ip == "::1")
            {
                ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[3].ToString();
            }
            return ip;
        }
        public async Task<IActionResult> LogOut()
        {
            await _identityService.LogOut();
            return RedirectToAction(nameof(IdentityManagementController.Login), "IdentityManagement");

        }
        #endregion
    }

}
