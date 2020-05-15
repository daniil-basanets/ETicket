using System.Linq;

namespace ETicket.DataAccess.Domain.Interfaces
{
    public interface IRepository<T, K> 
        where T : class 
    {
        IQueryable<T> GetAll();
        T Get(K id);
        void Create(T item);
        void Update(T item);
        void Delete(K id);
    }
}