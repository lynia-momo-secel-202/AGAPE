using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels
{
    public class ConcoursVM
    {
        public Guid ConcoursId { get; set; }
        public string? Libelle { get; set; }
        public string? Date { get; set; }
        public string? HeureDebut { get; set; }
        public string? HeureFin { get; set; }
        public string? Description { get; set; }
        public IFormFile? Flyers { get; set; }
        public IFormFile? Resultats { get; set; }
        public string? FlyersPath { get; set; }
        public string? ResultatsPath { get; set; }
    }
}
