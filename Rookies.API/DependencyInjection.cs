using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Rookies.Domain.Entities;
using Rookies.Persistence;

namespace Rookies.API;

public static class DependencyInjection
{

    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<RookiesDbContext>();

        context.Database.MigrateAsync().GetAwaiter().GetResult();

        await SeedAsync(context);
    }

    private static async ValueTask SeedAsync(RookiesDbContext dbContext)
    {
        var persons = new List<Person>
        {
            new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                FullName = "John Doe",
                DateOfBirth = new DateTime(1990, 1, 15),
                Gender = 1,
                BirthPlace = "New York",
                Address = "123 Main St, NY",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = null,
                CreatedBy = Guid.NewGuid(),
                ModifiedBy = Guid.Empty
            },
            new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                FullName = "Jane Smith",
                DateOfBirth = new DateTime(1988, 5, 22),
                Gender = 0,
                BirthPlace = "Los Angeles",
                Address = "456 Sunset Blvd, CA",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = null,
                CreatedBy = Guid.NewGuid(),
                ModifiedBy = Guid.Empty
            },
            new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Michael",
                LastName = "Nguyen",
                FullName = "Michael Nguyen",
                DateOfBirth = new DateTime(1995, 3, 30),
                Gender = 1,
                BirthPlace = "Houston",
                Address = "789 Westheimer Rd, TX",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = null,
                CreatedBy = Guid.NewGuid(),
                ModifiedBy = Guid.Empty
            },
            new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Linda",
                LastName = "Tran",
                FullName = "Linda Tran",
                DateOfBirth = new DateTime(1992, 12, 12),
                Gender = 0,
                BirthPlace = "San Francisco",
                Address = "321 Market St, CA",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = null,
                CreatedBy = Guid.NewGuid(),
                ModifiedBy = Guid.Empty
            },
            new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "David",
                LastName = "Kim",
                FullName = "David Kim",
                DateOfBirth = new DateTime(1985, 8, 19),
                Gender = 1,
                BirthPlace = "Seattle",
                Address = "777 Pine St, WA",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = null,
                CreatedBy = Guid.NewGuid(),
                ModifiedBy = Guid.Empty
            },
            new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Emily",
                LastName = "Pham",
                FullName = "Emily Pham",
                DateOfBirth = new DateTime(1993, 6, 5),
                Gender = 0,
                BirthPlace = "Chicago",
                Address = "999 Lake Shore Dr, IL",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = null,
                CreatedBy = Guid.NewGuid(),
                ModifiedBy = Guid.Empty
            },
            new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Daniel",
                LastName = "Lee",
                FullName = "Daniel Lee",
                DateOfBirth = new DateTime(1991, 11, 2),
                Gender = 1,
                BirthPlace = "Boston",
                Address = "111 Beacon St, MA",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = null,
                CreatedBy = Guid.NewGuid(),
                ModifiedBy = Guid.Empty
            },
            new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Sophia",
                LastName = "Nguyen",
                FullName = "Sophia Nguyen",
                DateOfBirth = new DateTime(1996, 7, 9),
                Gender = 0,
                BirthPlace = "Austin",
                Address = "222 Congress Ave, TX",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = null,
                CreatedBy = Guid.NewGuid(),
                ModifiedBy = Guid.Empty
            },
            new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Kevin",
                LastName = "Tran",
                FullName = "Kevin Tran",
                DateOfBirth = new DateTime(1989, 2, 27),
                Gender = 1,
                BirthPlace = "Denver",
                Address = "333 Colfax Ave, CO",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = null,
                CreatedBy = Guid.NewGuid(),
                ModifiedBy = Guid.Empty
            },
            new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Olivia",
                LastName = "Le",
                FullName = "Olivia Le",
                DateOfBirth = new DateTime(1997, 9, 14),
                Gender = 0,
                BirthPlace = "Miami",
                Address = "444 Ocean Dr, FL",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = null,
                CreatedBy = Guid.NewGuid(),
                ModifiedBy = Guid.Empty
            }
        };
        if (!(await dbContext.Persons.AnyAsync()))
        {
            await dbContext.BulkInsertAsync(persons);
        }
    }
}
