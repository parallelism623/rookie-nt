using mvc_todolist.Models.DbContexts;
using mvc_todolist.Models.Entities;
using mvc_todolist.Repositories.Implements;
using mvc_todolist.Repositories.Interfaces;
using mvc_todolist.Services;
using mvc_todolist.Services.ImportExport;

namespace mvc_todolist
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            return services.AddScoped<IUnitOfWork, UnitOfWork>()
                           .AddScoped<IImportExportService, ImportExportService>()
                           .AddScoped<IPersonService, PersonService>()
                           .AddMemoryCacheService();
        }


        public static async Task AddDataSeeder(this AppDbContext context)
        {

            if(!context.Persons.Any())
            {
                context.Persons.AddRange(GetSamplePersons());
                await context.SaveChangesAsync();
            }
        }
        private static List<Person> GetSamplePersons()
        {
           return new List<Person>
            {
                new Person { Id = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, ModifiedDate = null, CreateBy = Guid.NewGuid(), ModifiedBy = null, Username = "john.doe", Password = "Pass123!", FirstName = "John", LastName = "Doe", Address = "123 Main St", Gender = 0, Age = 30, DateOfBirth = new DateTime(1995, 3, 31), BirthYear = 1995 },
                new Person { Id = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, ModifiedDate = null, CreateBy = Guid.NewGuid(), ModifiedBy = null, Username = "jane.smith", Password = "Pass456!", FirstName = "Jane", LastName = "Smith", Address = "456 Oak Ave", Gender = 1, Age = 25, DateOfBirth = new DateTime(2000, 3, 31), BirthYear = 2000 },
                new Person { Id = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, ModifiedDate = null, CreateBy = Guid.NewGuid(), ModifiedBy = null, Username = "alex.lee", Password = "Pass789!", FirstName = "Alex", LastName = "Lee", Address = "789 Pine Rd", Gender = 2, Age = 28, DateOfBirth = new DateTime(1997, 3, 31), BirthYear = 1997 },
                new Person { Id = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, ModifiedDate = null, CreateBy = Guid.NewGuid(), ModifiedBy = null, Username = "mary.jones", Password = "Pass101!", FirstName = "Mary", LastName = "Jones", Address = "101 Elm St", Gender = 1, Age = 35, DateOfBirth = new DateTime(1990, 3, 31), BirthYear = 1990 },
                new Person { Id = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, ModifiedDate = null, CreateBy = Guid.NewGuid(), ModifiedBy = null, Username = "tom.brown", Password = "Pass202!", FirstName = "Tom", LastName = "Brown", Address = "202 Birch Ln", Gender = 0, Age = 40, DateOfBirth = new DateTime(1985, 3, 31), BirthYear = 1985 },
                new Person { Id = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, ModifiedDate = null, CreateBy = Guid.NewGuid(), ModifiedBy = null, Username = "lisa.white", Password = "Pass303!", FirstName = "Lisa", LastName = "White", Address = "303 Cedar Dr", Gender = 1, Age = 22, DateOfBirth = new DateTime(2003, 3, 31), BirthYear = 2003 },
                new Person { Id = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, ModifiedDate = null, CreateBy = Guid.NewGuid(), ModifiedBy = null, Username = "sam.green", Password = "Pass404!", FirstName = "Sam", LastName = "Green", Address = "404 Maple Ave", Gender = 2, Age = 27, DateOfBirth = new DateTime(1998, 3, 31), BirthYear = 1998 },
                new Person { Id = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, ModifiedDate = null, CreateBy = Guid.NewGuid(), ModifiedBy = null, Username = "emma.black", Password = "Pass505!", FirstName = "Emma", LastName = "Black", Address = "505 Willow St", Gender = 1, Age = 33, DateOfBirth = new DateTime(1992, 3, 31), BirthYear = 1992 },
                new Person { Id = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, ModifiedDate = null, CreateBy = Guid.NewGuid(), ModifiedBy = null, Username = "david.gray", Password = "Pass606!", FirstName = "David", LastName = "Gray", Address = "606 Ash Rd", Gender = 0, Age = 45, DateOfBirth = new DateTime(1980, 3, 31), BirthYear = 1980 },
                new Person { Id = Guid.NewGuid(), CreatedDate = DateTime.UtcNow, ModifiedDate = null, CreateBy = Guid.NewGuid(), ModifiedBy = null, Username = "sophia.blue", Password = "Pass707!", FirstName = "Sophia", LastName = "Blue", Address = "707 Spruce Ln", Gender = 1, Age = 29, DateOfBirth = new DateTime(1996, 3, 31), BirthYear = 1996 }
            };
        }
    }
}
