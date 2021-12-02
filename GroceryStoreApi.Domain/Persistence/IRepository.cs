using GroceryStoreApi.Domain.Model;
using System.Linq.Expressions;

namespace GroceryStoreApi.Domain.Persistence
{
    public interface ICustomerRepository
    {
        Customer Find(int id);
        
        IList<Customer> Query(Func<Customer, bool> where);
        
        void Delete(Customer target);
        void Save(Customer target);
        void Insert(Customer target);

        Customer[] GetAll();
    }
}
