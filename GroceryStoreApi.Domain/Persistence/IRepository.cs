using GroceryStoreApi.Domain.Model;
using System.Linq.Expressions;

namespace GroceryStoreApi.Domain.Persistence
{
    public interface IRepository
    {
        T Find<T>(int id) where T : class, IEntity;

        IQueryable<T> Query<T>();
        IQueryable<T> Query<T>(Expression<Func<T, bool>> where);
        
        void Delete(object target);
        void Save(object target);
        void Insert(object target);

        T[] GetAll<T>();
    }
}
