using GestAgape.Core.Entities;
using GestAgape.GenericRepository;
using GestAgape.Infrastructure.GenericRepository;
using GestAgape.Models;
using Microsoft.Extensions.Logging;

namespace GestAgape.Infrastructure.Repositories
{
    public class PasswordHistoryRepository : GenericRepository<PasswordHistory>, IPasswordHistoryRep
    {
        public PasswordHistoryRepository(IdentityContext context, ILogger logger) : base(context, logger) { }
    }
}
