using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        public RookiesController(IPersonService personService,
            IImportExportService importExportService)
        {
            
            _importExportService = importExportService;
            _personService = personService;  
        }
        public async Task<IActionResult> Index([FromQuery] QueryParameters<Person> queryParameters)
        {
            var persons = await _personService.GetPersonAsync(queryParameters) ?? new List<PersonViewModel>();
            SetTempData(GetCacheKeyCurrentListRookies(), GetPersonViewModelsJson(persons));
            return View(persons);
        }
        public IActionResult CreatePerson()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreatePerson(PersonViewModel person)
        {
            await _personService.CreatePersonAsync(person);
            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> GetOldestPeron()
        {
            var oldestPerson = await _personService.GetOldestPersonAsync();
            return View("Index", new List<PersonViewModel> { oldestPerson });
        }
        public async Task<IActionResult> Update(Guid id)
        {
            var personViewModel = await _personService.GetPersonByIdAsync(id);
            return View(personViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PersonViewModel personViewModel)
        {

            await _personService.UpdatePerson(personViewModel);
            SetTempData("message_response", GetResponseMessageJson("Ok", "Created new person successfully", 200));

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ViewDetail(Guid id)
        {
            var person = await _personService.GetPersonByIdAsync(id);
            return View(person);
        }
        public async Task<IActionResult> Remove(Guid id)
        {
            var person = await _personService.GetPersonByIdAsync(id);
            
            await _personService.RemovePerson(id);

            SetTempData("message_response", GetResponseMessageJson("Success", $"Person {person.FirstName + person.LastName} was removed from the list successfully", 200));
            return RedirectToAction("Index");
        }
        public IActionResult ExportToExcel()
        {

            var personFilterByAge = GetPersonViewModelsList(TempData[GetCacheKeyCurrentListRookies()]?.ToString());

            var exportData = _importExportService.Export(new ExportData<PersonViewModel>{Data = personFilterByAge, FileName = "PeopleData.xlsx" });
            
            return File(exportData.DataBytes, exportData.ContentType, exportData.FileName);
       
        }

        private void SetTempData(string key, object data)
        {
            TempData[key] = data;
        }
        private string GetCacheKeyCurrentListRookies()
        {
            return $"username:rookies:{CacheKey.CurrentRookiesData}";
        }
        private string GetPersonViewModelsJson(List<PersonViewModel> personViewModels)
        {
            return JsonSerializer.Serialize(personViewModels);
        }
        private string GetResponseMessageJson(string message, string details, int statusCode)
        {
            return JsonSerializer.Serialize(new Result(message, details, statusCode));
        }

        private List<PersonViewModel> GetPersonViewModelsList(string? personViewModels)
        {
            if(string.IsNullOrEmpty(personViewModels))
            {
                return new();
            }    
            return JsonSerializer.Deserialize<List<PersonViewModel>>(personViewModels!) ?? new();
        }

       
    }
}
