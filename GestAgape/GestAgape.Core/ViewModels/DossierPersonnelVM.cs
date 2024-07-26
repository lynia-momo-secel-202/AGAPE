using GestAgape.Core.Entities.Admission;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.ViewModels
{
    public class DossierPersonnelVM
    {

        public Guid DossierPersonnelId { get; set; }
        public string? ActeNaissance { get; set; }
        public IFormFile? ActeNaissanceFile { get; set; }
        public string? ReleveBac { get; set; }
        public IFormFile? ReleveBacFile { get; set; }
        public string? CNI { get; set; }
        public IFormFile? CNIFile { get; set; }
        public string? Photos { get; set; }
        public IFormFile? PhotosFile { get; set; }
        public string? ReleveNiveau1 { get; set; }
        public IFormFile? ReleveNiveau1File { get; set; }
        public string? ReleveNiveau2 { get; set; }
        public IFormFile? ReleveNiveau2File { get; set; }
        public string? ReleveMaster1 { get; set; }
        public IFormFile? ReleveMaster1File { get; set; }
        public string? ReleveBTS { get; set; }
        public IFormFile? ReleveBTSFile{ get; set; }
        public string? ReleveLicence { get; set; }
        public IFormFile? ReleveLicenceFile{ get; set; }
        public  Candidat? Candidat { get; set; }
    }
}
