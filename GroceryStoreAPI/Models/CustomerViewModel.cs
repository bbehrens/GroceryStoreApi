using GroceryStoreApi.Domain.Model;

namespace GroceryStoreAPI.Models
{
    public class CustomerViewModel
    {
        public CustomerViewModel(Customer customer)
        {
            Id = customer.Id;
            Name = customer.Name ?? string.Empty;
        }

        public int Id{ get; set; }
        public string Name{ get; set; }
    }
}
