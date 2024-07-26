using GestAgape.Core.Entities.Identity;
using GestAgape.GenericRepository;
using GestAgape.Infrastructure.GenericRepository;
using GestAgape.Models;
using Microsoft.Extensions.Logging;

namespace GestAgape.Infrastructure.Repositories { 
    public class PersonneRepository : GenericRepository<Personne>, IPersonneRep
    { 
        public PersonneRepository(IdentityContext context, ILogger logger) : base(context, logger) { } 
    } 
}
