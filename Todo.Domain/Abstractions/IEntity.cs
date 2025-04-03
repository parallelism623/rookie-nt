using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Domain.Abstractions;
public interface IEntity<TKey>
{
    public TKey Id { get; set; }
}
