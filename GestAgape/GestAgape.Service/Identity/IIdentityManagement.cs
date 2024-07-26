using GestAgape.Core.Entities.Identity;
using GestAgape.Core.Entities.Parametrage;
using GestAgape.Core.ViewModels;

namespace GestAgape.Service.Identity
{
    public interface IIdentityManagement
    {
        public Task<bool> Login(LoginVM model);
        public List<Affectation> GetAllAffectation { get; }
        public Affectation ConnectAffectUser(LoginVM model);
        public Task<string?> Register(RegisterVM model);
        public Task<bool> AssignToCamp(string userId, List<Guid> campusId);
        public Task<string?> ForgotPassword(ForgotPasswordVM model);
        public Task<bool> ResetPassword(ResetPasswordVM model);
        public Task<bool> ConfirmEmail(string userId, string code);
        public Task<bool> LogOut();
        public Task<bool> AddToRole(string? userId, _enumAppRoles role);
        public void Initialize();
        public bool VerifExistAffect(Guid CampusId, string UserId);

    }
}
