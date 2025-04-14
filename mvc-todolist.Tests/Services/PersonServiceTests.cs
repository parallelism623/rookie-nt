using Moq;
using mvc_todolist.ModelViews;
using mvc_todolist.Repositories.Interfaces;
using mvc_todolist.Services;
using mvc_todolist.Models.Entities;
namespace mvc_todolist.Tests.Services;

[TestFixture]
public class PersonServiceTests
{
    private Mock<IUnitOfWork>? _unitOfWorkMock;

    [Test]
    public async Task CreatePersonAsync_WhenSuccess_ShouldCalledPersonRepositoryAddOnUnitOfWork()
    {
        // Arrange

        SetUpDependencies();
        var person = GetPersonModel();
        var service = GetPersonService();
        // Action

        await service.CreatePersonAsync(person);

        //Assert

        _unitOfWorkMock?
            .Verify(u => u.PersonRepository.Add(It.Is<Person>(p => p.Username == person.Username))
            , Times.Once());
    }
    [Test]
    public async Task CreatePersonAsync_WhenSuccess_ShouldCalledSaveChangesAsyncOnUnitOfWork()
    {
        // Arrange

        SetUpDependencies();
        var person = GetPersonModel();
        var service = GetPersonService();
        // Action

        await service.CreatePersonAsync(person);

        //Assert

        _unitOfWorkMock?.Verify(u => u.SaveChangesAsync(), Times.Once());
    }

    private void SetUpDependencies()
    {
        _unitOfWorkMock = new();
        var personRepoMock = new Mock<IPersonRepository>();
        _unitOfWorkMock.Re
    }

    private PersonService GetPersonService()
    {
        return new PersonService(_unitOfWorkMock.Object);
    }
    private PersonViewModel GetPersonModel()
    {
        return new PersonViewModel()
        {
            Id = new Guid("11111111-1111-1111-1111-111111111111"),
            Username = "user1",
            FirstName = "John",
            LastName = "Doe",
            Address = "123 Main Street",
            Password = "secret123",
            Gender = 0,
            CreateAt = DateTime.Now,
            CreateBy = new Guid("22222222-2222-2222-2222-222222222222"),
            DateOfBirth = new(1990, 5, 15),
            Graduated = true
        };
    }

}