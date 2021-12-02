using GroceryStoreApi.Domain.Model;
using GroceryStoreApi.Domain.Persistence;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
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
    }
}