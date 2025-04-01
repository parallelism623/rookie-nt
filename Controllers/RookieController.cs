using ClosedXML.Excel;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using mvc_todolist.Commons;
using mvc_todolist.Models.Entities;
using mvc_todolist.Models.ModelViews;
using mvc_todolist.Repositories.Interfaces;

namespace mvc_todolist.Controllers
{
    public class RookiesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public RookiesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }
        public async Task<IActionResult> Index([FromQuery] QueryParameters<Person> queryParameters)
        {
            var personFilterByAge = await _unitOfWork.PersonRepository.GetAsync(filter: queryParameters.FilterExpression) ;
            return View(personFilterByAge.Adapt<List<PersonViewModel>>());
        }
        
        public async Task<IActionResult> ExportToExcel()
        {
            var personFilterByAge = await _unitOfWork.PersonRepository.GetAsync();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("People");

                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "First Name";
                worksheet.Cell(1, 3).Value = "Last Name";
                worksheet.Cell(1, 4).Value = "Gender";
                worksheet.Cell(1, 5).Value = "Date of Birth";
                worksheet.Cell(1, 6).Value = "Age";
                worksheet.Cell(1, 7).Value = "Address";

                int row = 2;
                foreach (var person in personFilterByAge)
                {
                    worksheet.Cell(row, 1).Value = person.Id.ToString();
                    worksheet.Cell(row, 2).Value = person.FirstName;
                    worksheet.Cell(row, 3).Value = person.LastName;
                    worksheet.Cell(row, 4).Value = person.Gender;
                    worksheet.Cell(row, 5).Value = person.DateOfBirth.ToString("yyyy-MM-dd");
                    worksheet.Cell(row, 6).Value = person.Age;
                    worksheet.Cell(row, 7).Value = person.Address;
                    row++;
                }


                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;


                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PeopleData.xlsx");
                }
            }
        }
    }
}
