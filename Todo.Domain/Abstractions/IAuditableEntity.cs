using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Domain.Abstractions
{
    public interface IAuditableEntity<TKey>
    {
        public DateTime CreatedAt { get; set; }
        public TKey CreateBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public TKey? ModifiedBy { get; set; }
    }
}
