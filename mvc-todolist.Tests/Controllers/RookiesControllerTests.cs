
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using mvc_todolist.Commons;
using mvc_todolist.Commons.Exceptions;
using mvc_todolist.Commons.Models;
using mvc_todolist.Controllers;
using mvc_todolist.Models.Entities;
using mvc_todolist.ModelViews;
using mvc_todolist.Services;
using mvc_todolist.Services.ImportExport;
using mvc_todolist.Tests.Commons.Extensions;
using mvc_todolist.Tests.Commons.Fakes;
using System.Text.Json;
namespace mvc_todolist.Tests.Controller
{
    [TestFixture]
    public class RookiesControllerTests
    {
        private Mock<IPersonService>? _personServiceMock;
        private Mock<IImportExportService>? _importExportServiceMock;
        private Mock<ILogger<RookiesController>>? _loggerMock;
        private Mock<ITempDataProvider>? _tempDataProviderMock;


        [Test]
        public async Task Index_WithValidQueryParameters_ShouldReturnViewsResult_WithListPersonViewModel()
        {
            //Arrange
            SetUpDependencies();
            var personViewModels = GetListPersonViewModels();
            var queryParameters = new QueryParameters<Person>();
            _personServiceMock!.Setup(p => p.GetPersonAsync(queryParameters)).ReturnsAsync(personViewModels);
            var controller = SetUpControllerWithDependencies();

            //Action

            var result = await controller.Index(queryParameters);

            // Assert
                
            _loggerMock?.VerifyLog(LogLevel.Information,
                    "[Controller]: {Controller} - [Action]: {Action} {@Data}", 
                    new Dictionary<string, object>()
                        {
                            {"Controller", "Rookies" },
                            {"Action", "Index" },
                            {"@Data",  queryParameters}
                        }, 
                    Times.Once());
            controller.TempData.Should().ContainKey("username:rookies:current_rookies_data");
            string tempDataJson = controller.TempData["username:rookies:current_rookies_data"] as string ?? default!;
            tempDataJson.Should().NotBeNull();
            var personFromTempData = JsonSerializer.Deserialize<List<PersonViewModel>>(tempDataJson);
            personFromTempData.Should().NotBeNull();
            personFromTempData.Should().BeEquivalentTo(personViewModels, "View data should equivalent input data samples");

            result.Should().BeOfType<ViewResult>("Index result should be a View Result");
            result.Should().NotBeNull("Index result should be not null");
        }
        [Test]
        public void CreatePerson_ShouldReturnViewsResult()
        {
            //Arrange
            SetUpDependencies();
            var controller = SetUpControllerWithDependencies();

            //Action
            var result = controller.CreatePerson();

            //Assert
            result.Should().BeOfType<ViewResult>("Create person should return a view result");
        }
        [Test]
        public async Task CreatePerson_ShouldReturnRedirectToActionResult_WhenCreatePersonSuccess()
        {
            // Arrange
            SetUpDependencies();
            var personCreateRequest = GetPersonModel();
            _personServiceMock?.Setup(p => p.CreatePersonAsync(personCreateRequest)) ;
            var controller = SetUpControllerWithDependencies();

            // Action

            var result = await controller.CreatePerson(personCreateRequest);

            // Assert

            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectToActionResult>()
                .Which.ActionName.Should().Be("Index");
           
        }

        [Test]

        public async Task CreatePerson_ShouldCallCreateOnPersonService_WhenCreatedPerson()
        {
            // Arrange
            SetUpDependencies();
            var personCreateRequest = GetPersonModel();
            _personServiceMock?.Setup(p => p.CreatePersonAsync(personCreateRequest));
            var controller = SetUpControllerWithDependencies();

            // Action

            await controller.CreatePerson(personCreateRequest);

            // Assert

            _personServiceMock!.Verify(p => p.CreatePersonAsync(personCreateRequest), Times.Once());
        }

        [Test]
        public async Task CreatePerson_ShouldCallLogInfo_WhenCalledWithValidPerson()
        {
            // Arrange
            SetUpDependencies();
            var personCreateRequest = GetPersonModel();
            _personServiceMock?.Setup(p => p.CreatePersonAsync(personCreateRequest));
            var controller = SetUpControllerWithDependencies();

            // Action

            await controller.CreatePerson(personCreateRequest);

            // Assert

            _loggerMock?.VerifyLog(LogLevel.Information,
                    "[Controller]: {Controller} - [Action]: {Action} {@Data}",
                    new Dictionary<string, object>()
                        {
                            {"Controller", "Rookies" },
                            {"Action", "CreatePerson" },
                            {"@Data",  personCreateRequest}
                        },
                    Times.Once());
        }

        [Test]
        public async Task GetOldestPerson_WhenCalledSuccess_ShouldReturnViewResultWithOldestPerson()
        {
            // Arrange
            SetUpDependencies();
            var personModel = GetPersonModel();
            _personServiceMock?.Setup(p => p.GetOldestPersonAsync()).ReturnsAsync(personModel);
            var controller = SetUpControllerWithDependencies();

            // Action

            var result = await controller.GetOldestPerson();

            // Assert

            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();

            var personsViewModelResult = viewResult.Model as List<PersonViewModel>;

            personsViewModelResult.Should().NotBeNull();
            personsViewModelResult.Should().BeEquivalentTo(new List<PersonViewModel>() { personModel });
        }


        [Test]
        public async Task GetOldestPerson_WhenCalledSuccess_ShouldLogInfoGetOldestPeron()
        {
            // Arrange
            SetUpDependencies();
            var personModel = GetPersonModel();
            _personServiceMock?.Setup(p => p.GetOldestPersonAsync()).ReturnsAsync(personModel);
            var controller = SetUpControllerWithDependencies();

            // Action

            await controller.GetOldestPerson();

            // Assert
            _loggerMock?.VerifyLog(LogLevel.Information,
                "[Controller]: {Controller} - [Action]: {Action} {@Data}",
                new Dictionary<string, object>()
                    {
                                    {"Controller", "Rookies" },
                                    {"Action", "GetOldestPerson" },
                                    {"@Data",  default!}
                    },
                Times.Once());
        }
        [Test]
        public async Task GetOldestPerson_WhenCalledSuccess_ShouldCallGetOldestPersonOnPersonRepository()
        {
            // Arrange
            SetUpDependencies();
            var personModel = GetPersonModel();
            _personServiceMock?.Setup(p => p.GetOldestPersonAsync()).ReturnsAsync(personModel);
            var controller = SetUpControllerWithDependencies();

            // Action

            await controller.GetOldestPerson();

            // Assert

            _personServiceMock?.Verify(p => p.GetOldestPersonAsync(), Times.Once);
        }


        [Test]
        public async Task Update_WhenCallSuccess_ShouldReturnViewResultUpdateWithModelUpdate()
        {
            SetUpDependencies();
            var personModel = GetPersonModel();
            _personServiceMock?.Setup(p => p.GetPersonByIdAsync(personModel.Id)).ReturnsAsync(personModel);
            var controller = SetUpControllerWithDependencies();

            // Action

            var result = await controller.Update(personModel.Id);

            // Assert

            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();

            var personsViewModelResult = viewResult.Model as PersonViewModel;

            personsViewModelResult.Should().NotBeNull();
            personsViewModelResult.Id.Should().Be(personModel.Id);
        }
        [Test]
        public async Task Update_WhenPersonNotFoundException_ShouldPropagatesException()
        {
            SetUpDependencies();
            var personModel = GetPersonModel();
            _personServiceMock?.Setup(p => p.GetPersonByIdAsync(personModel.Id))
                            .ThrowsAsync(new BadRequestException("Can not find Person by id"));
            var controller = SetUpControllerWithDependencies();

            // Action

            var result = async () => await controller.Update(personModel.Id);

            // Assert

            await result.Should().ThrowAsync<BadRequestException>()
                     .WithMessage("Can not find Person by id");
        }

        [Test]
        public async Task Update_WhenCallSuccess_ShouldCallGetPersonByIdOnPersonService()
        {
            SetUpDependencies();
            var personModel = GetPersonModel();
            _personServiceMock?.Setup(p => p.GetPersonByIdAsync(personModel.Id)).ReturnsAsync(personModel);
            var controller = SetUpControllerWithDependencies();

            // Action

            await controller.Update(personModel.Id);

            // Assert

            _personServiceMock?.Verify(p => p.GetPersonByIdAsync(personModel.Id), Times.Once());
        }

        [Test]
        public async Task Update_WhenStartingUpdate_ShouldLogInfoPersonUpdate()
        {
            // Arrange

            SetUpDependencies();
            var personModel = GetPersonModel();
            var controller = SetUpControllerWithDependencies();

            // Action

            await controller.Update(personModel);

            // Assert

            _loggerMock?.VerifyLog(LogLevel.Information,
                "[Controller]: {Controller} - [Action]: {Action} {@Data}",
                new Dictionary<string, object>()
                    {
                                    {"Controller", "Rookies" },
                                    {"Action", "Update" },
                                    {"@Data",  personModel}
                    },
                Times.Once());
        }
        [Test]
        public async Task Update_WhenUpdatedSuccess_ShouldSaveDataIntoTempData()
        {
            // Arrange

            SetUpDependencies();
            var result = new Result("Ok", "Update person successfully", 200);
            var controller = SetUpControllerWithDependencies();

            // Action

            await controller.Update(GetPersonModel());

            // Assert

            controller.TempData.Should().ContainKey("message_response");
            string tempDataJson = controller.TempData["message_response"] as string ?? default!;
            tempDataJson.Should().NotBeNull();
            var resultFromTempData = JsonSerializer.Deserialize<Result>(tempDataJson);
            resultFromTempData.Should().NotBeNull();
            resultFromTempData.Should().BeEquivalentTo(result, "View data should equivalent input data samples");
        }
        [Test]
        public async Task Update_WhenUpdatedSuccess_ShouldCallUpdatePersonOnPersonService()
        {
            // Arrange

            SetUpDependencies();
            var personModelUpdate = GetPersonModel();
            var controller = SetUpControllerWithDependencies();

            // Action

            await controller.Update(personModelUpdate);

            // Assert

            _personServiceMock?.Verify(p => p.UpdatePerson(personModelUpdate), Times.Once());
        }

        [Test]
        public async Task Update_WhenUpdatedSuccess_ShouldReturnRedirectToActionResult()
        {
            // Arrange

            SetUpDependencies();
            var personModelUpdate = GetPersonModel();
            var controller = SetUpControllerWithDependencies();

            // Action

            var result = await controller.Update(personModelUpdate);

            // Assert

            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectToActionResult>();

            var redirectToActionResult = result as RedirectToActionResult;

            redirectToActionResult.Should().NotBeNull();
            redirectToActionResult.ActionName.Should().Be("Index");
        }


        [Test]
        public async Task ViewDetail_WhenPersonNotFoundException_ShouldPropagatesException()
        {
            SetUpDependencies();
            var personModel = GetPersonModel();
            _personServiceMock?.Setup(p => p.GetPersonByIdAsync(personModel.Id))
                            .ThrowsAsync(new BadRequestException("Can not find Person by id"));
            var controller = SetUpControllerWithDependencies();

            // Action

            var result = async () => await controller.ViewDetail(personModel.Id);

            // Assert

            await result.Should().ThrowAsync<BadRequestException>()
                     .WithMessage("Can not find Person by id");
        }

        [Test]
        public async Task ViewDetail_WhenGetPersonDataSuccess_ShouldCallGetPersonByIdOnPersonService()
        {
            // Arrange
            SetUpDependencies();
            var personModel = GetPersonModel();
            _personServiceMock?.Setup(p => p.GetPersonByIdAsync(personModel.Id)).ReturnsAsync(personModel);
            var controller = SetUpControllerWithDependencies();

            // Action

            await controller.ViewDetail(personModel.Id);

            // Assert

            _personServiceMock?.Verify(p => p.GetPersonByIdAsync(personModel.Id), Times.Once());
        }

        [Test]
        public async Task ViewDetail_WhenGetPersonDataSuccess_ShouldReturnViewResultWithPersonData()
        {
            // Arrange
            SetUpDependencies();
            var personModel = GetPersonModel();
            _personServiceMock?.Setup(p => p.GetPersonByIdAsync(personModel.Id)).ReturnsAsync(personModel);
            var controller = SetUpControllerWithDependencies();

            // Action

            var result = await controller.ViewDetail(personModel.Id);

            // Assert

            result.Should().NotBeNull();
            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var personViewModel = viewResult.Model as PersonViewModel;
            personViewModel.Should().NotBeNull();
            personViewModel.Id.Should().Be(personViewModel.Id);
        }

        [Test]
        public async Task Remove_WhenRemovedPerson_ShouldReturnRedirectToActionResult()
        {
            // Arrange

            SetUpDependencies();
            var idDelete = Guid.NewGuid();
            var controller = SetUpControllerWithDependencies();
            // Action

            var result = await controller.Remove(idDelete);

            // Assert

            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectToActionResult>();

            var redirectToActionResult = result as RedirectToActionResult;
            redirectToActionResult.Should().NotBeNull();
            redirectToActionResult.ActionName.Should().BeEquivalentTo("Index");
        }

        [Test]
        public async Task Remove_WhenRemovedPerson_ShouldCallRemovePersonOnPersonService()
        {
            // Arrange

            SetUpDependencies();
            var idDelete = Guid.NewGuid();
            var controller = SetUpControllerWithDependencies();
            // Action

            await controller.Remove(idDelete);

            // Assert

            _personServiceMock?.Verify(p => p.RemovePerson(idDelete), Times.Once());
        }

        [Test]
        public async Task Remove_WhenRemovedPerson_ShouldSaveDataIntoTempData()
        {
            // Arrange

            SetUpDependencies();
            var idDelete = Guid.NewGuid();
            var dataSaveIntoTempData = new Result("Success", $"Person {idDelete} was removed from the list successfully", 200);
            var controller = SetUpControllerWithDependencies();
            // Action

            await controller.Remove(idDelete);

            // Assert
            controller.TempData.Should().NotBeNull();
            controller.TempData.Should().ContainKey("message_response");
            var tempDataJson = controller.TempData["message_response"] as string;

            tempDataJson.Should().NotBeNull();
            var dataFromTempData = JsonSerializer.Deserialize<Result>(tempDataJson);

            dataFromTempData.Should().BeEquivalentTo(dataSaveIntoTempData);
        }

        [Test]
        public async Task Remove_WhenStartRemovePerson_ShouldLogInfoAction()
        {
            // Arrange

            SetUpDependencies();
            var idDelete = Guid.NewGuid();
            var controller = SetUpControllerWithDependencies();
            // Action

            await controller.Remove(idDelete);

            // Assert

            _loggerMock?.VerifyLog(LogLevel.Information,
                "[Controller]: {Controller} - [Action]: {Action} {@Data}",
                new Dictionary<string, object>()
                    {
                        {"Controller", "Rookies" },
                        {"Action", "Remove" },
                        {"@Data",  idDelete}
                    },
                Times.Once());
        }

        [Test]
        public async Task Remove_WhenDependenciesThrowException_ShouldPropagatesException()
        {
            // Arrange

            SetUpDependencies();
            var idDelete = Guid.NewGuid();
            _personServiceMock?.Setup(p => p.RemovePerson(idDelete))
                              .ThrowsAsync(new BadRequestException("Person can not found by id"));
            var controller = SetUpControllerWithDependencies();
            // Action

            var result = async () => await controller.Remove(idDelete);

            // Assert

            await result.Should().ThrowAsync<BadRequestException>()
                        .WithMessage("Person can not found by id");
        }

        [Test]
        public void SetLanguage_WhenStartingSetLanguage_ShouldLogInfoAboutCulture()
        {
            // Arrange

            SetUpDependencies();
            var locale = "en-US";
            var controller = SetUpControllerWithDependencies();
            // Action

            controller.SetLanguage(locale);

            // Assert

            _loggerMock?.VerifyLog(LogLevel.Information,
                "[Controller]: {Controller} - [Action]: {Action} {@Data}",
                new Dictionary<string, object>()
                {
                    {"Controller", "Rookies" },
                    {"Action", "SetLanguage" },
                    {"@Data",  locale}
                },
                Times.Once());

        }
        [Test]
        public void SetLanguage_WhenChangedLanguage_ShouldSetLocaleIntoCookies()
        {
            // Arrange

            SetUpDependencies();
            var locale = "en-US";
            var controller = SetUpControllerWithDependencies();
            // Action

            controller.SetLanguage(locale);

            // Assert

            var cookiesProvider =
                controller.HttpContext.Features.Get<IResponseCookiesFeature>()?.Cookies as FakeResponseCookies;
            cookiesProvider.Should().NotBeNull();

            cookiesProvider.AppendedCookies.Should().ContainSingle(cookie =>
                cookie.Key == "_default_lang" &&
                cookie.Value.Contains(locale) &&  
                cookie.Options.Expires > DateTimeOffset.UtcNow);

        }

        [Test]
        public void SetLanguage_WhenChangedLanguageSuccess_ShouldReturnRedirectToActionResult()
        {
            // Arrange

            SetUpDependencies();
            var locale = "en-US";
            var controller = SetUpControllerWithDependencies();
            // Action

            var result = controller.SetLanguage(locale);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectToActionResult>();

            var redirectToActionResult = result as RedirectToActionResult;
            redirectToActionResult.Should().NotBeNull();
            redirectToActionResult.ActionName.Should().BeEquivalentTo("Index");
        }

        [Test]
        public void ExportToExcel_WhenExportSuccess_ShouldReturnFileContentResult()
        {
            //Arrange
            SetUpDependencies();
            var exportDataResult = new ExportData<PersonViewModel>()
            {
                DataBytes = [],
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileName = $"list-{nameof(PersonViewModel)}.xlsx"
            };
            _importExportServiceMock?.Setup(i => i.Export(It.IsAny<ExportData<PersonViewModel>>())).Returns(exportDataResult);
            var controller = SetUpControllerWithDependencies();

            //Action

            var result = controller.ExportToExcel();

            // Assert

            result.Should().NotBeNull();
            result.Should().BeOfType<FileContentResult>();

            var fileContentResult = result as FileContentResult;
            fileContentResult.Should().NotBeNull();
            fileContentResult.FileContents.Should().BeEquivalentTo(exportDataResult.DataBytes);
            fileContentResult.ContentType.Should()
                .BeEquivalentTo(exportDataResult.ContentType);
            fileContentResult.FileDownloadName.Should().BeEquivalentTo(exportDataResult.FileName);

        }
        private void SetUpDependencies()
        {
            _personServiceMock = new();
            _importExportServiceMock = new();
            _loggerMock = new();
            _tempDataProviderMock = new();
        }

        private RookiesController SetUpControllerWithDependencies()
        {

            var httpContext = new DefaultHttpContext();
            var fakeCookiesFeature = new FakeResponseCookiesFeature();
            httpContext.Features.Set<IResponseCookiesFeature>(fakeCookiesFeature);
            var tempDataDictionaryFactory = new TempDataDictionaryFactory(_tempDataProviderMock!.Object);
            ITempDataDictionary tempData = tempDataDictionaryFactory.GetTempData(httpContext);
            var controller = new RookiesController(_personServiceMock!.Object, _importExportServiceMock!.Object, _loggerMock!.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext},
                TempData = tempData
            };
            return controller;
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
        private List<PersonViewModel> GetListPersonViewModels()
        {
            return new()
            {
                new PersonViewModel
                {
                    Id = Guid.NewGuid(),
                    Username = "user2",
                    FirstName = "Jane",
                    LastName = "Smith",
                    Address = "456 Elm St",
                    Password = "pass2",
                    Gender = 2,
                    CreateAt = DateTime.Now,
                    CreateBy = Guid.NewGuid(),
                    DateOfBirth = new DateTime(1985, 5, 20),
                    Graduated = true
                },
                new PersonViewModel
                {
                    Id = Guid.NewGuid(),
                    Username = "user3",
                    FirstName = "Bob",
                    LastName = "Johnson",
                    Address = "789 Oak St",
                    Password = "pass3",
                    Gender = 1,
                    CreateAt = DateTime.Now,
                    CreateBy = Guid.NewGuid(),
                    DateOfBirth = new(1978, 11, 15),
                    Graduated = true
                },
                new PersonViewModel
                {
                    Id = Guid.NewGuid(),
                    Username = "user4",
                    FirstName = "Alice",
                    LastName = "Brown",
                    Address = "101 Pine St",
                    Password = "pass4",
                    Gender = 2,
                    CreateAt = DateTime.Now,
                    CreateBy = Guid.NewGuid(),
                    DateOfBirth = new(1995, 3, 10),
                    Graduated = false
                },
                new PersonViewModel
                {
                    Id = Guid.NewGuid(),
                    Username = "user5",
                    FirstName = "Charlie",
                    LastName = "Miller",
                    Address = "202 Maple Ave",
                    Password = "pass5",
                    Gender = 1,
                    CreateAt = DateTime.Now,
                    CreateBy = Guid.NewGuid(),
                    DateOfBirth = new DateTime(1988, 8, 25),
                    Graduated = true
                },
                new PersonViewModel
                {
                    Id = Guid.NewGuid(),
                    Username = "user6",
                    FirstName = "Diana",
                    LastName = "Wilson",
                    Address = "303 Cedar Rd",
                    Password = "pass6",
                    Gender = 2,
                    CreateAt = DateTime.Now,
                    CreateBy = Guid.NewGuid(),
                    DateOfBirth = new(1992, 12, 5),
                    Graduated = false
                },
                new PersonViewModel
                {
                    Id = Guid.NewGuid(),
                    Username = "user7",
                    FirstName = "Edward",
                    LastName = "Davis",
                    Address = "404 Birch Ln",
                    Password = "pass7",
                    Gender = 1,
                    CreateAt = DateTime.Now,
                    CreateBy = Guid.NewGuid(),
                    DateOfBirth = new DateTime(1982, 4, 3),
                    Graduated = true
                },
                new PersonViewModel
                {
                    Id = Guid.NewGuid(),
                    Username = "user8",
                    FirstName = "Fiona",
                    LastName = "Clark",
                    Address = "505 Walnut St",
                    Password = "pass8",
                    Gender = 2,
                    CreateAt = DateTime.Now,
                    CreateBy = Guid.NewGuid(),
                    DateOfBirth = new DateTime(1980, 2, 14),
                    Graduated = true
                },
                new PersonViewModel
                {
                    Id = Guid.NewGuid(),
                    Username = "user9",
                    FirstName = "George",
                    LastName = "Lewis",
                    Address = "606 Cherry St",
                    Password = "pass9",
                    Gender = 1,
                    CreateAt = DateTime.Now,
                    CreateBy = Guid.NewGuid(),
                    DateOfBirth = new DateTime(1975, 7, 30),
                    Graduated = true
                },
                new PersonViewModel
                {
                    Id = Guid.NewGuid(),
                    Username = "user10",
                    FirstName = "Helen",
                    LastName = "Young",
                    Address = "707 Poplar Ave",
                    Password = "pass10",
                    Gender = 2,
                    CreateAt = DateTime.Now,
                    CreateBy = Guid.NewGuid(),
                    DateOfBirth = new DateTime(2000, 6, 18),
                    Graduated = false
                }
            };
        }
    }
}
