using GroceryStoreApi.Domain.Model;
using GroceryStoreApi.Domain.Persistence;

namespace GroceryStoreAPI.Controllers.v1.Customers
{
    public interface ICustomerService
    {
        Customer Update(EditCustomerRequest request);
        Customer Create(CreateCustomerRequest request);
        void Delete(int id);
    }
    public class CustomerService : ICustomerService
    {
        private ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public Customer Create(CreateCustomerRequest request)
        {
            var customer = new Customer(){Name = request.Name};
            _repository.Save(customer);
            return customer;
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public Customer Update(EditCustomerRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}
