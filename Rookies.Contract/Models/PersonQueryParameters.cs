using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookies.Contract.Models;
public class PersonQueryParameters : PaginationQueryParameters
{
    public int? Gender { get; set; }
    public string? BirthPlace { get; set; }
    public string? Name { get; set; }
}