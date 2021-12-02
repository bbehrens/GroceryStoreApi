using GroceryStoreApi.Domain.Model;
using GroceryStoreApi.Domain.Persistence;
using GroceryStoreAPI.Controllers.Customers;
using GroceryStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Controllers
{
    
    [ApiController]
    public class CustomersController : ControllerBase
    {
        IRepository _repository;
        private ICustomerService _customerService;

        public CustomersController(IRepository repository, ICustomerService customerService)
        {
            _repository = repository;
            _customerService = customerService;
        }

        [HttpGet, Route("customers")]
        public async Task<ActionResult<IEnumerable<CustomerViewModel>>> List()
        {
            var customers = await Task.FromResult(_repository.GetAll<Customer>());
            return Ok(customers.Select(x=> new CustomerViewModel(x)));
        }

        [HttpGet, Route("customers/{id:int}")]
        public async Task<ActionResult<CustomerViewModel>> Get([FromRoute]int id)
        {
            var customer = await Task.FromResult(_repository.Find<Customer>(id));
            return Ok(new CustomerViewModel(customer));
        }

        [HttpPost, Route("customers")]
        public async Task<ActionResult<CustomerViewModel>> Create([FromBody]CreateCustomerRequest request)
        {
            var customer = await Task.FromResult(_customerService.Create(request));
            return Ok(new CustomerViewModel(customer));
        }

        [HttpPut, Route("customers")]
        public async Task<ActionResult<CustomerViewModel>> Edit([FromBody]EditCustomerRequest request)
        {
            var customer = await Task.FromResult(_customerService.Update(request));
            return Ok(new CustomerViewModel(customer));
        }

        [HttpDelete, Route("customers/{id:int}")]
        public async Task<ActionResult<CustomerViewModel>> Delete([FromRoute]int id)
        {
            await Task.Run(() => _customerService.Delete(id));
            return NoContent();
        }
    }
}
