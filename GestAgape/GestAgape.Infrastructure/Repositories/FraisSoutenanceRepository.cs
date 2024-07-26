using GestAgape.Core.Entities.Scolarite;
using GestAgape.GenericRepository;
using GestAgape.Infrastructure.GenericRepository;
using GestAgape.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Infrastructure.Repositories
{
    public class FraisSoutenanceRepository : GenericRepository<FraisSoutenance>, IFraisSoutenanceRep
    {
        public FraisSoutenanceRepository(IdentityContext context, ILogger logger) : base(context, logger) { }

    }
}
