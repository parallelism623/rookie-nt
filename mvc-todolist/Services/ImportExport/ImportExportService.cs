using ClosedXML.Excel;
using mvc_todolist.Commons.Helpers;
using mvc_todolist.Commons.Models;
using mvc_todolist.ModelViews;

namespace mvc_todolist.Services.ImportExport
{
    public class ImportExportService : IImportExportService
    {
        public ExportData<T> Export<T>(ExportData<T> exportModel)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(typeof(T).Name);

                var columns = ReflectionHelper.GetPropertiesNameOfType<T>();

                int row = 1;
                for(int i = 0; i < columns.Count(); i++)
                {
                    worksheet.Cell(row, i + 1).Value = columns[i];
                }

                row += 1;
                var data = exportModel.Data.Select(c =>
                {
                    var dictionary = new Dictionary<string, object>();
                    foreach (var it in ReflectionHelper.GetPropertiesInfoOfType<T>())
                    {
                        dictionary.Add(it.Name, it?.GetValue(c) ?? default!);
                    }
                    return dictionary;
                }).ToList();

                for(int i = 0; i < data.Count(); i++)
                {
                    for (int j = 0; j < columns.Count(); j++)
                    {
                        worksheet.Cell(row, j + 1).Value = data[i][columns[j]]?.ToString();
                    }
                    row++;
                }    
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;

                    exportModel.DataBytes = stream.ToArray();
                    exportModel.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    exportModel.FileName = $"list-{typeof(T).Name}.xlsx";
                    return exportModel;
                }
            }
            
        }
    }
}
