
using System.Linq.Expressions;
using System.Runtime.InteropServices.JavaScript;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using mvc_todolist.Commons.Enums;
using mvc_todolist.Models.DbContexts;
using mvc_todolist.Models.Entities;
using mvc_todolist.Repositories.Implements;
using mvc_todolist.Tests.Commons.DataSources;

namespace mvc_todolist.Tests.IntegrationTest
{
    [TestFixture]
    public class PersonRepositoryTests
    {

        private static AppDbContext? _dbContext;

        [SetUp]
        public void OneTimeSetUp()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            _dbContext = new AppDbContext(options);
            if (!_dbContext.Persons.Any())
            {
                _dbContext.Persons.AddRange(GetSamplePersons());
                _dbContext.SaveChanges();
            }
        }


        [Test]
        public async Task GetByIdAsync_WhenNotFound_ShouldReturnNull()
        {
            // Arrange

            var personId = Guid.Empty;
            var personRepository = GetPersonRepository();

            // Action

            var result = await personRepository.GetByIdAsync(personId);

            // Assert

            result.Should().BeNull();
        }


        [Test]
        public async Task GetByIdAsync_WhenGetSuccess_ShouldReturnPerson()
        {
            // Arrange

            var person = GetSamplePersons().FirstOrDefault();
            var personRepository = GetPersonRepository();

            // Action

            var result = await personRepository.GetByIdAsync(person.Id);

            // Assert

            result.Should().NotBeNull();
            result.Should().BeOfType<Person>();
            result.Should().BeEquivalentTo(person);
        }
        [Test]
        public void Add_WhenSuccess_ShouldExistsRecordInDbSet()
        {
            // Arrange
            var person = GetPersonSample();
            var personRepository = GetPersonRepository();

            // Action

            personRepository.Add(person);
            _dbContext?.SaveChanges();

            // Assert

            person.Id.Should().NotBeEmpty();
            var personInDbSet = _dbContext?.Persons.FirstOrDefault(p => p.Id == person.Id);
            personInDbSet.Should().NotBeNull();
            personInDbSet.Should().BeEquivalentTo(person);
        }

        [Test]
        public void Remove_WhenSuccess_ShouldRemovePersonOnDbContext()
        {
            // Arrange

            var person = GetPersonSample();
            var personRepository = GetPersonRepository();
            personRepository.Add(person);
            _dbContext?.SaveChanges();

            // Action

            personRepository.Remove(person);
            _dbContext?.SaveChanges();
            // Assert

            var personInDbSet = _dbContext?.Persons.FirstOrDefault(p => p.Id == person.Id);
            personInDbSet.Should().BeNull();
        }


        [Test]
        public void Update_WhenSuccess_ShouldChangedPersonOnDbContext()
        {
            // Arrange
            const string updateName = "HieuTest";
            var person = GetPersonSample();
            var personRepository = GetPersonRepository();
            personRepository.Add(person);
            _dbContext?.SaveChanges();
            person.Username = updateName;

            // Action

            personRepository.Update(person);
            _dbContext?.SaveChanges();
            // Assert

            var personInDbSet = _dbContext?.Persons.FirstOrDefault(p => p.Id == person.Id);
            personInDbSet.Should().NotBeNull();
            personInDbSet.Username.Should().BeEquivalentTo(updateName);
        }





        [TestCaseSource(typeof(PersonFilterTestCase))]
        public async Task GetAsync_WhenGetFilterSuccess_ShouldReturnSameDataInDbContextWithFilter(Expression<Func<Person, bool>> personFilter)
        {
            // Arrange 

            var dataFilter = _dbContext?.Persons?.Where(personFilter).ToList();
            var personRepository = GetPersonRepository();

            // Action

            var person = await personRepository.GetAsync(filter: personFilter);

            // Arrange

            person.Should().NotBeNull();
            person.Should().BeEquivalentTo(dataFilter);
        }

        private Person GetPersonSample()
        {
            return new()
            {
                CreateAt = DateTime.UtcNow,
                CreateBy = Guid.NewGuid(),
                Username = "johnsmith",
                Password = "pass123",
                FirstName = "John",
                LastName = "Smith",
                Address = "123 Main Street",
                Gender = 1,
                Age = 35,
                Graduated = true,
                DateOfBirth = new DateTime(1988, 4, 15),
                BirthYear = 1988
            };
        }



        [TearDown]
        public void OneTimeTearDown()
        {
            _dbContext?.Dispose();
        }
        private static List<Person> GetSamplePersons()
        {
            return new()
            {
                new() { Id = new Guid("a3f7b9c2-5e4d-4b8a-9c1e-7f2d3b6a8e9f"), CreateAt = new DateTime(2025, 4, 14, 0, 0, 0, DateTimeKind.Utc), ModifiedAt = null, CreateBy = new Guid("11111111-1111-1111-1111-111111111111"), ModifiedBy = null, Username = "john.doe", Password = "Pass123!", FirstName = "John", LastName = "Doe", Address = "123 Main St", Gender = 0, Age = 30, DateOfBirth = new DateTime(1995, 3, 31), BirthYear = 1995, Graduated = true },
                new() { Id = new Guid("d9e2c7b5-1a6f-4c9d-8b3a-2e4f7c8d1b6a"), CreateAt = new DateTime(2025, 4, 14, 0, 0, 0, DateTimeKind.Utc), ModifiedAt = null, CreateBy = new Guid("22222222-2222-2222-2222-222222222222"), ModifiedBy = null, Username = "jane.smith", Password = "Pass456!", FirstName = "Jane", LastName = "Smith", Address = "456 Oak Ave", Gender = 1, Age = 25, DateOfBirth = new DateTime(2000, 3, 31), BirthYear = 2000, Graduated = false },
                new() { Id = new Guid("f4a8d3e6-7b2c-4e9a-1d5b-8c3f9a2e7d4b"), CreateAt = new DateTime(2025, 4, 14, 0, 0, 0, DateTimeKind.Utc), ModifiedAt = null, CreateBy = new Guid("33333333-3333-3333-3333-333333333333"), ModifiedBy = null, Username = "alex.lee", Password = "Pass789!", FirstName = "Alex", LastName = "Lee", Address = "789 Pine Rd", Gender = 2, Age = 28, DateOfBirth = new DateTime(1997, 3, 31), BirthYear = 1997, Graduated = false },
                new() { Id = new Guid("b7c9e1f8-3d5a-4f2b-9a6c-1e8d2b4f7c3e"), CreateAt = new DateTime(2025, 4, 14, 0, 0, 0, DateTimeKind.Utc), ModifiedAt = null, CreateBy = new Guid("44444444-4444-4444-4444-444444444444"), ModifiedBy = null, Username = "mary.jones", Password = "Pass101!", FirstName = "Mary", LastName = "Jones", Address = "101 Elm St", Gender = 1, Age = 35, DateOfBirth = new DateTime(1990, 3, 31), BirthYear = 1990, Graduated = false },
                new() { Id = new Guid("c2f6a9d1-8e7b-4b3c-5d9e-6a1f3c8b2e5d"), CreateAt = new DateTime(2025, 4, 14, 0, 0, 0, DateTimeKind.Utc), ModifiedAt = null, CreateBy = new Guid("55555555-5555-5555-5555-555555555555"), ModifiedBy = null, Username = "tom.brown", Password = "Pass202!", FirstName = "Tom", LastName = "Brown", Address = "202 Birch Ln", Gender = 0, Age = 40, DateOfBirth = new DateTime(1985, 3, 31), BirthYear = 1985, Graduated = false },
                new() { Id = new Guid("e8b3f5a7-2c9d-4e1a-6b8f-9d2e7a4c1f6b"), CreateAt = new DateTime(2025, 4, 14, 0, 0, 0, DateTimeKind.Utc), ModifiedAt = null, CreateBy = new Guid("66666666-6666-6666-6666-666666666666"), ModifiedBy = null, Username = "lisa.white", Password = "Pass303!", FirstName = "Lisa", LastName = "White", Address = "303 Cedar Dr", Gender = 1, Age = 22, DateOfBirth = new DateTime(2003, 3, 31), BirthYear = 2003, Graduated = false },
                new() { Id = new Guid("a1d9c6e3-7f4b-4a2e-8c5d-3b9f1e6a8d2c"), CreateAt = new DateTime(2025, 4, 14, 0, 0, 0, DateTimeKind.Utc), ModifiedAt = null, CreateBy = new Guid("77777777-7777-7777-7777-777777777777"), ModifiedBy = null, Username = "sam.green", Password = "Pass404!", FirstName = "Sam", LastName = "Green", Address = "404 Maple Ave", Gender = 2, Age = 27, DateOfBirth = new DateTime(1998, 3, 31), BirthYear = 1998, Graduated = true },
                new() { Id = new Guid("f9e2b8c4-5a6d-4c1f-7e3a-2d8b9f5c1a6e"), CreateAt = new DateTime(2025, 4, 14, 0, 0, 0, DateTimeKind.Utc), ModifiedAt = null, CreateBy = new Guid("88888888-8888-8888-8888-888888888888"), ModifiedBy = null, Username = "emma.black", Password = "Pass505!", FirstName = "Emma", LastName = "Black", Address = "505 Willow St", Gender = 1, Age = 33, DateOfBirth = new DateTime(1992, 3, 31), BirthYear = 1992, Graduated = false },
                new() { Id = new Guid("d3a7f1e9-8b2c-4e5d-9a6f-7c1e3d8b2f4a"), CreateAt = new DateTime(2025, 4, 14, 0, 0, 0, DateTimeKind.Utc), ModifiedAt = null, CreateBy = new Guid("99999999-9999-9999-9999-999999999999"), ModifiedBy = null, Username = "david.gray", Password = "Pass606!", FirstName = "David", LastName = "Gray", Address = "606 Ash Rd", Gender = 0, Age = 45, DateOfBirth = new DateTime(1980, 3, 31), BirthYear = 1980, Graduated = true },
                new() { Id = new Guid("b6c8e2f5-1d9a-4f3b-5e7c-9a2f6b8d3c1e"), CreateAt = new DateTime(2025, 4, 14, 0, 0, 0, DateTimeKind.Utc), ModifiedAt = null, CreateBy = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), ModifiedBy = null, Username = "sophia.blue", Password = "Pass707!", FirstName = "Sophia", LastName = "Blue", Address = "707 Spruce Ln", Gender = 1, Age = 29, DateOfBirth = new DateTime(1996, 3, 31), BirthYear = 1996, Graduated = true }
            };
        }

        private PersonRepository GetPersonRepository()
        {
            return new PersonRepository(_dbContext);
        }
    }
}
