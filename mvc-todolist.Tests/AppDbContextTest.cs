
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using mvc_todolist.Models.DbContexts;

namespace mvc_todolist.Tests
{
    [TestFixture]
    public class AppDbContextTest
    {
        [Test]
        public void DbContext_WhenCreated_ShouldReturnAppDbContext()
        {
            // Arrange

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            // Action

            var dbContext = new AppDbContext(options);

            // Assert
            dbContext.Should().NotBeNull();
            dbContext.Should().BeOfType<AppDbContext>();
        }
    }
}
