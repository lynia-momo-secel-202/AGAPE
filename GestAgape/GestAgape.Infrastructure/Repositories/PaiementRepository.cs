using GestAgape.Core.Entities.Admission;
using GestAgape.GenericRepository;
using GestAgape.Infrastructure.GenericRepository;
using GestAgape.Models;
using Microsoft.Extensions.Logging;

namespace GestAgape.Infrastructure.Repositories
{
    public class PaiementRepository : GenericRepository<Paiement>, IPaiementRep
    {
        public PaiementRepository(IdentityContext context, ILogger logger) : base(context, logger) { }
    }
}
