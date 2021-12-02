using GroceryStoreApi.Domain.Persistence;
using GroceryStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Controllers.v1.Customers
{

    [ApiController]
    public class CustomersController : ControllerBase
    {
        ICustomerRepository _repository;
        private ICustomerService _customerService;

        public CustomersController(ICustomerRepository repository, ICustomerService customerService)
        {
            _repository = repository;
            _customerService = customerService;
        }

        [HttpGet, Route("customers")]
        public async Task<ActionResult<IEnumerable<CustomerViewModel>>> List()
        {
            var customers = await Task.FromResult(_repository.GetAll());
            return Ok(customers.Select(x=> new CustomerViewModel(x)));
        }

        [HttpGet, Route("customers/{id:int}") ]
        public async Task<ActionResult<CustomerViewModel>> Get([FromRoute]int id)
        {
            var customer = await Task.FromResult(_repository.Find(id));
            return Ok(new CustomerViewModel(customer));
        }

        [HttpPost, Route("customers")]
        public async Task<ActionResult<CustomerViewModel>> Create([FromBody]CreateCustomerRequest request)
        {
            var customer = await Task.FromResult(_customerService.Create(request));
            return  Created($"~customers/{customer.Id}", new CustomerViewModel(customer));
        }
                
        /// <response code="200">Customer Modified Successfully</response>
        /// <response code="404">Customer Not Found</response>           
        [HttpPut, Route("customers")]
        public async Task<ActionResult<CustomerViewModel>> Edit([FromBody]EditCustomerRequest request)
        {
            var customer = await Task.FromResult(_customerService.Update(request));
            return Ok(new CustomerViewModel(customer));
        }

        /// <response code="204">Customer Deleted Successfully</response>
        /// <response code="404">Customer Not Found</response>                        
        [HttpDelete, Route("customers/{id:int}")]
        public async void Delete([FromRoute]int id)
        {
            await Task.Run(() => _customerService.Delete(id));
            NoContent();
        }
    }
}
