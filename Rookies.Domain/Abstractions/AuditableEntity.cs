using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookies.Domain.Abstractions;
public interface IAuditableEntity
{
    DateTime CreatedAt { get; set; }
    DateTime? ModifiedAt { get;set; }
    Guid CreatedBy { get; set; }
    Guid? ModifiedBy { get; set; }
}
