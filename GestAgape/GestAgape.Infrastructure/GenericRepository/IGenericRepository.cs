using GestAgape.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestAgape.GenericRepository
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        DbSet<T> Values { get; }
        IEnumerable<T> GetAll();
        T Get(Guid id);
        IEnumerable<T> GetAllByDate(DateTime startDate, DateTime endDate);
        Task Insert(T entity);
        bool Update(T entity);
        bool Delete(T entity);

    }
}
