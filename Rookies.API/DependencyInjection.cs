using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Rookies.Application.Services.Crypto;
using Rookies.Domain.Entities;
using Rookies.Infrastructure.Services.Crypto;
using Rookies.Persistence;

namespace Rookies.API;

public static class DependencyInjection
{

    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<RookiesDbContext>();

        var cryptoServiceStrategy = scope.ServiceProvider.GetRequiredService<ICryptoServiceStrategy>();
        context.Database.MigrateAsync().GetAwaiter().GetResult();
        cryptoServiceStrategy.SetCryptoAlgorithm(CryptoAlgorithm.RSA);
        await SeedAsync(context, cryptoServiceStrategy);
    }

    private static async ValueTask SeedAsync(RookiesDbContext dbContext, ICryptoServiceStrategy cryptoServiceStrategy)
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
                ModifiedBy = Guid.Empty,
                Email = cryptoServiceStrategy.Encrypt("john.doe@example.com"),
                PhoneNumber = cryptoServiceStrategy.Encrypt("123-456-7890")
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
                ModifiedBy = Guid.Empty,
                Email = cryptoServiceStrategy.Encrypt("jane.smith@example.com"),
                PhoneNumber = cryptoServiceStrategy.Encrypt("234-567-8901")
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
                ModifiedBy = Guid.Empty,
                Email = cryptoServiceStrategy.Encrypt("michael.nguyen@example.com"),
                PhoneNumber = cryptoServiceStrategy.Encrypt("345-678-9012")
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
                ModifiedBy = Guid.Empty,
                Email = cryptoServiceStrategy.Encrypt("linda.tran@example.com"),
                PhoneNumber = cryptoServiceStrategy.Encrypt("456-789-0123")
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
                ModifiedBy = Guid.Empty,
                Email = cryptoServiceStrategy.Encrypt("david.kim@example.com"),
                PhoneNumber = cryptoServiceStrategy.Encrypt("567-890-1234")
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
                ModifiedBy = Guid.Empty,
                Email = cryptoServiceStrategy.Encrypt("emily.pham@example.com"),
                PhoneNumber = cryptoServiceStrategy.Encrypt("678-901-2345")
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
                ModifiedBy = Guid.Empty,
                Email = cryptoServiceStrategy.Encrypt("daniel.lee@example.com"),
                PhoneNumber = cryptoServiceStrategy.Encrypt("789-012-3456")
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
                ModifiedBy = Guid.Empty,
                Email = cryptoServiceStrategy.Encrypt("sophia.nguyen@example.com"),
                PhoneNumber = cryptoServiceStrategy.Encrypt("890-123-4567")
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
                ModifiedBy = Guid.Empty,
                Email = cryptoServiceStrategy.Encrypt("kevin.tran@example.com"),
                PhoneNumber = cryptoServiceStrategy.Encrypt("901-234-5678")
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
                ModifiedBy = Guid.Empty,
                Email = cryptoServiceStrategy.Encrypt("olivia.le@example.com"),
                PhoneNumber = cryptoServiceStrategy.Encrypt("012-345-6789")
            }
        };
        if (!(await dbContext.Persons.AnyAsync()))
        {
            await dbContext.BulkInsertAsync(persons);
        }
    }
}
