using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookies.Contract.Shared;
public class PagingResult<T>
{
    private const int MaxPageSize = 50;

    private int _pageSize;
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
    public int PageIndex { get; set; }
    public int TotalCount { get; set; }

    public IEnumerable<T>? Items { get; set; }
}
