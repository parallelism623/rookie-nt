
using ClosedXML.Excel;
using mvc_todolist.Commons.Models;
using mvc_todolist.ModelViews;
using mvc_todolist.Services.ImportExport;
using System.Reflection;
using FluentAssertions;

namespace mvc_todolist.Tests.Services
{
    [TestFixture]
    public class ImportExportServiceTests
    {

        [Test]
        public void Export_WhenSuccess_ShouldReturnExportWithColumnMatchPropertiesName()
        {
            // Arrange
            var persons = new List<PersonViewModel>
            {
                new() { Id = Guid.NewGuid(), FirstName = "John Doe", DateOfBirth = DateTime.Now },
                new() { Id = Guid.NewGuid(), FirstName = "Jane Smith", DateOfBirth = DateTime.Now }
            };

            var exportModel = new ExportData<PersonViewModel>
            {
                Data = persons
            };

            var exporter = GetImportExportService();

            // Action
            var result = exporter.Export(exportModel);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ExportData<PersonViewModel>>();
            result.DataBytes.Should().NotBeNull();
            result.DataBytes.Should().NotBeEmpty();

            result.ContentType.Should()
                .BeEquivalentTo("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            result.FileName.Should()
                .BeEquivalentTo("list-PersonViewModel.xlsx");
            using var stream = new MemoryStream(result.DataBytes);
            using (var workbook = new XLWorkbook(stream))
            {
                var worksheet = workbook.Worksheet(nameof(PersonViewModel));
                worksheet.Should().NotBeNull();

                var expectedColumns = typeof(PersonViewModel)
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Select(c => c.Name)
                    .ToList();

                for (int i = 0; i < expectedColumns.Count; i++)
                {
                    var headerValue = worksheet.Cell(1, i + 1).GetString();
                    expectedColumns[i].Should().BeEquivalentTo(headerValue);
                }

                int dataRowIndex = 2;
                foreach (var person in persons)
                {
                    int colIndex = 1;
                    foreach (var prop in typeof(PersonViewModel)
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                    {
                        var expectedValue = prop.GetValue(person)?.ToString() ?? string.Empty;
                        var actualValue = worksheet.Cell(dataRowIndex, colIndex).GetString();
                        expectedValue.Should().BeEquivalentTo(actualValue); 
                        colIndex++;
                    }
                    dataRowIndex++;
                }
            }
        }

        private ImportExportService GetImportExportService()
        {
            return new ImportExportService();
        }
    }
}
