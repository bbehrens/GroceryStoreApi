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
            CustomerJsonFileRepository.Load("database.json");
            var repo = new CustomerJsonFileRepository();
            var customerObjects = repo.GetAll();
            Assert.Equal(3, customerObjects.Length);
        }

        [Fact]
        public void ShouldHaveACustomerNamedBob()
        {
            CustomerJsonFileRepository.Load("database.json");
            var repo = new CustomerJsonFileRepository();            
            Assert.Equal(1, repo.Query(x=> x.Name == "Bob").Count);
        }

        [Fact]
        public void ShouldNotHaveACustomerNamedBrandon()
        {
            CustomerJsonFileRepository.Load("database.json");
            var repo = new CustomerJsonFileRepository();            
            Assert.Equal(0, repo.Query(x => x.Name == "Brandon").Count);
        }

        [Fact]
        public void ShouldDeleteAnItem()
        {
            CustomerJsonFileRepository.Load("database.json");
            var repo = new CustomerJsonFileRepository();     
            var bob = repo.Find(1);
            Assert.NotNull(bob);
            repo.Delete(bob);
            bob = repo.Find(1);
            Assert.Null(bob);
        }

        [Fact]
        public void ShouldUpdateAnItem()
        {
            CustomerJsonFileRepository.Load("database.json");
            var repo = new CustomerJsonFileRepository();     
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
            CustomerJsonFileRepository.Load("database.json");
            var repo = new CustomerJsonFileRepository();     
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
            CustomerJsonFileRepository.Load("database.json");
            var repo = new CustomerJsonFileRepository();       
            repo.Insert(new Customer(){Id = 4, Name = "Brandon"});
            repo.Persist("database1.json");
        }
    }
}