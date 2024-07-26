using FluentValidation.Results;

namespace GestAgape.Core.ViewModels.FluentValidators
{
    public class FluentUtilities
    {
        #region Fluent API Error Messages

        #region Password Messages
        public static string PasswordDoNotEmpty = ("Votre mot de passe ne peut pas être vide");
        public static string PasswordMinLenght = ("La longueur de votre mot de passe doit être d'au moins 8.");
        public static string PasswordMaxLenght = ("La longueur de votre mot de passe ne doit pas dépasser 16.");
        public static string PasswordMustHaveUpperCase = ("Votre mot de passe doit contenir au moins une lettre majuscule.");
        public static string PasswordMustHaveLowerCase = ("Votre mot de passe doit contenir au moins une lettre minuscule.");
        public static string PasswordMustHaveDigit = ("Votre mot de passe doit contenir au moins un chiffre.");
        public static string PasswordMustHaveSpecialChar = ("Votre mot de passe doit contenir au moins un caractere special (!? *.).");
        public static string PasswordDoNotMatch = ("Vos mots de passe ne correspondent pas");
        #endregion

        #region Email Messages
        public static string EmailDoNotEmpty = ("L'adresse e-mail ne peut pas être vide");
        public static string EmailAreInvalid = ("L'Adresse e-mail n'est pas valide");
        public static string EmailAreNotAllowedBecauseDomain = ("L'adresse e-mail n'appartient pas à un domaine valide");
        #endregion

        #region Phone Messages
        public static string PhoneDoNotEmpty = ("Le numéro de téléphone ne peut pas être vide");
        public static string PhoneLenght = ("La longueur du numéro de téléphone doit être de 9 chiffres");
        public static string PhoneAreInvalid = ("Le numéro de téléphone n'est pas correct");
        #endregion

        #region Annee Messages
        public static string AnneeDebutDoNotEmpty = ("L'annee de debut ne peut pas être vide");
        public static string AnneeLenght = ("La longueur d de téléphone doit être de 4 chiffres");
        public static string AnneeAreInvalid = ("L'annee de debut n'est pas correct");
        #endregion

        #region First & Last Name Messages
        public static string FirstNameDoNotEmpty = ("Le nom ne peut pas être vide");
        public static string LastNameDoNotEmpty = ("Le prénom ne peut pas être vide");
        public static string FirstNameLenght = ("La longueur du nom doit être comprise entre 2 & 25");
        public static string LastNameLenght = ("La longueur du prénom doit être comprise entre 2 & 25");
        #endregion

        #region Champs communs Parametrage Messages
        public static string LibelleDoNotEmpty = ("Le libellé ne peut pas être vide");
        public static string CodeDoNotEmpty = ("Le code ne peut pas être vide");
        public static string FraisConcoursDoNotEmpty = ("Veuillez renseigner les frais de concours");
        public static string AdresseDoNotEmpty = ("L'adresse ne peut pas être vide");
        public static string ResponsableDoNotEmpty = ("Le responsable ne peut pas être vide");
        public static string IpesDoNotEmpty = ("L'IPES ne peut pas être vide");
        #endregion

        #region Affectation Messages
        public static string UserDoNotEmpty = ("L'Utilisateur ne peut pas être vide");
        public static string CampusDoNotEmpty = ("Veuillez sélectionner au moins un campus");
        #endregion

        #region Classe Messages
        public static string SerieDoNotEmpty = ("La série ne peut pas être vide");
        public static string CycleDoNotEmpty = ("Le cycle ne peut pas être vide");
        public static string NiveauDoNotEmpty = ("Le niveau ne peut pas être vide");
        public static string FraisEtudeDossierDoNotEmpty = ("Veuillez renseigner les frais d'étude de dossier");
        #endregion

        #region IPES Messages
        public static string SiteWebDoNotEmpty = ("Le Site Web ne peut pas être vide");
        public static string ContactDoNotEmpty = ("Le contact ne peut pas être vide");
        public static string NomIPESDoNotEmpty = ("Le nom de l'IPES ne peut pas être vide");
        public static string BoitePostaleIPESDoNotEmpty = ("La boite postale de l'IPES ne peut pas être vide");
        public static string LogoDoNotEmpty = ("Le logo ne peut pas être vide");
        #endregion

        #region Chef Departement Messages
        public static string DepartementDoNotEmpty = ("Veuillez sélectionner le département");
        #endregion

        #region Personne Messages
        public static string SexeDoNotEmpty = ("Le sexe ne peut pas être vide");
        public static string DateNaissanceDoNotEmpty = ("La date de naissance ne peut pas être vide");
        public static string LieuNaissanceDoNotEmpty = ("Le lieu de naissancene peut pas être vide");
        public static string StatutMatrimonialDoNotEmpty = ("Le statut matrimonial ne peut pas être vide");
        public static string LangueDoNotEmpty = ("La langue ne peut pas être vide");
        public static string NationaliteDoNotEmpty = ("La nationalité ne peut pas être vide");
        #endregion

        #region Candidat Messages
        public static string ResidenceDoNotEmpty = ("Le lieu de résidence ne peut pas être vide");
        public static string EtablissementDoNotEmpty = ("Veuillez renseigner le dernier établissement fréquenté");
        #endregion 

        #region DemandeAdmission
        public static string AnneeAcademiqueDoNotEmpty = ("L'année académique ne peut pas être vide");
        public static string ClasseDoNotEmpty = ("Veuillez sélectionner la classe");
        public static string ConcoursDoNotEmpty = ("Veuillez sélectionner un concours");
        #endregion

        #region Paiement Messages
        public static string PaiementDoNotEmpty = ("Le montant du paiement ne peut pas être vide");
        #endregion

        #region TrancheScolarite Messages
        public static string MontantDoNotEmpty = ("Le montant ne peut pas être vide");
        public static string CampusDoNotEmpty2 = ("Le campus ne peut pas être vide");
        #endregion

        #region FraisDossierExamen Messages
        public static string FraisDossierExamenDoNotEmpty = ("Le  montant ne peut pas être vide");
        #endregion

        #region FluentAPI RegEx
        public static string RegExPhone = @"(6|2)(2|3|[5-9])[0-9]{7}";
        public static string RegExEmailDomain = @"( |^)[^ ]*@(ime-school.com|secelgroup.com|gmail.com|yahoo.fr|webmail.com)( |$)";
        public static string RegExNomPrenom = @"^[a-zA-Z0-9áàâäãåçéèêëíìîïñóòôöõúùûüýÿæœÁÀÂÄÃÅÇÉÈÊËÍÌÎÏÑÓÒÔÖÕÚÙÛÜÝŸÆŒ\s]*$";
        #endregion

        #region champs partagés

        public static string LibelleFormat = ("Le libellé ne doit pas contenir les caractères speciaux tels que &,#,~,...");
        public static string NomFormat = ("Le nom ne doit pas contenir les caractères speciaux tels que &#~@");
        public static string PrenomFormat = ("Le prénom ne doit pas contenir les caractères speciaux tels que &,#,~,@,...");

        #endregion  

        #region FluentAPI Utilities Methods
        public static ResponseVM GetValidationError(ValidationResult validationResult)
        {
            ResponseVM response = new ResponseVM();
            List<string> ValidationMessages = new List<string>();


            if (!validationResult.IsValid)
            {
                response.IsValid = false;
                foreach (ValidationFailure failure in validationResult.Errors)
                {
                    ValidationMessages.Add(failure.ErrorMessage);
                }
                response.ValidationMessages = ValidationMessages;
            }
            return response;
        }
        #endregion

        #region ChangementSerie

        public static string MotifDoNotEmpty = ("Le motif ne peut pas être vide");

        #endregion


        #endregion
    }
}
