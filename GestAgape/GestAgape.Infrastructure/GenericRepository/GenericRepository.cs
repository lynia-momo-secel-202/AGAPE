using GestAgape.Core.Entities;
using GestAgape.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GestAgape.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly IdentityContext context;
        private DbSet<T> entities;
        protected readonly ILogger _logger;


        public GenericRepository(IdentityContext context, ILogger logger)
        {
            this.context = context;
            entities = context.Set<T>();
            this._logger = logger;
        }
        DbSet<T> IGenericRepository<T>.Values => this.entities;

        public T Get(Guid id) => entities.FirstOrDefault(s => s.Id == id);




        public IEnumerable<T> GetAll()
        {
            return entities.ToList();
        }

        public IEnumerable<T> GetAllByDate(DateTime startDate, DateTime endDate)
        {
            var allEntities = entities.ToList();

            var filteredEntities = allEntities.Where(entity => entity.AddedDate >= startDate && entity.AddedDate <= endDate);

            return filteredEntities;
        }

        public bool Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                context.Remove(entity);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la suppression", typeof(T));
                return false;
            }
        }

        public async Task Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await context.AddAsync(entity);

        }
        public bool Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                context.Update(entity);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex} Echec lors de la mise à jour", typeof(T));
                return false;
            }
        }

 
    }
}
