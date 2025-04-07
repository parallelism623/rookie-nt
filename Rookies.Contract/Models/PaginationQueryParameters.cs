using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookies.Contract.Models;
public abstract class PaginationQueryParameters
{
    public int PageSize { get; set; } = 10;
    public int PageIndex { get; set; } = 1;

}
