﻿using GestAgape.Core.Entities.Admission;
using GestAgape.GenericRepository;
using GestAgape.Infrastructure.GenericRepository;
using GestAgape.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestAgape.Infrastructure.Repositories
{
    public class FraisEtudeDossierRepository : GenericRepository<FraisEtudeDossier>, IFraisEtudeDossierRep
    {
        public FraisEtudeDossierRepository(IdentityContext context, ILogger logger) : base(context, logger) { }
    }
}
