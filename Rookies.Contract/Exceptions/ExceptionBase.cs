using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookies.Contract.Exceptions;
public abstract class ExceptionBase : Exception
{
    protected ExceptionBase(string message) : base(message)
    {
    }

}

