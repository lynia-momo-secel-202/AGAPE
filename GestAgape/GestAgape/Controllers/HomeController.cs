using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using GestAgape.Core.Entities.Admission;
using GestAgape.Models;
using GestAgape.Service.Admissions;
using GestAgape.Service.Scolarite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GestAgape.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IScolarite _scolarite;
        private readonly IAdmission _admission;

        public HomeController(ILogger<HomeController> logger, IScolarite scolarite, IAdmission admission)
        {
            _logger = logger;
            _admission = admission;
            _scolarite = scolarite;
        }

        public IActionResult Dashboard()
        {
            ViewBag.TotalInscritsParJour = _scolarite.TotalInscritsParJour();
            ViewBag.TotalDA = _admission.TotalDAParJour();
            ViewBag.NbrePaiements = _admission.NbrePaiementParJour();
            ViewBag.NextConcours = _admission.NombreJourNextConcours();
            ViewBag.NombreAdmis = _admission.TotalAdmisLastConcours();
            ViewBag.LastConcours = _admission.GetAllConcours.LastOrDefault();
            ViewBag.TabInscrits = _scolarite.NbreInscritsAnnuel();  
            ViewBag.DixAA = _admission.LastAA();
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}