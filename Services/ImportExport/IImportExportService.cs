using mvc_todolist.Commons.Models;

namespace mvc_todolist.Services.ImportExport
{
    public interface IImportExportService
    {
        ExportData<T> Export<T>(ExportData<T> data);
    }
}
