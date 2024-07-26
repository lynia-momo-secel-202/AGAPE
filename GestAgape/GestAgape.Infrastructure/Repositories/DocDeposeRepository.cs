using GestAgape.Core.Entities.Admission;
using GestAgape.GenericRepository;
using GestAgape.Infrastructure.GenericRepository;
using GestAgape.Models;
using Microsoft.Extensions.Logging;

namespace GestAgape.Infrastructure.Repositories
{
    public class DocDeposeRepository : GenericRepository<DocDepose>, IDocDeposeRep
    {
        public DocDeposeRepository(IdentityContext context, ILogger logger) : base(context, logger) { }
    }
}
