
namespace EFCORE.Contract.Shared;

public class PagingResult<T>
{
    public int PageSize { get; set; }

    public int PageIndex { get; set; }
    public int TotalCount { get; set; }

    public IEnumerable<T>? Items { get; set; }
}