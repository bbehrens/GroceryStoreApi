using GroceryStoreApi.Domain.Model;
using GroceryStoreApi.Domain.Persistence;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.IO;
using Xunit;

namespace GroceryStoreApi.Test
{
    public class DatabaseTest
    {
        [Fact]
        public void LoadTheDatabase()
        {
            CustomerRepository.Load("database.json");
            var repo = new CustomerRepository();
            var customerObjects = repo.GetAll();
            Assert.Equal(3, customerObjects.Length);
        }

        [Fact]
        public void ShouldHaveACustomerNamedBob()
        {
            CustomerRepository.Load("database.json");
            var repo = new CustomerRepository();            
            Assert.Equal(1, repo.Query(x=> x.Name == "Bob").Count);
        }

        [Fact]
        public void ShouldNotHaveACustomerNamedBrandon()
        {
            CustomerRepository.Load("database.json");
            var repo = new CustomerRepository();            
            Assert.Equal(0, repo.Query(x => x.Name == "Brandon").Count);
        }

        [Fact]
        public void ShouldDeleteAnItem()
        {
            CustomerRepository.Load("database.json");
            var repo = new CustomerRepository();     
            var bob = repo.Find(1);
            Assert.NotNull(bob);
            repo.Delete(bob);
            bob = repo.Find(1);
            Assert.Null(bob);
        }

        [Fact]
        public void ShouldUpdateAnItem()
        {
            CustomerRepository.Load("database.json");
            var repo = new CustomerRepository();     
            var bob = repo.Find(1);
            Assert.NotNull(bob);
            Assert.Equal("Bob", bob.Name);
            var newDude = new Customer() {Id = bob.Id, Name = "new dude"};
            repo.Save(newDude);

            newDude = repo.Find(1);
            Assert.NotNull(newDude);
            Assert.Equal("new dude", newDude.Name);
            Assert.Equal(1, newDude.Id);

            Assert.Empty(repo.Query(x=> x.Name == "Bob"));                        
        }

        [Fact]
        public void ShouldAddAnItem()
        {
            CustomerRepository.Load("database.json");
            var repo = new CustomerRepository();     
            var bob = repo.Find(4);
            Assert.Null(bob);
            
            var brandon = new Customer() {Name = "brandon"};
            repo.Insert(brandon);

            brandon = repo.Find(4);
            Assert.NotNull(brandon);
            Assert.Equal("brandon", brandon.Name);
            Assert.Equal(4, brandon.Id);           
        }

        [Fact]
        public void Persist()
        {
            CustomerRepository.Load("database.json");
            var repo = new CustomerRepository();       
            repo.Insert(new Customer(){Id = 4, Name = "Brandon"});
            var customers = JsonConvert.SerializeObject(repo.GetAll(), 
                    new JsonSerializerSettings()
                    {
                        ContractResolver = new DefaultContractResolver{
                            NamingStrategy = new CamelCaseNamingStrategy()

                        },
                        Formatting = Formatting.Indented,
                    });
            customers = "{customers: " + customers + "}";
            var foo = JsonConvert.DeserializeObject(customers);
            var final = JsonConvert.SerializeObject(foo, new JsonSerializerSettings(){Formatting=Formatting.Indented});
            File.WriteAllText("database1.json", final);                
        }
    }
}