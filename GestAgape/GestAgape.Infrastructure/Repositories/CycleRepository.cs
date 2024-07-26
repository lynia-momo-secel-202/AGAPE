using GestAgape.Core.Entities.Parametrage;
using GestAgape.GenericRepository;
using GestAgape.Infrastructure.GenericRepository;
using GestAgape.Models;
using Microsoft.Extensions.Logging;

namespace GestAgape.Infrastructure.Repositories
{
    public class CycleRepository : GenericRepository<Cycle>, ICycleRep
    {
        public CycleRepository(IdentityContext context, ILogger logger) : base(context, logger) { }
    }
}
