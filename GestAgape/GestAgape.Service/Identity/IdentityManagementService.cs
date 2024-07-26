using GestAgape.Core.Entities;
using GestAgape.Core.Entities.Identity;
using GestAgape.Core.Entities.Parametrage;
using GestAgape.Core.ViewModels;
using GestAgape.Infrastructure.Utilities;
using GestAgape.Models;
using GestAgape.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GestAgape.Service.Identity
{

    public class IdentityManagementService : IIdentityManagement
    {
        #region Membres prives

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly SignInManager<ApplicationUsers> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        protected readonly ILogger _logger;
        private readonly IWebHostEnvironment _hostingEnv;
        private IdentityContext _context;
        #endregion

        #region Constructeur
        public IdentityManagementService(

            UserManager<ApplicationUsers> userManager,
            SignInManager<ApplicationUsers> signInManager,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork,
            ILoggerFactory loggerFactory,
            IWebHostEnvironment hostingEnv,
            IdentityContext context

        )
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            this._logger = loggerFactory.CreateLogger("logs");
            _hostingEnv = hostingEnv;
            _context = context;


        }
        #endregion

        public async Task<bool> Login(LoginVM model)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: true);

                if (result.Succeeded)
                {

                    ApplicationUsers User = await _userManager.FindByEmailAsync(model.Email);
                    Connexion c = new Connexion()
                    {
                        AddedDate = DateTime.UtcNow,
                        IPAddress = model.IPAdress,
                        ModifiedDate = DateTime.UtcNow,
                        Place = GestAgapeUtilitiesFunctions.GetLocation(model.IPAdress),
                        UserID = User.Id.ToString(),
                        Id = new Guid(),
                    };
                    await _unitOfWork.Connexion.Insert(c);
                    _unitOfWork.Save();
                    _unitOfWork.Commit();
                    model.UserId = c.UserID;
                    return true;

                }
                _unitOfWork.Rollback();
                return false;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la connexion de l'utilisateur {model.Email}", typeof(IdentityManagementService));
                return false;
            }

        }
        public List<Affectation> GetAllAffectation => _unitOfWork.Affectation.Values
                                                                                .Include(a => a.Campus)
                                                                                .Include(a => a.User)
                                                                                .ToList();
        public Affectation ConnectAffectUser(LoginVM model)
        {
            if (model == null)
                return null;
            try
            {
                Affectation? userAffect = GetAllAffectation.FirstOrDefault(e => e.UserId == model.UserId && e.CampusId == model.CampusId);

                return userAffect;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la connexion", typeof(IdentityManagementService));
                return null;
            }
        }

        public async Task<string?> Register(RegisterVM model)
        {
            try
            {

                var user = new ApplicationUsers
                {

                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Email,
                    RegistredDate = DateTime.UtcNow,
                    LatestModificationDate = DateTime.UtcNow,
                    ProfilePicture = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.ProfilePicture, FileType.Image, $"{model.Email}_{model.FirstName}", GestAgapeUtilitiesFunctions.ProfileImageFolder),
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                model.UserId = user.Id;

                if (result.Succeeded)
                {
                    if (model.Handicape == true)
                    {
                        model.HandicapeDes = "Oui ," + model.HandicapeDes;
                    }
                    else
                    {
                        model.HandicapeDes = "Non";
                    }
                    Personne personne = new Personne()
                    {
                        Id = new Guid(),
                        AddedDate = DateTime.UtcNow,
                        Email = model.Email,
                        ModifiedDate = DateTime.UtcNow,
                        Nom = model.FirstName,
                        Prenom = model.LastName,
                        Telephone = model.PhoneNumber,
                        Sexe = model.Sexe,
                        StatutMatrimonial = model.StatutMatrimonial,
                        Nationalite = model.Nationalite,
                        Region = model.Region,
                        Langue = model.Langue,
                        Handicape = model.HandicapeDes,
                        DateNaissance = model.DateNaissance,
                        LieuNaissance = model.LieuNaissance,
                        CurriculumVitae = GestAgapeUtilitiesFunctions.UploadFile(_hostingEnv, model.CurriculumVitae, FileType.Document, $"CV_{model.Email}_{model.FirstName}", GestAgapeUtilitiesFunctions.CvFolder),

                    };
                    await _unitOfWork.Personne.Insert(personne);
                    _unitOfWork.Save();

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    if (code != null)
                    {
                        _unitOfWork.Commit();
                        return code;

                    }
                    else
                    {
                        _unitOfWork.Rollback();
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la tentative de création du compte de l'utilisateur {model.Email}", typeof(IdentityManagementService));
                return null;
            }
            _unitOfWork.Rollback();
            return null;

        }
        public async Task<bool> LogOut()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la tentative de déconnexion", typeof(IdentityManagementService));
                return false;
            }
        }

        public async Task<string?> ForgotPassword(ForgotPasswordVM model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return null;
                }
                model.UserId = user.Id;
                string? code = await _userManager.GeneratePasswordResetTokenAsync(user);
                return code;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la tentative de récupération de mot de passe du compte {model.Email}", typeof(IdentityManagementService));
                return null;
            }
        }
        public async Task<bool> ResetPassword(ResetPasswordVM model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    _unitOfWork.Rollback();
                    return false;
                }
                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (result.Succeeded)
                {
                    PasswordHistory password = new PasswordHistory()
                    {
                        AddedDate = DateTime.UtcNow,
                        //IPAddress = model.IPAdress,
                        LastUsed = DateTime.UtcNow,
                        UserID = user.Id,
                        PasswordHash = user.PasswordHash,
                        ModifiedDate = DateTime.UtcNow,
                        Id = new Guid()
                    };
                    await _unitOfWork.PasswordHistory.Insert(password);
                    _unitOfWork.Save();
                    _unitOfWork.Commit();
                    return true;
                }
                _unitOfWork.Rollback();
                return false;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la tentative de réinitialisation du mot de passe du compte {model.Email}", typeof(IdentityManagementService));
                return false;
            }


        }

        public async Task<bool> ConfirmEmail(string userId, string code)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _unitOfWork.Rollback();
                    return false;
                }
                var result = await _userManager.ConfirmEmailAsync(user, code);
                if (result.Succeeded)
                {
                    _unitOfWork.Commit();
                    return true;
                }
                _unitOfWork.Rollback();
                return false;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la tentative de confirmation du compte {userId}", typeof(IdentityManagementService));
                return false;
            }
        }
        public async Task<bool> AssignToCamp(string userId, List<Guid> campusId)
        {

            try
            {
                foreach (Guid id in campusId)
                {
                    if (VerifExistAffect(id, userId) == false)
                    {
                        Affectation affectation = new Affectation()
                        {
                            AddedDate = DateTime.UtcNow,
                            CampusId = id,
                            UserId = userId,
                            ModifiedDate = DateTime.UtcNow,
                            DateAffectation = DateTime.UtcNow,
                            Id = new Guid()
                        };
                        await _unitOfWork.Affectation.Insert(affectation);
                    }
                }
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la tentative d'affectation de {userId} aux campus", typeof(IdentityManagementService));
                return false;
            }
        }
        public async Task<bool> AddToRole(string? userId, _enumAppRoles role)
        {
            try
            {
                //_unitOfWork.CreateTransaction();
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _unitOfWork.Rollback();
                    return false;
                }
                switch (role)
                {
                    case _enumAppRoles.Administrateur:
                        await _userManager.AddToRoleAsync(user, ApplicationRoles.Administrateur);
                        break;
                    case _enumAppRoles.SupportIT:
                        await _userManager.AddToRoleAsync(user, ApplicationRoles.SupportIT);
                        break;
                    case _enumAppRoles.caisse_et_Operations:
                        await _userManager.AddToRoleAsync(user, ApplicationRoles.caisse_et_Operations);
                        break;
                    case _enumAppRoles.Service_Examen:
                        await _userManager.AddToRoleAsync(user, ApplicationRoles.Service_Examen);
                        break;
                    case _enumAppRoles.Service_Discipline:
                        await _userManager.AddToRoleAsync(user, ApplicationRoles.Service_Discipline);
                        break;
                    case _enumAppRoles.Chef_Département:
                        await _userManager.AddToRoleAsync(user, ApplicationRoles.Chef_Département);
                        break;
                    case _enumAppRoles.Service_Logistique:
                        await _userManager.AddToRoleAsync(user, ApplicationRoles.Service_Logistique);
                        break;
                    case _enumAppRoles.Gestionnaire_Bourses:
                        await _userManager.AddToRoleAsync(user, ApplicationRoles.Gestionnaire_Bourses);
                        break;
                    default: _unitOfWork.Rollback(); return false;
                }
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError(ex, $"{ex} Echec lors de la tentative d'ajout du role N° {role} du compte {userId}", typeof(IdentityManagementService));
                return false;
            }
        }
        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() <= 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception)
            {
                throw;
            }
            if (!_roleManager.RoleExistsAsync(ApplicationRoles.Administrateur).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(ApplicationRoles.Administrateur)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(ApplicationRoles.SupportIT)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(ApplicationRoles.caisse_et_Operations)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(ApplicationRoles.Service_Examen)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(ApplicationRoles.Service_Discipline)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(ApplicationRoles.Chef_Département)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(ApplicationRoles.Service_Logistique)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(ApplicationRoles.Gestionnaire_Bourses)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUsers
                {
                    UserName = "Plys",
                    Email = "plyspcsnart@gmail.com",

                }, "Plys.bin@2022").GetAwaiter().GetResult();

                var AppUser = _context.ApplicationUser.FirstOrDefault(x => x.Email == "plyspcsnart@gmail.com");

                if (AppUser != null)
                {
                    _userManager.AddToRoleAsync(AppUser, ApplicationRoles.Administrateur).GetAwaiter().GetResult();
                }
            }
        }

        public bool VerifExistAffect(Guid CampusId, string UserId)
        {
            var affectation = _unitOfWork.Affectation.Values.FirstOrDefault(af => af.CampusId == CampusId && af.UserId == UserId);
            if (affectation == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }



    }
}
