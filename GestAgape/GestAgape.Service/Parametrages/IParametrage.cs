using GestAgape.Core.Entities;
using GestAgape.Core.Entities.Parametrage;
using GestAgape.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Service.Parametrages
{
    public interface IParametrage
    {
        #region Campus
        public List<Campus> GetAllCampus { get; }
        public bool CreateCampus(Campus model);
        public bool UpdateCampus(Campus model);
        public bool DeleteCampus(Campus model);
        public Campus GetCampus(Guid Id);
        public bool VerifCampus(Campus model);

        #endregion

        #region Chef Departement
        public bool CreateChefDepartement(ChefDepartementVM model);
        public bool UpdateChefDepartementVM(ChefDepartementVM model);
        public List<ChefDepartement> GetAllChefDepartement { get; }
        public ChefDepartement GetChefDepartement(Guid Id);
        public ChefDepartementVM GetChefDepartementVM(Guid Id);
        public bool ExistChefDepartement(string Nom, string Prenom, string Telephone, Guid Id);

        #endregion

        #region Classe
        public List<Classe> GetAllClasse { get; }
        public bool CreateClasse(Classe model);
        public bool UpdateClasse(Classe model);
        public bool DeleteClasse(Classe model);
        public Classe GetClasse(Guid Id);
        public bool VerifExistClasse(Guid specialiteId, Guid niveauId);

        #endregion

        #region Cycle
        public bool CreateCycle(Cycle model);
        public bool UpdateCycle(Cycle model);
        public Cycle GetCycle(Guid Id);
        public List<Cycle> GetAllCycle { get; }
        public bool VerifCycle(Cycle model, ExistType existType);

        #endregion

        #region Departement
        public List<Departement> GetAllDepartement { get; }
        public bool CreateDepartement(Departement model);
        public bool UpdateDepartement(Departement model);
        public bool DeleteDepartement(Departement model);
        public Departement GetDepartement(Guid Id);
        public bool ExistDepartement(string Libelle, string Code, ExistType existType);

        #endregion

        #region Filiere

        public List<Filiere> GetAllFiliere { get; }
        public bool CreateFiliere(Filiere model);
        public bool UpdateFiliere(Filiere model);
        public bool DeleteFiliere(Filiere model);
        public Filiere GetFiliere(Guid Id);
        public bool ExistFiliere(string Libelle, string Code, ExistType existType);


        #endregion

        #region Ipes

        public List<Ipes> GetAllIpes { get; }
        public bool CreateIpes(Ipes model);
        public bool UpdateIpes(Ipes model);
        public bool DeleteIpes(Ipes model);
        public Ipes GetIpes(Guid Id);
        public bool VerifIpes(Ipes model, ExistType exstType);


        #endregion

        #region Niveau
        public List<Niveau> GetAllNiveau { get; }
        public bool CreateNiveau(Niveau model);
        public bool UpdateNiveau(Niveau model);
        public Niveau GetNiveau(Guid Id);
        public bool VerifNiveau(Niveau model);

        #endregion

    }
}
