using System.Linq.Expressions;
using FluentAssertions;
using Mapster;
using Moq;
using mvc_todolist.Commons;
using mvc_todolist.Commons.Exceptions;
using mvc_todolist.ModelViews;
using mvc_todolist.Repositories.Interfaces;
using mvc_todolist.Services;
using mvc_todolist.Models.Entities;
namespace mvc_todolist.Tests.Services;

[TestFixture]
public class PersonServiceTests
{
    private Mock<IUnitOfWork>? _unitOfWorkMock;
    private Mock<IPersonRepository> _personRepositoryMock;
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

    [Test]
    public async Task GetOldestPerson_WhenSuccess_ShouldReturnPersonViewModels()
    {
        // Arrange
        _personRepositoryMock = new();
        var personViewModel = GetPersonModel();
        var persons = new List<Person>()
        {
            personViewModel.Adapt<Person>()
        };
        _personRepositoryMock
            .Setup(p => p.GetAsync(
                It.IsAny<Expression<Func<Person, bool>>>(),
                It.IsAny<Func<IQueryable<Person>, IOrderedQueryable<Person>>>(),
                It.IsAny<string>()))
            .ReturnsAsync(persons);
        _unitOfWorkMock = new();
        _unitOfWorkMock.Setup(p => p.PersonRepository).Returns(_personRepositoryMock.Object);
        var service = GetPersonService();
        // Action

        var result = await service.GetOldestPersonAsync();

        //Assert

        result.Should().NotBeNull();
        result.Should().BeOfType<PersonViewModel>();

        result.Should().BeEquivalentTo(personViewModel);
    }

    [Test]
    public async Task GetPersonAsync_WhenSuccess_ShouldReturnListPersonViewModel()
    {
        // Arrange

        SetUpDependencies();
        var queryParameters = GetQueryParameters();
        var personViewModelList = new List<PersonViewModel>
        {
            GetPersonModel()
        };
        _unitOfWorkMock.Setup(p => p.PersonRepository.GetAsync(null, null, null))
            .ReturnsAsync(personViewModelList.Adapt<List<Person>>());
        var service = GetPersonService();
        // Action

        var result = await service.GetPersonAsync(queryParameters);

        //Assert

        result.Should().NotBeNull();
        result.Should().BeOfType<List<PersonViewModel>>();
        result.Should().BeEquivalentTo(personViewModelList);
    }

    [Test]
    public async Task GetPersonByIdAsync_WhenNotFoundPerson_ShouldThrowBadRequestException()
    {
        // Arrange
        SetUpDependencies();
        _unitOfWorkMock?.Setup(u => u.PersonRepository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Person)null!);
        var service = GetPersonService();
        // Action

        var result = async () => await service.GetPersonByIdAsync(It.IsAny<Guid>());

        // Assert

        await result.Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Can not find Person by id");
    }

    [Test]
    public async Task GetPersonByIdAsync_WhenGetSuccess_ShouldReturnPersonViewModel()
    {
        // Arrange
        SetUpDependencies();
        var personViewModel = GetPersonModel();
        _unitOfWorkMock?.Setup(u => u.PersonRepository.GetByIdAsync(personViewModel.Id))
            .ReturnsAsync(personViewModel.Adapt<Person>());
        var service = GetPersonService();
        // Action

        var result = await service.GetPersonByIdAsync(personViewModel.Id);

        // Assert

        result.Should().NotBeNull();
        result.Should().BeOfType<PersonViewModel>();
        result.Should().BeEquivalentTo(personViewModel);
    }

    [Test]
    public async Task RemovePerson_WhenNotFoundPersonById_ShouldThrowBadRequest()
    {
        // Arrange
        SetUpDependencies();
        _personRepositoryMock.Setup(p => p.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Person)null!);
        var service = GetPersonService();
        // Action

        var result = async () => await service.RemovePerson(It.IsAny<Guid>());

        // Assert

        await result.Should()
            .ThrowAsync<BadRequestException>()
            .WithMessage("Can not find Person by id");
    }


    [Test]
    public async Task RemovePerson_WhenRemoveSuccess_ShouldCallRemoveOnPersonRepository()
    {
        // Arrange
        SetUpDependencies();
        var personViewModel = GetPersonModel();
        _personRepositoryMock.Setup(p => p.GetByIdAsync(personViewModel.Id))
            .ReturnsAsync(personViewModel.Adapt<Person>());
        var service = GetPersonService();
        // Action

        await service.RemovePerson(personViewModel.Id);

        // Assert

        _unitOfWorkMock?.Verify(u => u.PersonRepository.Remove(It.Is<Person>(p => p.Id == personViewModel.Id)), Times.Once());
        _unitOfWorkMock?.Verify(u => u.SaveChangesAsync(), Times.Once());
    }


    [Test]
    public async Task UpdatePerson_WhenUpdatedSuccess_ShouldCallUpdateAndSaveChangesOnPersonRepository()
    {
        // Arrange
        SetUpDependencies();
        var personViewModel = GetPersonModel();
        var service = GetPersonService();
        // Action

        await service.UpdatePerson(personViewModel);
        // Assert

        _unitOfWorkMock?.Verify(u => u.PersonRepository.Update(It.Is<Person>(p => p.Id == personViewModel.Id)),
            Times.Once);
        _unitOfWorkMock?.Verify(u => u.SaveChangesAsync(), Times.Once());
    }
    private void SetUpDependencies()
    {
        _unitOfWorkMock = new();
        _personRepositoryMock = new ();
        _unitOfWorkMock.Setup(u => u.PersonRepository).Returns(_personRepositoryMock.Object);
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


    private QueryParameters<Person> GetQueryParameters()
    {
        return new();
    }
}