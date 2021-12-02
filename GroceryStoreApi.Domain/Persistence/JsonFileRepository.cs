using GroceryStoreApi.Domain.Model;
using Newtonsoft.Json.Linq;

namespace GroceryStoreApi.Domain.Persistence
{
    public class CustomerRepository : ICustomerRepository
    {
        static IList<Customer> DATABASE;
        static object lockObject = new object();

        public static void Load(string fileName)
        {
            lock(lockObject) {
                if(DATABASE == null) {
                    var file = File.ReadAllText(fileName);
                    dynamic database = JObject.Parse(file);
                    JArray customers = database.customers;
                    DATABASE = customers.Select(x=> new Customer(){Id = (int)x["id"], Name = (string)x["name"]}).ToList<Customer>();       
                }
            }
        }

        public void Delete(Customer target)
        {
            lock(DATABASE) {
                DATABASE.Remove(target);
            }
        }

        public Customer Find(int id)
        {            
            return DATABASE.SingleOrDefault(x=> x.Id == id);
        }

        public Customer[] GetAll()
        {
            return DATABASE.ToArray();
        }

        public void Insert(Customer target)
        {
            lock(DATABASE) {
                var maxIdentifier = DATABASE.Max<Customer>(x=> x.Id);
                target.Id = maxIdentifier + 1;
                DATABASE.Add(target);
            }
        }        

        public IList<Customer> Query(Func<Customer, bool> where)
        {
            return DATABASE.Where(where).ToList<Customer>();
        }

        public void Save(Customer target)
        {
            lock(DATABASE){
                DATABASE.Single(x=> x.Id == target.Id).Name = target.Name;
            }
        }
    }
}
