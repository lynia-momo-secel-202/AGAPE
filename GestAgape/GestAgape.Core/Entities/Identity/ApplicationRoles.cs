using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Core.Entities.Identity
{
    public static class ApplicationRoles
    {
        public const string Administrateur = "Administrateur";
        public const string SupportIT = "Support Informatique";
        public const string caisse_et_Operations = "Service Caisse & Opérations";
        public const string Service_Examen = "Service des Examens";
        public const string Service_Discipline = "Service Discipline";
        public const string Chef_Département = "Chef de Département";
        public const string Service_Logistique = "Service Logistique";
        public const string Gestionnaire_Bourses = "Gestionnaire des Bourses";
        public const string Service_Comptabilite = "Service Comptabilité";
    }
    public enum _enumAppRoles
    {
        Administrateur, SupportIT, caisse_et_Operations, Service_Examen, 
        Service_Discipline, Chef_Département, Service_Logistique, Gestionnaire_Bourses
    }
}
