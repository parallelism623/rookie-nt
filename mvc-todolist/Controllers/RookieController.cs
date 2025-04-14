using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using mvc_todolist.Commons;
using mvc_todolist.Commons.Models;
using mvc_todolist.Models.Entities;
using mvc_todolist.ModelViews;
using mvc_todolist.Services;
using mvc_todolist.Services.ImportExport;
using System.Text.Json;

namespace mvc_todolist.Controllers
{
    public class RookiesController : Controller
    {
        private readonly IPersonService _personService;
        private readonly IImportExportService _importExportService;
        private readonly ILogger<RookiesController> _logger;
        public RookiesController(IPersonService personService,
            IImportExportService importExportService,
            ILogger<RookiesController> logger)
        {
            _logger = logger;
            _importExportService = importExportService;
            _personService = personService;  
        }
        public async Task<IActionResult> Index([FromQuery] QueryParameters<Person> queryParameters)
        {
            LoggingInfoAction("Index", queryParameters);
            var persons = await _personService.GetPersonAsync(queryParameters) ?? new List<PersonViewModel>();
            SetTempData(GetSessionKeyCurrentListRookies(), GetPersonViewModelsJson(persons));
            return View(persons);
        }
        public IActionResult CreatePerson()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreatePerson(PersonViewModel person)
        {
            LoggingInfoAction("CreatePerson", person);
            await _personService.CreatePersonAsync(person);
            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> GetOldestPerson()
        {
            LoggingInfoAction("GetOldestPerson");
            var oldestPerson = await _personService.GetOldestPersonAsync();
            return View("Index", new List<PersonViewModel> { oldestPerson ?? default! });
        }
        public async Task<IActionResult> Update(Guid id)
        {
            var personViewModel = await _personService.GetPersonByIdAsync(id);
            return View(personViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PersonViewModel personViewModel)
        {
            LoggingInfoAction("Update", personViewModel);
            await _personService.UpdatePerson(personViewModel);
            SetTempData("message_response", GetResponseMessageJson("Ok", "Update person successfully", 200));

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ViewDetail(Guid id)
        {
            var person = await _personService.GetPersonByIdAsync(id);
            return View(person);
        }
        public async Task<IActionResult> Remove(Guid id)
        {
            LoggingInfoAction("Remove", id);
            
            await _personService.RemovePerson(id);

            SetTempData("message_response", GetResponseMessageJson("Success", $"Person {id} was removed from the list successfully", 200));
            return RedirectToAction("Index");
        }

        public IActionResult SetLanguage(string culture)
        {
            LoggingInfoAction("SetLanguage", culture);
            Response.Cookies.Append(
                "_default_lang",
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return RedirectToAction("Index");
        }
        public IActionResult ExportToExcel()
        {

            var personFilterByAge = GetPersonViewModelsList(TempData[GetSessionKeyCurrentListRookies()]?.ToString());

            var exportData = _importExportService.Export(new ExportData<PersonViewModel>{Data = personFilterByAge, FileName = "PeopleData.xlsx" });
            
            return File(exportData.DataBytes, exportData.ContentType, exportData.FileName);
       
        }

        private void LoggingInfoAction(string action, object data = null)
        {
            _logger.LogInformation("[Controller]: {Controller} - [Action]: {Action} {@Data}", "Rookies", action, data);
        }
        private void SetTempData(string key, object data)
        {
            TempData[key] = data;
        }
        private string GetSessionKeyCurrentListRookies()
        {
            return $"username:rookies:current_rookies_data";
        }
        private static string GetPersonViewModelsJson(List<PersonViewModel> personViewModels)
        {
            return JsonSerializer.Serialize(personViewModels);
        }
        private static string GetResponseMessageJson(string message, string details, int statusCode)
        {
            return JsonSerializer.Serialize(new Result(message, details, statusCode));
        }

        private static List<PersonViewModel> GetPersonViewModelsList(string? personViewModels)
        {
            if(string.IsNullOrEmpty(personViewModels))
            {
                return new();
            }    
            return JsonSerializer.Deserialize<List<PersonViewModel>>(personViewModels) ?? new();
        }

       
    }
}
